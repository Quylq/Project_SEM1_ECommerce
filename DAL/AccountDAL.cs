using MySql.Data.MySqlClient;
using Persistence;

namespace DAL
{
    public class AccountDAL
    {
        private string query;
        private MySqlDataReader reader;

        public Account GetAccountByName(string acc_name)
        {
            query = $"select Account_ID, Account_Name, Account_Password from accounts where Account_Name = '{acc_name}'";

            //Mở kết nối đến database
            DbHelper.OpenConnection();

            //Thực thi lệnh
            reader = DbHelper.ExecQuery(query);

            Account account = null;
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

            return account;
        }

        public void SaveAccount(string acc_name, string acc_pass, string acc_role)
        {
            query = $"Insert into Accounts (Account_Name, Account_Password , Role) value ( '{acc_name}', '{acc_pass}', '{acc_role}')";

            //Mở kết nối đến database
            DbHelper.OpenConnection();

            //Thực thi lệnh
            reader = DbHelper.ExecQuery(query);
        }

    }
}
