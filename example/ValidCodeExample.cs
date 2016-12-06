using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using jsms.common;
using jsms.util;
using jsms.sms;
using jsms;

namespace example
{
    class ValidCodeExample
    {
        public static String app_key = "6be9204c30b9473e87bad4dc";
        public static String master_secret = "a564b268ba23631a8a34e687";

        public static void Main(string[] args)
        {
            Console.WriteLine("*****开始发送******");

            JSMSClient client = new JSMSClient(app_key, master_secret);

            // 验证 API
            // API文档地址 http://docs.jiguang.cn/jsms/server/rest_api_jsms/#api_2
            string msg_id = "27c8295f-3764-45b7-8c2b-f15047082c89";
            ValidPayload codes = new ValidPayload("909190");
            String codesjson = codes.ToJson(codes);
            Console.WriteLine(codesjson);
            client._SMSClient.validCodes(codes,msg_id);
            Console.ReadLine();
        }
    }
}
