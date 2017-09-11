using Jiguang.JSMS.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Jiguang.JSMS
{
    public class JSMSClient
    {
        private const string BASE_URL = "https://api.sms.jpush.cn/v1/";

        private static string APP_KEY;
        private static string MASTER_SECRET;

        private HttpClient httpClient;

        public JSMSClient(string appKey, string masterSecret)
        {
            if (string.IsNullOrEmpty(appKey))
                throw new ArgumentNullException(nameof(appKey));

            if (string.IsNullOrEmpty(masterSecret))
                throw new ArgumentNullException(nameof(masterSecret));

            APP_KEY = appKey;
            MASTER_SECRET = masterSecret;

            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var auth = Convert.ToBase64String(Encoding.UTF8.GetBytes(appKey + ":" + masterSecret));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", auth);
        }

        /// <summary>
        /// 发送文本验证码短信。
        /// <see cref="https://docs.jiguang.cn/jsms/server/rest_api_jsms/#api_1"/>
        /// </summary>
        /// <param name="mobile">手机号码</param>
        /// <param name="tempId">模板 Id</param>
        /// <returns></returns>
        public async Task<HttpResponse> SendCode(string mobile, int tempId)
        {
            if (string.IsNullOrEmpty(mobile))
                throw new ArgumentNullException(nameof(mobile));

            JObject json = new JObject
            {
                { "mobile", mobile },
                { "temp_id", tempId }
            };
            HttpContent httpContent = new StringContent(json.ToString(), Encoding.UTF8);
            string url = BASE_URL + "codes";
            HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(url, httpContent).ConfigureAwait(false);
            string httpResponseContent = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            return new HttpResponse(httpResponseMessage.StatusCode, httpResponseMessage.Headers, httpResponseContent);
        }

        public async Task<HttpResponse> SendVoiceCode(string jsonBody)
        {
            if (string.IsNullOrEmpty(jsonBody))
                throw new ArgumentNullException(nameof(jsonBody));

            HttpContent httpContent = new StringContent(jsonBody, Encoding.UTF8);
            string url = BASE_URL + "voice_codes";
            HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(url, httpContent).ConfigureAwait(false);
            string httpResponseContent = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            return new HttpResponse(httpResponseMessage.StatusCode, httpResponseMessage.Headers, httpResponseContent);
        }

        /// <summary>
        /// 发送语音验证码。
        /// <see cref="https://docs.jiguang.cn/jsms/server/rest_api_jsms/#api_2"/>
        /// </summary>
        /// <param name="voiceCode">语音验证码对象。</param>
        public async Task<HttpResponse> SendVoiceCode(VoiceCode voiceCode)
        {
            if (voiceCode == null)
                throw new ArgumentNullException(nameof(voiceCode));

            string body = voiceCode.ToString();
            return await SendVoiceCode(body).ConfigureAwait(false);
        }

        /// <summary>
        /// 判断验证码是否有效。
        /// <see cref="https://docs.jiguang.cn/jsms/server/rest_api_jsms/#api_3"/>
        /// </summary>
        /// <param name="msgId">调用发送验证码 API 的返回值。</param>
        /// <param name="code">验证码。</param>
        public async Task<HttpResponse> IsCodeValid(string msgId, string code)
        {
            if (string.IsNullOrEmpty(msgId))
                throw new ArgumentNullException(nameof(msgId));

            if (string.IsNullOrEmpty(code))
                throw new ArgumentNullException(nameof(code));

            JObject json = new JObject
            {
                { "code", code }
            };
            HttpContent httpContent = new StringContent(json.ToString(), Encoding.UTF8);
            string url = BASE_URL + "codes/" + msgId + "/valid";
            HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(url, httpContent).ConfigureAwait(false);
            string httpResponseContent = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            return new HttpResponse(httpResponseMessage.StatusCode, httpResponseMessage.Headers, httpResponseContent);
        }

        /// <summary>
        /// 发送模板短信。
        /// <see cref="https://docs.jiguang.cn/jsms/server/rest_api_jsms/#api_4"/>
        /// </summary>
        /// <param name="message">模板短信。</param>
        public async Task<HttpResponse> SendTemplateMessage(TemplateMessage message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            HttpContent httpContent = new StringContent(message.ToString(), Encoding.UTF8);
            string url = BASE_URL + "messages";
            HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(url, httpContent).ConfigureAwait(false);
            string httpResponseContent = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            return new HttpResponse(httpResponseMessage.StatusCode, httpResponseMessage.Headers, httpResponseContent);
        }

        /// <summary>
        /// 批量发送模板短信。模板 Id 必须一致。
        /// <see cref="https://docs.jiguang.cn/jsms/server/rest_api_jsms/#api_5"/>
        /// </summary>
        /// <param name="templateMessageList">模板短信对象列表。</param>
        public async Task<HttpResponse> SendTemplateMessage(List<TemplateMessage> templateMessageList)
        {
            if (templateMessageList == null || templateMessageList.Count == 0)
                throw new ArgumentException(nameof(templateMessageList));

            int tempId = templateMessageList[0].TemplateId;

            JArray recipients = new JArray();
            foreach (TemplateMessage msg in templateMessageList)
            {
                JObject item = new JObject
                {
                    { "mobile", msg.Mobile }
                };

                if (msg.TemplateParameters != null && msg.TemplateParameters.Count != 0)
                    item.Add(JsonConvert.SerializeObject(msg.TemplateParameters));

                recipients.Add(item);
            }

            JObject json = new JObject
            {
                { "temp_id", tempId },
                { "recipients", recipients }
            };

            HttpContent httpContent = new StringContent(json.ToString(), Encoding.UTF8);
            string url = BASE_URL + "messages/batch";
            HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(url, httpContent).ConfigureAwait(false);
            string httpResponseContent = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            return new HttpResponse(httpResponseMessage.StatusCode, httpResponseMessage.Headers, httpResponseContent);
        }
    }
}
