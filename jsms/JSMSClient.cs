using System;
using jsms.sms;

namespace jsms
{
    // Main Entrance - 该类为 JSMS 服务的主要入口
    public class JSMSClient
    { 
        public SMSClient _SMSClient;

        public JSMSClient(String appKey, String masterSecret)
        {
            _SMSClient = new SMSClient(appKey, masterSecret);
        } 
    }
}
