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
    class SendVoiceCodeExample
    {

        public static String app_key = "6be9204c30b9473e87bad4dc";
        public static String master_secret = "your master_secret";

        public static void Main(string[] args)
        {
            Console.WriteLine("*****开始发送******");

            JSMSClient client = new JSMSClient(app_key, master_secret);

            // 语音短信验证码 API
            // API文档地址 http://docs.jiguang.cn/jsms/server/rest_api_jsms/#api_1

            Voice_codesPayload voice_codes = new Voice_codesPayload("134888888888", 60);
            String voice_codesjson = voice_codes.ToJson(voice_codes);
            Console.WriteLine(voice_codesjson);
            client._SMSClient.sendVoice_codes(voice_codesjson);
            
            Console.ReadLine();
        }
    }
}
