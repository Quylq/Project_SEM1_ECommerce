using MySql.Data.MySqlClient;
using Persistence;

namespace DAL
{
    public class UserDAL
    {
        private string? query;
        private MySqlDataReader? reader;

        public bool CheckUserName(string _UserName)
        {
            query = $"select * from Users where UserName = '{_UserName}'";
            DbHelper.OpenConnection();
            reader = DbHelper.ExecQuery(query);
            if (reader.Read())
            {
                DbHelper.CloseConnection();
                return true;
            }
            else
            {
                DbHelper.CloseConnection();
                return false;
            }
        }
        public User? Login(string _UserName, string _Password)
        {
            query = $"select * from Users where UserName = '{_UserName}' and Password = '{_Password}'";
            DbHelper.OpenConnection();
            reader = DbHelper.ExecQuery(query);

            User user = new User();
            if (reader.Read())
            {
                user = GetUserInfo(reader);
                DbHelper.CloseConnection();
                return user;
            }
            else
            {
                DbHelper.CloseConnection();
                return null;
            }
        }
        public User GetUserByID(int _UserID)
        {
            query = $"select * from Users where UserID = '{_UserID}'";

            DbHelper.OpenConnection();
            reader = DbHelper.ExecQuery(query);

            User user = new User();
            if (reader.Read())
            {
                user = GetUserInfo(reader);
            }
            DbHelper.CloseConnection();
            return user;
        }
        private User GetUserInfo(MySqlDataReader reader)
        {
            User user = new User();
            string format = "dd/MM/yyyy";
            user.UserID = reader.GetInt32("UserID");
            user.UserName = reader.GetString("UserName");
            user.Password = reader.GetString("Password");
            user.FullName = reader.GetString("FullName");
            user.Birthday = reader.GetDateTime("Birthday").ToString(format);
            user.Email = reader.GetString("Email");
            user.Phone = reader.GetString("Phone");
            user.AddressID = reader.GetInt32("AddressID");
            user.Role = reader.GetString("Role");
            return user;
        }
        public bool InsertUser(User user)
        {
            query = $@"Insert into Users (UserID, UserName, Password , FullName, Birthday, Email, Phone, AddressID, Role) 
            value ('{user.UserID}', '{user.UserName}', '{user.Password}', '{user.FullName}', '{user.Birthday}', '{user.Email}', '{user.Phone}', '{user.AddressID}', '{user.Role}')";

            try
            {
                DbHelper.OpenConnection();
                reader = DbHelper.ExecQuery(query);
                DbHelper.CloseConnection();
                return true;
            }
            catch (MySql.Data.MySqlClient.MySqlException)
            {
                DbHelper.CloseConnection();
                return false;
            }
        }
        public bool UpdateUser(User user)
        {
            query = $@"update Users 
                set FullName = '{user.FullName}',
                    Birthday = '{user.Birthday}',
                    Email = '{user.Email}',
                    Phone = '{user.Phone}',
                    AddressID = '{user.AddressID}'
                where UserID = '{user.UserID}'";

            try
            {
                DbHelper.OpenConnection();
                reader = DbHelper.ExecQuery(query);
                DbHelper.CloseConnection();
                return true;
            }
            catch (MySql.Data.MySqlClient.MySqlException)
            {
                DbHelper.CloseConnection();
                return false;
            }
        }
        public bool UpdatePassword(int _UserID, string _Password)
        {
            query = $@"update Users 
                set Password = '{_Password}'
                where UserID = '{_UserID}'";

            if (_Password != null && _Password != "")
            {
                try
                {
                    DbHelper.OpenConnection();
                    reader = DbHelper.ExecQuery(query);
                    DbHelper.CloseConnection();
                    return true;
                }
                catch (MySql.Data.MySqlClient.MySqlException)
                {
                    DbHelper.CloseConnection();
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public int UserIDMax()
        {
            query = $"select max(UserID) from users";
            DbHelper.OpenConnection();
            reader = DbHelper.ExecQuery(query);
            int _UserID = 0;

            try
            {
                if (reader.Read())
                {
                    _UserID = reader.GetInt32("max(UserID)");
                }
            }
            catch (System.Data.SqlTypes.SqlNullValueException)
            {
                _UserID = 0;
            }
            DbHelper.CloseConnection();
            return _UserID;
        }
    }
}
