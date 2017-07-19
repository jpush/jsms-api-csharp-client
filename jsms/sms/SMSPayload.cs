using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace jsms.sms
{
    public class SMSPayload
    {
        public string mobile;
        public Int32 temp_id;

        [JsonProperty("temp_para")]
        public Dictionary<string, string> TempParameter { get; set; }

        [JsonProperty("send_time", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? SendTime { get; set; }

        public SMSPayload(string mobile, Int32 temp_id)
        {
            this.mobile = mobile;
            this.temp_id = temp_id;
        }

        public SMSPayload(string mobile, Int32 temp_id, Dictionary<string, string> temp_para)
        {
            this.mobile = mobile;
            this.temp_id = temp_id;
            TempParameter = temp_para;
        }

        public string ToJson(SMSPayload sms)
        {
            return JsonConvert.SerializeObject(sms, Formatting.None, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }

        public SMSPayload Check()
        {
            return this;
        }
    }
}
