using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Principal;
using System.Web.Http;

namespace AuditManager.Web.Api
{
    public class KImpersonate : IDisposable
    {
        //public const int LOGON32_LOGON_INTERACTIVE = 2;
        public const int LOGON32_LOGON_INTERACTIVE = 8;
        public const int LOGON32_PROVIDER_DEFAULT = 0;

        WindowsImpersonationContext impersonationContext;

        [DllImport("advapi32.dll")]
        public static extern int LogonUserA(String lpszUserName,
            String lpszDomain,
            String lpszPassword,
            int dwLogonType,
            int dwLogonProvider,
            ref IntPtr phToken);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int DuplicateToken(IntPtr hToken,
            int impersonationLevel,
            ref IntPtr hNewToken);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool RevertToSelf();

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern bool CloseHandle(IntPtr handle);

        private KImpersonate()
        {

        }

        public bool ImpersonateUser(String userName, String domain, String password)
        {
            WindowsIdentity tempWindowsIdentity;
            IntPtr token = IntPtr.Zero;
            IntPtr tokenDuplicate = IntPtr.Zero;

            if (RevertToSelf())
            {
                if (LogonUserA(userName, domain, password, LOGON32_LOGON_INTERACTIVE,
                    LOGON32_PROVIDER_DEFAULT, ref token) != 0)
                {
                    if (DuplicateToken(token, 2, ref tokenDuplicate) != 0)
                    {
                        tempWindowsIdentity = new WindowsIdentity(tokenDuplicate);
                        impersonationContext = tempWindowsIdentity.Impersonate();
                        if (impersonationContext != null)
                        {
                            CloseHandle(token);
                            CloseHandle(tokenDuplicate);
                            return true;
                        }
                        else
                        {
                            //Response.Write(Marshal.GetLastWin32Error());
                        }
                    }
                    else
                    {
                        //Response.Write(Marshal.GetLastWin32Error());
                    }
                }
                else
                {
                    //Response.Write(Marshal.GetLastWin32Error());
                }
            }
            else
            {
                //Response.Write(Marshal.GetLastWin32Error());
            }

            if (token != IntPtr.Zero)
                CloseHandle(token);
            else
            {
                //Response.Write(Marshal.GetLastWin32Error());
            }
            if (tokenDuplicate != IntPtr.Zero)
                CloseHandle(tokenDuplicate);
            else
            {
                //Response.Write(Marshal.GetLastWin32Error());
            }
            return false;
        }

        public void UndoImpersonation()
        {
            impersonationContext.Undo();
        }

        private static KImpersonate _KImpersonate = new KImpersonate();

        public static KImpersonate CreateInstance()
        {
            var obj = _KImpersonate ?? new KImpersonate();
            obj.ImpersonateUser("us-svcproddrms", "us.kworld.kpmg.com", "Interw0ven!");
            return obj;
        }

        public void Dispose()
        {
            UndoImpersonation();
        }
    }

    public class KImpersonate2 : IDisposable
    {
        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool LogonUser(
                String lpszUsername,
                String lpszDomain,
                String lpszPassword,
                int dwLogonType,
                int dwLogonProvider,
                ref IntPtr phToken);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public extern static bool CloseHandle(IntPtr handle);

        private static IntPtr tokenHandle = new IntPtr(0);
        private static WindowsImpersonationContext impersonatedUser;

        // If you incorporate this code into a DLL, be sure to demand that it
        // runs with FullTrust.
        [PermissionSetAttribute(SecurityAction.Demand, Name = "FullTrust")]
        public void Impersonate(string domainName, string userName, string password)
        {
            try
            {
                // Use the unmanaged LogonUser function to get the user token for
                // the specified user, domain, and password.
                const int LOGON32_PROVIDER_DEFAULT = 0;
                // Passing this parameter causes LogonUser to create a primary token.
                //const int LOGON32_LOGON_INTERACTIVE = 2;
                const int LOGON32_LOGON_INTERACTIVE = 8;
                tokenHandle = IntPtr.Zero;

                // ---- Step - 1 
                // Call LogonUser to obtain a handle to an access token.
                bool returnValue = LogonUser(
                                    userName,
                                    domainName,
                                    password,
                                    LOGON32_LOGON_INTERACTIVE,
                                    LOGON32_PROVIDER_DEFAULT,
                                    ref tokenHandle);         // tokenHandle - new security token

                if (false == returnValue)
                {
                    int ret = Marshal.GetLastWin32Error();
                    //Console.WriteLine("LogonUser call failed with error code : " + ret);
                    throw new System.ComponentModel.Win32Exception(ret);
                }

                // ---- Step - 2 
                WindowsIdentity newId = new WindowsIdentity(tokenHandle);

                // ---- Step - 3 
                impersonatedUser = newId.Impersonate();
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Exception occurred. " + ex.Message);
                throw ex;
            }
        }

        // Stops impersonation
        public void Undo()
        {
            impersonatedUser.Undo();
            // Free the tokens.
            if (tokenHandle != IntPtr.Zero)
                CloseHandle(tokenHandle);
        }

        public void Dispose()
        {
            Undo();
        }

        private KImpersonate2()
        {

        }

        private static KImpersonate2 _KImpersonate = new KImpersonate2();

        public static KImpersonate2 CreateInstance()
        {
            var obj = _KImpersonate ?? new KImpersonate2();
            obj.Impersonate("us.kworld.kpmg.com", "us-svcproddrms", "Interw0ven!");
            return obj;
        }
    }

    public class TestController : ApiController
    {
        [HttpGet]
        public string Test1()
        {
            var result = string.Empty;

            result = result + System.Environment.UserName;

            return result;
        }

        [HttpGet]
        public string Test2()
        {
            var result = string.Empty;

            result = result + System.Environment.UserName;

            using (KImpersonate2.CreateInstance())
            {
                result = result + "," + System.Environment.UserName;
            }

            result = result + "," + System.Environment.UserName;

            return result;
        }

        [HttpGet]
        public string Test3()
        {
            var result = string.Empty;

            result = result + System.Environment.UserName;

            using (KImpersonate.CreateInstance())
            {
                result = result + "," + System.Environment.UserName;
            }

            result = result + "," + System.Environment.UserName;

            return result;
        }
    }
}
