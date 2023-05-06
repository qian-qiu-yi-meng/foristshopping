using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DAL.ConnectionDB
{
    public class ConnectionMySQLDBDAL
    {
        #region 全局变量
        static string mysqlcon = string.Empty;
        #endregion
        #region  建立MySql数据库连接
        /// <summary>
        /// 建立数据库连接.
        /// </summary>
        /// <returns>返回MySqlConnection对象</returns>
        public static bool GetMySqlcon(string server, string userid, string password, string database, out string mess)
        {
            try
            {
                mysqlcon = $"server={server};user id={userid};password={password};database={database}"; //根据自己的设置
                MySqlConnection myCon = new MySqlConnection(mysqlcon);
                myCon.Open();
                mess = "打开成功";
                myCon.Close();
                Model.para.AppConfig.IsConnectionSuccess = true;
                return true;
            }
            catch (Exception ex)
            {
                mess = $"打开失败:{ex}";
                return false;
            }

        }
        #endregion
        #region  执行MySqlCommand增删改操作，返回受影响的行数。
        /// <summary>
        /// 执行MySqlCommand
        /// </summary>
        /// <param name="mysqlstr"></param>
        /// <returns></returns>
        public static int ExecuteNonMySQL(string mysqlstr)
        {
            MySqlConnection conn = new MySqlConnection(mysqlcon);
            conn.Open();
            MySqlCommand mysqlcom = new MySqlCommand(mysqlstr, conn);
            int count = mysqlcom.ExecuteNonQuery();
            mysqlcom.Dispose();
            conn.Close();
            conn.Dispose();
            return count;
        }
        /// <summary>
        /// 执行MySqlCommand
        /// </summary>
        /// <param name="M_str_sqlstr"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int ExecuteNonMySQL(string M_str_sqlstr, params MySqlParameter[] parameters)
        {
            MySqlConnection conn = new MySqlConnection(mysqlcon);
            conn.Open();
            MySqlCommand mysqlcom = new MySqlCommand(M_str_sqlstr, conn);
            mysqlcom.Parameters.AddRange(parameters);
            int count = mysqlcom.ExecuteNonQuery();
            mysqlcom.Dispose();
            conn.Close();
            conn.Dispose();
            return count;
        }
        public int ExecuteNonQuery(String sql)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(mysqlcon))
                {
                    connection.Open();
                    MySqlTransaction transaction = connection.BeginTransaction();

                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        try
                        {
                            PrepareCommand(cmd, connection, transaction, CommandType.Text, sql, null);

                            int rows = cmd.ExecuteNonQuery();
                            transaction.Commit();

                            cmd.Parameters.Clear();
                            return rows;
                        }
                        catch (MySqlException e1)
                        {
                            try
                            {
                                transaction.Rollback();
                            }
                            catch (Exception e2)
                            {
                                throw e2;
                            }

                            throw e1;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }



        public static DataTable GetMySqlRead(string mysqlsqlstr)
        {
            MySqlConnection conn = new MySqlConnection(mysqlcon);
            conn.Open();
            MySqlCommand mysqlcom = new MySqlCommand(mysqlsqlstr, conn);
            MySqlDataAdapter mda = new MySqlDataAdapter(mysqlcom);
            DataTable dt = new DataTable();
            mda.Fill(dt);
            conn.Close();
            return dt;
        }



        #endregion
















        ///// &lt;summary&gt;  
        ///// 对SQLite数据库执行增删改操作，返回受影响的行数。  
        ///// &lt;/summary&gt;  
        ///// &lt;param name="sql"&gt;要执行的增删改的SQL语句&lt;/param&gt;  
        ///// &lt;returns&gt;&lt;/returns&gt;  


        ///// &lt;summary&gt;  
        ///// 对SQLite数据库执行增删改操作，返回受影响的行数。  
        ///// &lt;/summary&gt;  
        ///// &lt;param name="sql"&gt;要执行的增删改的SQL语句&lt;/param&gt;  
        ///// &lt;returns&gt;&lt;/returns&gt;  
        //public int ExecuteNonQuery(String sql, MySqlParameter[] cmdParams)
        //    {
        //        try
        //        {
        //            using (MySqlConnection connection = this.getMySqlCon())
        //            {
        //                connection.Open();
        //                MySqlTransaction transaction = connection.BeginTransaction();

        //                using (MySqlCommand cmd = new MySqlCommand())
        //                {
        //                    try
        //                    {
        //                        PrepareCommand(cmd, connection, transaction, CommandType.Text, sql, cmdParams);

        //                        int rows = cmd.ExecuteNonQuery();
        //                        transaction.Commit();

        //                        cmd.Parameters.Clear();
        //                        return rows;
        //                    }
        //                    catch (MySqlException e1)
        //                    {
        //                        try
        //                        {
        //                            transaction.Rollback();
        //                        }
        //                        catch (Exception e2)
        //                        {
        //                            throw e2;
        //                        }

        //                        throw e1;
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            throw e;
        //        }
        //    }
        //    #endregion
        //    #region 对数据库执行查询操作
        //    /// &lt;summary&gt;
        //    /// 创建一个MySqlDataReader对象
        //    /// &lt;/summary&gt;
        //    /// &lt;param name="M_str_sqlstr"&gt;SQL语句&lt;/param&gt;
        //    /// &lt;returns&gt;返回MySqlDataReader对象&lt;/returns&gt;
        //    public DataTable getMySqlRead(string M_str_sqlstr)
        //    {
        //        MySqlConnection mysqlcon = this.getMySqlCon();
        //        mysqlcon.Open();
        //        MySqlCommand mysqlcom = new MySqlCommand(M_str_sqlstr, mysqlcon);
        //        MySqlDataAdapter mda = new MySqlDataAdapter(mysqlcom);
        //        DataTable dt = new DataTable();
        //        mda.Fill(dt);
        //        mysqlcon.Close();
        //        return dt;
        //    }
        //    /// &lt;summary&gt;
        //    /// 创建一个MySqlDataReader对象
        //    /// &lt;/summary&gt;
        //    /// &lt;param name="M_str_sqlstr"&gt;SQL语句&lt;/param&gt;
        //    /// &lt;returns&gt;返回MySqlDataReader对象&lt;/returns&gt;
        //    public DataTable getMySqlRead(string M_str_sqlstr, params MySqlParameter[] parameters)
        //    {
        //        MySqlConnection mysqlcon = this.getMySqlCon();
        //        mysqlcon.Open();
        //        MySqlCommand mysqlcom = new MySqlCommand(M_str_sqlstr, mysqlcon);
        //        mysqlcom.Parameters.AddRange(parameters);
        //        MySqlDataAdapter mda = new MySqlDataAdapter(mysqlcom);
        //        DataTable dt = new DataTable();
        //        mda.Fill(dt);
        //        mysqlcon.Close();
        //        return dt;
        //    }
        //    /// &lt;summary&gt;
        //    /// 执行一条计算查询结果语句，返回查询结果（object）。
        //    /// &lt;/summary&gt;
        //    /// &lt;param name="SQLString"&gt;计算查询结果语句&lt;/param&gt;
        //    /// &lt;returns&gt;查询结果（object）&lt;/returns&gt;
        //    private object ExecuteScalar(string SQLString)
        //    {
        //        using (MySqlConnection connection = this.getMySqlCon())
        //        {
        //            using (MySqlCommand cmd = new MySqlCommand(SQLString, connection))
        //            {
        //                try
        //                {
        //                    connection.Open();
        //                    object obj = cmd.ExecuteScalar();
        //                    if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
        //                    {
        //                        return null;
        //                    }
        //                    else
        //                    {
        //                        return obj;
        //                    }
        //                }
        //                catch (MySql.Data.MySqlClient.MySqlException e)
        //                {
        //                    connection.Close();
        //                    throw e;
        //                }
        //            }
        //        }
        //    }
        //    /// &lt;summary&gt;
        //    ///  用执行的数据库连接执行一个返回数据集的sql命令
        //    /// &lt;/summary&gt;
        //    /// &lt;param name="sql"&gt;&lt;/param&gt;
        //    /// &lt;returns&gt;&lt;/returns&gt;
        //    public MySqlDataReader ExecuteReader(String sql)
        //    {
        //        try
        //        {
        //            //创建一个MySqlConnection对象
        //            using (MySqlConnection connection = this.getMySqlCon())
        //            {
        //                connection.Open();
        //                MySqlTransaction transaction = connection.BeginTransaction();

        //                //创建一个MySqlCommand对象
        //                using (MySqlCommand cmd = new MySqlCommand())
        //                {
        //                    try
        //                    {
        //                        PrepareCommand(cmd, connection, transaction, CommandType.Text, sql, null);

        //                        MySqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
        //                        transaction.Commit();

        //                        cmd.Parameters.Clear();
        //                        return reader;
        //                    }
        //                    catch (MySqlException e1)
        //                    {
        //                        try
        //                        {
        //                            transaction.Rollback();
        //                        }
        //                        catch (Exception e2)
        //                        {
        //                            throw e2;
        //                        }

        //                        throw e1;
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            throw e;
        //        }
        //    }
        //    /// &lt;summary&gt;
        //    /// 查询返回Dtaset
        //    /// &lt;/summary&gt;
        //    /// &lt;param name="sql"&gt;&lt;/param&gt;
        //    /// &lt;returns&gt;&lt;/returns&gt;
        //    public DataTable ExecuteDataSet(String sql)
        //    {
        //        try
        //        {
        //            //创建一个MySqlConnection对象
        //            using (MySqlConnection connection = this.getMySqlCon())
        //            {
        //                connection.Open();
        //                MySqlTransaction transaction = connection.BeginTransaction();

        //                //创建一个MySqlCommand对象
        //                using (MySqlCommand cmd = new MySqlCommand())
        //                {
        //                    try
        //                    {
        //                        PrepareCommand(cmd, connection, transaction, CommandType.Text, sql, null);

        //                        MySqlDataAdapter adapter = new MySqlDataAdapter();
        //                        adapter.SelectCommand = cmd;
        //                        //DataSet ds = new DataSet();
        //                        DataTable dt = new DataTable();
        //                        adapter.Fill(dt);
        //                        //adapter.Fill(ds);

        //                        transaction.Commit();

        //                        //清除参数
        //                        cmd.Parameters.Clear();
        //                        return dt;

        //                    }
        //                    catch (MySqlException e1)
        //                    {
        //                        try
        //                        {
        //                            transaction.Rollback();
        //                        }
        //                        catch (Exception e2)
        //                        {
        //                            throw e2;
        //                        }

        //                        throw e1;
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            throw e;
        //        }
        //    }

        //    /// &lt;summary&gt;
        //    /// 查询返回Dtaset
        //    /// &lt;/summary&gt;
        //    /// &lt;param name="sql"&gt;&lt;/param&gt;
        //    /// &lt;returns&gt;&lt;/returns&gt;
        //    public DataSet ExecuteDataSet(String sql, MySqlParameter[] cmdParams)
        //    {
        //        try
        //        {
        //            //创建一个MySqlConnection对象
        //            using (MySqlConnection connection = this.getMySqlCon())
        //            {
        //                connection.Open();
        //                MySqlTransaction transaction = connection.BeginTransaction();

        //                //创建一个MySqlCommand对象
        //                using (MySqlCommand cmd = new MySqlCommand())
        //                {
        //                    try
        //                    {
        //                        PrepareCommand(cmd, connection, transaction, CommandType.Text, sql, cmdParams);

        //                        MySqlDataAdapter adapter = new MySqlDataAdapter();
        //                        adapter.SelectCommand = cmd;
        //                        DataSet ds = new DataSet();

        //                        adapter.Fill(ds);

        //                        transaction.Commit();

        //                        //清除参数
        //                        cmd.Parameters.Clear();
        //                        return ds;

        //                    }
        //                    catch (MySqlException e1)
        //                    {
        //                        try
        //                        {
        //                            transaction.Rollback();
        //                        }
        //                        catch (Exception e2)
        //                        {
        //                            throw e2;
        //                        }

        //                        throw e1;
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            throw e;
        //        }
        //    }
        //    #endregion
        //    #region 对数据执行分页操作
        //    /// &lt;summary&gt;
        //    /// 执行查询语句，返回DataTable
        //    /// &lt;/summary&gt;
        //    /// &lt;param name="SQLString"&gt;查询语句&lt;/param&gt;
        //    /// &lt;returns&gt;DataTable&lt;/returns&gt;
        //    private DataTable ExecuteDataTable(string SQLString)
        //    {
        //        using (MySqlConnection connection = this.getMySqlCon())
        //        {
        //            DataSet ds = new DataSet();
        //            try
        //            {
        //                connection.Open();
        //                MySqlDataAdapter command = new MySqlDataAdapter(SQLString, connection);
        //                command.Fill(ds, "ds");
        //            }
        //            catch (MySql.Data.MySqlClient.MySqlException ex)
        //            {
        //                throw new Exception(ex.Message);
        //            }
        //            connection.Close();
        //            return ds.Tables[0];
        //        }
        //    }

        //    /// &lt;summary&gt;
        //    /// 执行查询语句，返回DataTable
        //    /// &lt;/summary&gt;
        //    /// &lt;param name="db"&gt;数据库类型（Nozzle,Feeder,Head）&lt;/param&gt;
        //    /// &lt;param name="SQLString"&gt;查询语句&lt;/param&gt;
        //    /// &lt;returns&gt;DataTable&lt;/returns&gt;
        //    private DataTable ExecuteDataTable(string db, string SQLString)
        //    {
        //        using (MySqlConnection connection = this.getMySqlCon())
        //        {
        //            DataSet ds = new DataSet();
        //            try
        //            {
        //                connection.Open();
        //                MySqlDataAdapter command = new MySqlDataAdapter(SQLString, connection);
        //                command.Fill(ds, "ds");
        //            }
        //            catch (MySql.Data.MySqlClient.MySqlException ex)
        //            {
        //                throw new Exception(ex.Message);
        //            }
        //            return ds.Tables[0];
        //        }
        //    }

        //    /// &lt;summary&gt;
        //    /// 执行查询语句，返回DataSet
        //    /// &lt;/summary&gt;
        //    /// &lt;param name="SQLString"&gt;查询语句&lt;/param&gt;
        //    /// &lt;returns&gt;DataTable&lt;/returns&gt;
        //    private DataTable ExecuteDataTable(string SQLString, params MySqlParameter[] cmdParms)
        //    {
        //        using (MySqlConnection connection = this.getMySqlCon())
        //        {
        //            MySqlCommand cmd = new MySqlCommand();
        //            PrepareCommand(cmd, connection, null, CommandType.Text, SQLString, cmdParms);
        //            using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
        //            {
        //                DataSet ds = new DataSet();
        //                try
        //                {
        //                    da.Fill(ds, "ds");
        //                    cmd.Parameters.Clear();
        //                }
        //                catch (MySql.Data.MySqlClient.MySqlException ex)
        //                {
        //                    throw new Exception(ex.Message);
        //                }
        //                return ds.Tables[0];
        //            }
        //        }
        //    }
        //    //获取起始页码和结束页码
        //    private DataTable ExecuteDataTable(string cmdText, int startResord, int maxRecord)
        //    {
        //        using (MySqlConnection connection = this.getMySqlCon())
        //        {
        //            DataSet ds = new DataSet();
        //            try
        //            {
        //                connection.Open();
        //                MySqlDataAdapter command = new MySqlDataAdapter(cmdText, connection);
        //                command.Fill(ds, startResord, maxRecord, "ds");
        //            }
        //            catch (MySql.Data.MySqlClient.MySqlException ex)
        //            {
        //                throw new Exception(ex.Message);
        //            }
        //            return ds.Tables[0];
        //        }
        //    }
        //    /// &lt;summary&gt;
        //    /// 获取分页数据 在不用存储过程情况下
        //    /// &lt;/summary&gt;
        //    /// &lt;param name="recordCount"&gt;总记录条数&lt;/param&gt;
        //    /// &lt;param name="selectList"&gt;选择的列逗号隔开,支持top num&lt;/param&gt;
        //    /// &lt;param name="tableName"&gt;表名字&lt;/param&gt;
        //    /// &lt;param name="whereStr"&gt;条件字符 必须前加 and&lt;/param&gt;
        //    /// &lt;param name="orderExpression"&gt;排序 例如 ID&lt;/param&gt;
        //    /// &lt;param name="pageIdex"&gt;当前索引页&lt;/param&gt;
        //    /// &lt;param name="pageSize"&gt;每页记录数&lt;/param&gt;
        //    /// &lt;returns&gt;&lt;/returns&gt;
        //    public DataTable getPager(out int recordCount, string selectList, string tableName, string whereStr, string orderExpression, int pageIdex, int pageSize)
        //    {
        //        int rows = 0;
        //        DataTable dt = new DataTable();
        //        MatchCollection matchs = Regex.Matches(selectList, @"top\s+\d{1,}", RegexOptions.IgnoreCase);//含有top
        //        string sqlStr = sqlStr = string.Format("select {0} from {1} where 1=1 {2}", selectList, tableName, whereStr);
        //        if (!string.IsNullOrEmpty(orderExpression)) { sqlStr += string.Format(" Order by {0}", orderExpression); }
        //        if (matchs.Count & gt; 0) //含有top的时候
        //    {
        //            DataTable dtTemp = ExecuteDataTable(sqlStr);
        //            rows = dtTemp.Rows.Count;
        //        }
        //    else //不含有top的时候
        //        {
        //            string sqlCount = string.Format("select count(*) from {0} where 1=1 {1} ", tableName, whereStr);
        //            //获取行数
        //            object obj = ExecuteScalar(sqlCount);
        //            if (obj != null)
        //            {
        //                rows = Convert.ToInt32(obj);
        //            }
        //        }
        //        dt = ExecuteDataTable(sqlStr, (pageIdex - 1) * pageSize, pageSize);
        //        recordCount = rows;
        //        return dt;
        //    }
        //    #endregion
        //    /// &lt;summary&gt;
        //    /// 准备执行一个命令
        //    /// &lt;/summary&gt;
        //    /// &lt;param name="cmd"&gt;sql命令&lt;/param&gt;
        //    /// &lt;param name="conn"&gt;OleDb连接&lt;/param&gt;
        //    /// &lt;param name="trans"&gt;OleDb事务&lt;/param&gt;
        //    /// &lt;param name="cmdType"&gt;命令类型例如 存储过程或者文本&lt;/param&gt;
        //    /// &lt;param name="cmdText"&gt;命令文本,例如:Select * from Products&lt;/param&gt;
        //    /// &lt;param name="cmdParms"&gt;执行命令的参数&lt;/param&gt;
        private void PrepareCommand(MySqlCommand cmd, MySqlConnection conn, MySqlTransaction trans, CommandType cmdType, string cmdText, MySqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (MySqlParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }
    }
}