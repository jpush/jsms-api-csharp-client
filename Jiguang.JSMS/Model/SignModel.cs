using System;
using System.Collections.Generic;
using System.IO;
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
        /// 签名审核附带图片
        /// </summary>
        public Stream Image0 { get; set; }
        /// <summary>
        /// 签名审核附带图片
        /// </summary>
        public Stream Image1 { get; set; }
        /// <summary>
        /// 签名审核附带图片
        /// </summary>
        public Stream Image2 { get; set; }
        /// <summary>
        /// 签名审核附带图片
        /// </summary>
        public Stream Image3 { get; set; }
    }
}
