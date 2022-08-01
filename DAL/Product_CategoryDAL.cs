using MySql.Data.MySqlClient;
using Persistence;

namespace DAL
{
    public class Product_CategoryDAL
    {
        private string? query;
        private MySqlDataReader? reader;
        public bool CheckProduct_Category(int _ProductID, int _CategoryID)
        {
            bool result = false;
            query = $@"select * from Product_Categories
            where ProductID = {_ProductID} and CategoryID = {_CategoryID}";
            DbHelper.OpenConnection();
            reader = DbHelper.ExecQuery(query);
            if (reader.Read())
            {
                try
                {
                    int i = reader.GetInt32("CategoryID");
                    int j = reader.GetInt32("ProductID");
                    result = true;
                }
                catch (System.Exception)
                {
                }
            }
            DbHelper.CloseConnection();
            return result;
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
