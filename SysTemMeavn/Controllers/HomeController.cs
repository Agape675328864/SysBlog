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
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            B_SysUser model = B_SysUser_Provider.GetModel(int.Parse(User.Identity.Name.Split('+')[0]));
            ViewBag.Pic = "/img.png";
            return View();
        }

        public ActionResult Income()
        {
            return View();
        }
    }
}