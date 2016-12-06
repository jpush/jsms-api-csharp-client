using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using jsms.common;
using jsms.util;

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
            return JsonConvert.SerializeObject(sms,
                            Newtonsoft.Json.Formatting.None,
                            new JsonSerializerSettings
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
