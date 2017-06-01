using System;
using System.Collections.Generic;
using jsms.sms;
using jsms;

namespace example
{
    public class SendMessagesExample
    {
        public static String app_key = "fc8607f45edb65e477c25430";
        public static String master_secret = "your master_secret";

        public static void Main(string[] args)
        {
            Console.WriteLine("*****开始发送******");

            JSMSClient client = new JSMSClient(app_key, master_secret);

            // 短信验证码 API
            // API文档地址 http://docs.jiguang.cn/jsms/server/rest_api_jsms/#api_3

            Dictionary<string, string> temp_para = new Dictionary<string, string>();
            temp_para.Add("codes", "1914");
            string para = temp_para.ToString();
            Console.WriteLine(para);

            SMSPayload codes = new SMSPayload("134888888888", 9890, temp_para);
            String codesjson = codes.ToJson(codes);
            Console.WriteLine(codesjson);
            client._SMSClient.SendMessages(codesjson);

            Console.ReadLine();
        }
    }
}
