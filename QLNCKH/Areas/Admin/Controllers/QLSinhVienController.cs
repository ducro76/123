using QLNCKH.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace QLNCKH.Areas.Admin.Controllers
{
    public class QLSinhVienController : Controller
    {
        // GET: Admin/QLSinhVien
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
                    var sv = new SINHVIEN();

                    sv.MaSoSinhVien = values[1];
                    sv.HoTen = values[2];
                    sv.MaNganh = values[3];
                    sv.NgaySinh = Convert.ToDateTime(values[4]);
                    sv.CCCD = values[5];
                    sv.TKNganHang = values[6];
                    sv.SDT = values[7];
                    sv.Lop = values[8];
                    sv.NienKhoa = values[9];
                    sv.GioiTinh = values[10] == "1";
                    sv.DiaChi = values[11];
                    sv.ChiNhanhNganHang = values[12];
                    sv.MaAccount = Convert.ToInt32(values[13]);
                    sv.MaNhom = int.Parse(values[14]);
                    sv.Email = values[15];
                

                    // Thêm đối tượng SinhVien vào database
                    db.SINHVIENs.InsertOnSubmit(sv);
                    db.SubmitChanges();
                }
            }

            ViewBag.Message = "Thêm Dữ Liệu Thành Công.";
            return View();
        }

    }
}