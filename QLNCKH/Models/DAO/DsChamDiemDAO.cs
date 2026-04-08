using QLNCKH.Models.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;


namespace QLNCKH.Models.DAO
{
    public class DsChamDiemDAO
    {
        private static DsChamDiemDAO instance;
        public static DsChamDiemDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DsChamDiemDAO();
                }
                return instance;
            }
            private set
            {
                instance = value;
            }
        }

        public List<DTDsChamDiem> ListDsDeCuong(int mgv, string mact)
        {
            List<DTDsChamDiem> listDsDeCuong = new List<DTDsChamDiem>();
            DataTable dt = DataProvider.Instance.ExcuteQuery("[sp_LAYDUYETDETAITHEOMAGIANGVIEN] @magiangvien , @mact", new object[] { mgv , mact });
            foreach (DataRow item in dt.Rows) 
            {
                DTDsChamDiem danhsach = new DTDsChamDiem(item);
                listDsDeCuong.Add(danhsach);
            }
            return listDsDeCuong;
        }

        public List<DTDsChamDiem> ListLocDeTai(int mgv , string mact, DateTime ngaybd, DateTime ngaykt)
        {
            List<DTDsChamDiem> listDangKy = new List<DTDsChamDiem>();
            DataTable dt = DataProvider.Instance.ExcuteQuery("sp_LAYDUYETDETAITHEOMAGIANGVIENLOC @magiangvien , @mact , @ngaybd , @ngaykt", new object[] { mgv , mact, ngaybd, ngaykt });
            foreach (DataRow item in dt.Rows)
            {
                DTDsChamDiem Loc = new DTDsChamDiem(item);
                listDangKy.Add(Loc);
            }
            return listDangKy;
        }
        public bool CheckChamDiem(int mgv, int iddt)
        {
            if((int)DataProvider.Instance.ExcuteQuery("Select COUNT(*) FROM BIENBANCHAMDECUONG where MaGiangVien = " +mgv+"and IdDangKy="+iddt).Rows[0][0]>0)
            {
                return true;
            }    
            return false;
        }
    }
}