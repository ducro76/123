using QLNCKH.Models;
using QLNCKH.Models.DAO;
using QLNCKH.Models.DTO;
using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Word = Microsoft.Office.Interop.Word;
using System.Reflection;

namespace QLNCKH.Controllers
{
    public class GiangVienController : Controller
    {

        DataQLNCKHDataContext db = new DataQLNCKHDataContext();
        // GET: GiangVien
        public ActionResult Index()
        {
            var tb = db.THONGBAOs.OrderByDescending(s => s.Id).ToList().Take(10);
            return View(tb);
        }
        public void ThongBaoToDs(int id)
        {
            if (DataProvider.Instance.ExcuteNonQuery("UPDATE ThongBaoChamLai SET IsCheck = 1 WHERE Id=" + id) > 0)
            {
                Response.Redirect("~/GiangVien/DsDeCuong");
            }

        }
        public ActionResult PartialTBChamLai()
        {
            var tb = GiangVienDAO.Instance.DsThongBaoChamLai(LayGiangVien().MaGiangVien).OrderByDescending(n => n.Id);
            return PartialView(tb);
        }
        public ActionResult PartialMenuGiangVien()
        {
            return PartialView();
        }
        public GIANGVIEN LayGiangVien()
        {
            GIANGVIEN gv = Session["GiangVien"] as GIANGVIEN;
            return gv;
        }
        [HttpGet]
        public ActionResult DsDeCuong(string strLoc1, string strLoc2)
        {
            GIANGVIEN gv = LayGiangVien();
            List<DTDsChamDiem> listDangKy = DsChamDiemDAO.Instance.ListDsDeCuong(gv.MaGiangVien, gv.MaCT);
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
                List<DTDsChamDiem> listDangKyloc = DsChamDiemDAO.Instance.ListLocDeTai(gv.MaGiangVien, gv.MaCT, DateTime.Parse(strLoc1), DateTime.Parse(strLoc2));
                return View(listDangKyloc);
            }
        }
        [HttpGet]
        public ActionResult DsDeTaiNT(string strLoc1, string strLoc2)
        {
            GIANGVIEN gv = LayGiangVien();
            List<DTOChamDiemDT> listDangKy = DsChamDiemDTDA0.Instance.ListDsDeTai(gv.MaGiangVien, gv.MaCT);
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
                List<DTOChamDiemDT> listDangKyloc = DsChamDiemDTDA0.Instance.ListLocDeTai(gv.MaGiangVien, gv.MaCT, DateTime.Parse(strLoc1), DateTime.Parse(strLoc2));
                return View(listDangKyloc);
            }
        }

        [HttpPost]
        public ActionResult GetMinhChung(FormCollection f, HttpPostedFileBase fFileUpload)
        {
            GIANGVIEN gv = LayGiangVien();
            var bb = db.BIENBANCHAMDECUONGs.Where(s => s.IdDangKy == int.Parse(f["id"])).Where(s => s.MaGiangVien == gv.MaGiangVien).Count();
            if (bb == 0)
            {

                return RedirectToAction("DsDeCuong", "GiangVien");
            }
            else
            {
                var md = db.BIENBANCHAMDECUONGs.Where(s => s.IdDangKy == int.Parse(f["id"])).Where(s => s.MaGiangVien == gv.MaGiangVien).SingleOrDefault();
                //Lấy tên file (khai báo thư biện System.IO)
                var sFileName = Path.GetFileName(fFileUpload.FileName);
                //Lấy đường dẫn lưu file
                var path = Path.Combine(Server.MapPath("~/Theme/Luufile"), sFileName);
                //Kiểm tra ảnh bìa đã tồn tại chưa để lưu lên thư mục
                if (!System.IO.File.Exists(path))
                {
                    fFileUpload.SaveAs(path);
                }
                md.MinhChung = sFileName;
                db.SubmitChanges();
                return RedirectToAction("DsDeCuong", "GiangVien");
            }
        }

        [HttpPost]
        public JsonResult BienBanChamDeCuong(int iddt, int diem, string danhgia)
        {
            var count = db.BIENBANCHAMDECUONGs.Where(s => s.IdDangKy == iddt).Count();
            GIANGVIEN gv = LayGiangVien();
            if (count < 3)
            {

                var dt = db.DANGKies.SingleOrDefault(n => n.IDDangKy == iddt);
                BIENBANCHAMDECUONG bb = new BIENBANCHAMDECUONG();
                bb.Diem = diem;
                bb.IdDangKy = iddt;
                bb.MaHoiDong = dt.MaHoiDong;
                bb.MaGiangVien = gv.MaGiangVien;
                bb.DanhGia = danhgia;
                db.BIENBANCHAMDECUONGs.InsertOnSubmit(bb);
                db.SubmitChanges();
                var dem = db.BIENBANCHAMDECUONGs.Where(s => s.IdDangKy == iddt).Count();
                if (dem == 3)
                {
                    var tb = (float)db.BIENBANCHAMDECUONGs.Where(s => s.IdDangKy == iddt).Sum(s => s.Diem) / 3;
                    if (tb >= 5)
                    {
                        var dk = db.DANGKies.Where(s => s.IDDangKy == iddt).SingleOrDefault();
                        var sv = db.SINHVIENs.Where(s => s.MaSoSinhVien == dk.MaSoSinhVien).SingleOrDefault();
                        dk.TrangThai = 3;
                        DETAI detai = new DETAI();
                        detai.TenDeTai = dk.TenDeTai;
                        detai.GhiChu = dk.GhiChu;
                        detai.NgayThucHien = dk.NgayDangKy;
                        detai.MaNganh = sv.MaNganh;
                        detai.MaHoiDong = 4;
                        detai.NgayThucHien = dk.NgayDangKy;
                        detai.MaTrangThai = 3;
                        detai.MaSoSinhVien = dk.MaNhom;
                        detai.MaGiangVien = dk.MaGiangVien;
                        detai.MaNhom = dk.MaNhom;
                        db.DETAIs.InsertOnSubmit(detai);
                        db.SubmitChanges();
                    }
                    else
                    {
                        var dk = db.DANGKies.Where(s => s.IDDangKy == iddt).SingleOrDefault();
                        dk.TrangThai = 6;
                        db.SubmitChanges();
                    }
                }
                return Json(new { code = 200, msg = "Lưu điểm thành công" },
                        JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { code = 500, msg = "Khó quá! Bó tay" },
                   JsonRequestBehavior.AllowGet);
            }
        
        }

        [HttpPost]
        public JsonResult BienBanChamDeTai(int iddt, int diem, string danhgia)
        {
            var count = db.BIENBANNGHIEMTHUs.Where(s => s.MaDeTai == iddt).Count();
            if (count < 3)
            {
                GIANGVIEN gv = LayGiangVien();
                var dt = db.DETAIs.SingleOrDefault(n => n.MaDeTai == iddt);
                BIENBANNGHIEMTHU bb = new BIENBANNGHIEMTHU();
                bb.Diem = diem;
                bb.MaDeTai = iddt;
                bb.MaHoiDong = dt.MaHoiDong;
                bb.MaGiangVien = gv.MaGiangVien;
                bb.NhanXet = danhgia;
                db.BIENBANNGHIEMTHUs.InsertOnSubmit(bb);
                db.SubmitChanges();
                var dem = db.BIENBANNGHIEMTHUs.Where(s => s.MaDeTai == iddt).Count();
                if (dem == 3)
                {
                    var tb = (float)db.BIENBANNGHIEMTHUs.Where(s => s.MaDeTai == iddt).Sum(s => s.Diem) / 3;
                    if (tb >= 5)
                    {
                        var dk = db.DETAIs.Where(s => s.MaDeTai == iddt).SingleOrDefault();
                        dk.MaTrangThai = 5;
                        dk.KetQua = "Đạt";
                        dk.KinhPhiDuKien = 4000000;                 
                        db.SubmitChanges();
                    }
                    else
                    {
                        var dk = db.DETAIs.Where(s => s.MaDeTai == iddt).SingleOrDefault();
                        dk.MaTrangThai = 5;
                        dk.KetQua = "Không Đạt";
                        dk.KinhPhiDuKien = 4000000;
                        db.SubmitChanges();
                    }
                }
                return Json(new { code = 200, msg = "Lưu điểm thành công" },
                        JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { code = 500, msg = "Khó quá! Bó tay  " },
                   JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet] 
        public JsonResult TTDeTai(int idDT)
        {
            try
            {
                var list = (from dk in db.DANGKies 
                            where dk.IDDangKy == idDT
                            select new
                            {
                                TenDeTai = dk.TenDeTai,
                                GhiChu = dk.GhiChu,
                                LinkDeCuong = dk.LinkDeCuong
                            }).SingleOrDefault();
                return Json(new { code = 200, dt = list , msg = "Lay thong tin thanh cong." },
                       JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "That bai " + ex.Message },
                   JsonRequestBehavior.AllowGet);
            }         
        }

        [HttpGet]
        public JsonResult TTDeTaiNT(int idDT)
        {
            try
            {
                var list = (from dk in db.DETAIs
                            where dk.MaDeTai == idDT
                            select new
                            {
                                TenDeTai = dk.TenDeTai,
                                GhiChu = dk.GhiChu,
                                LinkDeTai = dk.LinkDeTai
                            }).SingleOrDefault();
                return Json(new { code = 200, dt = list, msg = "Lay thong tin thanh cong." },
                       JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "That bai " + ex.Message },
                   JsonRequestBehavior.AllowGet);
            }
        }
        public FileResult GetFile(FormCollection f)
        {
            string ReportURL = f["getfile"];
            string output = Server.MapPath("~/Theme/Luufile/") + ReportURL;
            return File(output, "application/pdf");
        }
        public ActionResult PartialNavGV()
        {
            return PartialView();
        }
        [HttpGet]
        public ActionResult ProfileGV()
        {
            GIANGVIEN gv = LayGiangVien();
            var pfgv = db.GIANGVIENs.SingleOrDefault(n => n.MaGiangVien == gv.MaGiangVien);
            return View(pfgv);
        }
        [HttpPost]
        public ActionResult ProfileGV(FormCollection f)
        {
            GIANGVIEN gv = LayGiangVien();
            var pfgv = db.GIANGVIENs.SingleOrDefault(n => n.MaGiangVien == gv.MaGiangVien);
            //pfgv.SDT = f["sSDT"];
            //pfgv.TKNganHang = f["sTKNH"];
            //pfgv.CCCD = f["sCCCD"];
            //pfgv.DiaChi = f["sDiaChi"];
            //pfgv.ChiNhanhNganHang = f["sCNNH"];
            //db.SubmitChanges();
            return View(pfgv);

        }
        [HttpPost]
        public ActionResult UploadFiles(FormCollection f)
        {
            // Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {
                try
                {
                    GIANGVIEN gv = LayGiangVien();
                    int count = db.BIENBANCHAMDECUONGs.Where(n => n.IdDangKy == int.Parse(f["iddk"]) && n.MaGiangVien == gv.MaGiangVien).Count();
                    if (count == 0)
                    {
                        return Json(new { code = 500, msg = "That bai " },
                   JsonRequestBehavior.AllowGet);
                    }
                    else { 
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        //string filename = Path.GetFileName(Request.Files[i].FileName);  

                        HttpPostedFileBase file = files[i];
                        string fname, sfile;

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];   
                        }
                        else
                        {
                            fname = file.FileName;
                        }
                        sfile = Path.Combine(Server.MapPath("~/Theme/Luufile"), fname);
                        file.SaveAs(sfile);
                        var id = Request.Form[0];
                        var bb = db.BIENBANCHAMDECUONGs.Where(s => s.IdDangKy == int.Parse(id)).SingleOrDefault();
                        bb.MinhChung = fname;
                        db.SubmitChanges();
                            // Get the complete folder path and store the file inside it.  
                           
                        }
                    // Returns message that successfully uploaded  
                     return Json(new { code = 200, msg = "thanh cong " },
                   JsonRequestBehavior.AllowGet); ;
                    }
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }

    }
}