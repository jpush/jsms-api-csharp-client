using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;

namespace Jiguang.JSMS.Model
{
    /// <summary>
    /// 签名
    /// </summary>
    public class SignModel
    {
        /// <summary>
        /// 签名内容
        /// </summary>
        public string Sign { get; set; }
        /// <summary>
        /// 签名类型，填写数字代号即可
        /// </summary>
        public int? Type { get; set; }
        /// <summary>
        /// 请提供签名相关的资质证件图片
        /// </summary>
        public Stream Image0 { get; set; }
        /// <summary>
        /// 请简略描述您的业务使用场景，不超过100个字
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 请简略描述您的业务使用场景，不超过100个字
        /// </summary>
        public MultipartFormDataContent ToForm()
        {
            MultipartFormDataContent content = new MultipartFormDataContent
            {
                { new StringContent(this.Sign), "sign" }
            };
            if (this.Image0 != null)
            {
                content.Add(new StreamContent(this.Image0), "image0", "image0");
            }
            if (this.Type != null)
            {
                content.Add(new StringContent(Convert.ToString(this.Type)), "type");
            }

            if (this.Remark != null)
            {
                content.Add(new StringContent(this.Remark), "remark");
            }
            return content;
        }
    }
}
