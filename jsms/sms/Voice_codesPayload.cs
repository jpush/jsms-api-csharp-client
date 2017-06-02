using System;
using Newtonsoft.Json;

namespace jsms.sms
{
    public class Voice_codesPayload
    {
        public string mobile;
        public Int16 ttl;

        public Voice_codesPayload(string mobile, Int16 ttl)
        {
            this.mobile = mobile;
            this.ttl = ttl;
        }

        public string ToJson(Voice_codesPayload sms)
        {
            return JsonConvert.SerializeObject(sms, Formatting.None, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }

        public Voice_codesPayload Check()
        {
            return this;
        }
    }
}
