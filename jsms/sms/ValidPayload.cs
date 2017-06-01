using Newtonsoft.Json;

namespace jsms.sms
{
    public class ValidPayload
    {
        public string code;

        public ValidPayload(string code)
        {
            this.code = code;
        }

        public string ToJson(ValidPayload code)
        {
            return JsonConvert.SerializeObject(code, Formatting.None, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }

        public ValidPayload Check()
        {
            return this;
        }
    }
}
