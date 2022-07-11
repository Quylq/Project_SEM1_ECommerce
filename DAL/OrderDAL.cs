using MySql.Data.MySqlClient;
using Persistence;

namespace DAL
{
    public class OrderDAL
    {
        private string? query;
        private MySqlDataReader? reader;

        public List<Order> GetOrdersByStatusOfSeller(string _Status, User seller)
        {
            query = $"select * from Orders o inner join Users u on o.CustomerID = u.UserID where o.SellerID = {seller.UserID} and o.Status = '{_Status}'";
            DbHelper.OpenConnection("Seller");
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
        public List<User> GetUsersByStatusOfSeller(string _Status, User seller)
        {
            query = $"select * from Orders o inner join Users u on o.CustomerID = u.UserID where o.SellerID = {seller.UserID} and o.Status = '{_Status}'";
            DbHelper.OpenConnection("Seller");
            reader = DbHelper.ExecQuery(query);

            List<User>? customers = new List<User>();

            while (reader.Read())
            {
                User customer = GetUserInfo(reader);
                customers.Add(customer);
            }
            DbHelper.CloseConnection();
            return customers;
        }
        private Order GetOrderInfo(MySqlDataReader reader)
        {
            Order order = new Order();

            order.OrderID = reader.GetInt32("OrderID");
            order.SellerID = reader.GetInt32("SellerID");
            order.CustomerID = reader.GetInt32("CustomerID");
            order.CreateDate = reader.GetDateTime("CreateDate");
            order.Status = reader.GetString("Status");

            return order;
        }
        private User GetUserInfo(MySqlDataReader reader)
        {
            User user = new User();
            user.UserID = reader.GetInt32("UserID");
            user.UserName = reader.GetString("UserName");
            user.Password = reader.GetString("Password");
            user.FullName = reader.GetString("FullName");
            user.Birthday = reader.GetDateTime("Birthday");
            user.Email = reader.GetString("Email");
            user.Phone = reader.GetString("Phone");
            user.Address = reader.GetString("Address");
            user.Role = reader.GetString("Role");
            return user;
        }
        public void SaveOrder(Order order)
        {
            query = $"Insert into Orders (UserID, CreateDate , Status) value ( '{order.SellerID}', '{order.CreateDate}', '{order.Status}')";

            DbHelper.OpenConnection("Seller");
            reader = DbHelper.ExecQuery(query);
            DbHelper.CloseConnection();
        }

        public void UpdateStatus(Order order, string _Status)
        {
            query = $"update Orders set Status = '{_Status}' where OrderID = {order.OrderID};";

            DbHelper.OpenConnection("Seller");
            reader = DbHelper.ExecQuery(query);
            DbHelper.CloseConnection();
        }

        private Product GetProductOfOrderDetails(MySqlDataReader reader)
        {
            Product product = new Product();

            product.ProductID = reader.GetInt32("ProductID");
            product.ProductName = reader.GetString("ProductName");
            product.Price = reader.GetInt32("Price");
            // product.Description = reader.GetString("Description");
            product.Quantity = reader.GetInt32("ProductNumber");

            return product;
        }
        public List<Product> GetOrderDetails(Order order)
        {
            query = $"select * from OrderDetails od inner join Products p on od.ProductID = p.ProductID where od.OrderID = {order.OrderID}";

            DbHelper.OpenConnection("Seller");
            reader = DbHelper.ExecQuery(query);
            List<Product> products = new List<Product>();
            Product product = new Product();
            while (reader.Read())
            {
                product = GetProductOfOrderDetails(reader);
                products.Add(product);
            }
            DbHelper.CloseConnection();
            return products;
        }
    }
}
