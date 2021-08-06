using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using BrightSign.Core.Models;
using NtlmHttpHandler;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Analytics;
using Newtonsoft.Json;

namespace BrightSign.Core.Utility.Web
{
    public class HttpBase
    {
        static HttpBase instance;
        public static HttpBase Instance => instance ?? (instance = new HttpBase());
        public string ContentType { get; set; } = Constants.CONTENT_TYPE_APPLICATIONJSON;
        string GetIDAPI = "GetID";
        string GetRemoteDataAPI = "GetRemoteData";
        string GetSnapshotConfigurationAPI = "GetSnapshotConfiguration";
        string SetSnapshotConfigurationAPI = "SetSnapshotConfiguration";


        public async Task<Tuple<bool, string>> GetDeviceRemoteData(string ipAddress, string port)
        {
            bool IsConnected = false;
            string Data = "";
            string url = string.Format("http://{0}:{1}/{2}", ipAddress, port, GetRemoteDataAPI);

            using (HttpServiceClient httpClient = new HttpServiceClient())
            {
                try
                {
                    httpClient.Timeout = new TimeSpan(0, 0, 0, 0, 5000);
                    var response = await httpClient.GetAsync(url);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        IsConnected = true;
                        string strChk = await response.Content.ReadAsStringAsync();
                        Debug.WriteLine("XML Response" + strChk);
                        Data = strChk;
                    }
                    else
                    {
                        IsConnected = false;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    IsConnected = false;
                }

            };

            return new Tuple<bool, string>(IsConnected, Data);

        }


        public async Task<Tuple<bool, string>> GetDeviceStatus(string ipAddress, string port)
        {
            bool IsConnected = false;
            string Data = "";

            string url = string.Format("http://{0}:{1}/{2}", ipAddress, port, GetIDAPI);

            using (HttpServiceClient httpClient = new HttpServiceClient())
            {
                try
                {
                    httpClient.Timeout = new TimeSpan(0, 0, 0, 0, 5000);

                    var response = await httpClient.GetAsync(url);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        IsConnected = true;
                        string strChk = await response.Content.ReadAsStringAsync();
                        Debug.WriteLine("XML Response" + strChk);
                        Data = strChk;
                    }
                    else
                    {
                        IsConnected = false;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    IsConnected = false;
                }

            };

            return new Tuple<bool, string>(IsConnected, Data);

        }


        public async Task<Tuple<bool, string>> GetSnapshotConfiguration(string ipAddress, string port)
        {
            bool IsSuccess = false;
            string Data = "";

            string url = String.Empty;

			url = string.Format("http://{0}:{1}/{2}", ipAddress, port, GetSnapshotConfigurationAPI);

            //if (Constants.IsCredentialsRequiredforSnapshots)
            //{
            //    url = string.Format("http://{0}:{1}@{2}:{3}/{4}", Constants.LoginUser, Constants.LoginPwd, ipAddress, port, GetSnapshotConfigurationAPI);

            //}
            //else
            //{
            //    url = string.Format("http://{0}:{1}/{2}", ipAddress, port, GetSnapshotConfigurationAPI);

            //}

            using (HttpServiceClient httpClient = new HttpServiceClient())
            {
                try
                {
                    httpClient.Timeout = new TimeSpan(0, 0, 0, 0, 5000);
                    var response = await httpClient.GetAsync(url);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        IsSuccess = true;
                        string strChk = await response.Content.ReadAsStringAsync();
                        Debug.WriteLine("XML Response" + strChk);
                        Data = strChk;
                    }
                    else
                    {
                        IsSuccess = false;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    IsSuccess = false;
                }

            };

            return new Tuple<bool, string>(IsSuccess, Data);

        }

        public async Task<Tuple<bool, string>> DownloadLog(string url)
        {
            bool IsSuccess = false;
            string byteData = null;

            using (HttpServiceClient httpClient = new HttpServiceClient())
            {
                try
                {
                    byteData = await httpClient.GetStringAsync(url);
                    if (byteData != null)
                    {
                        IsSuccess = true;
                    }
                    else
                    {
                        IsSuccess = false;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    IsSuccess = false;
                }

            };

            return new Tuple<bool, string>(IsSuccess, byteData);

        }

        internal async Task<bool> IsAuthorized(string ipAddress, string username = null, string pwd = null, string portnumber = null)
        {
            bool IsAuthorizedUser = false;

            string url = string.Empty;

            if (!string.IsNullOrEmpty(username))
            {
                url = string.Format("http://{0}:{1}@{2}/index.html", username, pwd, ipAddress);
            }
            else
            {
                if (string.IsNullOrEmpty(portnumber))
                {
                    url = string.Format("http://{0}", ipAddress);

                }
                else
                {
                    url = string.Format("http://{0}:{1}", ipAddress, portnumber);

                }
            }

            using (HttpServiceClient httpClient = new HttpServiceClient())
            {
                try
                {

                    var response = await httpClient.GetAsync(url);
                    // DiagnosticsUnauthorized
                    Debug.WriteLine("Response got for isAuthorized " + response.StatusCode);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        IsAuthorizedUser = true;
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        IsAuthorizedUser = false;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    IsAuthorizedUser = false;
                }

            };

            return IsAuthorizedUser;
        }



        public async Task<int> Login(string username, string password, string portnumber = null)
        {
            NetworkCredential credential = new NetworkCredential(username, password);// new NetworkCredential(username, password);
            //new NetworkCredential()
            //new NetworkCredential(username,password,"brightsign")
            

            var credcache = new CredentialCache();

            if (string.IsNullOrEmpty(portnumber))
            {
                credcache.Add(new Uri(string.Format("http://{0}/index.html", Constants.ActiveDevice.IpAddress)), "Digest", credential);

            }
            else
            {
                credcache.Add(new Uri(string.Format("http://{0}:8008", Constants.ActiveDevice.IpAddress)), "Digest", credential);

            }

            using (var client = new HttpClient(new HttpClientHandler { Credentials = credcache }))
            {
                HttpResponseMessage response = null;

                if (string.IsNullOrEmpty(portnumber))
                {
                    response = await client.GetAsync(new Uri(string.Format("http://{0}/index.html", Constants.ActiveDevice.IpAddress)));

                }
                else
                {
                    response = await client.GetAsync(new Uri(string.Format("http://{0}:8008", Constants.ActiveDevice.IpAddress)));

                }
                

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Was able to connect to Diagnostics");
                    Debug.WriteLine("Request message code " + response.RequestMessage.ToString());
                    Debug.WriteLine("REsponse message " + response.ToString());
                     
                    return 1;
                }

                
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Debug.WriteLine("Failed authentication in the URL");
                    Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                    keyValuePairs.Add("username", username);
                    keyValuePairs.Add("password", password);
                    keyValuePairs.Add("portnumber", portnumber);
                    Analytics.TrackEvent("Unauthorized Event", keyValuePairs);

                    Debug.WriteLine("Request message code " + response.RequestMessage.ToString());
                    Debug.WriteLine("REsponse message " + response.ToString());
                    return 0;
                }
                else
                {
                    Debug.WriteLine("Failed unknown status in the URL");
                    return 2;
                }
            }


        }

        public async Task<bool> SaveSnapshotsConfiguration(SnapshotConfigModel snapshot)
        {
            bool IsSuccess = false;


            //NetworkCredential credential = new NetworkCredential(Constants.LoginPwd, Constants.LoginPwd);

            //var credcache = new CredentialCache();


            string url = string.Format("http://{0}:{1}/{2}", Constants.ActiveDevice.IpAddress, Constants.httpPort, SetSnapshotConfigurationAPI);
            HttpClientHandler httpHandler;

            if (Constants.IsCredentialsRequiredforSnapshots)
            {
                const string scheme = Constants.AuthorizationType;

                NetworkCredential credentials = new NetworkCredential(Constants.LoginUser, Constants.LoginPwd);

                CredentialCache credentialCache = new CredentialCache { { Constants.ActiveDevice.IpAddress, Constants.httpPort, scheme, credentials } };

                //httpHandler = new HttpClientHandler()
                //{
                //    CookieContainer = new CookieContainer(),
                //    Credentials = credentialCache.GetCredential(Constants.ActiveDevice.IpAddress, Constants.httpPort, scheme)
                //};

                httpHandler = NtlmHttpHandlerFactory.Create();
                httpHandler.Credentials = credentialCache.GetCredential(Constants.ActiveDevice.IpAddress, Constants.httpPort, Constants.AuthorizationType);

            }
            else{
                httpHandler = new HttpClientHandler();
            }

            //using (HttpClient httpClient = new HttpClient(new HttpClientHandler { Credentials = credcache }))
            using (HttpClient httpClient = new HttpClient(httpHandler,false))
            {
                try
                {
                    Dictionary<string, string> postparams = new Dictionary<string, string>();
                    postparams.Add("enableRemoteSnapshot", snapshot.Enabled ? "yes" : "no");
                    postparams.Add("remoteSnapshotBsnTimeout", "5");
                    postparams.Add("remoteSnapshotDisplayPortrait", snapshot.DisplayPortraitMode.ToString());
                    postparams.Add("remoteSnapshotInterval", (snapshot.Interval * 60).ToString());
                    postparams.Add("remoteSnapshotJpegQualityLevel", snapshot.Quality.ToString());
                    postparams.Add("remoteSnapshotMaxImages", snapshot.MaxImages.ToString());

                    HttpContent content = new FormUrlEncodedContent(postparams);
                    var response = await httpClient.PostAsync(url, content);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        IsSuccess = false;
                        string strChk = await response.Content.ReadAsStringAsync();
                        Debug.WriteLine("XML Response" + strChk);
                        if (strChk == "OK")
                        {
                            IsSuccess = true;
                        }
                    }
                    else
                    {
                        IsSuccess = false;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    IsSuccess = false;
                }

            };


            return IsSuccess;
        }

        public async Task<Tuple<bool, Stream>> DownloadFile(string url, string password)
        {
            bool IsSuccess = false;
            Stream fileStream = null;

            Uri uri = new Uri(url);

            if (!string.IsNullOrEmpty(password))//(url.ToLower().Contains("admin"))
            {
                NetworkCredential credential = new NetworkCredential("admin", password);

                var credcache = new CredentialCache();
                credcache.Add(new Uri(string.Format(url)), "Digest", credential);

                try
                {
                    using (var client = new HttpClient(new HttpClientHandler { Credentials = credcache }))
                    {
                        var response = await client.GetByteArrayAsync(new Uri(string.Format(url)));
                        Stream stream = new MemoryStream(response);
                        return new Tuple<bool, Stream>(true, stream);
                    }
                }
                catch (Exception ex)
                {
                    return new Tuple<bool, Stream>(false, null);
                }
            }
            else
            {
                using (HttpServiceClient httpClient = new HttpServiceClient())
                {

                    var response = await httpClient.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        IsSuccess = true;
                        fileStream = await response.Content.ReadAsStreamAsync();
                    }
                }
            }
            return new Tuple<bool, Stream>(IsSuccess, fileStream);

        }
    }
}
