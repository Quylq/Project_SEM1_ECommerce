using MySql.Data.MySqlClient;
using Persistence;

namespace DAL
{
    public class AccountDAL
    {
        private string? query;
        private MySqlDataReader reader;

        public Account GetAccountByName(string acc_name)
        {
            query = $"select Account_ID, Account_Name, Account_Password, Role from accounts where Account_Name = '{acc_name}'";

            //Mở kết nối đến database
            DbHelper.OpenConnection();

            //Thực thi lệnh
            reader = DbHelper.ExecQuery(query);

            Account account = new Account();
            if (reader.Read())
            {
                account = GetAccountInfo(reader);
            }
            DbHelper.CloseConnection();
            return account;
        }

        private Account GetAccountInfo(MySqlDataReader reader)
        {

            Account account = new Account();
            account.AccountId = reader.GetInt32("Account_ID");
            account.AccountName = reader.GetString("Account_Name");
            account.AccountPassword = reader.GetString("Account_Password");
            account.Role = reader.GetString("Role");
            return account;
        }

        public void SaveAccount(Account account)
        {
            query = $"Insert into Accounts (Account_Name, Account_Password , Role) value ( '{account.AccountName}', '{account.AccountPassword}', '{account.Role}')";

            //Mở kết nối đến database
            DbHelper.OpenConnection();

            //Thực thi lệnh
            reader = DbHelper.ExecQuery(query);
            DbHelper.CloseConnection();
        }

        public void SaveUser(User user, Account account)
        {
            if (account.Role == "Customer")
            {
                query = $"Insert into Customers (Account_ID, Full_Name, Email , Birthday, Phone_Number, Address) value ( '{account.AccountId}', '{user.FullName}', '{user.Email}', '{user.Birthday.Date}', '{user.Phone}', '{user.Address}' )";
            }
            else if(account.Role == "Seller")
            {
                query = $"Insert into Sellers (Account_ID, Full_Name, Email , Birthday, Phone_Number, Address) value ( '{account.AccountId}', '{user.FullName}', '{user.Email}', '{user.Birthday.Date}', '{user.Phone}', '{user.Address}' )";
            }
 
            //Mở kết nối đến database
            DbHelper.OpenConnection();

            //Thực thi lệnh
            reader = DbHelper.ExecQuery(query);
            DbHelper.CloseConnection();
        }
    }
}
