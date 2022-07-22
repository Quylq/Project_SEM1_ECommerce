using MySql.Data.MySqlClient;
using Persistence;

namespace DAL
{
    public class Product_CategoryDAL
    {
        private string? query;
        private MySqlDataReader? reader;
        private Product_Category GetProduct_CategoryInfo(MySqlDataReader reader)
        {
            Product_Category product_Category = new Product_Category();

            product_Category.CategoryID = reader.GetInt32("CategoryID");
            product_Category.ProductID = reader.GetInt32("ProductID");

            return product_Category;
        }
        public void InsertProduct_Category(int _ProductID, int _CategoryID)
        {
            query = $"Insert into Product_Categories (ProductID, CategoryID) value ('{_ProductID}', '{_CategoryID}')";

            DbHelper.OpenConnection();
            reader = DbHelper.ExecQuery(query);
            DbHelper.CloseConnection();
        }
        public void DeleteProduct_CategoryByCategoryID(int _CategoryID)
        {
            query = $"Delete from product_categories where CategoryID = {_CategoryID}";
            
            DbHelper.OpenConnection();
            reader = DbHelper.ExecQuery(query);
            DbHelper.CloseConnection();
        }
    }
}