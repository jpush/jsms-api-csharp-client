using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using jsms.common;
using jsms.util;

namespace jsms.sms
{
    public class SMSPayload
    {
        public string mobile;
        public string temp_id;
        public Int16 ttl;

        public SMSPayload(string mobile, string temp_id)
        {
            this.mobile = mobile;
            this.temp_id = temp_id;
        }

        public SMSPayload(string mobile, Int16 ttl)
        {
            this.mobile = mobile;
            this.ttl = ttl;
        }

        public string ToJson(SMSPayload sms)
        {
            return JsonConvert.SerializeObject(sms);
        }
        public SMSPayload Check()
        {
            return this;
        }
    }
}
