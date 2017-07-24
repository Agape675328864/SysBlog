/*----------------------------------------------------------------
 * Copyright (C) 2017 GoGoTalk青少外教在线英语版权所有。 
 * 文件名：AliYn
 * 文件功能描述：

 * 创建者：杨红彬 (Administrator)
 * 时间：2017/7/1 17:24:52
 * 修改人：
 * 时间：
 * 修改说明：
 * 版本：V1.0.0
----------------------------------------------------------------*/
using Agape.Xml.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Profile;
using System.Web.Routing;

namespace SysTemCommHelper
{

    public class AliYn
    {
        /// <summary>
        /// 签名加密
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string PercentEncode(string value)
        {
            return UpperCaseUrlEncode(value)
                .Replace("+", "%20")
                .Replace("*", "%2A")
                .Replace("%7E", "~");
        }
        /// <summary>
        /// 签名加密
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string UpperCaseUrlEncode(string s)
        {
            char[] temp = HttpUtility.UrlEncode(s).ToCharArray();
            for (int i = 0; i < temp.Length - 2; i++)
            {
                if (temp[i] == '%')
                {
                    temp[i + 1] = char.ToUpper(temp[i + 1]);
                    temp[i + 2] = char.ToUpper(temp[i + 2]);
                }
            }
            return new string(temp);
        }
        /// <summary>
        /// ISO8601规范的UTC格式
        /// </summary>
        /// <returns></returns>
        public static string FormatISO8601Date()
        {
            DateTime dt = DateTime.Now;
            return dt.AddHours(-8).ToString("yyyy-MM-ddTHH:mm:ssZ");
        }


        /// <summary>
        /// 除去数组中的空值和签名参数并以字母a到z的顺序排序
        /// </summary>
        /// <param name="dicArrayPre">过滤前的参数组</param>
        /// <returns>过滤后的参数组</returns>
        public static Dictionary<string, string> FilterParaZHIFU(SortedDictionary<string, string> dicArrayPre)
        {
            Dictionary<string, string> dicArray = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> temp in dicArrayPre)
            {
                dicArray.Add(temp.Key, temp.Value);
            }

            return dicArray;
        }

        /// <summary>
        /// 组建参数
        /// </summary>
        /// <param name="sParaTemp"></param>
        /// <returns></returns>
        public static string BuildRequest(SortedDictionary<string, string> sParaTemp)
        {
            //待请求参数数组
            Dictionary<string, string> dicPara = new Dictionary<string, string>();
            dicPara = FilterParaZHIFU(sParaTemp);

            StringBuilder sbBuild = new StringBuilder();
            foreach (KeyValuePair<string, string> temp in dicPara)
            {
                sbBuild.Append(temp.Key + "=" + (temp.Value) + "&");
            }
            return sbBuild.ToString();
        }

        /// <summary>
        /// 获取配置XML文件阿里大鱼短信发送接口地址
        /// </summary>
        /// <returns></returns>
        public static HttpClient GetHttpClientJson()
        {
            string url = XmlHelper.GetSNSTemplate("SmsUrl", string.Format("{0}/Xml/app.xml", System.AppDomain.CurrentDomain.BaseDirectory));
            return new HttpClient { BaseAddress = new Uri(url) };
        }
        /// <summary>
        /// 获取配置XML文件信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetHttpClientJson(string key)
        {
            return XmlHelper.GetSNSTemplate(key, string.Format("{0}/Xml/app.xml", System.AppDomain.CurrentDomain.BaseDirectory));
        }

        /// <summary>
        /// 阿里大鱼发送短信请求
        /// </summary>
        /// <returns></returns>
        public static string HttpGetAlidy(string DictionarIes)
        {
            string Msg = string.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(GetHttpClientJson("SmsUrl") + "?" + DictionarIes);
            try
            {
                using (Stream data = request.GetResponse().GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(data))
                    {
                        Msg = reader.ReadToEnd();
                    }
                }
            }
            catch (WebException ex)
            {
                HttpWebResponse response = (HttpWebResponse)ex.Response;
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    using (Stream data = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(data))
                        {
                            Msg = reader.ReadToEnd();
                        }
                    }
                }
            }
            Msg = Msg.Replace('\\', ' ').Replace(" ", "");
            return Msg;
        }

    }
}
