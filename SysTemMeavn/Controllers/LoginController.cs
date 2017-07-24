using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SysTemCommHelper;
using SysTemDAL;
using SysTemModel;

namespace SysTemMeavn.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index(B_SysUser model)
        {
            if (Request.HttpMethod.Equals("GET", StringComparison.CurrentCultureIgnoreCase))
            {
                return View(model);
            }
            List<B_SysUser> list = B_SysUser_Provider.GetSysUserList(model.SysName, SysBlogCommonProvider.CalculateMd5Hash(model.Password));
            if (list.Count > 0)
            {
                B_SysUser Model = list.SingleOrDefault();
                string returnUrl = "";
                if (!string.IsNullOrEmpty(Request.QueryString["returnUrl"]))
                {
                    returnUrl = Request.QueryString["returnUrl"];
                }
                //保存登录用户id和用户名称
                FormsAuthentication.SetAuthCookie(string.Format("{0}+{1}", Model.Id, Model.SysName), false);
                returnUrl = Server.UrlDecode(returnUrl);
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Json(new { state = "y", returnUrl = returnUrl });
                }
                return Json(new { state = "y", returnUrl = Request.Url.ToString().Replace(Request.RawUrl, "") });
            }
            return Json(new { state = "n", info = "用户名或密码不存在！" });
        }
        /// <summary>
        /// 退出
        /// </summary>
        /// <returns></returns>
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }
    }
}