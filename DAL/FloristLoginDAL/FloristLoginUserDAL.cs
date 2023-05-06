using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace DAL.FloristLoginDAL
{
    public class FloristLoginUserDAL
    {
        
        public static bool SelectLoginUser(string userName,string passWord,out string mess)
        {
            try
            {
                //var users = ConnectionDB.ConnectionMySQLDBDAL.GetMySqlRead("select * from userinfo");
                //var objuser = Model.para.JsonUtils.ObjectToJson(users);
                //var userinfo = Model.para.JsonUtils.TSoList<Model.UserInfo>(objuser);
                florist_dbEntities db = new florist_dbEntities();
                var user = db.userinfo.Where(u => u.UserName == userName && u.PassWord == passWord).ToList();


                foreach (var item in user.ToList())
                {
                    if ((userName == item.UserName && passWord == item.PassWord) || (userName == item.Phone && passWord == item.PassWord))
                    {
                        mess = "登录成功";
                        return true;
                    }
                    else if ((userName == item.UserName && passWord != item.PassWord) || userName == item.Phone && passWord != item.PassWord)
                    {
                        mess = "用户名/电话号码或密码不正确";
                        return false;
                    }
                }
                mess = "用户不存在，请注册用户";
                return false;
            }
            catch (Exception ex)
            {
                mess = $"操作失败{ex}";
                return false;
            }
        }
    }
}
