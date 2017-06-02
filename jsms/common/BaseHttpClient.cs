using System;
using System.Text;
using System.Net;
using System.IO;
using System.Diagnostics;

namespace jsms.common
{
    public class BaseHttpClient
    {
        private const String CHARSET = "UTF-8";
        private const String RATE_LIMIT_QUOTA = "X-Rate-Limit-Limit";
        private const String RATE_LIMIT_Remaining = "X-Rate-Limit-Remaining";
        private const String RATE_LIMIT_Reset = "X-Rate-Limit-Reset";

        protected const int RESPONSE_OK = 200;

        //连接超时时间
        private const int DEFAULT_CONNECTION_TIMEOUT = (20 * 1000); // 20s

        //读取超时时间
        private const int DEFAULT_SOCKET_TIMEOUT = (30 * 1000);     // 30s

        public ResponseWrapper SendPost(String url, String auth, String reqParams)
        {
            return SendRequest("POST", url, auth, reqParams);
        }

        public ResponseWrapper SendDelete(String url, String auth, String reqParams)
        {
            return SendRequest("DELETE", url, auth, reqParams);
        }

        public ResponseWrapper SendGet(String url, String auth, String reqParams)
        {
            return SendRequest("GET", url, auth, reqParams);
        }

        public ResponseWrapper SendPut(String url, String auth, String reqParams)
        {
            return SendRequest("PUT", url, auth, reqParams);
        }

        /*
         *
         * method "POST" or "GET"
         * url
         * auth   可选
         */
        public ResponseWrapper SendRequest(String method, String url, String auth, String reqParams)
        {
            if (null != reqParams)
            {
                Console.WriteLine("Request Content - " + reqParams + " " + DateTime.Now);
            }

            ResponseWrapper result = new ResponseWrapper();
            HttpWebRequest myReq = null;
            HttpWebResponse response = null;

            try
            {
                // 利用工厂机制（factory mechanism）通过Create()方法来创建
                myReq = (HttpWebRequest)WebRequest.Create(url);

                //request类型
                myReq.Method = method;
                myReq.ContentType = "application/json";

                if (!String.IsNullOrEmpty(auth))
                {
                    myReq.Headers.Add("Authorization", "Basic " + auth);
                }

                if (method.Equals("POST") || method.Equals("PUT"))
                {
                    byte[] bs = Encoding.UTF8.GetBytes(reqParams);
                    myReq.ContentLength = bs.Length;
                    using (Stream reqStream = myReq.GetRequestStream())
                    {
                        reqStream.Write(bs, 0, bs.Length);
                        reqStream.Close();
                    }
                }

                response = (HttpWebResponse)myReq.GetResponse();
                result.responseCode = response.StatusCode;

                Console.WriteLine("Succeed to get response - 200 OK" + " " + DateTime.Now);
                Console.WriteLine("Response Content - {0}", result.ResponseContent + " " + DateTime.Now);

                if (Equals(response.StatusCode, HttpStatusCode.OK))
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        result.ResponseContent = reader.ReadToEnd();
                    }
                    String limitQuota = response.GetResponseHeader(RATE_LIMIT_QUOTA);
                    String limitRemaining = response.GetResponseHeader(RATE_LIMIT_Remaining);
                    String limitReset = response.GetResponseHeader(RATE_LIMIT_Reset);
                    Console.WriteLine("Succeed to get response - 200 OK" + " " + DateTime.Now);
                    Console.WriteLine("Response Content - {0}", result.ResponseContent + " " + DateTime.Now);
                }
                else
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        result.ResponseContent = reader.ReadToEnd();
                    }
                }
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    HttpStatusCode errorCode = ((HttpWebResponse)e.Response).StatusCode;
                    string statusDescription = ((HttpWebResponse)e.Response).StatusDescription;
                    using (StreamReader sr = new StreamReader(((HttpWebResponse)e.Response).GetResponseStream(), Encoding.UTF8))
                    {
                        result.ResponseContent = sr.ReadToEnd();
                    }
                    result.responseCode = errorCode;
                    result.exceptionString = e.Message;
                    String limitQuota = ((HttpWebResponse)e.Response).GetResponseHeader(RATE_LIMIT_QUOTA);
                    String limitRemaining = ((HttpWebResponse)e.Response).GetResponseHeader(RATE_LIMIT_Remaining);
                    String limitReset = ((HttpWebResponse)e.Response).GetResponseHeader(RATE_LIMIT_Reset);
                    Debug.Print(e.Message);
                    result.SetErrorObject();
                    Console.WriteLine(string.Format("fail to get response - {0}", errorCode) + " " + DateTime.Now);
                    Console.WriteLine(string.Format("Response Content - {0}", result.ResponseContent) + " " + DateTime.Now);
                }
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
                if (myReq != null)
                {
                    myReq.Abort();
                }
            }
            return result;
        }
    }
}
