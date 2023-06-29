using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Web;

namespace fileQuery20230621
{
    public class MysqlManager
    {
        public int lon;

        public bool manageFlag = false;

        private string conStr = null;
        private MySqlConnection msc = null;
        private MySqlCommand msco = null;
        private MySqlDataReader msdr = null;

        //存储query的数据 
        public List<string[]> queryList = new List<string[]>();

        public MysqlManager(string conStr)
        {
            this.conStr = conStr ?? throw new ArgumentNullException(nameof(conStr));
        }


       
        //数据库查询
        public void OpQueryMySql(string queryStr)
        {
            try
            {
                msc = new MySqlConnection(conStr);
                msco = new MySqlCommand(queryStr, this.msc);
                msc.Open();
                msdr = msco.ExecuteReader();

                queryList = new List<string[]>();

                while (msdr.Read())
                {
                    lon = msdr.FieldCount;
                    string[] info = new string[lon];
                    for (int i = 0; i < lon; i++)
                    {
                        info[i] = msdr[i].ToString();
                    }
                    queryList.Add(info);
                }
            }
            catch
            {
                //查询失败
            }
            finally
            {
                msco.Dispose();
                msc.Close();
            }
        }

         //数据库增删改
        public void OpAddDeleUpdateMySql(string opStr)
        {
            try
            {
                msc = new MySqlConnection(conStr);
                msco = new MySqlCommand(opStr, this.msc);
                msc.Open();
                msco.ExecuteNonQuery();
                manageFlag = true;
            }
            catch
            {
                manageFlag = false;
                //操作失败
            }
            finally
            {
                msco.Dispose();
                msc.Close();
            }

        }
    }
}
