using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.ComponentModel.DataAnnotations;

namespace QLNCKH.Models.DTO
{
    public class DTOThongBaoChamLai
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Lỗi")]
        [Display(Name = "Thông Báo")]
        public string ThongBao { get; set; }

        [Required(ErrorMessage = "Lỗi")]
        [Display(Name = "Mã Mã Hội Đồng")]
        public int MaHoiDong { get; set; }
        [Required(ErrorMessage = "Lỗi")]
        [Display(Name = "Đã Đọc")]
        public string IsCheck { get; set; }

        [Required(ErrorMessage = "Lỗi")]
        [Display(Name = "Tên Đề Tài")]
        public string TenDeTai { get; set; }

        public int ThoiGian { get; set; }
        public DTOThongBaoChamLai(DataRow row)
        {
            DateTime now = DateTime.Now;       
            TimeSpan timeDiff = now.Subtract((DateTime)row["DateModified"]);
            double totalMinutes = timeDiff.TotalMinutes;
            this.Id = (int)row["Id"];
            this.ThongBao = row["Thongbao"].ToString();
            this.MaHoiDong = (int)row["MaHoiDong"];
            this.IsCheck = (bool)row["IsCheck"]? "Đã Đọc" : "Chưa Đọc";
            this.TenDeTai = row["TenDeTai"].ToString();
            this.ThoiGian = (int)totalMinutes;
        }
    }
}