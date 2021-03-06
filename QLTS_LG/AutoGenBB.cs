﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows.Forms;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace QLTS_LG
{
    class AutoGenBB
    {

        static string connectionString = ConfigurationManager.ConnectionStrings["QLTS_LG.Properties.Settings.QLTSConnectionString"].ConnectionString;
        OracleConnection con = new OracleConnection(connectionString);
        OracleDataAdapter DataAdapter = new OracleDataAdapter();
        public string SoBBBG;

        public void AutoGenBBBG()
        {
            //DateTime date_BB = new DateTime();
            int i = 1;
            string prefix = "000";

            //string LastNumOfBB;
            var date_BBBG = DateTime.Now.ToString("yyyyMMdd");
            OracleCommand cmd = new OracleCommand("SELECT So_Bien_ban FROM ( select So_bien_ban from Bien_Ban order by So_Bien_ban DESC) where ROWNUM = 1", con); //lấy dữ liệu số biên bản bàn giao từ bảng Bien_Ban
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataTable dtSoBB = new DataTable();
            da.Fill(dtSoBB); //đổ dữ liệu vào table dtSoBB
            if (dtSoBB.Rows.Count == 0) //kiểm tra trường hợp chưa có data trong bảng
            {
                DataRow dr = dtSoBB.NewRow(); //tạo hàng dữ liệu dr trong dtSoBB
                dr["So_Bien_ban"] = date_BBBG + "-" + prefix.ToString() + i.ToString();
                dtSoBB.Rows.Add(dr); //thêm hàng dữ liệu vừa được tạo vào dtSoBB
                SoBBBG = dr["So_Bien_ban"].ToString();
            }
            else if (dtSoBB.Rows.Count > 0)
            {
                //string processing...
                int LastRowIndex = dtSoBB.Rows.Count - 1;
                string LastNumOfBB = dtSoBB.Rows[LastRowIndex][0].ToString(); //lay ra gia tri o hang cuoi cung
                int dateBBLen = date_BBBG.Length; //lay gia tri chieu dai cua phan ngay thang
                int LastNumLen = LastNumOfBB.Length; //lay gia tri toan bo chuoi
                string DateTimeString = LastNumOfBB.Substring(0, dateBBLen); //cắt ra phần ngày tháng


                string iNumber = LastNumOfBB.Substring(LastNumLen - (LastNumLen - dateBBLen) + 1); //cắt ra phần số thứ tự
                int iNum = Convert.ToInt32(iNumber);
                // nếu ngày tháng hiện tại bằng giá trị ngày tháng trong số biên bản gần nhất
                //int iNum2;

                //string prefix2;
                if (DateTimeString.Equals(date_BBBG) == true)
                {

                    //thêm phần tiền tố "000" vào trước phần số thứ tự i.      
                    iNum = iNum + 1;
                    //Cái này cẩn cải tiến dưới dạng vòng lặp
                    if (iNum >= 10 && iNum < (100))
                    {
                        prefix = prefix.Remove(0, 1);
                        SoBBBG = date_BBBG + "-" + prefix + iNum.ToString();

                    }
                    else if (iNum >= (100) && iNum < (1000))
                    {
                        prefix = prefix.Remove(0, 2);
                        SoBBBG = date_BBBG + "-" + prefix + iNum.ToString();
                    }
                    else if (iNum >= (1000))
                    {
                        prefix = prefix.Remove(0, 3);
                        SoBBBG = date_BBBG + "-" + prefix + iNum.ToString();
                    }
                    else
                    {
                        SoBBBG = date_BBBG + "-" + prefix + iNum.ToString();
                    }

                    
                }
                else if (DateTimeString.Equals(date_BBBG) == false)
                {
                    SoBBBG = date_BBBG + "-" + prefix.ToString() + i.ToString();
                }
            }

        }
    }
}
