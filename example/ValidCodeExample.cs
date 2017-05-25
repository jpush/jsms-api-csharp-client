using System;
using jsms.sms;
using jsms;

namespace example
{
    class ValidCodeExample
    {
        public static String app_key = "6be9204c30b9473e87bad4dc";
        public static String master_secret = "your master_secret";

        public static void Main(string[] args)
        {
            Console.WriteLine("*****开始发送******");

            JSMSClient client = new JSMSClient(app_key, master_secret);

            // 验证 API
            // API文档地址 http://docs.jiguang.cn/jsms/server/rest_api_jsms/#api_2
            string msg_id = "227effab-b00c-4569-8021-220a036b7ae6";
            ValidPayload codes = new ValidPayload("121806");
            String codesjson = codes.ToJson(codes);
            Console.WriteLine(codesjson);
            client._SMSClient.validCodes(codes, msg_id);
            Console.ReadLine();
        }
    }
}
