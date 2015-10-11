using AuditManager.Common;
using AuditManager.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AuditManager.Web.Common
{
    public class ExtApi
    {
        private const string LogController = "KWSLog/";
        private const string CreateWSController = "Create/";
        private const string CommonController = "Common/";

        private static string GetUrl(ExtApiType extApiType)
        {
            switch (extApiType)
            {
                case ExtApiType.Prj:
                    return ConfigUtility.GetPrjNDcs_url_Prj;
                case ExtApiType.Dcs:
                    return ConfigUtility.GetPrjNDcs_url_Dcs;
                case ExtApiType.DcsAll:
                    return ConfigUtility.GetPrjNDcs_url_Dcs_All;
                case ExtApiType.LauchSite_GetFoldersRollforward:
                    return ConfigUtility.GetLauchSite_url_GetFoldersRollforward;
            }

            return null;
        }

        public static dynamic GetResponse(ExtApiType prjDcsType, List<string> param)
        {
            //Do you have the security mode set to Transport in your config? 
            using (var client = new HttpClient())
            {
                dynamic result = null;

                if (prjDcsType == ExtApiType.LauchSite_GetFoldersRollforward)
                    client.BaseAddress = new Uri(ConfigUtility.GetLauchSite_baseUri);
                else
                    client.BaseAddress = new Uri(ConfigUtility.GetPrjNDcs_baseUri);

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var strUrl = string.Format("{0}{1}", GetUrl(prjDcsType), string.Join("/", param));

                HttpResponseMessage response = client.GetAsync(strUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<dynamic>().Result;
                }

                if (prjDcsType == ExtApiType.Dcs && result == null)
                    result = GetResponse(ExtApiType.DcsAll, new List<string> { });

                return result;
            }
        }

        public static void CreateWS(WsCreate wsCreate)
        {
            string postData = string.Format("EngNum={0}&EngDescription={1}&ClientId={2}&ManagerEmpId={3}&PartnerEmpId={4}&PartnerAssistanceEmpId={5}&KPMGOnly={6}&IsAuto={7}",
                wsCreate.EngagementNumber, wsCreate.EngagementDescription, wsCreate.ClientId, wsCreate.ManagerEmpId, wsCreate.PartnerEmpId, wsCreate.PartnerAssistanceEmpId, true, false);

            string url = string.Format("{0}{1}CreateWS", ConfigUtility.GetKWebAppBaseUrl, CreateWSController);

            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                wc.UseDefaultCredentials = true;

                string htmlResult = wc.UploadString(url, postData);

                var result = JsonConvert.DeserializeObject<dynamic>(htmlResult);

                if (!result.SelectToken("Success").Value)
                    throw new Exception(result.SelectToken("FailureMsg").Value);
            }
        }

        public static void ProcessKPMGOnly()
        {
            var kPMGOnlyForEng = AuditManager.Rep.AuditManagerDb.GetKPMGOnlyForAllEng();

            kPMGOnlyForEng.ForEach(x => UpdateWS(x.Key, x.Value));
        }

        public static void UpdateWS(string num, bool kPMGOnly)
        {
            string postData = string.Format("num={0}&kPMGOnly={1}", num, kPMGOnly);
            string url = string.Format("{0}{1}UpdateWS?{2}", ConfigUtility.GetKWebAppBaseUrl, CreateWSController, postData);

            PostRequest(url);
        }

        public static bool GetKPMGOnly(string num)
        {
            string postData = string.Format("num={0}", num);
            string url = string.Format("{0}{1}GetKPMGOnly?{2}", ConfigUtility.GetKWebAppBaseUrl, CommonController, postData);

            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                wc.UseDefaultCredentials = true;

                string htmlResult = wc.DownloadString(url);

                return bool.Parse(htmlResult);
            }
        }

        public static JArray GetSurveyInfo(string engagementNumber, double fileNumber)
        {
            string postData = string.Format("engagementNumber={0}&fileNumber={1}", engagementNumber, fileNumber);
            string url = string.Format("{0}{1}GetSurveyInfo?{2}", ConfigUtility.GetKWebAppBaseUrl, CommonController, postData);

            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                wc.UseDefaultCredentials = true;

                string htmlResult = wc.DownloadString(url);

                return JArray.Parse(htmlResult);
            }
        }

        public static void RequestUpdateCMS(string num, string comment)
        {
            string postData = string.Format("num={0}&comment={1}", num, comment);
            string url = string.Format("{0}{1}RequestUpdateCMS?{2}", ConfigUtility.GetKWebAppBaseUrl, LogController, postData);

            PostRequest(url);
        }

        public static void KWSLogAddDeleteUser(string num, string groupName, string userId, string kWSLogType)
        {
            string postData = string.Format("num={0}&groupName={1}&userId={2}&kWSLogType={3}", num, groupName, userId, kWSLogType);
            string url = string.Format("{0}{1}KWSLogAddDeleteUser?{2}", ConfigUtility.GetKWebAppBaseUrl, LogController, postData);

            PostRequest(url);
        }

        public static void KWSLogKFileActivity(WsSurveyModel wsSurveyModel)
        {
            var info = new
            {
                EAuditYear = wsSurveyModel.ENGYear,
                Workbooks = wsSurveyModel.WorkBooks,
                ClientYearEnd = wsSurveyModel.ClientYearEndDate == null ? null : wsSurveyModel.ClientYearEndDate.GetValueOrDefault().ToShortDateString(),
                BusinessUnit = wsSurveyModel.BusinessUnit,
                EAuditWorkFlow = wsSurveyModel.eAuditWorkflow,
                ProjectCode = wsSurveyModel.ProjectCode,
                DCSServer = wsSurveyModel.DCSServer,
                IsEBP = wsSurveyModel.EBPEngagement.ToBool<char>(),
                IsToMultiMAF = wsSurveyModel.SplitMAF.ToBool<char>(),
                IsToSingleMAF = wsSurveyModel.CombineMAF.ToBool<char>(),
                IsToDiffWorkFlow = wsSurveyModel.IsRFInDiffWF.ToBool<char>(),
                IsPartialRF = wsSurveyModel.IsPartilaRF.ToBool<char>(),
                RFModificationType = wsSurveyModel.RFModificationType,
                IsSAW = wsSurveyModel.IsSawEng,
                MAFName = wsSurveyModel.EngFileName,
                NoOfMAF = wsSurveyModel.NumberOfMAFs,
                PrimaryWBName = wsSurveyModel.PrimaryWBName,
                InstructionForRF = wsSurveyModel.GroupOrMultiInfo,
                RequiredDate = wsSurveyModel.RequiredDate == null ? null : wsSurveyModel.RequiredDate.GetValueOrDefault().ToShortDateString(),
            };

            var serInfo = JsonConvert.SerializeObject(info);

            //string postData = string.Format("Id=0&EngNum={0}&FileNum={1}&KPMGOnly={2}&IsUnderPreservation={3}&KFileActivityType={4}&Info={5}",
            //    wsSurveyModel.EngagementNumber, wsSurveyModel.DRMSFileNumber, wsSurveyModel.KPMGOnly.ToBool<char>(), 
            //    wsSurveyModel.Preservation.ToBool<char>(), string.Format("Request{0}", wsSurveyModel.SurveyRequestType), serInfo);

            //var postData = new { Id = 0, EngNum = wsSurveyModel.EngagementNumber, FileNum = wsSurveyModel.DRMSFileNumber, KPMGOnly = wsSurveyModel.KPMGOnly.ToBool<char>(),
            //                     IsUnderPreservation = wsSurveyModel.Preservation.ToBool<char>(),
            //                     KFileActivityType = string.Format("Request{0}", wsSurveyModel.SurveyRequestType),
            //                     Info = serInfo
            //};

            //string url = string.Format("{0}{1}KWSLogKFileActivity?{2}", ConfigUtility.GetKWebAppBaseUrl, LogController, postData);

            //PostRequest(url);


            using (var client = new WebClient())
            {
                client.UseDefaultCredentials = true;

                var values = new NameValueCollection
                {
                    //{ "Id", "0" },
                    { "EngNum", wsSurveyModel.EngagementNumber },
                    { "FileNum", wsSurveyModel.DRMSFileNumber.ToString() },
                    { "KPMGOnly", wsSurveyModel.KPMGOnly.ToBool<char>().ToString() },
                    { "IsUnderPreservation", wsSurveyModel.Preservation.ToBool<char>().ToString() },
                    { "KFileActivityType", string.Format("Request{0}", wsSurveyModel.SurveyRequestType) },
                    { "Info", serInfo },
                };
                var result = client.UploadValues(string.Format("{0}{1}KWSLogKFileActivity", ConfigUtility.GetKWebAppBaseUrl, LogController), values);
                // TODO: do something with the results returned by the controller action
            }
        }

        public static void KWSLogKFileActivity(FileActivity_UpdateModel activityUpdateModel)
        {
            var genericInfo = new 
            {
                FileType = activityUpdateModel.WsFileType.ToString(),
                FileIn = activityUpdateModel.FileIn.ToString(),
                FileUniqueId = activityUpdateModel.FileUniqueId,
                NonAuditFlag = activityUpdateModel.NonAuditFlag,
                IsPage_Act = activityUpdateModel.IsPage_Act,
            };

            var info = new
            {
                GenericInfo = JsonConvert.SerializeObject(genericInfo),
            };

            var serInfo = JsonConvert.SerializeObject(info);

            using (var client = new WebClient())
            {
                client.UseDefaultCredentials = true;

                var values = new NameValueCollection
                {
                    { "EngNum", activityUpdateModel.EngNum },
                    { "FileNum", activityUpdateModel.FileNum.ToString() },
                    { "KFileActivityType", activityUpdateModel.WsActivityType.ToString().Replace("Activity_", "") },
                    { "Comment", activityUpdateModel.Comment },
                    { "Info", serInfo },
                };
                var result = client.UploadValues(string.Format("{0}{1}KWSLogKFileActivity", ConfigUtility.GetKWebAppBaseUrl, LogController), values);
                // TODO: do something with the results returned by the controller action
            }
        }

        private static string GetResponseAsString(HttpWebRequest httpWReq)
        {
            var response = (HttpWebResponse)httpWReq.GetResponse();

            return new StreamReader(response.GetResponseStream()).ReadToEnd();
        }

        private static void PostData(HttpWebRequest httpWReq, string postData)
        {
            var encoding = new ASCIIEncoding();

            byte[] data = encoding.GetBytes(postData);

            httpWReq.ContentLength = data.Length;

            using (Stream stream = httpWReq.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
        }

        private static void PostRequest(string url)
        {
            Task task = new Task(() =>
            {
                var httpWReq = (HttpWebRequest)WebRequest.Create(url);
                httpWReq.UseDefaultCredentials = true;
                httpWReq.Method = "POST";
                httpWReq.ContentType = "application/x-www-form-urlencoded";
                string postData = "";
                PostData(httpWReq, postData);
                string response = GetResponseAsString(httpWReq);

            }
                //,CancellationToken.None
                //,TaskCreationOptions.None
           );

            task.Start(TaskScheduler.FromCurrentSynchronizationContext());
        }

        public static void Test()
        {
            //ExtApi.GetKPMGOnly("7777777775");
            //string serialisedData = JsonConvert.SerializeObject(new { num = 7777777775, kPMGOnly = false });

            //using (HttpClient hc = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true }))
            //{
            //    hc.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    hc.BaseAddress = new Uri(ConfigUtility.GetKWebAppBaseUrl);

            //    //string content = string.Format("num={0}&groupName={1}&userId={2}&kWSLogType={3}", num, groupName, userId, kWSLogType);
            //    //StringContent queryString = new StringContent(content);

            //    MediaTypeFormatter jsonFormatter = new JsonMediaTypeFormatter();
            //    HttpContent datas = new ObjectContent<dynamic>(new
            //    {
            //        num = "7777777775",
            //        kPMGOnly = false
            //    }, jsonFormatter);



            //    var r = hc.PostAsync(string.Format("{0}UpdateWS", CreateWSController), datas).Result;
            //}

            //using (WebClient wc = new WebClient())
            //{
            //    wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            //    wc.UseDefaultCredentials = true;

            //    string HtmlResult = wc.UploadString("http://localhost:58578/api/Create/UpdateWS", "POST", "num=7777777775&kPMGOnly=false");
            //}

            //using (WebClient wc = new WebClient())
            //{
            //    wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            //    wc.UseDefaultCredentials = true;
            //    string HtmlResult = wc.UploadString("http://USEOMAPD1852/api/Create/CreateWS", "EngNum=7777777775&EngDescription=TestEng&ClientId=1000500347&ManagerEmpId=1893323&PartnerEmpId=2181468&PartnerAssistanceEmpId=2784462&KPMGOnly=true");
            //}


            //string url = "http://myserver/method";
            //string content = string.Format("num={0}&groupName={1}&userId={2}&kWSLogType={3}", num, groupName, userId, kWSLogType);
            //StringContent queryString = new StringContent(content);
            //HttpClientHandler handler = new HttpClientHandler();
            //HttpClient httpClient = new HttpClient(handler);
            //HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
            //HttpResponseMessage response = await httpClient.PostAsync(request, queryString);

            //using (var client = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true }))
            //{
            //    client.BaseAddress = new Uri(ConfigUtility.GetKWebAppBaseUrl);

            //    client.DefaultRequestHeaders.Accept.Clear();

            //    //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //    string content = string.Format("num={0}&groupName={1}&userId={2}&kWSLogType={3}", num, groupName, userId, kWSLogType);
            //    StringContent queryString = new StringContent(content);

            //    var response = client.PostAsync("KWSLogAddDeleteUser", queryString);
            //    //var response = client.GetAsync("GetTest");
            //    if (response.Result.IsSuccessStatusCode)
            //    {
            //        // Get the URI of the created resource.
            //        Uri gizmoUrl = response.Result.Headers.Location;
            //    }
            //}
        }
    }
}