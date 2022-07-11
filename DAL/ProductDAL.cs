using MySql.Data.MySqlClient;
using Persistence;

namespace DAL
{
    public class ProductDAL
    {
        private string? query;
        private MySqlDataReader? reader;

        public List<Product> GetProductsByName(string _ProductName)
        {
            query = $"select ProductID, ProductName, Price, Description, Quantity from Products where ProductName like '%{_ProductName}%'";

            DbHelper.OpenConnection();
            reader = DbHelper.ExecQuery(query);

            List<Product>? products = new List<Product>();

            while (reader.Read())
            {
                Product product = GetProductInfo(reader);
                products.Add(product);
            }
            DbHelper.CloseConnection();
            return products;
        }
        public List<Product> GetProductsByUser(User user)
        {
            
            query = $"select p.ProductID, p.ProductName, p.Price, p.Description, p.Quantity from Products p inner join Users_Product up on p.ProductID = up.ProductID where up.UserID = {user.UserID}";
            DbHelper.OpenConnection();
            reader = DbHelper.ExecQuery(query);

            List<Product>? products = new List<Product>();

            while (reader.Read())
            {
                Product product = GetProductInfo(reader);
                products.Add(product);
            }
            DbHelper.CloseConnection();
            return products;
        }
        public List<Product> GetProductsByCategory(Category category)
        {
            
            query = $"select p.ProductID, p.ProductName, p.Price, p.Description, p.Quantity from Products p inner join Product_Categories pc on p.ProductID = pc.ProductID where pc.CategoryID = {category.CategoryID}";
            DbHelper.OpenConnection();
            reader = DbHelper.ExecQuery(query);

            List<Product>? products = new List<Product>();

            while (reader.Read())
            {
                Product product = GetProductInfo(reader);
                products.Add(product);
            }
            DbHelper.CloseConnection();
            return products;
        }
        public List<Product> GetProductsByNameAndUser(string _ProductName, User user)
        {
            query = $"select p.ProductID, p.ProductName, p.Price, p.Description, p.Quantity from Products p inner join Users_Product up on p.ProductID = up.ProductID where p.ProductName like '%{_ProductName}%'  and up.UserID = {user.UserID}";
            DbHelper.OpenConnection();
            reader = DbHelper.ExecQuery(query);

            List<Product>? products = new List<Product>();

            while (reader.Read())
            {
                Product product = GetProductInfo(reader);
                products.Add(product);
            }
            DbHelper.CloseConnection();
            return products;
        }

        private Product GetProductInfo(MySqlDataReader reader)
        {
            Product product = new Product();

            product.ProductID = reader.GetInt32("ProductID");
            product.ProductName = reader.GetString("ProductName");
            product.Price = reader.GetInt32("Price");
            product.Description = reader.GetString("Description");
            product.Quantity = reader.GetInt32("Quantity");

            return product;
        }

        public void SaveProduct(Product product)
        {
            query = $"Insert into Products (ProductID, ProductName, Price , Description, Quantity) value ('{product.ProductID}', '{product.ProductName}', '{product.Price}', '{product.Description}', '{product.Quantity}')";

            DbHelper.OpenConnection();
            reader = DbHelper.ExecQuery(query);
            DbHelper.CloseConnection();
        }
        public void SaveUsers_Product(User user, Product product)
        {
            query = $"Insert into Users_Product (UserID, ProductID) value ( '{user.UserID}', '{product.ProductID}')";
            DbHelper.OpenConnection();
            reader = DbHelper.ExecQuery(query);
            DbHelper.CloseConnection();
        }
        public void SaveProduct_Categories(Product product, Category category)
        {
            query = $"Insert into Product_Categories (CategoryID, ProductID) value ( '{category.CategoryID}', '{product.ProductID}')";
            DbHelper.OpenConnection();
            reader = DbHelper.ExecQuery(query);
            DbHelper.CloseConnection();
        }
        public void UpdateDescription(Product product, string _Description)
        {
            query = $"update Products set Description = '{_Description}' where ProductID = {product.ProductID};";

            DbHelper.OpenConnection();
            reader = DbHelper.ExecQuery(query);
            DbHelper.CloseConnection();
        }

        public void UpdateQuantity(Product product, int _Quantity)
        {
            query = $"update Products set Quantity = '{_Quantity}' where ProductID = {product.ProductID}";
            DbHelper.OpenConnection();
            reader = DbHelper.ExecQuery(query);
            DbHelper.CloseConnection();
        }

        public int ProductIDMax()
        {
            query = $"select  max(ProductID) from products";
            DbHelper.OpenConnection();
            reader = DbHelper.ExecQuery(query);

            int _ProductID = 0;
            if (reader.Read())
            {
                _ProductID = reader.GetInt32("max(ProductID)");
            }
            DbHelper.CloseConnection();

            return _ProductID;
        }
    }
}