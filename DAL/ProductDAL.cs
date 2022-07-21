using MySql.Data.MySqlClient;
using Persistence;

namespace DAL
{
    public class ProductDAL
    {
        private string? query;
        private MySqlDataReader? reader;
        // Lấy danh sách sản phẩm theo tên
        public List<Product> GetProductsByName(string _ProductName)
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
            return products;
        }
        // Lấy danh sách sản phẩm theo ID người bán
        public List<Product> GetProductsByShopID(int _ShopID)
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
            return products;
        }
        // Lấy danh sách sản phẩm theo ID người bán và không thuộc danh mục
        public List<Product> GetProductsByShopIDAndCategoryID(int _ShopID, int _CategoryID)
        {          
            query = $@"select * from Products p 
            inner join Product_Categories pc on pc.ProductID = p.ProductID
            where p.ShopID = {_ShopID} and pc.CategoryID != {_CategoryID}
            group by p.productID";
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
            query = $"select p.ProductID, p.ShopID, p.ProductName, p.Price, p.Description, p.Quantity from Products p inner join Product_Categories pc on p.ProductID = pc.ProductID where pc.CategoryID = {_CategoryID}";
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
        public List<Product> GetProductsByOrderID(int _OrderID)
        {
            query = $"select * from Products p inner join OrderDetails od on p.ProductID = od.ProductID where od.OrderID = {_OrderID};";
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
            product.ShopID = reader.GetInt32("ShopID");
            product.ProductName = reader.GetString("ProductName");
            product.Price = reader.GetInt32("Price");
            product.Description = reader.GetString("Description");
            product.Quantity = reader.GetInt32("Quantity");

            return product;
        }

        public void InsertProduct(Product product)
        {
            query = $"Insert into Products (ProductID, ShopID, ProductName, Price , Description, Quantity) value ('{product.ProductID}', '{product.ShopID}', '{product.ProductName}', '{product.Price}', '{product.Description}', '{product.Quantity}')";

            DbHelper.OpenConnection();
            reader = DbHelper.ExecQuery(query);
            DbHelper.CloseConnection();
        }
        // Cập nhật mô tả
        public void UpdateDescriptionOfProduct(int _ProductID, string _Description)
        {
            query = $"update Products set Description = '{_Description}' where ProductID = {_ProductID};";

            DbHelper.OpenConnection();
            reader = DbHelper.ExecQuery(query);
            DbHelper.CloseConnection();
        }
        // Cập nhật số lượng
        public void UpdateQuantityOfProduct(int _ProductID, int _Quantity)
        {
            query = $"update Products set Quantity = '{_Quantity}' where ProductID = {_ProductID}";
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
            try
            {
                if (reader.Read())
                {
                    _ProductID = reader.GetInt32("max(ProductID)");
                }
            }
            catch (System.Exception)
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
