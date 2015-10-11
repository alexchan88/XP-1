using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Hosting;

namespace AuditManager.Common
{
    public class AmUtil
    {
        public static string GetActivityUserNameWithDomain()
        {
            var sb = new StringBuilder();
            ConfigUtility.GetActivityUser().Split(',').ToList().ForEach(x => sb.Append(x.ToUserIdWithDomainName() + ","));
            return sb.ToString(0, sb.Length - 1);
        }

        public static string GetLineSeperator
        {
            get { return "**********************************************************************************************************************"; }
        }

        public static string GetCurrentUser
        {
            get
            {
                //System.Environment.SetEnvironmentVariable("Role", "Goodrole");
                return System.Environment.UserName;
                //return System.Security.Principal.WindowsIdentity.GetCurrent().Name.SplitNGet('\\', 1);
            }
        }

        public static string GetMachineName
        {
            get
            {
                //System.Net.Dns.GetHostName()
                return System.Environment.MachineName;
            }
        }

        public static string GetIP
        {
            get
            {
                return HttpContext.Current.Request.UserHostAddress;
                //return System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(0).ToString();
            }
        }

        public static bool IsAdminUser()
        {
            var adminUsr = ConfigUtility.GetAdminUser().Split(',');
            return adminUsr.Contains(GetCurrentUser);
        }

        public static bool IsSuperUser()
        {
            var usr = ConfigUtility.GetSuperUser().Split(',');
            return usr.Contains(GetCurrentUser);
        }

        public static bool IsMaintenanceUser()
        {
            var maintenanceUser = ConfigUtility.MaintenanceUser().Split(',');
            return maintenanceUser.Contains(GetCurrentUser);
        }

        public static string GetEMailImgPath
        {
            get { return HostingEnvironment.ApplicationPhysicalPath + "//Email//Images//"; }
        }

        private static string GetEMailTemplatePath(string emailTemplateName)
        {
            switch (emailTemplateName.ToUpper())
            {
                case "CLOSURE_INITIATE":
                    return HostingEnvironment.ApplicationPhysicalPath + "//Email//Template//Closure.html";
                case "WSPROFILE_PRESERVATION_ON":
                    return HostingEnvironment.ApplicationPhysicalPath + "//Email//Template//Preservation-ON.html";
                case "WSPROFILE_PRESERVATION_OFF":
                    return HostingEnvironment.ApplicationPhysicalPath + "//Email//Template//Preservation-OFF.html";
                case "WS_CREATE":
                    return HostingEnvironment.ApplicationPhysicalPath + "//Email//Template//Create_WS.html";
                case "WORKSPACE_REQUESTACCESS":
                    return HostingEnvironment.ApplicationPhysicalPath + "//Email//Template//RequestAccess.html";
                default:
                    return null;
            }
        }

        private static dynamic GetEMailBody(string emailTemplateName,
            string wsNum, string wsName, List<Tuple<string, string>> recipients, string mailInfo)
        {
            StringBuilder body = null;

            var image001 = new LinkedResource(GetEMailImgPath + "image001.jpg");
            image001.ContentId = Guid.NewGuid().ToString();
            var image002 = new LinkedResource(GetEMailImgPath + "image002.png");
            image002.ContentId = Guid.NewGuid().ToString();
            var image003 = new LinkedResource(GetEMailImgPath + "image003.jpg");
            image003.ContentId = Guid.NewGuid().ToString();

            using (StreamReader reader = new StreamReader(GetEMailTemplatePath(emailTemplateName), System.Text.Encoding.Default))
            {
                body = new StringBuilder(reader.ReadToEnd());
                //reader.Close();

                body.Replace("{{CurrentDate}}", DateTime.Now.ToShortDateString());

                if (recipients != null)
                    body.Replace("{{UserList}}", string.Join("; ", recipients.Select(x => x.Item1).ToList()));

                body.Replace("{{WorkspaceID}}", wsNum);
                body.Replace("{{WorkspaceName}}", wsName);

                body.Replace("{{image001}}", image001.ContentId);
                body.Replace("{{image002}}", image002.ContentId);
                body.Replace("{{image003}}", image003.ContentId);
            }

            switch (emailTemplateName.ToUpper())
            {
                case "CLOSURE_INITIATE":
                    {
                        body.Replace("{{ClosureFiles}}", mailInfo);
                        return new { Body = body.ToString(), image001 = image001, image002 = image002, image003 = image003 };
                    }
                case "WS_CREATE":
                    {
                        return new { Body = body.ToString(), image001 = image001, image002 = image002, image003 = image003 };
                    }
                case "WORKSPACE_REQUESTACCESS":
                    {
                        return new { Body = body.ToString(), image001 = image001, image002 = image002, image003 = image003 };
                    }
                default:
                    return null;
            }
        }


        private static void SendMail(MailMessage mail)
        {
            if (ConfigUtility.IsTestMail)
            {
                mail.From = new MailAddress(ConfigUtility.TestFromMailId);
                mail.To.Clear();
                mail.To.Add(ConfigUtility.TestToMailId);
                if (!ConfigUtility.TestToMailId.Equals(AmUtil.GetCurrentUser.ToKPMGEmail(), StringComparison.OrdinalIgnoreCase))
                    mail.To.Add(AmUtil.GetCurrentUser.ToKPMGEmail());
                mail.CC.Clear();
            }

            if (!ConfigUtility.GetEnv.Equals("prod", StringComparison.OrdinalIgnoreCase))
                mail.Subject = string.Format("{0} -- [Environment - {1}]", mail.Subject, ConfigUtility.GetEnv);

            using (SmtpClient client = new SmtpClient())
            {
                //client.DeliveryMethod = SmtpDeliveryMethod.Network;
                //client.UseDefaultCredentials = false;
                //client.Host = ConfigUtility.GetSmptSrvr;
                try
                {
                    client.Send(mail);
                }
                catch (Exception ex)
                {

                }
            }
        }

        public static string SendMail_Closure(string emailTemplateName,
            string wsNum, string wsName, List<Tuple<string, string>> recipients, string mailInfo)
        {
            var body = GetEMailBody(emailTemplateName, wsNum, wsName, recipients, mailInfo);

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(ConfigUtility.GetClosureConfig.Item2);

            var toAddress = string.Join(",", recipients.Select(x => x.Item2.ToEmail()).ToList());
            mail.To.Add(toAddress);
            mail.CC.Add(ConfigUtility.GetClosureConfig.Item1);

            if (ConfigUtility.IncludeBcc)
            {
                mail.Bcc.Add(string.Join(",", ConfigUtility.EmailBcc.Split(',').ToList().Select(x => x.ToEmail()).ToList()));
            }

            mail.Subject = ConfigUtility.GetClosureConfig.Item3.Replace("{{WorkspaceName}}", wsName); ;
            mail.Body = body.Body;
            mail.IsBodyHtml = true;

            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(mail.Body, null, "text/html");

            htmlView.LinkedResources.Add(body.image001);
            htmlView.LinkedResources.Add(body.image002);
            htmlView.LinkedResources.Add(body.image003);

            mail.AlternateViews.Add(htmlView);

            //using (SmtpClient client = new SmtpClient())
            //{
            //    //client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //    //client.UseDefaultCredentials = false;
            //    //client.Host = ConfigUtility.GetSmptSrvr;

            //    client.Send(mail);
            //}

            SendMail(mail);

            return mail.Body;
        }

        public static string SendMail_Preservation(string emailTemplateName,
            string wsNum, string wsName, string wsClientDesc, List<Tuple<string, string>> recipients,
            List<Tuple<string, string>> wsAdmin,
            string subject)
        {
            StringBuilder body = null;
            using (StreamReader reader = new StreamReader(GetEMailTemplatePath(emailTemplateName), System.Text.Encoding.Default))
            {
                body = new StringBuilder(reader.ReadToEnd());
                //reader.Close();

                body.Replace("{{CurrentDate}}", DateTime.Now.ToShortDateString());
                body.Replace("{{WorkspaceName}}", wsName);
                body.Replace("{{ClientName}}", wsClientDesc);
                body.Replace("{{WsOwners_Admin}}", string.Join(",", wsAdmin.Select(x => x.Item2).ToList()));
                body.Replace("{{Ws_Members}}", string.Join(",", recipients.Select(x => x.Item2).ToList()));
            }

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(ConfigUtility.GetPreservationConfig.Item2, ConfigUtility.GetPreservationConfig.Item3);
            mail.Subject = subject;

            var toAddress = string.Join(",", recipients.Select(x => x.Item2.ToEmail()).ToList());
            mail.To.Add(toAddress);
            mail.CC.Add(ConfigUtility.GetClosureConfig.Item1);

            if (ConfigUtility.IncludeBcc)
            {
                mail.Bcc.Add(string.Join(",", ConfigUtility.EmailBcc.Split(',').ToList().Select(x => x.ToEmail()).ToList()));
            }

            mail.Subject = subject;
            mail.Body = body.ToString();
            mail.IsBodyHtml = true;

            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(mail.Body, null, "text/html");

            mail.AlternateViews.Add(htmlView);

            //using (SmtpClient client = new SmtpClient())
            //{
            //    //client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //    //client.UseDefaultCredentials = false;
            //    //client.Host = ConfigUtility.GetSmptSrvr;

            //    client.Send(mail);
            //}

            SendMail(mail);

            return mail.Body;
        }

        public static string SendMail_WsCreate(string emailTemplateName,
            string wsNum, string wsName, List<Tuple<string, string, string>> recipients)
        {
            var body = GetEMailBody(emailTemplateName, wsNum, wsName, null, null);

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(ConfigUtility.GetWsCreateConfig.Item2);

            var nonBlankRecipient = recipients.Where(x => !string.IsNullOrWhiteSpace(x.Item2)).ToList<Tuple<string, string, string>>();

            var toAddress = string.Join(",", nonBlankRecipient.Select(x => x.Item3.ToEmail()).ToList());
            mail.To.Add(toAddress);
            //mail.CC.Add(ConfigUtility.GetClosureConfig.Item1);

            if (ConfigUtility.IncludeBcc)
            {
                mail.Bcc.Add(string.Join(",", ConfigUtility.EmailBcc.Split(',').ToList().Select(x => x.ToEmail()).ToList()));
            }

            mail.Subject = ConfigUtility.GetWsCreateConfig.Item4.Replace("{{WorkspaceID}}", wsNum);

            StringBuilder sbBody = new StringBuilder(body.Body);

            sbBody.Replace("{{Manager}}", recipients.Where(x => x.Item1.Equals("Manager")).FirstOrDefault().Item2);
            sbBody.Replace("{{Partner}}", recipients.Where(x => x.Item1.Equals("Partner")).FirstOrDefault().Item2);
            sbBody.Replace("{{PartnerAssistance}}", recipients.Where(x => x.Item1.Equals("PartnerAssistance")).FirstOrDefault().Item2);
            sbBody.Replace("{{Creator}}", recipients.Where(x => x.Item1.Equals("Creator")).FirstOrDefault().Item2);
            sbBody.Replace("{{amUrl}}", ConfigUtility.GetRootUrl());

            //body.Body.Replace("{{Manager}}", recipients.Where(x => x.Item1.Equals("Manager")).FirstOrDefault().Item2);
            //body.Body.Replace("{{Partner}}", recipients.Where(x => x.Item1.Equals("Partner")).FirstOrDefault().Item2);
            //body.Body.Replace("{{Creator}}", recipients.Where(x => x.Item1.Equals("Creator")).FirstOrDefault().Item2);
            //body.Body.Replace("{{amUrl}}", ConfigUtility.GetRootUrl());

            //mail.Body = body.Body;
            mail.Body = sbBody.ToString();

            //mail.Body.Replace("{{Manager}}", recipients.Where(x => x.Item1.Equals("Manager")).FirstOrDefault().Item2);
            //mail.Body.Replace("{{Partner}}", recipients.Where(x => x.Item1.Equals("Partner")).FirstOrDefault().Item2);
            //mail.Body.Replace("{{Creator}}", recipients.Where(x => x.Item1.Equals("Creator")).FirstOrDefault().Item2);
            //mail.Body.Replace("{{amUrl}}", ConfigUtility.GetRootUrl());

            mail.IsBodyHtml = true;

            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(mail.Body, null, "text/html");

            htmlView.LinkedResources.Add(body.image001);
            htmlView.LinkedResources.Add(body.image002);
            htmlView.LinkedResources.Add(body.image003);

            mail.AlternateViews.Add(htmlView);

            //using (SmtpClient client = new SmtpClient())
            //{
            //    //client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //    //client.UseDefaultCredentials = false;
            //    //client.Host = ConfigUtility.GetSmptSrvr;

            //    client.Send(mail);
            //}

            SendMail(mail);

            return mail.Body;
        }

        public static string SendMail_RequestAccess(string emailTemplateName,
            string wsNum, string wsName, List<Tuple<string, string>> recipients, string accessRequested, string mailInfo)
        {
            var body = GetEMailBody(emailTemplateName, wsNum, wsName, recipients, mailInfo);

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(ConfigUtility.GetClosureConfig.Item2);

            var toAddress = string.Join(",", recipients.Select(x => x.Item2.ToEmail()).ToList());
            mail.To.Add(toAddress);
            mail.CC.Add(ConfigUtility.GetClosureConfig.Item1);

            if (ConfigUtility.IncludeBcc)
            {
                mail.Bcc.Add(string.Join(",", ConfigUtility.EmailBcc.Split(',').ToList().Select(x => x.ToEmail()).ToList()));
            }

            //mail.Subject = ConfigUtility.GetClosureConfig.Item3.Replace("{{WorkspaceName}}", wsName);

            mail.Subject = "Workspace Access Request to " + wsName;
            
            StringBuilder sbBody = new StringBuilder(body.Body);
            sbBody.Replace("{{Requestor}}", GetCurrentUser);
            sbBody.Replace("{{AccessRequested}}", accessRequested);
            sbBody.Replace("{{amUrl}}", ConfigUtility.GetRootUrl());
            sbBody.Replace("{{Comments}}", mailInfo);

            //mail.Body = body.Body;
            mail.Body = sbBody.ToString();
            mail.IsBodyHtml = true;

            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(mail.Body, null, "text/html");

            htmlView.LinkedResources.Add(body.image001);
            htmlView.LinkedResources.Add(body.image002);
            htmlView.LinkedResources.Add(body.image003);

            mail.AlternateViews.Add(htmlView);

            SendMail(mail);

            return mail.Body;
        }

        public static string SendMail_RequestAccessToAuditManager(string comment, bool toIncludeCurrentUser)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(ConfigUtility.GetAuditManagerRequestAccessConfig.Item3);

            var toAddress = string.Join(",", ConfigUtility.GetAuditManagerRequestAccessConfig.Item5.Split(',').ToList().Select(x => x.ToEmail()).ToList());
            mail.To.Add(toAddress);
            if (toIncludeCurrentUser)
                mail.CC.Add(string.Format("{0},{1}", ConfigUtility.GetAuditManagerRequestAccessConfig.Item1, AmUtil.GetCurrentUser.ToEmail()));
            else
                mail.CC.Add(ConfigUtility.GetAuditManagerRequestAccessConfig.Item1);

            if (ConfigUtility.IncludeBcc)
            {
                mail.Bcc.Add(string.Join(",", ConfigUtility.EmailBcc.Split(',').ToList().Select(x => x.ToEmail()).ToList()));
            }

            mail.Subject = ConfigUtility.GetAuditManagerRequestAccessConfig.Item2;
            mail.Priority = MailPriority.High;

            mail.Body = string.Format("<p>User  \"{0}\" needs access to AuditManager, and the {1} provided the following reason.</p> <blockquote>{2}</blockquote><p>Please notify this user by sending an e-mail to <a href='mailto:{3}@kpmg.com?subject=Access Granted&body=Hi {4}, %0D%0A%0D%0AYou have been granted access to Audit Manger. %0D%0A%0D%0ARegards,%0D%0AAudit Manager Team'>{5}</a> once access has been granted.</p>", AmUtil.GetCurrentUser, toIncludeCurrentUser ? "User" : "System", comment, AmUtil.GetCurrentUser, AmUtil.GetCurrentUser, AmUtil.GetCurrentUser, AmUtil.GetCurrentUser);
            mail.IsBodyHtml = true;

            SendMail(mail);

            return mail.Body;
        }

        public static string SendMail_Error(Exception e)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(ConfigUtility.GetAuditManagerRequestAccessConfig.Item3);

            var toAddress = string.Join(",", ConfigUtility.GetAuditManagerRequestAccessConfig.Item5.Split(',').ToList().Select(x => x.ToEmail()).ToList());
            mail.To.Add(toAddress);
            mail.CC.Add(ConfigUtility.GetAuditManagerRequestAccessConfig.Item1);

            if (ConfigUtility.IncludeBcc)
            {
                mail.Bcc.Add(string.Join(",", ConfigUtility.EmailBcc.Split(',').ToList().Select(x => x.ToEmail()).ToList()));
            }

            mail.Subject = e.Message;
            mail.Priority = MailPriority.High;

            mail.Body = e.StackTrace;
            mail.IsBodyHtml = true;

            SendMail(mail);

            return mail.Body;
        }

        public static void SendMail_Test()
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("viveksingh1@kpmg.com");

            mail.To.Add("viveksingh1@kpmg.com");
            
            mail.Subject = "Test Mail For John";
            //mail.Priority = MailPriority.High;

            mail.Body = "<html><head><title></title></head><body><p>John Test</p></body></html>";
            mail.IsBodyHtml = true;

            using (SmtpClient client = new SmtpClient())
            {
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Host = "smtpout.us.kworld.kpmg.com";
                try
                {
                    client.Send(mail);
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}
