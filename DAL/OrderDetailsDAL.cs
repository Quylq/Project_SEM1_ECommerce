using MySql.Data.MySqlClient;
using Persistence;

namespace DAL
{
    public class OrderDetailsDAL
    {
        private string? query;
        private MySqlDataReader? reader;

        
        
        public void SaveOrderDetails(OrderDetails orderDetails)
        {
            query = $"";

            DbHelper.OpenConnection();
            reader = DbHelper.ExecQuery(query);
            DbHelper.CloseConnection();
        }
    }
}
