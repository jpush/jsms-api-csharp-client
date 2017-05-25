﻿using System;
using jsms.common;
using jsms.sms;
using jsms;

namespace example
{
    class SendCodeExample
    {
        public static String app_key = "6be9204c30b9473e87bad4dc";
        public static String master_secret = "your master_secret";

        public static void Main(string[] args)
        {
            Console.WriteLine("*****开始发送******");

            JSMSClient client = new JSMSClient(app_key, master_secret);

            // 短信验证码 API
            // API文档地址 http://docs.jiguang.cn/jsms/server/rest_api_jsms/#api

            SMSPayload codes = new SMSPayload("134888888888", 1);
            String codesjson = codes.ToJson(codes);
            Console.WriteLine(codesjson);
            ResponseWrapper result = client._SMSClient.sendCodes(codesjson);
            Console.WriteLine("result.responseContent");
            Console.WriteLine(result.responseContent);

            Console.ReadLine();
        }
    }
}
