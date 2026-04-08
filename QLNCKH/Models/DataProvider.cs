using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace QLNCKH.Models
{
    public class DataProvider
    {
        private static DataProvider instance;
        string connection = ConfigurationManager.ConnectionStrings["QLDETAINCKHConnectionString1"].ConnectionString;
        public static DataProvider Instance
        {
            get
            {
                if (instance == null)
                {

                    instance = new DataProvider();
                }
                return instance;
            }
            private set
            {
                instance = value;
            }
        }

        private DataProvider()
        {
            connection = ConfigurationManager.ConnectionStrings["QLDETAINCKHConnectionString1"].ConnectionString;
        }



        public DataTable ExcuteQuery(string query, object[] parameter = null)
        {
            DataTable dt = new DataTable();
            using (SqlConnection Con = new SqlConnection(connection))
            {
                Con.Open();
                SqlCommand cmd = new SqlCommand(query, Con);
                if (parameter != null)
                {
                    string[] listpara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listpara)
                    {
                        if (item.Contains('@'))
                        {
                            cmd.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                    }
                }
                SqlDataAdapter sql = new SqlDataAdapter(cmd);
                sql.Fill(dt);
                Con.Close();
            }
            return dt;
        }
        public int ExcuteNonQuery(string query, object[] parameter = null)
        {
            int dt = 0;
            using (SqlConnection Con = new SqlConnection(connection))
            {
                Con.Open();
                SqlCommand cmd = new SqlCommand(query, Con);
                if (parameter != null)
                {
                    string[] listpara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listpara)
                    {
                        if (item.Contains('@'))
                        {
                            cmd.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                    }
                }
                dt = cmd.ExecuteNonQuery();
                Con.Close();
            }
            return dt;
        }
        public DataTable ExcuteQuery1(string query, string colum, object[] parameter = null)
        {
            DataTable dt = new DataTable();
            using (SqlConnection Con = new SqlConnection(connection))
            {
                Con.Open();
                SqlCommand cmd = new SqlCommand(query, Con);
                if (parameter != null)
                {
                    string[] listpara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listpara)
                    {
                        if (item.Contains('@'))
                        {
                            cmd.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                    }
                }
                SqlDataReader rdr;
                rdr = cmd.ExecuteReader();
                dt.Columns.Add(colum, typeof(string));
                dt.Load(rdr);
                Con.Close();
            }
            return dt;
        }

    }
}