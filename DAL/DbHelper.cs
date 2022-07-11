using MySql.Data.MySqlClient;

namespace DAL
{
    public class DbHelper
    {
        private static MySqlConnection? connection;

        public static MySqlConnection GetConnection(string _Role)
        {
            if (connection == null)
            {
                if (_Role == "Seller")
                {
                    connection = new MySqlConnection
                    {
                        ConnectionString = @"server=localhost;port=3306;user=seller;password=123456;database=Ecommerce"
                    };
                }
                else if (_Role == "Customer")
                {
                    connection = new MySqlConnection
                    {
                        ConnectionString = @"server=localhost;port=3306;user=customer;password=123456;database=Ecommerce"
                    };
                }
            }

            return connection;
        }

        public static MySqlConnection OpenConnection(string _Role)
        {
            if(connection == null)
            {
                GetConnection(_Role);
            }
            
            connection.Open();

            return connection;
        }

        public static void CloseConnection()
        {
            if (connection != null)
            {
                connection.Close();
            }
        }

        public static MySqlDataReader ExecQuery(string sqlCommand)
        {
            MySqlCommand command = new MySqlCommand(sqlCommand, connection);
            return command.ExecuteReader();
        }
    }
}