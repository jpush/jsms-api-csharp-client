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
        public async Task<HttpResponse> SendCodeAsync(string mobile, int tempId)
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

        /// <summary>
        /// 发送文本验证码短信。
        /// <see cref="https://docs.jiguang.cn/jsms/server/rest_api_jsms/#api_1"/>
        /// </summary>
        /// <param name="mobile">手机号码</param>
        /// <param name="tempId">模板 Id</param>
        public HttpResponse SendCode(string mobile, int tempId)
        {
            Task<HttpResponse> task = Task.Run(() => SendCodeAsync(mobile, tempId));
            task.Wait();
            return task.Result;
        }

        public async Task<HttpResponse> SendVoiceCodeAsync(string jsonBody)
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
        public async Task<HttpResponse> SendVoiceCodeAsync(VoiceCode voiceCode)
        {
            if (voiceCode == null)
                throw new ArgumentNullException(nameof(voiceCode));

            string body = voiceCode.ToString();
            return await SendVoiceCodeAsync(body).ConfigureAwait(false);
        }

        /// <summary>
        /// 发送语音验证码。
        /// <see cref="https://docs.jiguang.cn/jsms/server/rest_api_jsms/#api_2"/>
        /// </summary>
        /// <param name="voiceCode">语音验证码对象。</param>
        public HttpResponse SendVoiceCode(VoiceCode voiceCode)
        {
            Task<HttpResponse> task = Task.Run(() => SendVoiceCodeAsync(voiceCode));
            task.Wait();
            return task.Result;
        }

        /// <summary>
        /// 判断验证码是否有效。
        /// <see cref="https://docs.jiguang.cn/jsms/server/rest_api_jsms/#api_3"/>
        /// </summary>
        /// <param name="msgId">调用发送验证码 API 的返回值。</param>
        /// <param name="code">验证码。</param>
        public async Task<HttpResponse> IsCodeValidAsync(string msgId, string code)
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
        /// 判断验证码是否有效。
        /// <see cref="https://docs.jiguang.cn/jsms/server/rest_api_jsms/#api_3"/>
        /// </summary>
        /// <param name="msgId">调用发送验证码 API 的返回值。</param>
        /// <param name="code">验证码。</param>
        public HttpResponse IsCodeValid(string msgId, string code)
        {
            Task<HttpResponse> task = Task.Run(() => IsCodeValidAsync(msgId, code));
            task.Wait();
            return task.Result;
        }

        /// <summary>
        /// 发送模板短信的异步方法。
        /// <see cref="https://docs.jiguang.cn/jsms/server/rest_api_jsms/#api_4"/>
        /// </summary>
        /// <param name="message">模板短信对象。</param>
        public async Task<HttpResponse> SendTemplateMessageAsync(TemplateMessage message)
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
        /// 发送模板短信。
        /// <see cref="https://docs.jiguang.cn/jsms/server/rest_api_jsms/#api_4"/>
        /// </summary>
        /// <param name="message">模板短信对象。</param>
        public HttpResponse SendTemplateMessage(TemplateMessage message)
        {
            Task<HttpResponse> task = Task.Run(() => SendTemplateMessageAsync(message));
            task.Wait();
            return task.Result;
        }

        /// <summary>
        /// 批量发送模板短信。模板 Id 必须一致。
        /// <see cref="https://docs.jiguang.cn/jsms/server/rest_api_jsms/#api_5"/>
        /// </summary>
        /// <param name="templateMessageList">模板短信对象列表。</param>
        public async Task<HttpResponse> SendTemplateMessageAsync(List<TemplateMessage> templateMessageList)
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

        /// <summary>
        /// 批量发送模板短信。模板 Id 必须一致。
        /// <see cref="https://docs.jiguang.cn/jsms/server/rest_api_jsms/#api_5"/>
        /// </summary>
        /// <param name="templateMessageList">模板短信对象列表。</param>
        public HttpResponse SendTemplateMessage(List<TemplateMessage> templateMessageList)
        {
            Task<HttpResponse> task = Task.Run(() => SendTemplateMessageAsync(templateMessageList));
            task.Wait();
            return task.Result;
        }

        /// <summary>
        /// 按时发送模板短信。
        /// </summary>
        /// <param name="message">TemplateMessage 对象。</param>
        public async Task<HttpResponse> SendTemplateMessageByTimeAsnyc(string sendTime, TemplateMessage message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            if (string.IsNullOrEmpty(sendTime))
                throw new ArgumentNullException(nameof(sendTime));

            JObject json = JObject.FromObject(message);
            json.Add("send_time", sendTime);

            Console.WriteLine(json.ToString());

            HttpContent httpContent = new StringContent(json.ToString(), Encoding.UTF8);
            string url = BASE_URL + "schedule";
            HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(url, httpContent).ConfigureAwait(false);
            string httpResponseContent = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            return new HttpResponse(httpResponseMessage.StatusCode, httpResponseMessage.Headers, httpResponseContent);
        }

        /// <summary>
        /// 按时发送模板短信。
        /// </summary>
        /// <param name="message">TemplateMessage 对象。</param>
        public HttpResponse SendTemplateMessageByTime(string sendTime, TemplateMessage message)
        {
            Task<HttpResponse> task = Task.Run(() => SendTemplateMessageByTimeAsnyc(sendTime, message));
            task.Wait();
            return task.Result;
        }

        public async Task<HttpResponse> SendTemplateMessageListByTimeAsync(int tempId, string sendTime, List<TemplateMessage> templateMessageList)
        {
            if (templateMessageList == null || templateMessageList.Count == 0)
                throw new ArgumentException(nameof(templateMessageList));

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
                { "send_time", sendTime },
                { "temp_id", tempId },
                { "recipients", recipients }
            };

            HttpContent httpContent = new StringContent(json.ToString(), Encoding.UTF8);
            string url = BASE_URL + "schedule/batch";
            HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(url, httpContent).ConfigureAwait(false);
            string httpResponseContent = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            return new HttpResponse(httpResponseMessage.StatusCode, httpResponseMessage.Headers, httpResponseContent);
        }

        /// <summary>
        /// 定时批量发送模板短信。
        /// <see cref="https://docs.jiguang.cn/jsms/server/rest_api_jsms_schedule/#api_2"/>
        /// </summary>
        /// <param name="templateMessageList">模板短信列表。注意该方法会忽略 TemplateMessage 对象中的 TempId 属性。</param>
        public HttpResponse SendTemplateMessageListByTime(int tempId, string sendTime, List<TemplateMessage> templateMessageList)
        {
            Task<HttpResponse> task = Task.Run(() => SendTemplateMessageListByTimeAsync(tempId, sendTime, templateMessageList));
            task.Wait();
            return task.Result;
        }

        public async Task<HttpResponse> UpdateScheduleTaskAsync(string scheduleId, string sendTime, TemplateMessage templateMessage)
        {
            JObject json = JObject.FromObject(templateMessage);
            json.Add("send_time", sendTime);

            HttpContent httpContent = new StringContent(json.ToString(), Encoding.UTF8);
            string url = BASE_URL + "schedule/" + scheduleId;
            HttpResponseMessage httpResponseMessage = await httpClient.PutAsync(url, httpContent).ConfigureAwait(false);
            string httpResponseContent = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            return new HttpResponse(httpResponseMessage.StatusCode, httpResponseMessage.Headers, httpResponseContent);
        }

        /// <summary>
        /// 更新定时短信任务。
        /// </summary>
        /// <param name="scheduleId">定时任务 Id</param>
        /// <param name="sendTime">更新后的定时任务时间</param>
        /// <param name="templateMessage">模板短信对象</param>
        public HttpResponse UpdateScheduleTask(string scheduleId, string sendTime, TemplateMessage templateMessage)
        {
            Task<HttpResponse> task = Task.Run(() => UpdateScheduleTaskAsync(scheduleId, sendTime, templateMessage));
            task.Wait();
            return task.Result;
        }

        public async Task<HttpResponse> UpdateScheduleTaskListAsync(string scheduleId, int tempId, string sendTime, List<TemplateMessage> templateMessageList)
        {
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
                { "send_time", sendTime },
                { "temp_id", tempId },
                { "recipients", recipients }
            };

            HttpContent httpContent = new StringContent(json.ToString(), Encoding.UTF8);
            string url = BASE_URL + "schedule/batch/" + scheduleId;
            HttpResponseMessage httpResponseMessage = await httpClient.PutAsync(url, httpContent).ConfigureAwait(false);
            string httpResponseContent = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            return new HttpResponse(httpResponseMessage.StatusCode, httpResponseMessage.Headers, httpResponseContent);
        }

        /// <summary>
        /// 更新批量定时短信任务。
        /// </summary>
        /// <param name="scheduleId">定时任务 Id</param>
        /// <param name="tempId">模板短信 Id</param>
        /// <param name="sendTime">发送时间</param>
        /// <param name="templateMessageList">模板短信对象列表</param>
        public HttpResponse UpdateScheduleTaskList(string scheduleId, int tempId, string sendTime, List<TemplateMessage> templateMessageList)
        {
            Task<HttpResponse> task = Task.Run(() => UpdateScheduleTaskListAsync(scheduleId, tempId, sendTime, templateMessageList));
            task.Wait();
            return task.Result;
        }

        public async Task<HttpResponse> QueryScheduleTaskAsync(string scheduleId)
        {
            if (string.IsNullOrEmpty(scheduleId))
                throw new ArgumentNullException(nameof(scheduleId));

            string url = BASE_URL + "schedule/" + scheduleId;
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(url).ConfigureAwait(false);
            string httpResponseContent = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            return new HttpResponse(httpResponseMessage.StatusCode, httpResponseMessage.Headers, httpResponseContent);
        }

        /// <summary>
        /// 查询模板短信定时发送任务。
        /// <see cref="https://docs.jiguang.cn/jsms/server/rest_api_jsms_schedule/#api_5"/>
        /// </summary>
        /// <param name="scheduleId"></param>
        /// <returns></returns>
        public HttpResponse QueryScheduleTask(string scheduleId)
        {
            Task<HttpResponse> task = Task.Run(() => QueryScheduleTaskAsync(scheduleId));
            task.Wait();
            return task.Result;
        }

        public async Task<HttpResponse> DeleteScheduleTaskAsync(string scheduleId)
        {
            if (string.IsNullOrEmpty(scheduleId))
                throw new ArgumentNullException(nameof(scheduleId));

            string url = BASE_URL + "schedule/" + scheduleId;
            HttpResponseMessage httpResponseMessage = await httpClient.DeleteAsync(url).ConfigureAwait(false);
            string httpResponseContent = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            return new HttpResponse(httpResponseMessage.StatusCode, httpResponseMessage.Headers, httpResponseContent);
        }

        /// <summary>
        /// 删除模板短信定时发送任务。
        /// <see cref="https://docs.jiguang.cn/jsms/server/rest_api_jsms_schedule/#api_6"/>
        /// </summary>
        /// <param name="scheduleId">定时任务 Id。</param>
        public HttpResponse DeleteScheduleTask(string scheduleId)
        {
            Task<HttpResponse> task = Task.Run(() => DeleteScheduleTaskAsync(scheduleId));
            task.Wait();
            return task.Result;
        }
    }
}
