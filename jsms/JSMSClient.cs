using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using jsms.common;
using jsms.sms;

namespace jsms
{
    /// <summary>
    /// Main Entrance - 该类为JPush服务的主要入口
    /// </summary>
    public class JSMSClient
    { 
        public SMSClient _SMSClient;
        /// <param name="app_key">Portal上产生的app_key</param>
        /// <param name="masterSecret">你的API MasterSecret</param>
        public JSMSClient(String app_key, String masterSecret)
        {
            this._SMSClient = new SMSClient(app_key, masterSecret);
        } 
    }
}
