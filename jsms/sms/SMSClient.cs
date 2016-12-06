using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using jsms.util;
using jsms.common;
using System.Diagnostics;

namespace jsms.sms
{
    public class SMSClient : BaseHttpClient
    {
        private const String HOST_NAME_SSL = "https://api.sms.jpush.cn/v1/";
        private const String CODES_PATH = "codes";
        private const String VOICE_CODES_PATH = "voice_codes";
        private const String VALID_PATH = "codes";

        private String appKey;
        private String masterSecret;
        public SMSClient(String appKey, String masterSecret)
        {
            Preconditions.checkArgument(!String.IsNullOrEmpty(appKey), "appKey should be set");
            Preconditions.checkArgument(!String.IsNullOrEmpty(masterSecret), "masterSecret should be set");
            this.appKey = appKey;
            this.masterSecret = masterSecret;
        }
        public ResponseWrapper sendCodes(SMSPayload payload)
        {
            Preconditions.checkArgument(payload != null, "pushPayload should not be empty");
            payload.Check();
            String payloadJson = payload.ToJson(payload);
            return sendCodes(payloadJson);
        }
        public ResponseWrapper sendCodes(string payloadString)
        {
            Preconditions.checkArgument(!string.IsNullOrEmpty(payloadString), "payloadString should not be empty");

            String url = HOST_NAME_SSL;
            url += CODES_PATH;
            ResponseWrapper result = sendPost(url, Authorization(), payloadString);
            return result;
        }

        public  String Authorization()
        {

            Debug.Assert(!string.IsNullOrEmpty(this.appKey));
            Debug.Assert(!string.IsNullOrEmpty(this.masterSecret));

            String origin = this.appKey + ":" + this.masterSecret;
            return Base64.getBase64Encode(origin);
        }
    }
}
