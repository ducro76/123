using QLNCKH.Models;
using QLNCKH.Models.DAO;
using QLNCKH.Models.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace QLNCKH.Controllers
{
    public class QuanLyController : Controller
    {
        DataQLNCKHDataContext db = new DataQLNCKHDataContext();
        // GET: QuanLy
        public ActionResult PartialMenuQL()
        {
            return PartialView();
        }
        public ActionResult Index()
        {
            var tb = db.THONGBAOs.OrderByDescending(s => s.Id).ToList().Take(10);
            return View(tb);
        }
        public ActionResult DetailTBQL(int id)
        {
            var tb = db.THONGBAOs.Where(a => a.Id == id).Single();
            return View(tb);
        }
        public GIANGVIEN LayGiangVien()
        {
            GIANGVIEN ql = Session["GiangVien"] as GIANGVIEN;
            return ql;
        }
        [HttpGet]
        public JsonResult DsHoiDong()
        {
            try
            {
                GIANGVIEN ql = LayGiangVien();
                var DsHD = (from hd in db.HOIDONGDUYETDECUONGs
                            where hd.MaKhoa == ql.MaKhoa 
                            select new
                            {
                                hd.MaHoiDong,
                                hd.TenHoiDong,
                                hd.MaKhoa,
                                hd.SoLuongTV
                            }

                          ).ToList();

                return Json(new { code = 200, hd = DsHD, msg = "Lay thong tin thanh cong." },
                        JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lay thong tin that bai " + ex.Message },
                   JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult DsHoiDongNT()
        {
            try
            {
                GIANGVIEN ql = LayGiangVien();
                var DsHD = (from hd in db.HOIDONGNGHIEMTHUs
                            where hd.MaKhoa == ql.MaKhoa
                            select new
                            {
                                hd.MaHoiDong,
                                hd.TenHoiDong,
                                hd.MaKhoa,
                                hd.SoLuongTV
                            }

                          ).ToList();

                return Json(new { code = 200, hd = DsHD, msg = "Lay thong tin thanh cong." },
                        JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lay thong tin that bai " + ex.Message },
                   JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult ThemPCDeTai(int idDT,int idHD)
        {
            try
            {
                var dt = db.DANGKies.SingleOrDefault(n => n.IDDangKy == idDT);
                dt.MaHoiDong = idHD;
                dt.TrangThai = 2;
                db.SubmitChanges();
                return Json(new { code = 200, msg = "Phân công đề tài thành công!" },
                        JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Khó quá! Bó tay  " + ex.Message },
                   JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult ThemPCDeTaiNT(int idDT, int idHD)
        {
            try
            {
                var dt = db.DETAIs.SingleOrDefault(n => n.MaDeTai == idDT);
                dt.MaHoiDong = idHD;
                dt.MaTrangThai = 4;
                db.SubmitChanges();
                return Json(new { code = 200, msg = "Phân công đề tài thành công!" },
                        JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Khó quá! Bó tay  " + ex.Message },
                   JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult XoaDeTai(int idDT)
        {
            try
            {
                var dt = db.DANGKies.SingleOrDefault(c => c.IDDangKy == idDT);
                db.DANGKies.DeleteOnSubmit(dt);
                db.SubmitChanges();
                return Json(new { code = 200, msg = "Xóa đề tài thành công!" },
                        JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Khó quá! Bó tay  " + ex.Message },
                   JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult XoaDeTaiNT(int idDT)
        {
            try
            {
                var dt = db.DETAIs.SingleOrDefault(c => c.MaDeTai == idDT);
                db.DETAIs.DeleteOnSubmit(dt);
                db.SubmitChanges();
                return Json(new { code = 200, msg = "Xóa đề tài thành công!" },
                        JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Khó quá! Bó tay  " + ex.Message },
                   JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult DsDeTai(string strLoc1, string strLoc2)
        {

            GIANGVIEN ql = LayGiangVien();
            List<DTDangKy> listDangKy = DangKyDAO.Instance.ListDangKyTheoMaCTDefault(ql.MaCT);
            if (string.IsNullOrEmpty(strLoc1) && string.IsNullOrEmpty(strLoc2))
            {
               
                return View(listDangKy); 
            }
            else if(string.IsNullOrEmpty(strLoc1) || string.IsNullOrEmpty(strLoc2))
            {              
                return View(listDangKy);
            }    
            else 
            {
                List<DTDangKy> listDangKyloc = DangKyDAO.Instance.ListDangKyTheoMaCT(ql.MaCT, DateTime.Parse(strLoc1), DateTime.Parse(strLoc2));
                return View(listDangKyloc);
            }


            //GIANGVIEN ql = LayGiangVien();

            //List<DTDangKy> listDangKy = DangKyDAO.Instance.ListDangKyTheoMaKhoa(ql.MaKhoa);
            //return View(listDangKy);
        }
        [HttpGet]
        public ActionResult DsDeTaiNghiemThu(string strLoc1, string strLoc2)
        {

            GIANGVIEN ql = LayGiangVien();
            List<DTDeTai> listDangKy = DeTaiDAO.Instance.ListDeTaiTheoMaCTDefault(ql.MaCT);
            if (string.IsNullOrEmpty(strLoc1) && string.IsNullOrEmpty(strLoc2))
            {

                return View(listDangKy);
            }
            else if (string.IsNullOrEmpty(strLoc1) || string.IsNullOrEmpty(strLoc2))
            {

                return View(listDangKy);
            }
            else
            {
                List<DTDeTai> listDangKyloc = DeTaiDAO.Instance.ListDeTaiTheoMaCT(ql.MaCT, DateTime.Parse(strLoc1), DateTime.Parse(strLoc2));
                return View(listDangKyloc);
            }


            //GIANGVIEN ql = LayGiangVien();

            //List<DTDangKy> listDangKy = DangKyDAO.Instance.ListDangKyTheoMaKhoa(ql.MaKhoa);
            //return View(listDangKy);
        }
        public ActionResult PartialTrangThai()
        {
            return PartialView();
        }

        public ActionResult PhanCongHoiDong()
        {
            GIANGVIEN ql = LayGiangVien();
            return View(db.HOIDONGDUYETDECUONGs.Where(n => n.MaCT == ql.MaCT).ToList());
        }

        public ActionResult PhanCongHoiDongNghiemThu()
        {
            GIANGVIEN ql = LayGiangVien();
            return View(db.HOIDONGNGHIEMTHUs.Where(n => n.MaCT == ql.MaCT).ToList());
        }

        [HttpGet]
        public JsonResult Detail(int idDT)
        {
            try
            {
                var list = (from s in db.DANGKies
                            join t in db.GIANGVIENs on s.MaGiangVien equals t.MaGiangVien
                            join c in db.TRANGTHAIs on s.TrangThai equals c.MaTrangThai
                            join h in db.HOIDONGDUYETDECUONGs on s.MaHoiDong equals h.MaHoiDong
                            join sv in db.SINHVIENs on s.MaSoSinhVien equals sv.MaSoSinhVien 
                            where s.IDDangKy == idDT
                            select new
                            {
                                TenDT = s.TenDeTai,
                                TenSV = sv.HoTen,
                                MaSV = s.MaSoSinhVien,
                                TenHD = h.TenHoiDong,
                                TenTT = c.TenTrangThai,
                                LinkDT = s.LinkDeCuong
                            }).SingleOrDefault();
                return Json(new { code = 200, dt = list, msg = "Lay thong tin thanh cong." },
                        JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lay thong tin that bai " + ex.Message },
                   JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult DetailNT(int idDT)
        {
            try
            {
                var list = (from s in db.DETAIs
                            join t in db.GIANGVIENs on s.MaGiangVien equals t.MaGiangVien
                            join c in db.TRANGTHAIs on s.MaTrangThai equals c.MaTrangThai
                            join h in db.HOIDONGNGHIEMTHUs on s.MaHoiDong equals h.MaHoiDong
                            join sv in db.SINHVIENs on s.MaSoSinhVien equals sv.MaSoSinhVien
                            where s.MaDeTai == idDT
                            select new
                            {
                                TenDT = s.TenDeTai,
                                TenSV = sv.HoTen,
                                MaSV = s.MaSoSinhVien,
                                TenHD = h.TenHoiDong,
                                TenTT = c.TenTrangThai,
                                LinkDT = s.LinkDeTai
                            }).SingleOrDefault();
                return Json(new { code = 200, dt = list, msg = "Lay thong tin thanh cong." },
                        JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lay thong tin that bai " + ex.Message },
                   JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult DsGiangVien()
        {
            try
            {
                GIANGVIEN ql = LayGiangVien();
                var DsGV = (from gv in db.GIANGVIENs
                            where gv.MaCT == ql.MaCT && gv.MaSoGiangVien.Contains("GV")
                            select new
                            {
                                gv.MaGiangVien,
                                gv.MaSoGiangVien,
                                gv.TenGiangVien,
                                gv.TrinhDo,
                                gv.MaCT,
                                gv.NgaySinh
                            }

                          ).ToList();

                return Json(new { code = 200, gv = DsGV, msg = "Lay thong tin thanh cong." },
                        JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lay thong tin that bai " + ex.Message },
                   JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult DsGiangVienHD(int id)
        {
            try
            {
                var DsctHDGV = (from gv in db.GIANGVIENs join t in db.CHITIETHOIDONGDECUONGs on gv.MaGiangVien equals t.MaGiangVien
                            where t.MaHoiDong == id
                            select new
                            {
                                MaGV = gv.MaGiangVien,
                                MsGV = gv.MaSoGiangVien,
                                TenGV = gv.TenGiangVien,
                                TdGV = gv.TrinhDo,
                                MaKhoa = gv.MaKhoa,
                                NgaySinh = gv.NgaySinh
                            }

                        ).ToList();
                return Json(new { code = 200,gvcu= DsctHDGV, msg = "Lay thong tin thanh cong." },
                        JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lay thong tin that bai " + ex.Message },
                   JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult ThemHoiDong(int SL)
        {
            try
            {
                GIANGVIEN ql = LayGiangVien();
                for (int i = 1; i <= SL; i++)
                {
                    string slhoidong = (Convert.ToInt32(HoiDongDao.Instance.LaysoluongHoidong(ql.MaKhoa)) + i).ToString();
                    string tenhoidong = "Hội Đồng " + slhoidong + "";
                    var hd = new HOIDONGDUYETDECUONG();
                    hd.TenHoiDong = tenhoidong;
                    hd.MaKhoa = ql.MaKhoa;
                    hd.SoLuongTV = false;
                    hd.MaCT = ql.MaCT;
                    db.HOIDONGDUYETDECUONGs.InsertOnSubmit(hd);
                }
                db.SubmitChanges();
                return Json(new { code = 200, msg = "Thêm hội đồng thành công." },
                        JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Khó quá! Bó tay  " + ex.Message },
                   JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ThemHoiDongNghiemThu(int SL)
        {
            try
            {
                GIANGVIEN ql = LayGiangVien();
                for (int i = 1; i <= SL; i++)
                {
                    string slhoidong = (Convert.ToInt32(HoiDongDao.Instance.LaysoluongHoidongNT(ql.MaKhoa)) + i).ToString();
                    string tenhoidong = "Hội Đồng " + slhoidong + "";
                    var hd = new HOIDONGNGHIEMTHU();
                    hd.TenHoiDong = tenhoidong;
                    hd.MaKhoa = ql.MaKhoa;
                    hd.SoLuongTV = false;
                    db.HOIDONGNGHIEMTHUs.InsertOnSubmit(hd);
                }
                db.SubmitChanges();
                return Json(new { code = 200, msg = "Thêm hội đồng thành công." },
                        JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Khó quá! Bó tay  " + ex.Message },
                   JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult XoaHoiDong(int idHD)
        {
            try
            {
                var cthd = db.CHITIETHOIDONGDECUONGs.Where(c => c.MaHoiDong == idHD);
                var hd = db.HOIDONGDUYETDECUONGs.SingleOrDefault(c => c.MaHoiDong == idHD);
                db.CHITIETHOIDONGDECUONGs.DeleteAllOnSubmit(cthd);
                db.HOIDONGDUYETDECUONGs.DeleteOnSubmit(hd);
                db.SubmitChanges();
                return Json(new { code = 200, msg = "Xóa hội đồng thành công." },
                        JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Khó quá! Bó tay  " + ex.Message },
                   JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult XoaHoiDongNT(int idHD)
        {
            try
            {
                var cthd = db.CHITIETHOIDONGNGHIEMTHUs.Where(c => c.MaHoiDong == idHD);
                var hd = db.CHITIETHOIDONGNGHIEMTHUs.SingleOrDefault(c => c.MaHoiDong == idHD);
                db.CHITIETHOIDONGNGHIEMTHUs.DeleteAllOnSubmit(cthd);
                db.CHITIETHOIDONGNGHIEMTHUs.DeleteOnSubmit(hd);
                db.SubmitChanges();
                return Json(new { code = 200, msg = "Xóa hội đồng thành công." },
                        JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Khó quá! Bó tay  " + ex.Message },
                   JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult AddThanhVienHD(int iHD, string strHD)
        {
            try
            {
                string[] listTV = strHD.Split('|');
                CHITIETHOIDONGDECUONG cthddc1 = new CHITIETHOIDONGDECUONG();
                CHITIETHOIDONGDECUONG cthddc2 = new CHITIETHOIDONGDECUONG();
                CHITIETHOIDONGDECUONG cthddc3 = new CHITIETHOIDONGDECUONG();
                cthddc1.MaHoiDong = iHD;
                cthddc1.MaGiangVien = Convert.ToInt32(listTV[0]);

                cthddc2.MaHoiDong = iHD;
                cthddc2.MaGiangVien = Convert.ToInt32(listTV[1]);

                cthddc3.MaHoiDong = iHD;
                cthddc3.MaGiangVien = Convert.ToInt32(listTV[2]);
                db.CHITIETHOIDONGDECUONGs.InsertOnSubmit(cthddc1);
                db.CHITIETHOIDONGDECUONGs.InsertOnSubmit(cthddc2);
                db.CHITIETHOIDONGDECUONGs.InsertOnSubmit(cthddc3);
                db.SubmitChanges();

                return Json(new { code = 200, msg = "Phân công hội đồng thành công" },
                        JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Khó quá! Bó tay  " + ex.Message },
                   JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult AddThanhVienHDNT(int iHD, string strHD)
        {
            try
            {
                string[] listTV = strHD.Split('|');
                CHITIETHOIDONGNGHIEMTHU cthddc1 = new CHITIETHOIDONGNGHIEMTHU();
                CHITIETHOIDONGNGHIEMTHU cthddc2 = new CHITIETHOIDONGNGHIEMTHU();
                CHITIETHOIDONGNGHIEMTHU cthddc3 = new CHITIETHOIDONGNGHIEMTHU();
                cthddc1.MaHoiDong = iHD;
                cthddc1.MaGiangVien = Convert.ToInt32(listTV[0]);

                cthddc2.MaHoiDong = iHD;
                cthddc2.MaGiangVien = Convert.ToInt32(listTV[1]);

                cthddc3.MaHoiDong = iHD;
                cthddc3.MaGiangVien = Convert.ToInt32(listTV[2]);
                db.CHITIETHOIDONGNGHIEMTHUs.InsertOnSubmit(cthddc1);
                db.CHITIETHOIDONGNGHIEMTHUs.InsertOnSubmit(cthddc2);
                db.CHITIETHOIDONGNGHIEMTHUs.InsertOnSubmit(cthddc3);
                db.SubmitChanges();

                return Json(new { code = 200, msg = "Phân công hội đồng thành công" },
                        JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Khó quá! Bó tay  " + ex.Message },
                   JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult FixThanhVienHD(int idHoiDong, string strHDfix)
        {
            try
            {
                string[] listTV = strHDfix.Split('|');
                var tv1 = db.CHITIETHOIDONGDECUONGs.SingleOrDefault(n => n.MaHoiDong == idHoiDong && n.MaGiangVien == Convert.ToInt32(listTV[0]));
                tv1.MaGiangVien = Convert.ToInt32(listTV[3]);
                var tv2 = db.CHITIETHOIDONGDECUONGs.SingleOrDefault(n => n.MaHoiDong == idHoiDong && n.MaGiangVien == Convert.ToInt32(listTV[1]));
                tv2.MaGiangVien = Convert.ToInt32(listTV[4]);
                var tv3 = db.CHITIETHOIDONGDECUONGs.SingleOrDefault(n => n.MaHoiDong == idHoiDong && n.MaGiangVien == Convert.ToInt32(listTV[2]));
                tv3.MaGiangVien = Convert.ToInt32(listTV[5]);
                db.SubmitChanges();

                return Json(new { code = 200, msg = "Phân công hội đồng thành công" },
                        JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Khó quá! Bó tay  " + ex.Message },
                   JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult FixThanhVienHDNT(int idHoiDong, string strHDfix)
        {
            try
            {
                string[] listTV = strHDfix.Split('|');
                var tv1 = db.CHITIETHOIDONGNGHIEMTHUs.SingleOrDefault(n => n.MaHoiDong == idHoiDong && n.MaGiangVien == Convert.ToInt32(listTV[0]));
                tv1.MaGiangVien = Convert.ToInt32(listTV[3]);
                var tv2 = db.CHITIETHOIDONGNGHIEMTHUs.SingleOrDefault(n => n.MaHoiDong == idHoiDong && n.MaGiangVien == Convert.ToInt32(listTV[1]));
                tv2.MaGiangVien = Convert.ToInt32(listTV[4]);
                var tv3 = db.CHITIETHOIDONGNGHIEMTHUs.SingleOrDefault(n => n.MaHoiDong == idHoiDong && n.MaGiangVien == Convert.ToInt32(listTV[2]));
                tv3.MaGiangVien = Convert.ToInt32(listTV[5]);
                db.SubmitChanges();

                return Json(new { code = 200, msg = "Phân công hội đồng thành công" },
                        JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Khó quá! Bó tay  " + ex.Message },
                   JsonRequestBehavior.AllowGet);
            }
        }

    }
}
   