using MySql.Data.MySqlClient;
using Persistence;

namespace DAL
{
    public class UserDAL
    {
        private string? query;
        private MySqlDataReader reader;

        public User GetUserByName(string _UserName)
        {
            query = $"select UserID, UserName, Password, FullName, Birthday, Email, Phone, Address, Role from Users where UserName = '{_UserName}'";

            //Mở kết nối đến database
            DbHelper.OpenConnection();

            //Thực thi lệnh
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
            user.UserId = reader.GetInt32("UserID");
            user.UserName = reader.GetString("UserName");
            user.Password = reader.GetString("Password");
            user.FullName = reader.GetString("FullName");
            user.Birthday = reader.GetDateTime("Birthday");
            user.Email = reader.GetString("Email");
            user.Phone = reader.GetString("Phone");
            user.Address = reader.GetString("Address");
            user.Role = reader.GetString("Role");
            return user;
        }

        public void SaveUser(User user)
        {
            query = $"Insert into Users (UserName, Password , FullName, Birthday, Email, Phone, Address, Role) value ( '{user.UserName}', '{user.Password}', '{user.FullName}', '{user.Birthday}', '{user.Email}', '{user.Phone}', '{user.Address}', '{user.Role}')";

            //Mở kết nối đến database
            DbHelper.OpenConnection();

            //Thực thi lệnh
            reader = DbHelper.ExecQuery(query);
            DbHelper.CloseConnection();
        }

    }
}
