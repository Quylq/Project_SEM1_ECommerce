using MySql.Data.MySqlClient;
using Persistence;

namespace DAL
{
    public class ProductDAL
    {
        private string? query;
        private MySqlDataReader? reader;
        // Lấy danh sách sản phẩm theo tên
        public List<Product>? GetProductsByName(string _ProductName)
        {
            query = $"select * from Products where ProductName like '%{_ProductName}%'";

            DbHelper.OpenConnection();
            reader = DbHelper.ExecQuery(query);

            List<Product>? products = new List<Product>();

            while (reader.Read())
            {
                Product product = GetProductInfo(reader);
                products.Add(product);
                
            }
            DbHelper.CloseConnection();
            if (products.Count > 0)
            {
                return products;
            }
            else
            {
                return null;
            }
            
        }
        // Lấy danh sách sản phẩm theo ID người bán
        public List<Product>? GetProductsByShopID(int _ShopID)
        {          
            query = $"select * from Products where ShopID = {_ShopID}";
            DbHelper.OpenConnection();
            reader = DbHelper.ExecQuery(query);

            List<Product>? products = new List<Product>();

            while (reader.Read())
            {
                Product product = GetProductInfo(reader);
                products.Add(product);
            }
            DbHelper.CloseConnection();
            if (products.Count > 0)
            {
                return products;
            }
            else
            {
                return null;
            }
        }
        // Lấy danh sách sản phẩm theo tên và ShopID
        public List<Product> GetProductsByNameAndShopID(string _ProductName, int _ShopID)
        {
            query = $"select * from Products where ProductName like '%{_ProductName}%' and ShopID = {_ShopID}";
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
        // Lấy danh sách sản phẩm theo ID danh mục
        public List<Product> GetProductsByCategory(int _CategoryID)
        {    
            query = $@"select p.ProductID, p.ShopID, p.ProductName, p.Price, p.Description, p.Amount from Products p 
            inner join Product_Categories pc on p.ProductID = pc.ProductID 
            where pc.CategoryID = {_CategoryID}";
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
        // Lấy danh sách sản phẩm theo ID đơn hàng
        private Product GetProductInfo(MySqlDataReader reader)
        {
            Product product = new Product();

            product.ProductID = reader.GetInt32("ProductID");
            product.ShopID = reader.GetInt32("ShopID");
            product.ProductName = reader.GetString("ProductName");
            product.Price = reader.GetInt32("Price");
            product.Description = reader.GetString("Description").Replace("\t", " ");
            product.Amount = reader.GetInt32("Amount");

            return product;
        }

        public bool InsertProduct(Product product)
        {
            query = $"Insert into Products (ProductID, ShopID, ProductName, Price , Description, Amount) value ('{product.ProductID}', '{product.ShopID}', '{product.ProductName}', '{product.Price}', '{product.Description}', '{product.Amount}')";

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
        // Cập nhật mô tả
        public bool UpdateDescriptionOfProduct(int _ProductID, string _Description)
        {
            query = $"update Products set Description = '{_Description}' where ProductID = {_ProductID};";

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
        // Cập nhật số lượng
        public bool UpdateAmountOfProduct(int _ProductID, int _Amount)
        {
            query = $"update Products set Amount = '{_Amount}' where ProductID = {_ProductID}";
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
        public int ProductIDMax()
        {
            query = $"select  max(ProductID) from products";
            DbHelper.OpenConnection();
            reader = DbHelper.ExecQuery(query);

            int _ProductID = 0;
            try
            {
                if (reader.Read())
                {
                    _ProductID = reader.GetInt32("max(ProductID)");
                }
            }
            catch (System.Data.SqlTypes.SqlNullValueException)
            {
                _ProductID = 0;
            }
            DbHelper.CloseConnection();
            return _ProductID;
        }
        public Product GetProductByID(int _ProductID)
        {
            query = $"select * from Products where ProductID = '{_ProductID}'";

            DbHelper.OpenConnection();
            reader = DbHelper.ExecQuery(query);

            Product product = new Product();
            if (reader.Read())
            {
                product = GetProductInfo(reader);
            }
            DbHelper.CloseConnection();
            return product;
        }
    }
}
