using QLNCKH.Models.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace QLNCKH.Models.DAO
{
    public class HoiDongDao
    {
        private static HoiDongDao instance;
        public static HoiDongDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new HoiDongDao();
                }
                return instance;
            }
            private set
            {
                instance = value;
            }
        }
        private HoiDongDao() { }
        public string LaysoluongHoidong(string makhoa)
        {
            string query = "[sp_LAYSOLUONGHOIDONG] @MaKhoa";
            DataTable kq = DataProvider.Instance.ExcuteQuery(query, new object[] { makhoa });
            string CountHoidong = kq.Rows[0][0].ToString();
            return CountHoidong;
        }
        public string LaysoluongHoidongNT(string makhoa)
        {
            string query = "[sp_LAYSOLUONGHOIDONGNT] @MaKhoa";
            DataTable kq = DataProvider.Instance.ExcuteQuery(query, new object[] { makhoa });
            string CountHoidong = kq.Rows[0][0].ToString();
            return CountHoidong;
        }
    }
}