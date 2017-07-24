using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using SysTemModel.ResultModel;

namespace SysTemCommHelper
{
    public class SysBlogCommonProvider
    {
        /// <summary>
        /// 用户上传图片头像
        /// </summary>
        /// <param name="file"></param>
        /// <param name="type"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static bool PhoneUserSave(HttpPostedFile file, UpLoadImageType type, string filename = "")
        {
            string path = string.Empty;

            if (type == UpLoadImageType.UserHead)
            {
                path = Consts.UserImagePath;
            }

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (IsAllowedExtension(file))
            {
                file.SaveAs(path + filename);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 上传发帖图片
        /// </summary>
        /// <param name="file"></param>
        /// <param name="type"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static bool ArticleImgSave(HttpPostedFile file, UpLoadImageType type, string filename = "", string GuidPath = "")
        {
            string path = string.Empty;

            if (type == UpLoadImageType.ArticleImg)
            {
                path = string.Format(Consts.ArticleImgPath, GuidPath);
            }

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (IsAllowedExtension(file))
            {
                file.SaveAs(path + filename);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 校验图片后缀名是否正确
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool IsAllowedExtension(HttpPostedFile file)
        {
            string[] arrExtension = { ".gif", ".jpg", ".jpeg", ".bmp", ".png" };
            if (file.FileName != string.Empty)
            {
                string strOldFilePath = file.FileName;
                string strExtension = strOldFilePath.Substring(strOldFilePath.LastIndexOf(".", System.StringComparison.Ordinal));
                for (int i = 0; i < arrExtension.Length; i++)
                {
                    if (strExtension.Equals(arrExtension[i]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string CalculateMd5Hash(string input)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            foreach (byte t in hash)
            {
                sb.Append(t.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
