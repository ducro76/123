using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNCKH.Controllers
{
    public class TrangChuController : Controller
    {
        // GET: Home
        public ActionResult TrangChu()
        {
            return View();
        }
        public ActionResult PartialNav()
        {
            return PartialView();
        }
        public ActionResult PartialFooter()
        {
            return PartialView();
            
        }
        public ActionResult PartialTopNav()
        {
            return PartialView();
        }

        public ActionResult About()
        {
            return View();
        }
        public ActionResult TinTuc()
        {
            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult ThongBao()
        {
            return View();
        }

    }
}