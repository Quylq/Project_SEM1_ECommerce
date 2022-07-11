using MySql.Data.MySqlClient;
using Persistence;

namespace DAL
{
    public class UserDAL
    {
        private string? query;
        private MySqlDataReader? reader;

        public User GetUserByName(string _UserName)
        {
            query = $"select UserID, UserName, Password, FullName, Birthday, Email, Phone, Address, Role from Users where UserName = '{_UserName}'";

            DbHelper.OpenConnection("Seller");
            reader = DbHelper.ExecQuery(query);

            User user = new User();
            if (reader.Read())
            {
                user = GetUserInfo(reader);
            }
            DbHelper.CloseConnection();
            return user;
        }
        public User GetUserByID(int _UserID)
        {
            query = $"select * from Users where UserID = '{_UserID}'";

            DbHelper.OpenConnection("Seller");
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
            user.UserID = reader.GetInt32("UserID");
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
            query = $"Insert into Users (UserID, UserName, Password , FullName, Birthday, Email, Phone, Address, Role) value ('{user.UserID}', '{user.UserName}', '{user.Password}', '{user.FullName}', '{user.Birthday}', '{user.Email}', '{user.Phone}', '{user.Address}', '{user.Role}')";

            DbHelper.OpenConnection("Seller");
            reader = DbHelper.ExecQuery(query);
            DbHelper.CloseConnection();
        }
    }
}
