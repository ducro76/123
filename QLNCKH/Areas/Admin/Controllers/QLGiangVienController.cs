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
    public class QLGiangVienController : Controller
    {
        // GET: Admin/QLGiangVien
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
                    var gv = new GIANGVIEN();

                    gv.MaSoGiangVien = values[1];
                    gv.TenGiangVien = values[2];
                    gv.Nganh = values[3];
                    gv.TrinhDo = values[4];
                    gv.NgaySinh = Convert.ToDateTime(values[5]);
                    gv.MaAccount = Convert.ToInt32(values[6]);
                    gv.MaKhoa = values[7];
                    gv.Gmail = values[8];
                    gv.MaCT = values[9];                 
                    // Thêm đối tượng SinhVien vào database
                    db.GIANGVIENs.InsertOnSubmit(gv);
                    db.SubmitChanges();
                }
            }

            ViewBag.Message = "Thêm Dữ Liệu Thành Công.";
            return View();
        }
    }
}