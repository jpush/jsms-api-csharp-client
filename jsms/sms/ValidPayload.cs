using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using jsms.common;
using jsms.util;

namespace jsms.sms
{
    public class ValidPayload
    {
        public string code;
        public ValidPayload(string code)
        {
            this.code = code;
        }

        public string ToJson(ValidPayload code)
        {
            return JsonConvert.SerializeObject(code,
                            Newtonsoft.Json.Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });
        }
        public ValidPayload Check()
        {
            return this;
        }
    }
}
