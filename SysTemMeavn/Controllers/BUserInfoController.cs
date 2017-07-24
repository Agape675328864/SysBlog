using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SysTemDAL;
using SysTemMeavn.Filters;
using SysTemModel;
namespace SysTemMeavn.Controllers
{
    [Authorization]
    public class BUserInfoController : Controller
    {
        // GET: BUserInfo
        public ActionResult BUserIndex()
        {
            string NickName = Request.Form["NickName"];
            int Page = Convert.ToInt32(Request.Form["Page"] ?? "0");
            Tuple<List<B_UserInfo>, int> list = B_UserInfo_Provider.GetUserListPage(Page + 1, 15, NickName);
            ViewBag.Count = list.Item2;
            ViewBag.NickName = NickName;
            ViewBag.Page = Page;
            return View(list.Item1);
        }
        /// <summary>
        /// 编辑获取用户信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditUserInfo(int Id)
        {
            return View(B_UserInfo_Provider.GetUserInfoListById(Id.ToString()).FirstOrDefault());
        }
        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditUserInfo(B_UserInfo model)
        {
            B_UserInfo NModel = B_UserInfo_Provider.GetUserInfoListById(model.Id.ToString()).FirstOrDefault();
            NModel.Gender = model.Gender;
            NModel.State = model.State;
            if (B_UserInfo_Provider.UpdateUserInfo(NModel))
            {
                return Json(new { state = "y", info = "修改成功！" });
            }
            else
            {
                return Json(new { state = "n", info = "修改失败！" });
            }
        }
    }
}