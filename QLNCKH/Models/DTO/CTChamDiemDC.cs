using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNCKH.Models.DTO
{
    public class CTChamDiemDC
    {
        public string TenDeTai { get; set; }
        public int IDDC { get; set; }
        public string TenGiangVien { get; set; }
        public string GhiChu { get; set; }

        public List<CTBienBan> DSBienBan = new List<CTBienBan>();
    }
}