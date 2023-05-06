using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DAL.FloristLoginDAL
{
    public class FloristGetVerificationCodeDAL
    {
        static string code;
        static Thread threadCode;
        static int Timeout = 300;
        /// <summary>
        /// 找回密码验证
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        /// <param name="mess"></param>
        /// <returns></returns>
        public static bool GetLoginVerificationCode(string phone, string email, out string mess)
        {
            try
            {
                foreach (var item in DataConversion())
                {
                    if (phone == item.Phone && email == item.Email)
                    {
                        MailMessage mail = new MailMessage();  //实例化一个发送邮件类
                        mail.From = new MailAddress("3535297841@qq.com");   //发件人邮箱地址
                        mail.To.Add(new MailAddress(email));    //收件人邮箱地址
                        mail.Subject = "【情有独钟花店网】找回密码";    //邮件标题
                        code = CreateRandomCode(6);   //生成伪随机的6位数验证码
                        mail.Body = "验证码是: " + code + "，请在5分钟内进行验证。验证码提供给他人可能导致账号被盗，请勿泄露，谨防被骗。系统邮件请勿回复。";  //邮件内容          
                        SmtpClient client = new SmtpClient("smtp.qq.com");   //实例化一个SmtpClient类。
                        client.EnableSsl = true;    //使用安全加密连接
                        client.Credentials = new NetworkCredential("3535297841@qq.com", "fojxadvgeprzdaie");//验证发件人身份(发件人的邮箱，邮箱里的生成授权码);        
                        client.Send(mail);
                        mess = "发送成功";
                        threadCode = new Thread(Time);
                        threadCode.IsBackground = true;
                        threadCode.Start();
                        return true;
                    }
                    else if (phone == item.Phone && email != item.Email)
                    {
                        mess = "邮箱地址与账号不匹配，请检查邮箱与账号";
                        return false;
                    }
                }
                mess = "账号不存在";
                return false;
            }
            catch (Exception ex)
            {
                mess = $"发送错误:{ex}";
                return false;
            }

        }
        /// <summary>
        /// 随机验证码
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string CreateRandomCode(int length)  //生成由数字和大小写字母组成的验证码
        {
            string list = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM1234567890";
            //list中存放着验证码的元素
            Random random = new Random();
            string code = "";   //验证码
            for (int i = 0; i < length; i++)   //循环6次得到一个伪随机的六位数验证码
            {
                code += list[random.Next(0, list.Length - 1)];
            }
            return code;
        }
        /// <summary>
        /// 设置验证码过期时间
        /// </summary>
        static void Time()
        {
            while (true)
            {
                Thread.Sleep(1000);
                CountDown();
            }
        }
        /// <summary>
        /// 过期产生新的验证码
        /// </summary>
        public static void CountDown()
        {
            if (Timeout <= 0)
            {
                code = CreateRandomCode(6);
                Timeout = 300;
            }
            else
            {
                Timeout--;
            }
        }
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="password"></param>
        /// <param name="phone"></param>
        /// <param name="verificationcode"></param>
        /// <param name="mess"></param>
        /// <returns></returns>
        public static bool ResetPassword(string password, string phone, string verificationcode, out string mess)
        {
            try
            {
                if (code == verificationcode)
                {
                    var users = ConnectionDB.ConnectionMySQLDBDAL.ExecuteNonMySQL($"update userinfo set password='{password}' where phone='{phone}'");
                    if (users == 1)
                    {
                        mess = "重置成功";
                        return true;
                    }
                }
                mess = "验证码不正确";
                return false;
            }
            catch (Exception ex)
            {
                mess = ex.ToString();
                return false;
            }
        }
        /// <summary>
        /// 获取注册验证码
        /// </summary>
        /// <param name="email"></param>
        /// <param name="phone"></param>
        /// <param name="mess"></param>
        /// <returns></returns>
        public static bool RegistrationVerificationCode(string email, string phone,out string mess)
        {
            try
            {
                foreach (var item in DataConversion())
                {
                    if (item.Phone == phone || item.Email == email)
                    {
                        mess = "用户已存在，请检查邮箱和电话号码或通过忘记密码找回";
                        return false;
                    }
                }
                MailMessage mail = new MailMessage();  //实例化一个发送邮件类
                mail.From = new MailAddress("3535297841@qq.com");   //发件人邮箱地址
                mail.To.Add(new MailAddress(email));    //收件人邮箱地址
                mail.Subject = "【情有独钟花店网】注册验证码";    //邮件标题
                code = CreateRandomCode(6);   //生成伪随机的6位数验证码
                mail.Body = "验证码是: " + code + "，请在5分钟内进行验证。验证码提供给他人可能导致账号被盗，请勿泄露，谨防被骗。系统邮件请勿回复。";  //邮件内容          
                SmtpClient client = new SmtpClient("smtp.qq.com");   //实例化一个SmtpClient类。
                client.EnableSsl = true;    //使用安全加密连接
                client.Credentials = new NetworkCredential("3535297841@qq.com", "fojxadvgeprzdaie");//验证发件人身份(发件人的邮箱，邮箱里的生成授权码);        
                client.Send(mail);
                mess = "发送成功";
                threadCode = new Thread(Time);
                threadCode.IsBackground = true;
                threadCode.Start();
                return true;
            }
            catch (Exception ex)
            {
                mess =ex.ToString();
                return false;
            }
        }
        /// <summary>
        /// 验证注册验证码
        /// </summary>
        /// <param name="password"></param>
        /// <param name="phone"></param>
        /// <param name="verificationcode"></param>
        /// <param name="mess"></param>
        /// <returns></returns>
        public static bool RegisteredAccount(string password,string phone,string verificationcode,out string mess)
        {
            try
            {
                if (code == verificationcode)
                {
                    var users = ConnectionDB.ConnectionMySQLDBDAL.ExecuteNonMySQL($"insert ");
                    if (users == 1)
                    {
                        mess = "重置成功";
                        return true;
                    }
                }
                mess = "验证码不正确";
                return false;
            }
            catch (Exception ex)
            {
                mess = ex.ToString();
                return false;
            }
        }
        /// <summary>
        /// 获取用户数据信息
        /// </summary>
        /// <returns></returns>
        public static List<Model.userinfo> DataConversion()
        {
            var users = ConnectionDB.ConnectionMySQLDBDAL.GetMySqlRead("select * from userinfo");
            var objuser = Model.para.JsonUtils.ObjectToJson(users);
            var userinfo = Model.para.JsonUtils.TSoList<Model.userinfo>(objuser);
            return userinfo;
        }
    }
}
