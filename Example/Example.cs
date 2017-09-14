using Jiguang.JSMS;
using Jiguang.JSMS.Model;
using System;
using System.Collections.Generic;

namespace Example
{
    class Example
    {
        static JSMSClient jsmsClient = new JSMSClient("Your app's AppKey", "Your app's MasterSecret");

        static void Main(string[] args)
        {
            SendCode();
            SendTemplateMessageByTime();
            CheckAccountBalance();

            Console.ReadLine();
        }

        /// <summary>
        /// 发送文本验证码短信。
        /// <see cref="https://docs.jiguang.cn/jsms/server/rest_api_jsms/#api_1"/>
        /// </summary>
        private static void SendCode()
        {
            HttpResponse httpResponse = jsmsClient.SendCode("13000001111", 1);
            Console.WriteLine(httpResponse.Content);
        }

        /// <summary>
        /// 定时发送验证码短信。
        /// <see cref="https://docs.jiguang.cn/jsms/server/rest_api_jsms_schedule/#api_1"/>
        /// </summary>
        private static void SendTemplateMessageByTime()
        {
            HttpResponse httpResponse = jsmsClient.SendTemplateMessageByTime("2017-09-12 14:10:00", new TemplateMessage
            {
                Mobile = "13000001111",
                TemplateId = 1,
                TemplateParameters = new Dictionary<string, string>
                {
                    { "code", "1243" }
                }
            });

            Console.WriteLine(httpResponse.Content);
        }

        /// <summary>
        /// 查询账号余量，账号余量指未分配给某个应用，属于账号共享的短信余量。
        /// <see cref="https://docs.jiguang.cn/jsms/server/rest_jsms_api_account/#api_1"/>
        /// </summary>
        private static void CheckAccountBalance()
        {
            HttpResponse httpResponse = jsmsClient.CheckAccountBalance("Your DevKey", "Your API DevSecret");
            Console.WriteLine(httpResponse.Content);
        }

    }
}
