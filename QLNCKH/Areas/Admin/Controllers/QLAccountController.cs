using QLNCKH.Models;
using QLNCKH.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace QLNCKH.Areas.Admin.Controllers
{
    public class QLAccountController : Controller
    {
        DataQLNCKHDataContext db = new DataQLNCKHDataContext();

        // List all accounts
        [HttpGet]
        public ActionResult Index()
        {
            var list = db.ACCOUNTs.OrderBy(a => a.MaAccount).ToList();
            return View(list);
        }

        // Import CSV
        [HttpPost]
        public ActionResult Import(HttpPostedFileBase csvfile)
        {
            if (csvfile == null || csvfile.ContentLength == 0)
            {
                TempData["Message"] = "Bạn Chưa Chọn File.";
                return RedirectToAction("Index");
            }

            using (var reader = new StreamReader(csvfile.InputStream, Encoding.GetEncoding("iso-8859-1")))
            {
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    var ac = new ACCOUNT();
                    ac.UserName = values.Length > 1 ? values[1] : null;
                    ac.Email = values.Length > 2 ? values[2] : null;
                    ac.Pass = values.Length > 3 ? values[3] : null;
                    ac.HoVaTen = values.Length > 4 ? values[4] : null;
                    if (values.Length > 5 && int.TryParse(values[5], out int t)) ac.MaTypeAccount = t;
                    db.ACCOUNTs.InsertOnSubmit(ac);
                    db.SubmitChanges();
                }
            }

            TempData["Message"] = "Thêm Dữ Liệu Thành Công.";
            return RedirectToAction("Index");
        }

        // Details
        [HttpGet]
        public ActionResult Details(int id)
        {
            var ac = db.ACCOUNTs.SingleOrDefault(a => a.MaAccount == id);
            if (ac == null) return HttpNotFound();
            return View(ac);
        }

        // Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ACCOUNT model)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(model.Pass)) model.Pass = GetMD5(model.Pass);
                db.ACCOUNTs.InsertOnSubmit(model);
                db.SubmitChanges();

                // Ensure corresponding profile records exist so newly created account can log in without null session objects.
                try
                {
                    // If student -> ensure SINHVIEN exists
                    if (model.MaTypeAccount.HasValue && model.MaTypeAccount.Value == 1)
                    {
                        var existSv = db.SINHVIENs.SingleOrDefault(s => s.MaSoSinhVien == model.UserName);
                        if (existSv == null)
                        {
                            var sv = new SINHVIEN()
                            {
                                MaSoSinhVien = model.UserName,
                                HoTen = model.HoVaTen,
                                Email = model.Email,
                                MaAccount = model.MaAccount
                            };
                            db.SINHVIENs.InsertOnSubmit(sv);
                            db.SubmitChanges();
                        }
                    }
                    // If giảng viên / quản lý / admin types -> ensure GIANGVIEN exists
                    else if (model.MaTypeAccount.HasValue && (new int[] { 2, 3, 4, 5, 6 }.Contains(model.MaTypeAccount.Value)))
                    {
                        var existGv = db.GIANGVIENs.SingleOrDefault(g => g.MaSoGiangVien == model.UserName);
                        if (existGv == null)
                        {
                            var gv = new GIANGVIEN()
                            {
                                MaSoGiangVien = model.UserName,
                                TenGiangVien = model.HoVaTen,
                                Gmail = model.Email,
                                MaAccount = model.MaAccount
                            };
                            db.GIANGVIENs.InsertOnSubmit(gv);
                            db.SubmitChanges();
                        }
                    }
                    // otherwise do not create profile automatically
                }
                catch
                {
                    // ignore profile creation errors so account creation still succeeds; admin can add profiles later
                }
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // Edit
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var ac = db.ACCOUNTs.SingleOrDefault(a => a.MaAccount == id);
            if (ac == null) return HttpNotFound();
            return View(ac);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ACCOUNT model)
        {
            var ac = db.ACCOUNTs.SingleOrDefault(a => a.MaAccount == model.MaAccount);
            if (ac == null) return HttpNotFound();
            if (ModelState.IsValid)
            {
                ac.UserName = model.UserName;
                ac.Email = model.Email;
                ac.HoVaTen = model.HoVaTen;
                ac.MaTypeAccount = model.MaTypeAccount;
                if (!string.IsNullOrEmpty(model.Pass)) ac.Pass = GetMD5(model.Pass);
                db.SubmitChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // Delete
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var ac = db.ACCOUNTs.SingleOrDefault(a => a.MaAccount == id);
            if (ac == null) return HttpNotFound();
            return View(ac);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(ACCOUNT model)
        {
            int id = model.MaAccount;
            var ac = db.ACCOUNTs.SingleOrDefault(a => a.MaAccount == id);
            if (ac == null) return HttpNotFound();
            try
            {
                // Remove references from dependent profiles so ACCOUNT can be deleted without FK errors.
                var svs = db.SINHVIENs.Where(s => s.MaAccount == id).ToList();
                foreach (var sv in svs)
                {
                    sv.MaAccount = null;
                }
                var gvs = db.GIANGVIENs.Where(g => g.MaAccount == id).ToList();
                foreach (var gv in gvs)
                {
                    gv.MaAccount = null;
                }
                db.SubmitChanges();

                db.ACCOUNTs.DeleteOnSubmit(ac);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Xóa tài khoản thất bại: " + ex.Message;
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        // Utility: MD5 hashing
        private static string GetMD5(string str)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                var fromData = System.Text.Encoding.UTF8.GetBytes(str);
                var targetData = md5.ComputeHash(fromData);
                var sb = new System.Text.StringBuilder();
                for (int i = 0; i < targetData.Length; i++)
                {
                    sb.Append(targetData[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
}
