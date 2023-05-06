using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ConnectionDBDAL
{
    public class ConnectionWindowsSQLDBDAL
    {
        public static bool GetWindowsSqlServicecon(string service, string database,out string mess)
        {
            try
            {
                string sqlStr = $"server={service};database={database};Trusted_Connection=SSPI";
                SqlConnection conn = new SqlConnection(sqlStr);
                conn.Open();
                mess = "打开成功";
                return true;
            }
            catch (Exception ex)
            {
                mess = $"打开失败：{ex}";
                return false;
            }
            

        }
    }
}
