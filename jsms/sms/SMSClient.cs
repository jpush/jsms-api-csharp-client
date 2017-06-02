using System;
using jsms.util;
using jsms.common;
using System.Diagnostics;

namespace jsms.sms
{
    public class SMSClient : BaseHttpClient
    {
        private const String HOST_NAME_SSL = "https://api.sms.jpush.cn/v1/";
        private const String CODES_PATH = "codes/";
        private const String MESSAGES_PATH = "messages";
        private const String VOICE_CODES_PATH = "voice_codes";
        private const String VALID_PATH = "/valid";

        private String appKey;
        private String masterSecret;
        private String auth;

        public SMSClient(String appKey, String masterSecret)
        {
            Preconditions.CheckArgument(!String.IsNullOrEmpty(appKey), "appKey should be set");
            Preconditions.CheckArgument(!String.IsNullOrEmpty(masterSecret), "masterSecret should be set");
            this.appKey = appKey;
            this.masterSecret = masterSecret;
            auth = Authorization();
        }

        public ResponseWrapper SendCodes(SMSPayload payload)
        {
            Preconditions.CheckArgument(payload != null, "pushPayload should not be empty");
            payload.Check();
            String payloadJson = payload.ToJson(payload);
            return SendCodes(payloadJson);
        }

        public ResponseWrapper SendCodes(string payloadString)
        {
            Preconditions.CheckArgument(!string.IsNullOrEmpty(payloadString), "payloadString should not be empty");
            String url = HOST_NAME_SSL;
            url += CODES_PATH;
            return SendPost(url, auth, payloadString);
        }

        public ResponseWrapper SendMessages(SMSPayload payload)
        {
            Preconditions.CheckArgument(payload != null, "pushPayload should not be empty");
            payload.Check();
            String payloadJson = payload.ToJson(payload);
            return SendMessages(payloadJson);
        }

        public ResponseWrapper SendMessages(string payloadString)
        {
            Preconditions.CheckArgument(!string.IsNullOrEmpty(payloadString), "payloadString should not be empty");
            String url = HOST_NAME_SSL;
            url += MESSAGES_PATH;
            return SendPost(url, auth, payloadString);
        }

        public ResponseWrapper SendVoice_codes(Voice_codesPayload payload)
        {
            Preconditions.CheckArgument(payload != null, "pushPayload should not be empty");
            payload.Check();
            String payloadJson = payload.ToJson(payload);
            return SendVoice_codes(payloadJson);
        }

        public ResponseWrapper SendVoice_codes(string payloadString)
        {
            Preconditions.CheckArgument(!string.IsNullOrEmpty(payloadString), "payloadString should not be empty");
            String url = HOST_NAME_SSL;
            url += VOICE_CODES_PATH;
            return SendPost(url, auth, payloadString);
        }

        public ResponseWrapper ValidCodes(ValidPayload payload, string msg_id)
        {
            Preconditions.CheckArgument(payload != null, "pushPayload should not be empty");
            payload.Check();
            String payloadJson = payload.ToJson(payload);
            return ValidCodes(payloadJson, msg_id);
        }

        public ResponseWrapper ValidCodes(string payloadString, string msg_id)
        {
            Preconditions.CheckArgument(!string.IsNullOrEmpty(payloadString), "payloadString should not be empty");
            String url = HOST_NAME_SSL;
            url += CODES_PATH;
            url += msg_id;
            url += VALID_PATH;
            return SendPost(url, auth, payloadString);
        }

        public String Authorization()
        {
            Debug.Assert(!string.IsNullOrEmpty(appKey));
            Debug.Assert(!string.IsNullOrEmpty(masterSecret));

            String origin = appKey + ":" + masterSecret;
            return Base64.GetBase64Encode(origin);
        }
    }
}
