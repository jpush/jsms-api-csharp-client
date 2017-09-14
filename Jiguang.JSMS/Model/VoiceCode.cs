using Newtonsoft.Json;
using System.ComponentModel;

namespace Jiguang.JSMS.Model
{
    /// <summary>
    /// 语音验证码。
    /// </summary>
    public class VoiceCode
    {
        /// <summary>
        /// 手机号码，必填。
        /// </summary>
        [JsonProperty("mobile", Required = Required.Always)]
        public string Mobile { get; set; }

        /// <summary>
        /// 语音验证码的值，仅支持 4 - 8 个数字。
        /// </summary>
        [JsonProperty("code")]
        public int Code { get; set; }

        /// <summary>
        /// 语音验证码播报语言。
        /// 0：中文；1：英文；2：中英混合。
        /// </summary>
        [JsonProperty("voice_lang")]
        [DefaultValue(-1)]
        public int Languarge { get; set; }

        /// <summary>
        /// 验证码有效期。单位秒，默认为 60 秒。
        /// </summary>
        [JsonProperty("ttl")]
        public int Life { get; set; } = 60;

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
