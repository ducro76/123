using System;
using System.Collections.Generic;
using System.Linq;
using QLNCKH.Models.DTO;
using System.Web;
using System.Data;

namespace QLNCKH.Models.DAO
{
    public class DsChamDiemDTDA0
    {
        private static DsChamDiemDTDA0 instance;
        public static DsChamDiemDTDA0 Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DsChamDiemDTDA0();
                }
                return instance;
            }
            private set
            {
                instance = value;
            }
        }

        public List<DTOChamDiemDT> ListDsDeTai(int mgv, string mact)
        {
            List<DTOChamDiemDT> listDsDeTai = new List<DTOChamDiemDT>();
            DataTable dt = DataProvider.Instance.ExcuteQuery("[sp_LAYDSCHAMDETAITHEOMAGIANGVIEN] @magiangvien , @mact", new object[] { mgv, mact });
            foreach (DataRow item in dt.Rows)
            {
                DTOChamDiemDT danhsach = new DTOChamDiemDT(item);
                listDsDeTai.Add(danhsach);
            }
            return listDsDeTai;
        }

        public List<DTOChamDiemDT> ListLocDeTai(int mgv, string mact, DateTime ngaybd, DateTime ngaykt)
        {
            List<DTOChamDiemDT> listDsDeTai = new List<DTOChamDiemDT>();
            DataTable dt = DataProvider.Instance.ExcuteQuery("[sp_LAYDSCHAMDETAITHEOMAGIANGVIENLOC] @magiangvien , @mact , @ngaybd , @ngaykt", new object[] { mgv, mact, ngaybd, ngaykt });
            foreach (DataRow item in dt.Rows)
            {
                DTOChamDiemDT Loc = new DTOChamDiemDT(item);
                listDsDeTai.Add(Loc);
            }
            return listDsDeTai;
        }
        public bool CheckChamDiem(int mgv, int iddt)
        {
            if ((int)DataProvider.Instance.ExcuteQuery("Select COUNT(*) FROM BIENBANNGHIEMTHU where MaGiangVien = " + mgv + "and MaDeTai=" + iddt).Rows[0][0] > 0)
            {
                return true;
            }
            return false;
        }
    }
}