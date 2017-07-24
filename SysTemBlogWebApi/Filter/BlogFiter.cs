using Jusfoun.YunCompany.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;

namespace SysTemBlogWebApi.Filter
{
    /// <summary>
    /// 过滤器
    /// </summary>
    public class BlogFiter : System.Web.Http.Filters.ActionFilterAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            string Authorization= HttpContext.Current.Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(CookieHelper.Get((Authorization))))
            {
                HttpResponseMessage result = new HttpResponseMessage { Content = new System.Net.Http.StringContent("{\"Result\":0,\"msg\":\"登录失效\"}", Encoding.GetEncoding("UTF-8"), "application/json") };
                actionContext.Response = result;
            }
        }
    }
}