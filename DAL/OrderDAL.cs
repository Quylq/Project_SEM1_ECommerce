using MySql.Data.MySqlClient;
using Persistence;

namespace DAL
{
    public class OrderDAL
    {
        private string? query;
        private MySqlDataReader? reader;
        // Lấy danh sách order theo ID khách hàng và trạng thái
        public List<Order>? GetOrdersByStatusAndUserID(string _Status, int _UserID)
        {
            query = $"select * from Orders o inner join Users u on o.UserID = u.UserID where o.UserID = {_UserID} and o.Status = '{_Status}'";
            DbHelper.OpenConnection();
            reader = DbHelper.ExecQuery(query);

            List<Order>? orders = new List<Order>();

            while (reader.Read())
            {
                Order order = GetOrderInfo(reader);
                orders.Add(order);
            }
            DbHelper.CloseConnection();
            if (orders.Count > 0)
            {
                return orders;
            }
            else
            {
                return null;
            }
        }
        public List<Order> GetOrdersByUserID(int _UserID)
        {
            query = $"select * from Orders o inner join Users u on o.UserID = u.UserID where o.UserID = {_UserID} and o.Status != 'Shopping'";
            DbHelper.OpenConnection();
            reader = DbHelper.ExecQuery(query);

            List<Order>? orders = new List<Order>();

            while (reader.Read())
            {
                Order order = GetOrderInfo(reader);
                orders.Add(order);
            }
            DbHelper.CloseConnection();
            return orders;
        }
        // Lấy danh sách order theo ID cửa hàng và trạng thái
        public List<Order> GetOrdersByStatusAndShopID(string _Status, int _ShopID)
        {
            query = $"select * from Orders o inner join Shops s on o.ShopID = s.ShopID where o.ShopID = {_ShopID} and o.Status = '{_Status}'";
            DbHelper.OpenConnection();
            reader = DbHelper.ExecQuery(query);

            List<Order>? orders = new List<Order>();

            while (reader.Read())
            {
                Order order = GetOrderInfo(reader);
                orders.Add(order);
            }
            DbHelper.CloseConnection();
            return orders;
        }
        public List<Order>? GetOrdersByShopID(int _ShopID)
        {
            query = $"select * from Orders o inner join Shops s on o.ShopID = s.ShopID where o.ShopID = {_ShopID} and o.Status != 'Shopping'";
            DbHelper.OpenConnection();
            reader = DbHelper.ExecQuery(query);

            List<Order>? orders = new List<Order>();

            while (reader.Read())
            {
                Order order = GetOrderInfo(reader);
                orders.Add(order);
            }
            DbHelper.CloseConnection();
            if (orders.Count > 0)
            {
                return orders;
            }
            else
            {
                return null;
            }
        }
        private Order GetOrderInfo(MySqlDataReader reader)
        {
            Order order = new Order();
            string format = "yyyy-MM-dd HH:mm:ss";
            order.OrderID = reader.GetInt32("OrderID");
            order.UserID = reader.GetInt32("UserID");
            order.ShopID = reader.GetInt32("ShopID");
            order.CreateDate = reader.GetDateTime("CreateDate").ToString(format);
            order.Status = reader.GetString("Status");

            return order;
        }
        // Cập nhật status của đơn hàng
        public bool UpdateStatusOfOrder(int _OrderID, string _Status)
        {
            query = $"update Orders set Status = '{_Status}' where OrderID = {_OrderID}";

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
        public void OrderingProcess(int _OrderID, string _CreateDate)
        {
            OrderDetailsDAL orderDetailsDAL = new OrderDetailsDAL();
            List<OrderDetails>? oDList = orderDetailsDAL.GetOrderDetailsListByOrderID(_OrderID);
            MySqlTransaction transaction;
            MySqlConnection connection = DbHelper.OpenConnection();
            transaction = connection.BeginTransaction();
            MySqlCommand command = new MySqlCommand();
            command.Connection = connection;
            command.Transaction = transaction;
            try
            {
                if (oDList != null)
                {
                    command.CommandText = $"update Orders set CreateDate = '{_CreateDate}' where OrderID = {_OrderID}";
                    command.ExecuteNonQuery();
                    command.CommandText = $"update Orders set Status = 'Processing' where OrderID = {_OrderID}";
                    command.ExecuteNonQuery();
                    foreach (OrderDetails orderDetails in oDList)
                    {
                        command.CommandText = $"update Products set Amount = Amount - {orderDetails.Quantity} where ProductID = {orderDetails.ProductID}";
                        command.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                Console.WriteLine("  Message: {0}", ex.Message);
                try
                {
                    transaction.Rollback();
                }
                catch (MySqlException ex2)
                {
                    Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                    Console.WriteLine("  Message: {0}", ex2.Message);
                }
            }
            DbHelper.CloseConnection();
        }
        public void OrderFailed(int _OrderID)
        {
            OrderDetailsDAL orderDetailsDAL = new OrderDetailsDAL();
            List<OrderDetails>? oDList = orderDetailsDAL.GetOrderDetailsListByOrderID(_OrderID);
            
            MySqlConnection connection = DbHelper.OpenConnection();
            MySqlCommand command = connection.CreateCommand();
            MySqlTransaction transaction = connection.BeginTransaction();
            command.Connection = connection;
            command.Transaction = transaction;
            try
            {
                if (oDList != null)
                {
                    command.CommandText = $"update Orders set Status = 'Failed' where OrderID = {_OrderID}";
                    command.ExecuteNonQuery();
                    foreach (OrderDetails orderDetails in oDList)
                    {
                        command.CommandText = $"update Products set Amount = Amount + {orderDetails.Quantity} where ProductID = {orderDetails.ProductID}";
                        command.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                Console.WriteLine("  Message: {0}", ex.Message);
                try
                {
                    transaction.Rollback();
                }
                catch (MySqlException ex2)
                {
                    Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                    Console.WriteLine("  Message: {0}", ex2.Message);
                }
            }
            
            DbHelper.CloseConnection();
        }
        public bool UpdateCreateDateOfOrder(int _OrderID, string _CreateDate)
        {
            query = $"update Orders set CreateDate = '{_CreateDate}' where OrderID = {_OrderID};";

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
        public bool InsertOrder(Order order)
        {
            query = $@"Insert into Orders (OrderID, UserID, ShopID, CreateDate , Status) 
            value ('{order.OrderID}', '{order.UserID}', '{order.ShopID}', '{order.CreateDate}', '{order.Status}')";

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
        public int OrderIDMax()
        {
            query = $"select max(OrderID) from orders";
            DbHelper.OpenConnection();
            reader = DbHelper.ExecQuery(query);
            int _OrderID = 0;

            try
            {
                if (reader.Read())
                {
                    _OrderID = reader.GetInt32("max(OrderID)");
                }
            }
            catch (System.Data.SqlTypes.SqlNullValueException)
            {
                _OrderID = 0;
            }
            DbHelper.CloseConnection();
            return _OrderID;
        }
        public Order GetOrderByID(int _OrderID)
        {
            query = $"select * from Orders where OrderID = '{_OrderID}'";
            DbHelper.OpenConnection();
            reader = DbHelper.ExecQuery(query);

            Order order = new Order();

            if (reader.Read())
            {
                order = GetOrderInfo(reader);
            }
            DbHelper.CloseConnection();
            return order;
        }
    }
}
