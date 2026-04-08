using QLNCKH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace QLNCKH.Controllers
{
 
    public class UserController : Controller
    {
        DataQLNCKHDataContext db = new DataQLNCKHDataContext();
        // GET: User
        public ActionResult Login()
        {
            return View();
        }

        public static string GetMD5(string str)
        {

            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");
            }
            return byte2String;
        }
        [HttpPost]
        public ActionResult Login(FormCollection f, ACCOUNT tkkh)
        {
            var tk = f["username"];
            var mk = f["password"];
            ACCOUNT ac = db.ACCOUNTs.SingleOrDefault(n => n.UserName == tk && n.Pass == GetMD5(mk));
           
            if (ac != null && ac.MaTypeAccount == 1)
            {
                Session["TaiKhoan"] = ac;
                SINHVIEN sv = db.SINHVIENs.SingleOrDefault(n => n.MaSoSinhVien == tk.ToString());
                Session["SinhVien"] = sv;
                return RedirectToAction("StudentDB", "StudentDB");
            }
            else if (ac != null && ac.MaTypeAccount == 2)
            {
                Session["TaiKhoan"] = ac;
                GIANGVIEN gv = db.GIANGVIENs.SingleOrDefault(n => n.MaSoGiangVien == tk.ToString());
                Session["GiangVien"] = gv;
                return RedirectToAction("Index", "GiangVien");
            }
            else if (ac != null && ac.MaTypeAccount == 5)
            {
                Session["TaiKhoan"] = ac;
                GIANGVIEN ql = db.GIANGVIENs.SingleOrDefault(n => n.MaSoGiangVien == tk.ToString());
                Session["GiangVien"] = ql;
                return RedirectToAction("Index", "QuanLyTong");
            }
            else if (ac != null && ac.MaTypeAccount == 3)
            {
                Session["TaiKhoan"] = ac;
                GIANGVIEN ql = db.GIANGVIENs.SingleOrDefault(n => n.MaSoGiangVien == tk.ToString());
                Session["GiangVien"] = ql;
                return RedirectToAction("Index", "QuanLy");
            }
            else if (ac != null && ac.MaTypeAccount == 4)
            {
                Session["TaiKhoan"] = ac;
                GIANGVIEN ql = db.GIANGVIENs.SingleOrDefault(n => n.MaSoGiangVien == tk.ToString());
                Session["GiangVien"] = ql;
                return RedirectToAction("Index", "QuanLyVien");
            }
            else
            {
                ViewBag.Message = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
        public ActionResult SuccessPage()
        {
            return View();
        }
    }
}