using jsms;
using jsms.common;
using jsms.sms;
using System;
using System.Collections.Generic;

namespace example
{
    class ScheduleExample
    {
        public static string app_key = "";
        public static string master_secret = "";

        public static void Main(string[] args)
        {
            JSMSClient client = new JSMSClient(app_key, master_secret);

            SMSPayload payload = new SMSPayload("18611111111", 1)
            {
                SendTime = new DateTime(2017, 8, 2),    // 定时任务必须设置发送时间，若未设置则默认立即发送。
                TempParameter = new Dictionary<string, string>
                {
                    {"code", "111"}
                }
            };

            ResponseWrapper result = client._SMSClient.SendMessages(payload);

            Console.WriteLine("result.responseContent");
            Console.WriteLine(result.ResponseContent);

            Console.ReadLine();
        }
    }
}
