using MySql.Data.MySqlClient;
using Persistence;

namespace DAL
{
    public class OrderDAL
    {
        private string? query;
        private MySqlDataReader? reader;
        // Lấy danh sách order theo ID khách hàng và trạng thái
        public List<Order> GetOrdersByStatusAndUserID(string _Status, int _UserID)
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
        private Order GetOrderInfo(MySqlDataReader reader)
        {
            Order order = new Order();

            order.OrderID = reader.GetInt32("OrderID");
            order.UserID = reader.GetInt32("UserID");
            order.ShopID = reader.GetInt32("ShopID");
            order.CreateDate = reader.GetString("CreateDate");
            order.Status = reader.GetString("Status");

            return order;
        }
        // Cập nhật status của đơn hàng
        public void UpdateStatusOfOrder(int _OrderID, string _Status)
        {
            query = $"update Orders set Status = '{_Status}' where OrderID = {_OrderID};";

            DbHelper.OpenConnection();
            reader = DbHelper.ExecQuery(query);
            DbHelper.CloseConnection();
        }
        public void InsertOrder(Order order)
        {
            query = $"Insert into Orders (OrderID, UserID, ShopID, CreateDate , Status) value ('{order.OrderID}', '{order.UserID}', '{order.ShopID}', '{order.CreateDate}', '{order.Status}')";

            DbHelper.OpenConnection();
            reader = DbHelper.ExecQuery(query);
            DbHelper.CloseConnection();
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
            catch (System.Exception)
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
