using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SysTemMeavn.Filters
{
    public class Authorization: AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (string.IsNullOrEmpty(filterContext.HttpContext.User.Identity.Name.Split('+')[0]) || string.IsNullOrEmpty(filterContext.HttpContext.User.Identity.Name.Split('+')[1]))
            {
                //filterContext.Result = new RedirectResult(UrlHelper.GenerateUrl("", "login", "Account", null, null, null, false));

                string returnUrl = filterContext.HttpContext.Server.UrlEncode(filterContext.HttpContext.Request.RawUrl);
                filterContext.Result = new RedirectResult("/login");
                filterContext.HttpContext.Response.Write("<script>parent.window.location = '/login?returnUrl=" + returnUrl + "';</script>");
                filterContext.HttpContext.Response.End();
            }
            // base.OnAuthorization(filterContext);
        }
    }
}