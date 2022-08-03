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
            orderDetails.Quantity = reader.GetInt32("Quantity");

            return orderDetails;
        }
        // Lấy danh sách orderDetails theo OrderID
        public List<OrderDetails> GetOrderDetailsListByOrderID(int _OrderID)
        {
            query = $"select * from OrderDetails where OrderID = {_OrderID}";
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
            query = $"Insert into OrderDetails (OrderID, ProductID, Quantity) value ('{orderDetails.OrderID}', '{orderDetails.ProductID}', '{orderDetails.Quantity}')";

            DbHelper.OpenConnection();
            reader = DbHelper.ExecQuery(query);
            DbHelper.CloseConnection();
        }
        public void UpdateQuantityOfOrderDetails(OrderDetails orderDetails)
        {
            query = $@"update OrderDetails set Quantity = {orderDetails.Quantity} 
            where OrderID = {orderDetails.OrderID} and ProductID = {orderDetails.ProductID}";

            DbHelper.OpenConnection();
            reader = DbHelper.ExecQuery(query);
            DbHelper.CloseConnection();
        }
        public void DeleteOrderDetails()
        {
            query = $@"Delete from OrderDetails where Quantity = 0";

            DbHelper.OpenConnection();
            reader = DbHelper.ExecQuery(query);
            DbHelper.CloseConnection();
        }
    }
}
