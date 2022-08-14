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
        public List<OrderDetails>? GetOrderDetailsListByOrderID(int _OrderID)
        {
            query = $"select * from OrderDetails where OrderID = {_OrderID} and Quantity != 0";
            DbHelper.OpenConnection();
            reader = DbHelper.ExecQuery(query);

            List<OrderDetails>? order = new List<OrderDetails>();

            while (reader.Read())
            {
                OrderDetails orderDetails = GetOrderDetailsInfo(reader);
                order.Add(orderDetails);
            }
            DbHelper.CloseConnection();
            if (order.Count > 0)
            {
                return order;
            }
            else
            {
                return null;
            }
        }
        public List<OrderDetails>? GetOrderDetailsListByUserIDAndStatus(int _UserID, string _Status)
        {
            query = $@"select * from OrderDetails od 
            inner join Orders o on o.OrderID = od.OrderID 
            where o.UserID = {_UserID} and o.Status = '{_Status}' and od.Quantity != 0";
            DbHelper.OpenConnection();
            reader = DbHelper.ExecQuery(query);

            List<OrderDetails> order = new List<OrderDetails>();

            while (reader.Read())
            {
                OrderDetails orderDetails = GetOrderDetailsInfo(reader);
                order.Add(orderDetails);
            }
            DbHelper.CloseConnection();
            if (order.Count > 0)
            {
                return order;
            }
            else
            {
                return null;
            }
        }
        public bool CheckProductOfCart(int _UserID, int _ProductID)
        {
            query = $@"select * from OrderDetails od 
            inner join Orders o on o.OrderID = od.OrderID 
            where o.UserID = {_UserID} and o.Status = 'Shopping' and od.ProductID = {_ProductID}";
            DbHelper.OpenConnection();
            reader = DbHelper.ExecQuery(query);

            if (reader.Read())
            {
                DbHelper.CloseConnection();
                return true;
            }
            else
            {
                DbHelper.CloseConnection();
                return false;
            }
        }
        public int GetQuantityInCart(int _UserID, int _ProductID)
        {
            query = $@"select * from OrderDetails od 
            inner join Orders o on o.OrderID = od.OrderID 
            where o.UserID = {_UserID} and o.Status = 'Shopping' and od.ProductID = {_ProductID}";
            DbHelper.OpenConnection();
            reader = DbHelper.ExecQuery(query);

            int Quantity = 0;
            if (reader.Read())
            {
                Quantity = reader.GetInt32("Quantity");
                DbHelper.CloseConnection();
                return Quantity;
            }
            else
            {
                DbHelper.CloseConnection();
                return 0;
            }
        }
        public int GetOrderIDOfCart(int _UserID, int _ShopID)
        {
            query = $@"select * from OrderDetails od 
            inner join Orders o on o.OrderID = od.OrderID 
            where o.UserID = {_UserID} and o.Status = 'Shopping' and o.ShopID = {_ShopID}";
            DbHelper.OpenConnection();
            reader = DbHelper.ExecQuery(query);

            int _OrderID = 0;
            if (reader.Read())
            {
                _OrderID = reader.GetInt32("OrderID");
                DbHelper.CloseConnection();
                return _OrderID;
            }
            else
            {
                DbHelper.CloseConnection();
                return 0;
            }
        }
        public int GetProductNumberOfCart(int _UserID)
        {
            query = $@"Select Count(*) from OrderDetails od 
            inner join Orders o on o.OrderID = od.OrderID 
            where o.UserID = {_UserID} and o.Status = 'Shopping' and od.Quantity > 0";
            DbHelper.OpenConnection();
            reader = DbHelper.ExecQuery(query);

            int result = 0;

            if (reader.Read())
            {
                result = reader.GetInt32("Count(*)");
                DbHelper.CloseConnection();
                return result;
            }
            else
            {
                DbHelper.CloseConnection();
                return result;
            }
        }
        public int GetQuantityOfOrderID(int _OrderID)
        {
            query = $@"select sum(Quantity) from Orders o 
                    inner join orderdetails od on od.OrderID = o.OrderID
                    where o.Status != 'Shopping' and o.OrderID = {_OrderID}";
            DbHelper.OpenConnection();
            reader = DbHelper.ExecQuery(query);

            int result = 0;

            if (reader.Read())
            {
                result = reader.GetInt32("sum(Quantity)");
                DbHelper.CloseConnection();
                return result;
            }
            else
            {
                DbHelper.CloseConnection();
                return result;
            }
        }
        public bool InsertOrderDetails(OrderDetails orderDetails)
        {
            query = $"Insert into OrderDetails (OrderID, ProductID, Quantity) value ('{orderDetails.OrderID}', '{orderDetails.ProductID}', '{orderDetails.Quantity}')";

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
        public bool UpdateOrderDetails(OrderDetails orderDetails)
        {
            query = $@"update OrderDetails set Quantity = {orderDetails.Quantity} 
            where OrderID = {orderDetails.OrderID} and ProductID = {orderDetails.ProductID}";

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
    }
}
