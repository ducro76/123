
using OfficeOpenXml;
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
    public class QuanLyTongController : Controller
    {
        // GET: QuanLyTong
        DataQLNCKHDataContext db = new DataQLNCKHDataContext();
        // GET: QuanLy
        public ActionResult PartialMenuQLT()
        {
            return PartialView();
        }
        public ActionResult Index()
        {
            var tb = db.THONGBAOs.OrderByDescending(s => s.Id).ToList().Take(10);
            return View(tb);
        }
        public ActionResult DsDeCuong(string strLoc1, string strLoc2)
        {
            List<DTDangKy> listdecuong = DangKyDAO.Instance.ListTongDeCuong();

            if (string.IsNullOrEmpty(strLoc1) && string.IsNullOrEmpty(strLoc2))
            {

                return View(listdecuong);
            }
            else if (string.IsNullOrEmpty(strLoc1) || string.IsNullOrEmpty(strLoc2))
            {
                
                return View(listdecuong);
            }
            else
            {
                ViewBag.str1 = strLoc1;
                ViewBag.str2 = strLoc2;

                List<DTDangKy> listDangKyloc = DangKyDAO.Instance.ListTongDeCuongLoc(DateTime.Parse(strLoc1), DateTime.Parse(strLoc2));
                return View(listDangKyloc);
            }
        }
        [HttpGet]
        public void GenerateReport(string str1, string str2)
        {
                List<DTDangKy> listdecuong = DangKyDAO.Instance.ListTongDeCuong();
                ExcelPackage pck = new ExcelPackage();
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");
                ws.Cells["A1"].Value = "Communication";
                ws.Cells["B1"].Value = "Com1";

                ws.Cells["A2"].Value = "Report";
                ws.Cells["B2"].Value = "Report1";

                ws.Cells["A3"].Value = "Date";
                ws.Cells["B3"].Value = string.Format("{0:dd MMMM yyyy} at {0:H: mm tt}", DateTimeOffset.Now);

                ws.Cells["A6"].Value = "Tên Đề Tài";
                ws.Cells["B6"].Value = "Mã Số Sinh Viên";
                ws.Cells["C6"].Value = "Tên Giảng Viên";
                ws.Cells["D6"].Value = "Ghi Chú";
                ws.Cells["E6"].Value = "Tên trạng thái";

                int rowStart = 7;
                if (string.IsNullOrEmpty(str1) && string.IsNullOrEmpty(str2))
                {
                    foreach (var item in listdecuong)
                    {
                        ws.Cells[string.Format("A{0}", rowStart)].Value = item.TenDeTai;
                        ws.Cells[string.Format("B{0}", rowStart)].Value = item.MaSoSinhVien;
                        ws.Cells[string.Format("C{0}", rowStart)].Value = item.TenGiangVien;
                        ws.Cells[string.Format("D{0}", rowStart)].Value = item.GhiChu;
                        ws.Cells[string.Format("E{0}", rowStart)].Value = item.TenTrangThai;
                        rowStart++;
                    }

                    ws.Cells["A:AZ"].AutoFitColumns();
                    Response.Clear();
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment: filename=" + "ExcelReport.xlsx");
                    Response.BinaryWrite(pck.GetAsByteArray());
                    Response.End();
                }
                else if (string.IsNullOrEmpty(str1) || string.IsNullOrEmpty(str2))
                {
                    foreach (var item in listdecuong)
                    {
                        ws.Cells[string.Format("A{0}", rowStart)].Value = item.TenDeTai;
                        ws.Cells[string.Format("B{0}", rowStart)].Value = item.MaSoSinhVien;
                        ws.Cells[string.Format("C{0}", rowStart)].Value = item.TenGiangVien;
                        ws.Cells[string.Format("D{0}", rowStart)].Value = item.GhiChu;
                        ws.Cells[string.Format("E{0}", rowStart)].Value = item.TenTrangThai;
                        rowStart++;
                    }

                    ws.Cells["A:AZ"].AutoFitColumns();
                    Response.Clear();
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment: filename=" + "ExcelReport.xlsx");
                    Response.BinaryWrite(pck.GetAsByteArray());
                    Response.End();

                }
                else
                {

                    List<DTDangKy> listDangKyloc = DangKyDAO.Instance.ListTongDeCuongLoc(DateTime.Parse(str1), DateTime.Parse(str2));
                    foreach (var item in listDangKyloc)
                    {
                        ws.Cells[string.Format("A{0}", rowStart)].Value = item.TenDeTai;
                        ws.Cells[string.Format("B{0}", rowStart)].Value = item.MaSoSinhVien;
                        ws.Cells[string.Format("C{0}", rowStart)].Value = item.TenGiangVien;
                        ws.Cells[string.Format("D{0}", rowStart)].Value = item.GhiChu;
                        ws.Cells[string.Format("E{0}", rowStart)].Value = item.TenTrangThai;
                        rowStart++;
                    }

                    ws.Cells["A:AZ"].AutoFitColumns();
                    Response.Clear();
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment: filename=" + "ExcelReport.xlsx");
                    Response.BinaryWrite(pck.GetAsByteArray());
                    Response.End();

                }          
                
        }

        public void GenerateReportDETAI(string str1, string str2)
        {
            List<DTDeTai> listdetai = DeTaiDAO.Instance.ListTongDeTai();
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");
            ws.Cells["A1"].Value = "Communication";
            ws.Cells["B1"].Value = "Com1";

            ws.Cells["A2"].Value = "Report";
            ws.Cells["B2"].Value = "Report1";

            ws.Cells["A3"].Value = "Date";
            ws.Cells["B3"].Value = string.Format("{0:dd MMMM yyyy} at {0:H: mm tt}", DateTimeOffset.Now);

            ws.Cells["A6"].Value = "Tên Đề Tài";
            ws.Cells["B6"].Value = "Mã Số Sinh Viên";
            ws.Cells["C6"].Value = "Tên Giảng Viên";
            ws.Cells["D6"].Value = "Ghi Chú";
            ws.Cells["E6"].Value = "Tên trạng thái";

            int rowStart = 7;
            if (string.IsNullOrEmpty(str1) && string.IsNullOrEmpty(str2))
            {
                foreach (var item in listdetai)
                {
                    ws.Cells[string.Format("A{0}", rowStart)].Value = item.TenDeTai;
                    ws.Cells[string.Format("B{0}", rowStart)].Value = item.MaSoSinhVien;
                    ws.Cells[string.Format("C{0}", rowStart)].Value = item.TenGiangVien;
                    ws.Cells[string.Format("D{0}", rowStart)].Value = item.GhiChu;
                    ws.Cells[string.Format("E{0}", rowStart)].Value = item.TenTrangThai;
                    rowStart++;
                }

                ws.Cells["A:AZ"].AutoFitColumns();
                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment: filename=" + "ExcelReport.xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
                Response.End();
            }
            else if (string.IsNullOrEmpty(str1) || string.IsNullOrEmpty(str2))
            {
                foreach (var item in listdetai)
                {
                    ws.Cells[string.Format("A{0}", rowStart)].Value = item.TenDeTai;
                    ws.Cells[string.Format("B{0}", rowStart)].Value = item.MaSoSinhVien;
                    ws.Cells[string.Format("C{0}", rowStart)].Value = item.TenGiangVien;
                    ws.Cells[string.Format("D{0}", rowStart)].Value = item.GhiChu;
                    ws.Cells[string.Format("E{0}", rowStart)].Value = item.TenTrangThai;
                    rowStart++;
                }

                ws.Cells["A:AZ"].AutoFitColumns();
                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment: filename=" + "ExcelReport.xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
                Response.End();

            }
            else
            {

                List<DTDeTai> listDeTaiLoc = DeTaiDAO.Instance.ListTongDeTaiLoc(DateTime.Parse(str1), DateTime.Parse(str2));
                foreach (var item in listDeTaiLoc)
                {
                    ws.Cells[string.Format("A{0}", rowStart)].Value = item.TenDeTai;
                    ws.Cells[string.Format("B{0}", rowStart)].Value = item.MaSoSinhVien;
                    ws.Cells[string.Format("C{0}", rowStart)].Value = item.TenGiangVien;
                    ws.Cells[string.Format("D{0}", rowStart)].Value = item.GhiChu;
                    ws.Cells[string.Format("E{0}", rowStart)].Value = item.TenTrangThai;
                    rowStart++;
                }

                ws.Cells["A:AZ"].AutoFitColumns();
                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment: filename=" + "ExcelReport.xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
                Response.End();

            }

        }
        public ActionResult DsDeTai(string strLoc1, string strLoc2)
        {
            List<DTDeTai> listdetai = DeTaiDAO.Instance.ListTongDeTai();
            if (string.IsNullOrEmpty(strLoc1) && string.IsNullOrEmpty(strLoc2))
            {
                return View(listdetai);
            }
            else if (string.IsNullOrEmpty(strLoc1) || string.IsNullOrEmpty(strLoc2))
            {
                return View(listdetai);
            }
            else
            {
                List<DTDeTai> listDangKyloc = DeTaiDAO.Instance.ListTongDeTaiLoc(DateTime.Parse(strLoc1), DateTime.Parse(strLoc2));
                return View(listDangKyloc);
            }
        }
    }
}