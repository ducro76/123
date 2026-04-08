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
        // GET: Admin/QLAccount
        DataQLNCKHDataContext db = new DataQLNCKHDataContext();

        public ActionResult Index(HttpPostedFileBase csvfile)
        {
            // Kiểm tra nếu không có file được chọn
            if (csvfile == null || csvfile.ContentLength == 0)
            {
                ViewBag.Message = "Bạn Chưa Chọn File.";
                return View();
            }

            // Đọc dữ liệu từ file CSV
            using (var reader = new StreamReader(csvfile.InputStream, Encoding.GetEncoding("iso-8859-1")))
            {
                // Bỏ qua dòng tiêu đề
                reader.ReadLine();

                // Duyệt qua từng dòng dữ liệu trong file CSV
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    // Tạo một đối tượng SinhVien từ dữ liệu trong file CSV
                    var ac = new ACCOUNT();

                    ac.UserName = values[1];
                    ac.Email = values[2];
                    ac.Pass = values[3];
                    ac.HoVaTen = values[4];
                    ac.MaTypeAccount = Convert.ToInt32(values[5]);
    
                    // Thêm đối tượng SinhVien vào database
                    db.ACCOUNTs.InsertOnSubmit(ac);
                    db.SubmitChanges();
                }
            }

            ViewBag.Message = "Thêm Dữ Liệu Thành Công.";
            return View();
        }
    }
}