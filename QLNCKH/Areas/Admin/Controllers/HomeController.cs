using QLNCKH.Models;
using QLNCKH.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNCKH.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        DataQLNCKHDataContext db = new DataQLNCKHDataContext();
        // GET: Admin/Home
        public ActionResult Index()
        {
            var dv = (from dt in db.DETAIs
                      join n in db.NGANHs on dt.MaNganh equals n.MaNganh
                      join k in db.KHOAs on n.MaKhoa equals k.MaKhoa
                      group dt by new { k.MaKhoa, k.TenKhoa } into g
                      select new
                      {
                          SLDT = g.Count(),
                          ChiPhi = g.Sum(dt => dt.KinhPhiDuKien),
                          TenKhoa = g.Key.TenKhoa,
                          MaKhoa = g.Key.MaKhoa
                      }).ToList();
            List<CTThongKeKhoa> ds = new List<CTThongKeKhoa>();
            foreach (var item in dv)
            {
                CTThongKeKhoa dt = new CTThongKeKhoa();
                dt.MaKhoa = item.MaKhoa;
                dt.TenKhoa = item.TenKhoa;
                dt.SLDT = item.SLDT;
                dt.ChiPhi = (float)item.ChiPhi;
                var ct = (from q in db.DANGKies
                          join e in db.GIANGVIENs on q.MaGiangVien equals e.MaGiangVien
                          join r in db.CHUONGTRINHs on e.MaCT equals r.MaCT
                          where e.MaKhoa == dt.MaKhoa
                          group q by new { r.TenCT ,r.MaCT} into g
                          select new
                          {
                              SLDK = g.Count(),
                              TenChuongTrinh = g.Key.TenCT,
                              MaCT = g.Key.MaCT
                          }).ToList();
                foreach (var t in ct)
                {
                    CTThongKeCT ctbb = new CTThongKeCT();
                    ctbb.MaCT = t.MaCT;
                    ctbb.TenChuongTrinh = t.TenChuongTrinh;
                    ctbb.SLDK = t.SLDK;                  
                    dt.DSCT.Add(ctbb);
                }
                ds.Add(dt);
            }
            return View(ds);
        }
    }
}