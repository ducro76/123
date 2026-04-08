using QLNCKH.Models;
using QLNCKH.Models.DAO;
using QLNCKH.Models.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace QLNCKH.Controllers
{
    public class StudentDetaiController : Controller
    {
        // GET: StudentDetai
        DataQLNCKHDataContext db = new DataQLNCKHDataContext();
       public ActionResult StudentDetai()
        {
            return View();
        }
        public ActionResult _PartialProgressBar()
        {
            return PartialView();
        }
        public ActionResult TrangThaiDeTai()
        {
            SINHVIEN sv = (SINHVIEN)Session["SinhVien"];
            List<DTDangKy> listDangKy = DangKyDAO.Instance.ListDangKy(sv.MaSoSinhVien);
            var dk = db.DANGKies.Where(n => n.MaSoSinhVien == sv.MaSoSinhVien && n.KetQua == false).ToList();
            if(dk.Count() > 0)
            {
                ViewBag.TTDT = dk.ElementAt(0).TrangThai;
            }    
           // TempData["DeTaiHienTai"] = TrangThai.Instance.TrangThaiDeTaiDangLam(sv.MaSoSinhVien); 
            return View(listDangKy);
        }
        public SINHVIEN LaySinhVien()
        {
            SINHVIEN sv = Session["SinhVien"] as SINHVIEN;
            return sv;
        }
        public ActionResult DsDeTai()
        {
            SINHVIEN sv = (SINHVIEN)Session["SinhVien"];
            List<DTDeTai> listDeTai = DeTaiDAO.Instance.ListDeTaiThucHien(sv.MaSoSinhVien);
            var dk = db.DETAIs.Where(n => n.MaSoSinhVien == sv.MaSoSinhVien).OrderByDescending(n => n.MaDeTai).ToList();
            if (dk.Count>0)
            {
                ViewBag.TTDT = dk.ElementAt(0).MaTrangThai;
            }
            //TempData["DeTaiHienTai"] = TrangThai.Instance.TrangThaiDeTaiDangLamNghiemThu(sv.MaSoSinhVien);
            return View(listDeTai);
        }
        [HttpGet]
        public JsonResult Detail(int idDT)
        {
            try
            {
                var list = (from   s in db.DANGKies
                            join t in db.GIANGVIENs
                               on s.MaGiangVien equals t.MaGiangVien
                                                        join c in db.TRANGTHAIs
                            on s.TrangThai equals c.MaTrangThai
                                                        join h in db.HOIDONGDUYETDECUONGs
                            on s.MaHoiDong equals h.MaHoiDong
                            join sv in db.SINHVIENs
                            on s.MaSoSinhVien equals sv.MaSoSinhVien
                            where s.IDDangKy == idDT  
                           select new
                           {
                               TenDT = s.TenDeTai,
                               TenSV =sv.HoTen,
                               MaSV = s.MaSoSinhVien,
                               TenHD = h.TenHoiDong,
                               TenTT = c.TenTrangThai,
                               LinkDT = s.LinkDeCuong
                           }).SingleOrDefault();
                

            return Json(new { code = 200, dt= list, msg = "Lay thong tin thanh cong." },
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lay thong tin that bai " + ex.Message },
                   JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult DetailDTNT(int idDT)
        {
            try
            {
                var list = (from s in db.DETAIs
                            join t in db.GIANGVIENs
                               on s.MaGiangVien equals t.MaGiangVien
                            join c in db.TRANGTHAIs
                            on s.MaTrangThai equals c.MaTrangThai
                            join h in db.HOIDONGNGHIEMTHUs
                            on s.MaHoiDong equals h.MaHoiDong
                            join sv in db.SINHVIENs
                            on s.MaSoSinhVien equals sv.MaSoSinhVien
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
        [HttpPost]
        public ActionResult UploadFilesDeTai()
        {
            // Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    string namefile = "";
                    var id = Request.Form[0];
                    var dt = db.DETAIs.Where(s => s.MaDeTai == int.Parse(id)).SingleOrDefault();
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
                            namefile += fname+"\\";
                        }
                        sfile = Path.Combine(Server.MapPath("~/Theme/Luufile"), fname);
                        file.SaveAs(sfile);
                        // Get the complete folder path and store the file inside it.  

                    }
                    dt.LinkDeTai = namefile;
                    dt.MaTrangThai = 4;
                    db.SubmitChanges();
                    // Returns message that successfully uploaded  
                    return Json("File Uploaded Successfully!");
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