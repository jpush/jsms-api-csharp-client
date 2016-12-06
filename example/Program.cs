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
    class Program
    {

        public static String app_key = "6be9204c30b9473e87bad4dc";
        public static String master_secret = "a564b268ba23631a8a34e687";

        public static void Main(string[] args)
        {
            Console.WriteLine("*****开始发送******");
            JSMSClient client = new JSMSClient(app_key, master_secret);
            SMSPayload user = new SMSPayload("13480600811", "1");
            String userjson = user.ToJson(user);
            Console.WriteLine(userjson);
            client._SMSClient.sendCodes(user);
            Console.ReadLine();
        }
    }
}
