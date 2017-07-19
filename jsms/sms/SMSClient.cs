using jsms.util;
using jsms.common;

namespace jsms.sms
{
    public class SMSClient : BaseHttpClient
    {
        private const string HOST_NAME_SSL = "https://api.sms.jpush.cn/v1/";
        private const string CODES_PATH = "codes/";
        private const string MESSAGES_PATH = "messages";
        private const string VOICE_CODES_PATH = "voice_codes";
        private const string VALID_PATH = "/valid";
        private const string SCHEDULE = "schedule";

        private string appKey;
        private string masterSecret;
        private string auth;

        public SMSClient(string appKey, string masterSecret)
        {
            Preconditions.CheckArgument(!string.IsNullOrEmpty(appKey), "appKey should be set");
            Preconditions.CheckArgument(!string.IsNullOrEmpty(masterSecret), "masterSecret should be set");
            this.appKey = appKey;
            this.masterSecret = masterSecret;
            auth = Authorization();
        }

        /// <summary>
        /// 发送短信验证码。
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public ResponseWrapper SendCodes(SMSPayload payload)
        {
            Preconditions.CheckArgument(payload != null, "pushPayload should not be empty");
            string payloadJson = payload.ToJson(payload);
            return SendCodes(payloadJson);
        }

        public ResponseWrapper SendCodes(string payloadstring)
        {
            Preconditions.CheckArgument(!string.IsNullOrEmpty(payloadstring), "payloadstring should not be empty");
            string url = HOST_NAME_SSL;
            url += CODES_PATH;
            return SendPost(url, auth, payloadstring);
        }

        /// <summary>
        /// 发送模板短信。
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public ResponseWrapper SendMessages(SMSPayload payload)
        {
            Preconditions.CheckArgument(payload != null, "pushPayload should not be empty");
            string payloadJsonStr = payload.ToJson(payload);

            if (payload.SendTime != null)
                return SendMessagesByDate(payloadJsonStr);
            else
                return SendMessages(payloadJsonStr);
        }

        public ResponseWrapper SendMessages(string payloadstring)
        {
            string url = HOST_NAME_SSL + MESSAGES_PATH;
            return SendPost(url, auth, payloadstring);
        }

        public ResponseWrapper SendVoice_codes(Voice_codesPayload payload)
        {
            Preconditions.CheckArgument(payload != null, "pushPayload should not be empty");
            string payloadJson = payload.ToJson(payload);
            return SendVoice_codes(payloadJson);
        }

        public ResponseWrapper SendVoice_codes(string payloadstring)
        {
            Preconditions.CheckArgument(!string.IsNullOrEmpty(payloadstring), "payloadstring should not be empty");
            string url = HOST_NAME_SSL;
            url += VOICE_CODES_PATH;
            return SendPost(url, auth, payloadstring);
        }

        public ResponseWrapper ValidCodes(ValidPayload payload, string msg_id)
        {
            Preconditions.CheckArgument(payload != null, "pushPayload should not be empty");
            string payloadJson = payload.ToJson(payload);
            return ValidCodes(payloadJson, msg_id);
        }

        public ResponseWrapper ValidCodes(string payloadstring, string msg_id)
        {
            Preconditions.CheckArgument(!string.IsNullOrEmpty(payloadstring), "payloadstring should not be empty");
            string url = HOST_NAME_SSL;
            url += CODES_PATH;
            url += msg_id;
            url += VALID_PATH;
            return SendPost(url, auth, payloadstring);
        }

        /// <summary>
        /// 发送模块短信定时任务。
        /// </summary>
        public ResponseWrapper SendMessagesByDate(string payloadJsonStr)
        {
            string url = HOST_NAME_SSL + SCHEDULE;
            return SendPost(url, auth, payloadJsonStr);
        }

        /// <summary>
        /// 查询定时任务信息。
        /// </summary>
        /// <param name="scheduleId">定时任务 Id</param>
        /// <returns></returns>
        public ResponseWrapper QuerySchedule(string scheduleId)
        {
            Preconditions.CheckArgument(!string.IsNullOrEmpty(scheduleId), "scheduleId should not be empty");
            string url = HOST_NAME_SSL + SCHEDULE + "/" + scheduleId;
            return SendGet(url, auth, scheduleId);
        }

        /// <summary>
        /// 更新单条模板短信定时发送任务。
        /// </summary>
        /// <param name="scheduleId">定时任务 Id</param>
        /// <param name="payload">更新后的 SMSPayload</param>
        /// <returns></returns>
        public ResponseWrapper UpdateSchedule(string scheduleId, SMSPayload payload)
        {
            Preconditions.CheckArgument(!string.IsNullOrEmpty(scheduleId), "scheduleId should not be empty");
            Preconditions.CheckArgument(payload != null, "pushPayload should not be empty");
            string url = HOST_NAME_SSL + SCHEDULE + "/" + scheduleId;
            string payloadJsonStr = payload.ToJson(payload);
            return SendPut(url, auth, payloadJsonStr);
        }

        /// <summary>
        /// 删除模板短信定时任务。
        /// </summary>
        /// <param name="scheduleId">定时任务 Id</param>
        public ResponseWrapper DeleteSchedule(string scheduleId)
        {
            Preconditions.CheckArgument(!string.IsNullOrEmpty(scheduleId), "scheduleId should not be empty");
            string url = HOST_NAME_SSL + SCHEDULE + "/" + scheduleId;
            return SendDelete(url, auth, null);
        }

        public string Authorization()
        {
            string origin = appKey + ":" + masterSecret;
            return Base64.GetBase64Encode(origin);
        }
    }
}
