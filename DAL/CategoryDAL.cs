using MySql.Data.MySqlClient;
using Persistence;

namespace DAL
{
    public class CategoryDAL
    {
        private string? query;
        private MySqlDataReader? reader;
        public List<Category> GetCategoriesByShopID(int _ShopID)
        {
            query = $"select * from Categories where ShopID = {_ShopID}";

            DbHelper.OpenConnection();
            reader = DbHelper.ExecQuery(query);
            List<Category> categories = new List<Category>();
            Category category = new Category();
            while (reader.Read())
            {
                category = GetCategoryInfo(reader);
                categories.Add(category);
            }
            DbHelper.CloseConnection();
            return categories;
        }

        private Category GetCategoryInfo(MySqlDataReader reader)
        {
            Category category = new Category();
            category.CategoryID = reader.GetInt32("CategoryID");
            category.ShopID = reader.GetInt32("ShopID");
            category.CategoryName = reader.GetString("CategoryName");
            
            return category;
        }
        public void InsertCategory(Category category)
        {
            query = $"Insert into Categories (ShopID, CategoryName) value ('{category.ShopID}', '{category.CategoryName}')";

            DbHelper.OpenConnection();
            reader = DbHelper.ExecQuery(query);
            DbHelper.CloseConnection();
        }
        public void DeleteCategoryByID(int _CategoryID)
        {
            // query = $"Delete from Categories where categoryID = {_CategoryID}";
            query = $"update Categories set ShopID = null where CategoryID = {_CategoryID}";
            DbHelper.OpenConnection();
            reader = DbHelper.ExecQuery(query);
            DbHelper.CloseConnection();
        }
    }
}