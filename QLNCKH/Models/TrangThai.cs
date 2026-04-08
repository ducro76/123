using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QLNCKH.Models;
namespace QLNCKH.Models
{
    public class TrangThai
    {
        public List<TRANGTHAI> trangThai = new List<TRANGTHAI>();
        DataQLNCKHDataContext db = new DataQLNCKHDataContext();
        public static TrangThai instance;

        public static TrangThai Instance
        {
            get
            {
                if (instance == null)
                {

                    instance = new TrangThai();
                }
                return instance;
            }
            private set
            {
                instance = value;
            }
        }
        private TrangThai()
        {
            trangThai = db.TRANGTHAIs.ToList();
        }

        public int TrangThaiDeTaiDangLam(string MSSV )
        {
            int q;
            var tt = db.DANGKies.Where(n=>n.MaSoSinhVien ==MSSV).ToList();
            if (tt.Count > 0)
            {
                q = (int)tt.ElementAt(0).TrangThai;
            }
            else
            {
                q = 1;
            }
            return q;
        }

        public int TrangThaiDeTaiDangLamNghiemThu(string MSSV)
        {
            int q;
            var tt = db.DETAIs.Where(n => n.MaSoSinhVien == MSSV && n.MaTrangThai <= 5).ToList();
            if(tt.Count>0)
            {
                q = (int)tt.ElementAt(0).MaTrangThai;
            }
            else
            {
                q = 1;
            }

            return q;
        }
        public DANGKY  DeTaiDangDangKy(string MSSV)
        {
            DANGKY dk = db.DANGKies.Where(n => n.MaSoSinhVien == MSSV && n.KetQua == false).SingleOrDefault();
            return dk;
        }
        public bool InsertThongBaoChamDiem(string ThongBao ,int IdHoiDong ,int IdDangKy,bool ICheck,DateTime Date)
        {
            string query = "sp_AddThongBaoChamLai @thongbao , @idhoidong , @iddangky , @check , @date ";
            if (DataProvider.Instance.ExcuteNonQuery(query,new object[] { ThongBao, IdHoiDong, IdDangKy, ICheck, Date })>0)
            {
                return true;
            }
            return false;
        }

    }
}