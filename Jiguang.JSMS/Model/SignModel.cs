using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Jiguang.JSMS.Model
{
    /// <summary>
    /// 签名类型
    /// </summary>
    public enum SignType
    {

        /// <summary>
        /// 1:公司名称全称或简称： 需提供营业执照
        /// </summary>
        CompanyName = 1,
        /// <summary>
        /// 2:工信部备案的网站全称或简称： 需提供主办单位的营业执照
        /// </summary>
        WebsiteName,
        /// <summary>
        /// 3:APP应用全称或简称： 需提供任一应用商店的下载链接及开发者的营业执照
        /// </summary>
        AppName,
        /// <summary>
        /// 4:公众号或小程序名称的全称或简称： 需提供公众号（小程序）主体营业执照
        /// </summary>
        MiniApplet,
        /// <summary>
        /// 5:电商平台店铺全称或简称： 需提供经营者营业执照
        /// </summary>
        EShopName,
        /// <summary>
        /// 6:商标名称全称或简称： 需提供商标注册证书
        /// </summary>
        BrandName,
        /// <summary>
        /// 7:其他： 需提供所有者营业执照
        /// </summary>
        Other
    }
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
        /// 签名类型
        /// </summary>
        public SignType Type { get; set; }
        /// <summary>
        /// 签名审核附带图片
        /// </summary>
        public Stream Image { get; set; }
        /// <summary>
        /// 业务使用场景，不超多100字
        /// </summary>
        public string Remark { get; set; }
    }
}
