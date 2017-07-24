using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Jusfoun.YunCompany.Common
{
    public class CookieHelper
    {
        public static string GetCookieName(string key)
        {
            if (!String.IsNullOrEmpty(key))
            {
                //读取cookie前缀
                string cookiePre = Tool.GetValueFromWebConfig("CookiePre");
                if (!String.IsNullOrEmpty(cookiePre)) key = cookiePre + key;
            }
            return key;
        }
        /// <summary>
        /// 公用添加cookie
        /// </summary>
        /// <param name="cookie"></param>
        private static void Set(HttpCookie cookie)
        {
            HttpResponse response = HttpContext.Current.Response;
            if (response != null)
            {
                //指定客户端脚本是否可以访问【默认为false,true不可访问】
                cookie.HttpOnly = false;
                //指定统一的path
                cookie.Path = "/";
                //设置跨域，在二级域名下也可以访问
                //cookie.Domain = "";//例如:sohu.com
                //设置cookie的到期时间
                //cookie.Expires.AddDays(7);
                response.AppendCookie(cookie);
            }
        }
        /// <summary>
        /// 添加cookie
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Set(string key, string value)
        {
            key = GetCookieName(key);
            Set(new HttpCookie(key, value));
        }
        /// <summary>
        /// 添加cookie，设置到期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expires"></param>
        public static void Set(string key, string value, DateTime expires)
        {
            key = GetCookieName(key);
            HttpCookie cookie = new HttpCookie(key, value);
            cookie.Expires = expires;
            Set(cookie);
        }
        /// <summary>
        /// 添加cookie，设置到期时间戳
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timestamp"></param>
        public static void Set(string key, string value, int timestamp)
        {
            key = GetCookieName(key);
            HttpCookie cookie = new HttpCookie(key, value);
            cookie.Expires = Calendar.StrToTime(timestamp);
            Set(cookie);
        }
        private static string Get(HttpCookie cookie, string key)
        {
            key = GetCookieName(key);
            if (cookie != null)
            {
                if (!string.IsNullOrEmpty(key) && cookie.HasKeys)
                    return cookie.Values[key];
                else
                    return cookie.Value;
            }

            return "";
        }
        /// <summary>
        /// 获取cookie
        /// </summary>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        public static string Get(string cookieName)
        {
            cookieName = GetCookieName(cookieName);
            return Get(cookieName, null);
        }
        /// <summary>
        /// 获取cookie
        /// </summary>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        public static string Get(string cookieName, string key)
        {
            HttpRequest request = HttpContext.Current.Request;
            if (request != null)
                return Get(request.Cookies[cookieName], key);
            return "";
        }



        /// <summary>
        /// 删除cookie
        /// </summary>
        /// <param name="cookieName"></param>
        public static void Delete(string cookieName)
        {
            cookieName = GetCookieName(cookieName);
            HttpCookie cookie = new HttpCookie(cookieName);
            cookie.Expires = DateTime.Now.AddDays(-3d);
            cookie.Path = "/";
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        public static void DeleteOne(string cookieName)
        {

            HttpCookie cookie = new HttpCookie(cookieName);
            cookie.Expires = DateTime.Now.AddDays(-3d);
            cookie.Path = "/";
            HttpContext.Current.Response.AppendCookie(cookie);
        }
        /// <summary>
        /// 删除所有cookie
        /// </summary>
        public static void DeleteAll()
        {
            HttpCookie aCookie;
            string cookieName;
            int limit = HttpContext.Current.Request.Cookies.Count;
            for (int i = 0; i < limit; i++)
            {
                cookieName = HttpContext.Current.Request.Cookies[i].Name;
                aCookie = new HttpCookie(cookieName);
                aCookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(aCookie);
            }
        }

    }
}
