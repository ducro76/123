using QLNCKH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace QLNCKH.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        DataQLNCKHDataContext db = new DataQLNCKHDataContext();
        // GET: Admin/Account
        [HttpGet]
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

            if (ac != null && ac.MaTypeAccount == 6)
            {
                Session["TaiKhoan"] = ac;
                GIANGVIEN ad = db.GIANGVIENs.SingleOrDefault(n => n.MaSoGiangVien == tk.ToString());
                Session["Admin"] = ad;
                return RedirectToAction("Index","Home");
            }        
            else
            {

                ViewBag.ThongBao = "Tài khoản hoặc mật khẩu không đúng";
            }
            return View();
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session["TaiKhoan"] = null;
            return RedirectToAction("Login", "Account");
        }
    }
}