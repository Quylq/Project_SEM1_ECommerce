using MySql.Data.MySqlClient;
using Persistence;

namespace DAL
{
    public class OrderDetailsDAL
    {
        private string? query;
        private MySqlDataReader? reader;
        private OrderDetails GetOrderDetailsInfo(MySqlDataReader reader)
        {
            OrderDetails orderDetails = new OrderDetails();

            orderDetails.OrderID = reader.GetInt32("OrderID");
            orderDetails.ProductID = reader.GetInt32("ProductID");
            orderDetails.ProductNumber = reader.GetInt32("ProductNumber");

            return orderDetails;
        }
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
        // Lấy danh sách orderDetails theo OrderID
        public List<OrderDetails> GetOrderDetailsListByOrderID(int _OrderID)
        {
            query = $"select * from OrderDetails od inner join Orders o on o.OrderID = od.OrderID where o.OrderID = {_OrderID}";
            DbHelper.OpenConnection();
            reader = DbHelper.ExecQuery(query);

            List<OrderDetails> order = new List<OrderDetails>();

            while (reader.Read())
            {
                OrderDetails orderDetails = GetOrderDetailsInfo(reader);
                order.Add(orderDetails);
            }
            DbHelper.CloseConnection();
            return order;
        }
        public List<OrderDetails> GetOrderDetailsListByUserIDAndStatus(int _UserID, string _Status)
        {
            query = $"select * from OrderDetails od inner join Orders o on o.OrderID = od.OrderID where o.UserID = {_UserID} and o.Status = '{_Status}'";
            DbHelper.OpenConnection();
            reader = DbHelper.ExecQuery(query);

            List<OrderDetails> order = new List<OrderDetails>();

            while (reader.Read())
            {
                OrderDetails orderDetails = GetOrderDetailsInfo(reader);
                order.Add(orderDetails);
            }
            DbHelper.CloseConnection();
            return order;
        }
        public void InsertOrderDetails(OrderDetails orderDetails)
        {
            query = $"Insert into OrderDetails (OrderID, ProductID, ProductNumber) value ('{orderDetails.OrderID}', '{orderDetails.ProductID}', '{orderDetails.ProductNumber}')";

            DbHelper.OpenConnection();
            reader = DbHelper.ExecQuery(query);
            DbHelper.CloseConnection();
        }
        public void UpdateProductNumberOfOrderDetails(OrderDetails orderDetails)
        {
            query = $"update OrderDetails set ProductNumber = {orderDetails.ProductNumber} where OrderID = {orderDetails.OrderID} and ProductID = {orderDetails.ProductID}";

            DbHelper.OpenConnection();
            reader = DbHelper.ExecQuery(query);
            DbHelper.CloseConnection();
        }
    }
}