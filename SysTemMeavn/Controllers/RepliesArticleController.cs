using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SysTemDAL;
using SysTemModel;

namespace SysTemMeavn.Controllers
{
    public class RepliesArticleController : Controller
    {
        // GET: RepliesArticle
        public ActionResult RepliesArticleIndex()
        {
            string Title = Request.Form["Title"];
            string NickName = Request.Form["NickName"];
            int Page = Convert.ToInt32(Request.Form["Page"] ?? "0");
            string fiter = " re.State in (0,1)";
            if (!string.IsNullOrEmpty(Title))
            {
                fiter += " and ar.Title like '%" + Title + "%'";
            }
            if (!string.IsNullOrEmpty(NickName))
            {
                fiter += " and us.NickName like '%" + NickName + "%'";
            }
            Tuple<List<RepliesArticleTemp>, int> list = B_Article_Provider.RepliesArticleTemp(Page + 1, 15, fiter);
            ViewBag.Count = list.Item2;
            ViewBag.Title = Title;
            ViewBag.NickName = NickName;
            ViewBag.Page = Page;
            return View(list.Item1);
        }

        public ActionResult UpdateReArticleS(int id, int state)
        {
            B_RepliesArticle model = new B_RepliesArticle() { Id = id, State = state };
            if (B_Article_Provider.UpdateReAritcleStatus(model))
            {
                return Json(new { state = "y", info = "修改成功！" });
            }
            else
            {
                return Json(new { state = "y", info = "修改成功！" });
            }

        }
    }
}