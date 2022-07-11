using MySql.Data.MySqlClient;

namespace DAL
{
    public class DbHelper
    {
        private static MySqlConnection? connection;

        public static MySqlConnection GetConnection()
        {
            if (connection == null)
            {
                connection = new MySqlConnection
                {
                    ConnectionString = @"server=localhost;port=3306;user=root;password=19072003;database=Ecommerce"
                };
            }

            return connection;
        }

        public static MySqlConnection OpenConnection()
        {
            if(connection == null)
            {
                GetConnection();
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