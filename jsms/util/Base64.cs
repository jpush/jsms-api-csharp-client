using System;
using System.Text;

namespace jsms.util
{
    class Base64
    {
        public static String GetBase64Encode(String str)
        {
            byte[] bytes = Encoding.Default.GetBytes(str);
            return Convert.ToBase64String(bytes);
        }
    }
}
