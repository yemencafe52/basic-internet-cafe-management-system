using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YemenCafe
{
    class User
    {
        private byte user_no;
        private string user_username;
        private string user_password;
        private int error_no;

        internal User(byte number)
        {
            try
            {
                AccessDB db = new AccessDB(Constants.GetConnectionString);
                string sql = "select user_no,user_username,user_password from tblUsers where user_no=" + number.ToString();
                
                if (db.ExcuteQuery(sql))
                {
                    if (db.DataReader.Read())
                    {
                        this.user_no = Convert.ToByte(db.DataReader["user_no"]);
                        this.user_username = Convert.ToString(db.DataReader["user_username"]);
                        this.user_password = Convert.ToString(db.DataReader["user_password"]);

                        this.error_no = 0;

                    }
                }

                db.CloseConnection();
            }
            catch
            {
                error_no = 1;
            }
        }
        internal User()
        {

        }

        internal User(
             byte user_no,
             string user_username,
             string user_password,
             int error_no

            )
        {
            this.user_no = user_no;
            this.user_username = user_username;
            this.user_password = user_password;
            this.error_no = error_no;
        }




        public byte Number
        {
            get
            {
                return this.user_no;
            }
        }


        public string UserName
        {
            get
            {
                return this.user_username;
            }
        }


        internal string Password
        {
            get
            {
                return this.user_password;
            }
        }


        internal int ErrorNumber
        {
            get
            {
                return this.error_no;
            }
        }

    }


    internal static class UserManager
    {
        private static User activeUser = null;
        internal static bool AddNewUser(User user)
        {
            bool res = false;

            try
            {
                AccessDB db = new AccessDB(Constants.GetConnectionString);
                string sql = "insert into tblUsers (user_no,user_username,user_password) values("+ user.Number.ToString() +",'"+user.UserName+"','"+user.Password+"')"; 

                if(db.ExcuteNonQuery(sql) == 1)
                {
                    res = true;
                }


            }
            catch
            {

            }

            return res;

        }

        internal static bool UpdateUserInfo(User user)
        {
            bool res = false;

            try
            {
                AccessDB db = new AccessDB(Constants.GetConnectionString);
                string sql = "update tblUsers set user_password='"+ user.Password +"' where user_no=" + user.Number.ToString(); ;

                if (db.ExcuteNonQuery(sql) == 1)
                {
                    res = true;
                }
            }
            catch
            {

            }

            return res;

        }

        internal static bool Login(string username , string password ,ref User user)
        {
            bool res = false;

            try
            {
                AccessDB db = new AccessDB(Constants.GetConnectionString);
                string sql = "select user_no,user_username,user_password from tblUsers where user_username='"+ username + "' and user_password='"+ password +"'";

                if (db.ExcuteQuery(sql))
                {
                    if(db.DataReader.Read())
                    {
                        user = new User(Convert.ToByte(db.DataReader["user_no"]), Convert.ToString(db.DataReader["user_username"]), Convert.ToString(db.DataReader["user_password"]), 0);
                        activeUser = user;
                        res = true;
                    }
                }

                db.CloseConnection();
                
                
            }
            catch
            {

            }

            return res;

        }

        internal static User GetActiveUser
        {
            get
            {
                return activeUser;
            }
        }

        internal static List<User> GetUsers()
        {
            List<User> res = new List<User>();

            try
            {
                AccessDB db = new AccessDB(Constants.GetConnectionString);
                string sql = "select user_no,user_username,user_password from tblUsers";

                if(db.ExcuteQuery(sql))
                {
                    while(db.DataReader.Read())
                    {
                        res.Add(new User(Convert.ToByte(db.DataReader["user_no"]), Convert.ToString(db.DataReader["user_username"]), Convert.ToString(db.DataReader["user_password"]), 0));
                    }
                }

                db.CloseConnection();

            }
            catch
            {
                res.Clear();
            }

            return res;

        }

        internal static byte GenerateNewUserNumber()
        {

            byte res = 0;
            try
            {

                AccessDB db = new AccessDB(Constants.GetConnectionString);
                string sql = "select max(user_no) as res from tblUsers";

                if (db.ExcuteQuery(sql))
                {
                    if (db.DataReader.Read())
                    {
                        string res0 = db.DataReader["res"].ToString();
                        if (byte.TryParse(res0, out res))
                        {
                            res++;
                        }
                    }
                }

                db.CloseConnection();

            }
            catch
            {
            }

            return res;
        }

    }

}
