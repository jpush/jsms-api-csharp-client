using Newtonsoft.Json;
using System.Collections.Generic;

namespace Jiguang.JSMS.Model
{
    /// <summary>
    /// 模板短信。
    /// </summary>
    public class TemplateMessage
    {
        /// <summary>
        /// 手机号码。
        /// </summary>
        [JsonProperty("mobile")]
        public string Mobile { get; set; }

        /// <summary>
        /// 模板 Id。
        /// </summary>
        [JsonProperty("temp_id")]
        public int TemplateId { get; set; }

        /// <summary>
        /// 模板参数，需要替换的键值对。
        /// </summary>
        [JsonProperty("temp_para", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, string> TemplateParameters { get; set; }

        public override string ToString()
        {
            return GetJson();
        }

        private string GetJson()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings
            {
                DefaultValueHandling = DefaultValueHandling.Ignore
            });
        }
    }
}
