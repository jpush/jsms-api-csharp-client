using Jiguang.JSMS.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Jiguang.JSMS
{
    /// <summary>
    /// 极光短信（JSMS)客户端。
    /// </summary>
    public class JSMSClient
    {
        /// <summary>
        /// 对外暴露 HttpClient，可以增加或修改设置。
        /// </summary>
        public HttpClient httpClient;

        /// <summary>
        /// 初始化 JSMSClient 对象。其中，App Key 和 Master Secret 都可在极光官网控制台上找到。
        /// </summary>
        /// <param name="appKey">应用的 App Key</param>
        /// <param name="masterSecret">应用的 Master Secret</param>
        public JSMSClient(string appKey, string masterSecret)
        {
            if (string.IsNullOrEmpty(appKey))
                throw new ArgumentNullException(nameof(appKey));

            if (string.IsNullOrEmpty(masterSecret))
                throw new ArgumentNullException(nameof(masterSecret));

            httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://api.sms.jpush.cn/v1/")
            };
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var auth = Convert.ToBase64String(Encoding.UTF8.GetBytes(appKey + ":" + masterSecret));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", auth);
        }

        /// <summary>
        /// <see cref="SendCode(string, int)"/>
        /// </summary>
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
            HttpResponseMessage httpResponseMessage = await httpClient.PostAsync("codes", httpContent).ConfigureAwait(false);
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

        /// <summary>
        /// 发送语音验证码。
        /// </summary>
        /// <param name="jsonBody">消息体。</param>
        public async Task<HttpResponse> SendVoiceCodeAsync(string jsonBody)
        {
            if (string.IsNullOrEmpty(jsonBody))
                throw new ArgumentNullException(nameof(jsonBody));

            HttpContent httpContent = new StringContent(jsonBody, Encoding.UTF8);
            HttpResponseMessage httpResponseMessage = await httpClient.PostAsync("voice_codes", httpContent).ConfigureAwait(false);
            string httpResponseContent = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            return new HttpResponse(httpResponseMessage.StatusCode, httpResponseMessage.Headers, httpResponseContent);
        }

        /// <summary>
        /// <see cref="SendVoiceCode(VoiceCode)"/>
        /// </summary>
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
        /// <see cref="IsCodeValid(string, string)"/>
        /// </summary>
        public async Task<HttpResponse> IsCodeValidAsync(string msgId, string code)
        {
            if (string.IsNullOrEmpty(msgId))
                throw new ArgumentNullException(nameof(msgId));

            if (string.IsNullOrEmpty(code))
                throw new ArgumentNullException(nameof(code));

            var url = $"codes/{msgId}/valid";

            JObject json = new JObject
            {
                { "code", code }
            };
            HttpContent httpContent = new StringContent(json.ToString(), Encoding.UTF8);

            HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(url, httpContent).ConfigureAwait(false);
            string httpResponseContent = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            return new HttpResponse(httpResponseMessage.StatusCode, httpResponseMessage.Headers, httpResponseContent);
        }

        /// <summary>
        /// 判断验证码是否有效。
        /// <see cref="https://docs.jiguang.cn/jsms/server/rest_api_jsms/#api_3"/>
        /// </summary>
        /// <param name="msgId">调用发送验证码短信 API 的返回值。</param>
        /// <param name="code">验证码。</param>
        public HttpResponse IsCodeValid(string msgId, string code)
        {
            Task<HttpResponse> task = Task.Run(() => IsCodeValidAsync(msgId, code));
            task.Wait();
            return task.Result;
        }

        /// <summary>
        /// <see cref="SendTemplateMessage(TemplateMessage)"/>
        /// </summary>
        public async Task<HttpResponse> SendTemplateMessageAsync(TemplateMessage message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            HttpContent httpContent = new StringContent(message.ToString(), Encoding.UTF8);
            HttpResponseMessage httpResponseMessage = await httpClient.PostAsync("messages", httpContent).ConfigureAwait(false);
            string httpResponseContent = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            return new HttpResponse(httpResponseMessage.StatusCode, httpResponseMessage.Headers, httpResponseContent);
        }

        /// <summary>
        /// 发送单条模板短信。
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
        /// <see cref="SendTemplateMessage(List{TemplateMessage})"/>
        /// </summary>
        public async Task<HttpResponse> SendTemplateMessageAsync(List<TemplateMessage> templateMessageList)
        {
            if (templateMessageList == null || templateMessageList.Count == 0)
                throw new ArgumentException(nameof(templateMessageList));

            int? tempId = templateMessageList[0].TemplateId;

            JArray recipients = new JArray();
            foreach (TemplateMessage msg in templateMessageList)
            {
                JObject item = new JObject
                {
                    { "mobile", msg.Mobile }
                };

                if (msg.TemplateParameters != null && msg.TemplateParameters.Count != 0)
                {
                    item.Add("temp_para", JObject.FromObject(msg.TemplateParameters));
                }

                recipients.Add(item);
            }

            JObject json = new JObject
            {
                { "temp_id", tempId },
                { "recipients", recipients }
            };

            HttpContent httpContent = new StringContent(json.ToString(), Encoding.UTF8);
            HttpResponseMessage httpResponseMessage = await httpClient.PostAsync("messages/batch", httpContent).ConfigureAwait(false);
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
        /// <see cref="SendTemplateMessageByTime(string, TemplateMessage)"/>
        /// </summary>
        public async Task<HttpResponse> SendTemplateMessageByTimeAsnyc(string sendTime, TemplateMessage message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            if (string.IsNullOrEmpty(sendTime))
                throw new ArgumentNullException(nameof(sendTime));

            JObject json = JObject.FromObject(message);
            json.Add("send_time", sendTime);

            HttpContent httpContent = new StringContent(json.ToString(), Encoding.UTF8);
            HttpResponseMessage httpResponseMessage = await httpClient.PostAsync("schedule", httpContent).ConfigureAwait(false);
            string httpResponseContent = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            return new HttpResponse(httpResponseMessage.StatusCode, httpResponseMessage.Headers, httpResponseContent);
        }

        /// <summary>
        /// 按时发送模板短信。
        /// <see cref="https://docs.jiguang.cn/jsms/server/rest_api_jsms_schedule/#api_1"/>
        /// </summary>
        /// <param name="sendTime">发送日期，格式为 "yyyy-MM-dd HH:mm:ss"</param>
        /// <param name="message">TemplateMessage 对象。</param>
        public HttpResponse SendTemplateMessageByTime(string sendTime, TemplateMessage message)
        {
            Task<HttpResponse> task = Task.Run(() => SendTemplateMessageByTimeAsnyc(sendTime, message));
            task.Wait();
            return task.Result;
        }

        /// <summary>
        /// <see cref="SendTemplateMessageListByTime(int, string, List{TemplateMessage})"/>
        /// </summary>
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
                {
                    item.Add("temp_para", JObject.FromObject(msg.TemplateParameters));
                }

                recipients.Add(item);
            }

            JObject json = new JObject
            {
                { "send_time", sendTime },
                { "temp_id", tempId },
                { "recipients", recipients }
            };

            HttpContent httpContent = new StringContent(json.ToString(), Encoding.UTF8);
            HttpResponseMessage httpResponseMessage = await httpClient.PostAsync("schedule/batch", httpContent).ConfigureAwait(false);
            string httpResponseContent = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            return new HttpResponse(httpResponseMessage.StatusCode, httpResponseMessage.Headers, httpResponseContent);
        }

        /// <summary>
        /// 定时批量发送模板短信。
        /// <see cref="https://docs.jiguang.cn/jsms/server/rest_api_jsms_schedule/#api_2"/>
        /// </summary>
        /// <param name="tempId">短信模板 Id</param>
        /// <param name="sendTime">发送日期，格式为 "yyyy-MM-dd HH:mm:ss"</param>
        /// <param name="templateMessageList">模板短信列表。注意该方法会忽略 TemplateMessage 对象中的 TempId 属性，而以传入的 tempId 为准。</param>
        public HttpResponse SendTemplateMessageListByTime(int tempId, string sendTime, List<TemplateMessage> templateMessageList)
        {
            Task<HttpResponse> task = Task.Run(() => SendTemplateMessageListByTimeAsync(tempId, sendTime, templateMessageList));
            task.Wait();
            return task.Result;
        }

        /// <summary>
        /// <see cref="UpdateScheduleTask(string, string, TemplateMessage)"/>
        /// </summary>
        public async Task<HttpResponse> UpdateScheduleTaskAsync(string scheduleId, string sendTime, TemplateMessage templateMessage)
        {
            JObject json = JObject.FromObject(templateMessage);
            json.Add("send_time", sendTime);
            HttpContent httpContent = new StringContent(json.ToString(), Encoding.UTF8);

            HttpResponseMessage httpResponseMessage = await httpClient.PutAsync($"schedule/{scheduleId}", httpContent).ConfigureAwait(false);
            string httpResponseContent = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            return new HttpResponse(httpResponseMessage.StatusCode, httpResponseMessage.Headers, httpResponseContent);
        }

        /// <summary>
        /// 更新单条定时短信任务。
        /// <see cref="https://docs.jiguang.cn/jsms/server/rest_api_jsms_schedule/#api_3"/>
        /// </summary>
        /// <param name="scheduleId">定时任务 Id</param>
        /// <param name="sendTime">更新后的定时任务时间，格式为 "yyyy-MM-dd HH:mm:ss"</param>
        /// <param name="templateMessage">模板短信对象。注意该方法会忽略 TemplateMessage 对象中的 TempId 属性，而以传入的 tempId 为准。</param>
        public HttpResponse UpdateScheduleTask(string scheduleId, string sendTime, TemplateMessage templateMessage)
        {
            Task<HttpResponse> task = Task.Run(() => UpdateScheduleTaskAsync(scheduleId, sendTime, templateMessage));
            task.Wait();
            return task.Result;
        }

        /// <summary>
        /// <see cref="UpdateScheduleTaskList(string, int, string, List{TemplateMessage})"/>
        /// </summary>
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
                    item.Add("temp_para", JObject.FromObject(msg.TemplateParameters));

                recipients.Add(item);
            }

            JObject json = new JObject
            {
                { "send_time", sendTime },
                { "temp_id", tempId },
                { "recipients", recipients }
            };

            HttpContent httpContent = new StringContent(json.ToString(), Encoding.UTF8);
            HttpResponseMessage httpResponseMessage = await httpClient.PutAsync($"schedule/batch/{scheduleId}", httpContent).ConfigureAwait(false);
            string httpResponseContent = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            return new HttpResponse(httpResponseMessage.StatusCode, httpResponseMessage.Headers, httpResponseContent);
        }

        /// <summary>
        /// 更新批量定时短信任务。
        /// </summary>
        /// <param name="scheduleId">定时任务 Id</param>
        /// <param name="tempId">模板短信 Id</param>
        /// <param name="sendTime">更新后的定时任务时间，格式为 "yyyy-MM-dd HH:mm:ss"</param>
        /// <param name="templateMessageList">模板短信对象列表。注意该方法会忽略 TemplateMessage 对象中的 TempId 属性，而以传入的 tempId 为准。</param>
        public HttpResponse UpdateScheduleTaskList(string scheduleId, int tempId, string sendTime, List<TemplateMessage> templateMessageList)
        {
            Task<HttpResponse> task = Task.Run(() => UpdateScheduleTaskListAsync(scheduleId, tempId, sendTime, templateMessageList));
            task.Wait();
            return task.Result;
        }

        /// <summary>
        /// <see cref="QueryScheduleTask(string)"/>
        /// </summary>
        public async Task<HttpResponse> QueryScheduleTaskAsync(string scheduleId)
        {
            if (string.IsNullOrEmpty(scheduleId))
                throw new ArgumentNullException(nameof(scheduleId));

            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync($"schedule/{scheduleId}").ConfigureAwait(false);
            string httpResponseContent = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            return new HttpResponse(httpResponseMessage.StatusCode, httpResponseMessage.Headers, httpResponseContent);
        }

        /// <summary>
        /// 查询模板短信定时发送任务。
        /// <see cref="https://docs.jiguang.cn/jsms/server/rest_api_jsms_schedule/#api_5"/>
        /// </summary>
        /// <param name="scheduleId">定时发送任务 Id，由调用定时短信提交 API 后返回得到。</param>
        public HttpResponse QueryScheduleTask(string scheduleId)
        {
            Task<HttpResponse> task = Task.Run(() => QueryScheduleTaskAsync(scheduleId));
            task.Wait();
            return task.Result;
        }

        /// <summary>
        /// <see cref="DeleteScheduleTask(string)"/>
        /// </summary>
        public async Task<HttpResponse> DeleteScheduleTaskAsync(string scheduleId)
        {
            if (string.IsNullOrEmpty(scheduleId))
                throw new ArgumentNullException(nameof(scheduleId));

            HttpResponseMessage httpResponseMessage = await httpClient.DeleteAsync($"schedule/{scheduleId}").ConfigureAwait(false);
            string httpResponseContent = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            return new HttpResponse(httpResponseMessage.StatusCode, httpResponseMessage.Headers, httpResponseContent);
        }

        /// <summary>
        /// 删除模板短信定时发送任务。
        /// <see cref="https://docs.jiguang.cn/jsms/server/rest_api_jsms_schedule/#api_6"/>
        /// </summary>
        /// <param name="scheduleId">定时发送任务 Id，由调用定时短信提交 API 后返回得到。</param>
        public HttpResponse DeleteScheduleTask(string scheduleId)
        {
            Task<HttpResponse> task = Task.Run(() => DeleteScheduleTaskAsync(scheduleId));
            task.Wait();
            return task.Result;
        }

        /// <summary>
        /// <see cref="CheckAccountBalance(string, string)"/>
        /// </summary>
        public async Task<HttpResponse> CheckAccountBalanceAsync(string devKey, string apiDevSecret)
        {
            if (string.IsNullOrEmpty(devKey))
                throw new ArgumentNullException(nameof(devKey));

            if (string.IsNullOrEmpty(apiDevSecret))
                throw new ArgumentNullException(nameof(apiDevSecret));

            var request = new HttpRequestMessage() {
                RequestUri = new Uri("https://api.sms.jpush.cn/v1/accounts/dev"),
                Method = HttpMethod.Get,
            };
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var auth = Convert.ToBase64String(Encoding.UTF8.GetBytes(devKey + ":" + apiDevSecret));
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", auth);

            HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(request).ConfigureAwait(false);
            string httpResponseContent = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            return new HttpResponse(httpResponseMessage.StatusCode, httpResponseMessage.Headers, httpResponseContent);
        }

        /// <summary>
        /// 检查账号余量，账号余量指未分配给某个应用，属于账号共享的短信余量。
        /// <para><see cref="https://docs.jiguang.cn/jsms/server/rest_jsms_api_account/#api_1"/></para>
        /// </summary>
        /// <param name="devKey">开发者标识。可以在极光官网控制台的个人信息中找到。</param>
        /// <param name="apiDevSecret">可以在极光官网控制台的个人信息中找到。</param>
        public HttpResponse CheckAccountBalance(string devKey, string apiDevSecret)
        {
            Task<HttpResponse> task = Task.Run(() => CheckAccountBalanceAsync(devKey, apiDevSecret));
            task.Wait();
            return task.Result;
        }

        /// <summary>
        /// <see cref="CheckAppBalance"/>
        /// </summary>
        public async Task<HttpResponse> CheckAppBalanceAsync()
        {
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync("accounts/app").ConfigureAwait(false);
            string httpResponseContent = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            return new HttpResponse(httpResponseMessage.StatusCode, httpResponseMessage.Headers, httpResponseContent);
        }

        /// <summary>
        /// 查询应用余量，应用余量指分配给某个应用单独使用的短信余量。
        /// <para><see cref="https://docs.jiguang.cn/jsms/server/rest_jsms_api_account/#api_2"/></para>
        /// </summary>
        public HttpResponse CheckAppBalance()
        {
            Task<HttpResponse> task = Task.Run(() => CheckAppBalanceAsync());
            task.Wait();
            return task.Result;
        }

        /// <summary>
        /// <seealso cref="CreateMessageTemplate(TemplateMessage)"/>
        /// </summary>
        public async Task<HttpResponse> CreateMessageTemplateAsync(TemplateMessage template)
        {
            HttpContent httpContent = new StringContent(template.ToString(), Encoding.UTF8);
            HttpResponseMessage httpResponseMessage = await httpClient.PostAsync("templates", httpContent).ConfigureAwait(false);
            string httpResponseContent = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            return new HttpResponse(httpResponseMessage.StatusCode, httpResponseMessage.Headers, httpResponseContent);
        }

        /// <summary>
        /// 创建短信模板。
        /// <para><see cref="https://docs.jiguang.cn/jsms/server/rest_api_jsms_templates/#api_1"/></para>
        /// </summary>
        /// <param name="template">短信模板对象。</param>
        public HttpResponse CreateMessageTemplate(TemplateMessage template)
        {
            Task<HttpResponse> task = Task.Run(() => CreateMessageTemplateAsync(template));
            task.Wait();
            return task.Result;
        }

        /// <summary>
        /// <seealso cref="UpdateMessageTemplate(TemplateMessage)"/>
        /// </summary>
        public async Task<HttpResponse> UpdateMessageTemplateAsync(TemplateMessage template)
        {
            if (template.TemplateId == null)
                throw new ArgumentNullException(nameof(template.TemplateId));

            HttpContent httpContent = new StringContent(template.ToString(), Encoding.UTF8);
            HttpResponseMessage httpResponseMessage = await httpClient.PutAsync($"templates/{template.TemplateId}", httpContent).ConfigureAwait(false);
            string httpResponseContent = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            return new HttpResponse(httpResponseMessage.StatusCode, httpResponseMessage.Headers, httpResponseContent);
        }

        /// <summary>
        /// 更新短信模板。
        /// <para><see cref="https://docs.jiguang.cn/jsms/server/rest_api_jsms_templates/#api_2"/></para>
        /// </summary>
        /// <param name="template">短信模板对象。</param>
        public HttpResponse UpdateMessageTemplate(TemplateMessage template)
        {
            Task<HttpResponse> task = Task.Run(() => UpdateMessageTemplateAsync(template));
            task.Wait();
            return task.Result;
        }

        /// <summary>
        /// <seealso cref="QueryMessageTemplate(int)"/>
        /// </summary>
        public async Task<HttpResponse> QueryMessageTemplateAsync(int tempId)
        {
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync($"templates/{tempId}").ConfigureAwait(false);
            string httpResponseContent = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            return new HttpResponse(httpResponseMessage.StatusCode, httpResponseMessage.Headers, httpResponseContent);
        }

        /// <summary>
        /// 查询短信模板。
        /// <para><see cref="https://docs.jiguang.cn/jsms/server/rest_api_jsms_templates/#api_3"/></para>
        /// </summary>
        public HttpResponse QueryMessageTemplate(int tempId)
        {
            Task<HttpResponse> task = Task.Run(() => QueryMessageTemplateAsync(tempId));
            task.Wait();
            return task.Result;
        }

        /// <summary>
        /// <seealso cref="DeleteMessageTempleteAsync(int)"/>
        /// </summary>
        public async Task<HttpResponse> DeleteMessageTempleteAsync(int tempId)
        {
            HttpResponseMessage httpResponseMessage = await httpClient.DeleteAsync($"templates/{tempId}").ConfigureAwait(false);
            string httpResponseContent = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            return new HttpResponse(httpResponseMessage.StatusCode, httpResponseMessage.Headers, httpResponseContent);
        }

        /// <summary>
        /// 删除短信模板。
        /// <para><see cref="https://docs.jiguang.cn/jsms/server/rest_api_jsms_templates/#api_4"/></para>
        /// </summary>
        public HttpResponse DeleteMessageTemplete(int tempId)
        {
            Task<HttpResponse> task = Task.Run(() => DeleteMessageTempleteAsync(tempId));
            task.Wait();
            return task.Result;
        }

        #region 签名接口

        /// <summary>
        /// 创建签名
        /// </summary>
        /// <param name="signModel"></param>
        /// <returns></returns>
        public async Task<HttpResponse> CreateSignAsync(SignModel signModel)
        {
            if (signModel == null || string.IsNullOrEmpty(signModel.Sign))
                throw new ArgumentNullException(nameof(signModel));
            if (signModel.Remark?.Length > 100){
                throw new NotSupportedException("签名备注不能超过100个字");
            }

            MultipartFormDataContent content = new MultipartFormDataContent
            {
                { new StringContent(signModel.Sign), "sign" },
                { new StringContent(((int)signModel.Type).ToString()), "type"}
            };

            
            if (signModel.Image != null)
            {
                content.Add(new StreamContent(signModel.Image), "image");
                //content.Add(new StreamContent(signModel.Image), "image0");
            }
            if (!string.IsNullOrEmpty(signModel.Remark))
            {
                content.Add(new StringContent(signModel.Remark), "remark");
            }

            using (var resp = await httpClient.PostAsync("sign", content))
            {
                string respStr = await resp.Content.ReadAsStringAsync();
                return new HttpResponse(resp.StatusCode, resp.Headers, respStr);
            }

        }

        /// <summary>
        /// 新增签名
        /// </summary>
        /// <param name="signModel"></param>
        /// <returns></returns>
        public HttpResponse CreateSign(SignModel signModel)
        {
            Task<HttpResponse> task = Task.Run(() => CreateSignAsync(signModel));
            task.Wait();
            return task.Result;
        }

        /// <summary>
        /// 修改签名
        /// </summary>
        /// <param name="signId"></param>
        /// <param name="signModel"></param>
        /// <returns></returns>
        public async Task<HttpResponse> UpdateSignAsync(int signId, SignModel signModel)
        {
            if (signModel == null || string.IsNullOrEmpty(signModel.Sign))
                throw new ArgumentNullException(nameof(signModel));

            if (signModel.Remark?.Length > 100)
            {
                throw new NotSupportedException("签名备注不能超过100个字");
            }

            MultipartFormDataContent content = new MultipartFormDataContent
            {
                { new StringContent(signModel.Sign), "sign" },
                { new StringContent(((int)signModel.Type).ToString()), "type"}
            };


            if (signModel.Image != null)
            {
                content.Add(new StreamContent(signModel.Image), "image0");
            }
            if (!string.IsNullOrEmpty(signModel.Remark))
            {
                content.Add(new StringContent(signModel.Remark));
            }

            using (var resp = await httpClient.PostAsync($"sign/{signId}", content))
            {
                string respStr = await resp.Content.ReadAsStringAsync();
                return new HttpResponse(resp.StatusCode, resp.Headers, respStr);
            }
        }
        /// <summary>
        /// 修改签名
        /// </summary>
        /// <param name="signId"></param>
        /// <param name="signModel"></param>
        /// <returns></returns>
        public HttpResponse UpdateSign(int signId, SignModel signModel)
        {
            Task<HttpResponse> task = Task.Run(() => UpdateSignAsync(signId, signModel));
            task.Wait();
            return task.Result;
        }

        /// <summary>
        /// 查询签名 
        /// </summary>
        /// <param name="signId"></param>
        /// <returns></returns>
        public async Task<HttpResponse> QuerySignAsync(int signId)
        {
            using (var resp = await httpClient.GetAsync($"sign/{signId}"))
            {
                string respStr = await resp.Content.ReadAsStringAsync();
                return new HttpResponse(resp.StatusCode, resp.Headers, respStr);
            }
        }
        /// <summary>
        /// 查询签名 
        /// </summary>
        /// <param name="signId"></param>
        /// <returns></returns>
        public HttpResponse QuerySign(int signId)
        {
            Task<HttpResponse> task = Task.Run(() => QuerySignAsync(signId));
            task.Wait();
            return task.Result;
        }

        /// <summary>
        /// 删除签名
        /// </summary>
        /// <param name="signId"></param>
        /// <returns></returns>
        public async Task<HttpResponse> DeleteSignAsync(int signId)
        {
            using (var resp = await httpClient.DeleteAsync($"sign/{signId}"))
            {
                string respStr = await resp.Content.ReadAsStringAsync();
                return new HttpResponse(resp.StatusCode, resp.Headers, respStr);
            }
        }

        /// <summary>
        /// 删除签名
        /// </summary>
        /// <param name="signId"></param>
        /// <returns></returns>
        public HttpResponse DeleteSign(int signId)
        {
            Task<HttpResponse> task = Task.Run(() => DeleteSignAsync(signId));
            task.Wait();
            return task.Result;
        }

        #endregion


    }
}
