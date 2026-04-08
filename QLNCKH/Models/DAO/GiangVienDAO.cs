using QLNCKH.Models.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace QLNCKH.Models.DAO
{
    public class GiangVienDAO
    {
        private static GiangVienDAO instance;
        public static GiangVienDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GiangVienDAO();
                }
                return instance;
            }
            private set
            {
                instance = value;
            }
        }

        public List<DTOThongBaoChamLai> DsThongBaoChamLai(int mgv)
        {
            List<DTOThongBaoChamLai> dsTb = new List<DTOThongBaoChamLai>();
            DataTable dt = DataProvider.Instance.ExcuteQuery("[sp_LAYDSTBChamLai] @magiangvien ", new object[] { mgv });
            foreach (DataRow item in dt.Rows)
            {
                DTOThongBaoChamLai danhsach = new DTOThongBaoChamLai(item);
                dsTb.Add(danhsach);
            }
            return dsTb;
        }
    }
}