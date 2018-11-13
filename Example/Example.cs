using Jiguang.JSMS;
using Jiguang.JSMS.Model;
using System;
using System.Collections.Generic;
using System.IO;

namespace Example
{
    class Example
    {
        static JSMSClient jsmsClient = new JSMSClient("AppKey", "MasterSecret");

        static void Main(string[] args)
        {
            SendCode();
            SendTemplateMessageByTime();
            CheckAccountBalance();
            CreateMessageTemplate();
            AddSign();

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

        /// <summary>
        /// 查询应用余量，应用余量指分配给某个应用单独使用的短信余量。
        /// </summary>
        private static void CheckAppBalance()
        {
            HttpResponse httpResponse = jsmsClient.CheckAppBalance();
            Console.WriteLine(httpResponse.Content);
        }

        /// <summary>
        /// 创建短信模板示例。
        /// </summary>
        private static void CreateMessageTemplate()
        {
            HttpResponse httpResponse = jsmsClient.CreateMessageTemplate(new TemplateMessage
            {
                Content = "您好，您的验证码是{{code}}，1分钟内有效！",  // 短信内容。
                Type = 1,                                            // 代表验证码类型。
                ValidDuration = 60,                                  // 有效时间为 60 秒。
                Remark = "此模板用于用户注册"                         // 备注。不会显示在短信中。
            });
            Console.WriteLine(httpResponse.Content);
        }


        /// <summary>
        /// 新增签名
        /// </summary>
        private static async void AddSign()
        {
            using (var img = File.OpenRead("Kendo001.jpg"))
            {
                var httpResponse = await jsmsClient.AddSignAsync(new SignModel
                {
                    Sign = "个性签名",
                    //Image0=img,
                    //Image1=img,
                });

                Console.WriteLine(httpResponse.Content);
            }    
        }
    }
}
