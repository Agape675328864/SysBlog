using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SysTemDAL;
using SysTemModel;

namespace SysTemMeavn.Controllers
{
    public class BArticleInfoController : Controller
    {
        // GET: BArticleInfo
        public ActionResult BArticleIndex()
        {
            
            string Title = Request.Form["Title"];
            int Page = Convert.ToInt32(Request.Form["Page"] ?? "0");
            string fiter = " State in (0,1)";
            if (!string.IsNullOrEmpty(Title))
            {
                fiter += " and Title like '%" + Title + "%'";
            }
            Tuple<List<B_Article>, int> list = B_Article_Provider.GetArticleList(Page + 1, 15, fiter);
            ViewBag.Count = list.Item2;
            ViewBag.Title = Title;
            ViewBag.Page = Page;
            return View(list.Item1);
        }
        /// <summary>
        /// 文章详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult DetaileArticleInfo(int Id)
        {
            List<B_Article> list = B_Article_Provider.ArticleList(Id);
            return View(list);
        }

    }
}