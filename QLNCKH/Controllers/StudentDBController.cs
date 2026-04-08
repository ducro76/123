using QLNCKH.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Reflection;
using QLNCKH.Models.DTO;

namespace QLNCKH.Controllers
{
    public class StudentDBController : Controller
    {
        DataQLNCKHDataContext db = new DataQLNCKHDataContext();
        // GET: StudentDB

        public ActionResult DSDanhGiaDC()
        {
            SINHVIEN sv = LaySinhVien();
            var dv = (from dk in db.DANGKies                   
                      join gv in db.GIANGVIENs on dk.MaGiangVien equals gv.MaGiangVien
                      where dk.TrangThai>=2 && dk.MaSoSinhVien == sv.MaSoSinhVien
                      select new 
                      {
                          dk.TenDeTai,
                          dk.IDDangKy,
                          dk.MaGiangVien,
                          gv.TenGiangVien,
                          dk.GhiChu,
                      }
                      ).ToList();
            List<CTChamDiemDC> ds = new List<CTChamDiemDC>();
            foreach(var item in dv)
            {
                CTChamDiemDC dt = new CTChamDiemDC();
                dt.TenDeTai = item.TenDeTai;
                dt.IDDC = item.IDDangKy;
                dt.TenGiangVien = item.TenGiangVien;
                dt.GhiChu = item.GhiChu;
                var ct = (from bb in db.BIENBANCHAMDECUONGs
                          join gv in db.GIANGVIENs on bb.MaGiangVien equals gv.MaGiangVien
                          where bb.IdDangKy == dt.IDDC
                          select new
                          {
                              bb.IdDangKy,
                              gv.TenGiangVien,
                              bb.DanhGia,
                              bb.Diem,
                          }).ToList();
                foreach(var t in ct)
                {
                    CTBienBan ctbb = new CTBienBan();
                    ctbb.IDDC = (int)t.IdDangKy;
                    ctbb.TenGiangVien = t.TenGiangVien;
                    ctbb.DanhGia = t.DanhGia;
                    ctbb.Diem = (float)t.Diem;
                    dt.DSBienBan.Add(ctbb);
                }
                ds.Add(dt);
            }

            return View(ds);
        }
        public ActionResult DSDanhGiaDT()
        {
            SINHVIEN sv = LaySinhVien();
            var dv = (from dt in db.DETAIs
                      join gv in db.GIANGVIENs on dt.MaGiangVien equals gv.MaGiangVien
                      where dt.MaTrangThai >= 5 && dt.MaSoSinhVien == sv.MaSoSinhVien
                      select new
                      {
                          dt.TenDeTai,
                          dt.MaDeTai,
                          dt.MaGiangVien,
                          gv.TenGiangVien,
                          dt.GhiChu,
                      }
                      ).ToList();
            List<CTChamDiemDC> ds = new List<CTChamDiemDC>();
            foreach (var item in dv)
            {
                CTChamDiemDC dt = new CTChamDiemDC();
                dt.TenDeTai = item.TenDeTai;
                dt.IDDC = item.MaDeTai;
                dt.TenGiangVien = item.TenGiangVien;
                dt.GhiChu = item.GhiChu;
                var ct = (from bb in db.BIENBANNGHIEMTHUs
                          join gv in db.GIANGVIENs on bb.MaGiangVien equals gv.MaGiangVien
                          where bb.MaDeTai == dt.IDDC
                          select new
                          {
                              bb.MaDeTai,
                              gv.TenGiangVien,
                              bb.NhanXet,
                              bb.Diem,
                          }).ToList();
                foreach (var t in ct)
                {
                    CTBienBan ctbb = new CTBienBan();
                    ctbb.IDDC = (int)t.MaDeTai;
                    ctbb.TenGiangVien = t.TenGiangVien;
                    ctbb.DanhGia = t.NhanXet;
                    ctbb.Diem = (float)t.Diem;
                    dt.DSBienBan.Add(ctbb);
                }
                ds.Add(dt);
            }

            return View(ds);
        }
        [HttpGet]
        public JsonResult GetChamDiem(int iddk )
        {
            try
            {
                
                var dv = (from bb in db.BIENBANCHAMDECUONGs
                          join gv in db.GIANGVIENs on bb.MaGiangVien equals gv.MaGiangVien 
                          where bb.IdDangKy == iddk
                          select new
                          {
                              MaGV = bb.MaGiangVien,
                              TenGV = gv.TenGiangVien,
                              Diem = bb.Diem,
                              DanhGia = bb.DanhGia, 
                          }
                ).ToList();
                return Json(new { code = 200, cd = dv, msg = "Thanh cong" },
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Thêm that bai " + ex.Message },
                   JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult StudentDB()
        {
            var tb = db.THONGBAOs.OrderByDescending(s=>s.Id).ToList().Take(10);
            return View(tb);
        }
        public SINHVIEN LaySinhVien()
        {
            SINHVIEN sv = Session["SinhVien"] as SINHVIEN;
            return sv;
        }

        public ActionResult DetailTB(int id)
        {
            var tb = db.THONGBAOs.Where(a => a.Id == id).Single();
            return View(tb);
        }
        public ActionResult PartialNav()
        {
            return PartialView();
        }
        public ActionResult PartialMenu()
        {
            return PartialView();
        }
        public ActionResult PartialMenuKhoa()
        {
            var kh = db.KHOAs.OrderByDescending(a => a.MaKhoa).ToList();
            return PartialView(kh);
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session["TaiKhoan"] = null;
            return RedirectToAction("Login", "User");
        }
        [HttpGet]
        public ActionResult DangKy()
        {
            ViewBag.MaGiangVien = new SelectList(db.GIANGVIENs.Where(n => n.MaSoGiangVien.Contains("GV")).ToList().OrderBy(n => n.TenGiangVien), "MaGiangVien", "TenGiangVien");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult DangKy(DANGKY dk, FormCollection f, HttpPostedFileBase fFileUpload)
        {
            ViewBag.MaGiangVien = new SelectList(db.GIANGVIENs.Where(n => n.MaSoGiangVien.Contains("GV")).ToList().OrderBy(n => n.TenGiangVien), "MaGiangVien", "TenGiangVien");
            if (String.IsNullOrEmpty(f["sTenDT"]))
            {
                ViewBag.err1 = " Vui lòng nhập tên đề tài.";
            }
            if (String.IsNullOrEmpty(f["sGhichu"]))
            {
                ViewBag.err3 = " Vui lòng nhập ghi chú.";
            }           
            if (ModelState.IsValid)
            {
                //NHOM idnhom = db.NHOMs.Where(t => t.TenNhom == f["txtTenNhom"]).SingleOrDefault();
                SINHVIEN sv = new SINHVIEN();
                sv = (SINHVIEN)Session["SinhVien"];
                //string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fFileUpload.FileName);
                //string filePath = Server.MapPath("~/Theme/Luufile/") + Path.GetFileName(fFileUpload.FileName);
                //if (!System.IO.File.Exists(filePath))
                //{
                //    fFileUpload.SaveAs(filePath);
                //}
                //string input = filePath;
                //string output = Server.MapPath("~/Theme/Luufile/") + fileNameWithoutExtension + ".pdf";
                //ConvertWordToSpecifiedFormat(input, output, Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF);
                dk.TenDeTai = f["sTenDT"];
                dk.GhiChu = f["sGhichu"];
                dk.MaSoSinhVien = f["sMasv"];
                dk.LinkDeCuong = "Chưa Nộp Đề Cương";
                dk.MaNhom = sv.MaSoSinhVien;
                dk.KetQua = false;
                dk.TrangThai = 1;
                dk.MaHoiDong = 1;
                dk.NgayDangKy = DateTime.Now;
                dk.MaGiangVien = int.Parse(f["MaGiangVien"]);
                db.DANGKies.InsertOnSubmit(dk);
                db.SubmitChanges();
                return RedirectToAction("TrangThaiDeTai", "StudentDetai");
            }
            return View();

        }
        public ActionResult PartialFormNhom()
        {
            SINHVIEN sv = LaySinhVien();
            var lsNhom = db.CHITIETNHOMs.Where(n => n.MaNhom == sv.MaSoSinhVien && n.TrangThai == 1).OrderBy(n => n.IdCTNhom).ToList();
            return PartialView(lsNhom);
        }
        [HttpPost]
        public JsonResult AddNhom(string strNhom)
        {
            SINHVIEN sv = LaySinhVien();
            string id = sv.MaSoSinhVien;
            try
            {
                
                string[] listTV = strNhom.Split('|');
              
                for (int i=4;i<listTV.Length-1;i=i+2)
                {
                    string tensv = listTV[i];
                    string mssv = listTV[i + 1];
                    CHITIETNHOM tempCT = db.CHITIETNHOMs.SingleOrDefault(n => n.MaSoSinhVien == mssv && n.TrangThai ==1);
                    if (tempCT == null || tempCT.ToString() == "")
                    {
                        CHITIETNHOM tempChiTiet = new CHITIETNHOM();
                        tempChiTiet.MaNhom = sv.MaSoSinhVien;
                        tempChiTiet.HoTen = tensv;
                        tempChiTiet.MaSoSinhVien = mssv;
                        tempChiTiet.TrangThai = 1;

                        db.CHITIETNHOMs.InsertOnSubmit(tempChiTiet);
                        db.SubmitChanges();                      
                    }


                }
                return Json(new { code = 200, cd = id, msg = listTV[2] },
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Thêm that bai " + ex.Message },
                   JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult PartialModalNhom()
        {
            SINHVIEN sv = LaySinhVien();
            var lsNhom = db.CHITIETNHOMs.Where(n => n.MaNhom == sv.MaSoSinhVien && n.TrangThai == 1).OrderBy(n=>n.IdCTNhom).ToList();
            return PartialView(lsNhom);
        
        }
        public ActionResult PartialDangKy()
        {
            SINHVIEN sv = LaySinhVien();
            var lst = db.DANGKies.Where(n => n.MaNhom == sv.MaSoSinhVien && n.KetQua == false).ToList();
            return PartialView(lst);

        }

        private static void ConvertWordToSpecifiedFormat(object input, object output, object format)
        {
            Microsoft.Office.Interop.Word._Application application = new Microsoft.Office.Interop.Word.Application();
            application.Visible = false;
            object missing = Missing.Value;
            object isVisible = true;
            object readOnly = false;
            Microsoft.Office.Interop.Word._Document document = application.Documents.Open(ref input, ref missing, ref readOnly, ref missing, ref missing, ref missing, ref missing,
                                    ref missing, ref missing, ref missing, ref missing, ref isVisible, ref missing, ref missing, ref missing, ref missing);

            document.Activate();
            document.SaveAs(ref output, ref format, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing,
                            ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);
            application.Quit(ref missing, ref missing, ref missing);
        }
        [HttpGet]
        public ActionResult ProfileSV()
        {
            SINHVIEN sv = LaySinhVien();
            var pfsv = db.SINHVIENs.SingleOrDefault(n => n.MaSinhVien == sv.MaSinhVien);
            return View(pfsv);
        }
        [HttpPost]
        public ActionResult ProfileSV(FormCollection f)
        {
            SINHVIEN sv = LaySinhVien();
            var pfsv = db.SINHVIENs.SingleOrDefault(n => n.MaSinhVien == sv.MaSinhVien);
            pfsv.SDT = f["sSDT"];
            pfsv.TKNganHang = f["sTKNH"];
            pfsv.CCCD = f["sCCCD"];
            pfsv.DiaChi = f["sDiaChi"];
            pfsv.ChiNhanhNganHang = f["sCNNH"];
            db.SubmitChanges();
            return View(pfsv);

        }
        [HttpPost]
        public ActionResult UploadFiles()
        {
            
            // Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {
                
                try
                {
                    SINHVIEN sv = LaySinhVien();
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        //string filename = Path.GetFileName(Request.Files[i].FileName);  

                        HttpPostedFileBase file = files[i];
                        string fname,sfile;

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
                       
                        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);
                        var dt = db.DANGKies.Where(s => s.MaSoSinhVien == sv.MaSoSinhVien && s.KetQua==false).SingleOrDefault();
                        string[] listname = fname.Split('.');
                        if (dt.KetQua == false)
                        {
                            dt.LinkDeCuong = fileNameWithoutExtension + ".pdf";                          
                            //ThongBaoChamLai tb = new ThongBaoChamLai();
                            //tb.Thongbao = "Thông Báo Chấm Lại";
                            //tb.MaHoiDong = dt.MaHoiDong;
                            //tb.IdDangKy = dt.IDDangKy;
                            //tb.IsCheck = false;
                            //tb.DateModified = DateTime.Now;
                            //db.ThongBaoChamLais.InsertOnSubmit(tb);
                            db.SubmitChanges();
                            if (dt.TrangThai == 6)
                            {
                                var df1 = db.BIENBANCHAMDECUONGs.Where(n => n.IdDangKy == dt.IDDangKy).ToList();
                                if(df1.Count>0)
                                {
                                    db.BIENBANCHAMDECUONGs.DeleteAllOnSubmit(df1);
                                    db.SubmitChanges();
                                }
                                var gf = db.ThongBaoChamLais.Where(n => n.IdDangKy == dt.IDDangKy).ToList();
                                if(gf.Count() < 1 )
                                {                               
                                    TrangThai.Instance.InsertThongBaoChamDiem("Thông Báo Chấm Lại", (int)dt.MaHoiDong, dt.IDDangKy, false, DateTime.Now); 
                                }      
                                else if(gf.Count()>=1)
                                {
                                    DataProvider.Instance.ExcuteNonQuery("DELETE FROM ThongBaoChamLai WHERE IdDangKy =" + dt.IDDangKy);
                                    TrangThai.Instance.InsertThongBaoChamDiem("Thông Báo Chấm Lại", (int)dt.MaHoiDong, dt.IDDangKy, false, DateTime.Now);
                                }    
                            }
                        }
                        sfile = Path.Combine(Server.MapPath("~/Theme/Luufile"), fname);
                        file.SaveAs(sfile);
                        string input = sfile;
                        if (listname[listname.Length - 1] != "pdf")
                        {
                            string output = Server.MapPath("~/Theme/Luufile/") + fileNameWithoutExtension + ".pdf";
                            ConvertWordToSpecifiedFormat(input, output, Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF);
                        }
                            
                        // Get the complete folder path and store the file inside it.  
                        
                        


                        


                    }
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