using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ConnectionDBDAL
{
    public class ConnectionSQLDBDAL
    {
        public static bool GetSqlServicecon(string server, string userid, string password, string database, out string mess)
        {
            try
            {
                string sqlstr = $"server={server};uid={userid};pwd={password};database={database}";
                SqlConnection conn = new SqlConnection(sqlstr);
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
