using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysTemCommHelper
{
    public class Consts
    {
        /// <summary>
        /// 上传用户头像地址
        /// </summary>
        public static readonly string UserImagePath = ConfigurationManager.AppSettings["imagesource-user"];
        /// <summary>
        /// 用户头像显示地址
        /// </summary>
        public static readonly string UserPhotoPath = ConfigurationManager.AppSettings["UserURLPath"];
        /// <summary>
        /// 上传发帖图片
        /// </summary>
        public static readonly string ArticleImgPath = ConfigurationManager.AppSettings["imagesource-Article"];
        /// <summary>
        /// 发帖图片显示
        /// </summary>
        public static readonly string ArticlePath = ConfigurationManager.AppSettings["ArticlePath"];
    }
}
