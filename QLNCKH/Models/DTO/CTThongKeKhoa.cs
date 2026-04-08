using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNCKH.Models.DTO
{
    public class CTThongKeKhoa
    {
        public string MaKhoa { get; set; }
        public string TenKhoa { get; set; }
        public int SLDT { get; set; }
        public double ChiPhi { get; set; }

        public List<CTThongKeCT> DSCT = new List<CTThongKeCT>();
    }
}