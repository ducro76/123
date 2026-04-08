using QLNCKH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNCKH.Areas.Admin.Controllers
{
    public class QLThongBaoController : Controller
    {
        // GET: Admin/QLThongBao
        DataQLNCKHDataContext db = new DataQLNCKHDataContext();
        public ActionResult Index()
        {
            var ds = db.THONGBAOs.ToList().OrderByDescending(n => n.Id).Take(10);
            return View(ds);
        }
        public ActionResult Create()
        {           
            return View();
        }
        [HttpPost]
        public ActionResult Create(THONGBAO model)
        {
            if (ModelState.IsValid)
            {
                // Thêm dữ liệu vào database
                using (db)
                {
                    model.NgayGui = DateTime.Now;
                    model.NguoiNhan = 1;
                    db.THONGBAOs.InsertOnSubmit(model);
                    db.SubmitChanges();
                }
                return RedirectToAction("Index"); // Chuyển hướng đến trang index
            }
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var tb = db.THONGBAOs.Where(n => n.Id == id).SingleOrDefault();
            return View(tb);
        }
        [HttpPost]
        [ValidateInput (false)]
        public ActionResult Edit(int id , THONGBAO t)
        {
            t = db.THONGBAOs.Where(n => n.Id == id).SingleOrDefault();
            t.TenThongBao = Request.Form["TenThongBao"];
            t.NoiDungTb = Request.Form["NoiDungTb"];
            t.NgayGui = DateTime.Now;
            
            db.SubmitChanges();
            return RedirectToAction("Index"); // Chuyển hướng đến trang index
        }
        public ActionResult Delete(int id)
        {
            var tb = db.THONGBAOs.Where(n => n.Id == id).SingleOrDefault();
            return View(tb);
        }
        [HttpPost]
        public ActionResult Delete(int id ,THONGBAO t)
        {
            t = db.THONGBAOs.Where(n => n.Id == id).SingleOrDefault();
            db.THONGBAOs.DeleteOnSubmit(t);
            db.SubmitChanges();
            return RedirectToAction("Index"); // Chuyển hướng đến trang index
        }
    }
}