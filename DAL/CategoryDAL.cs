using MySql.Data.MySqlClient;
using Persistence;

namespace DAL
{
    public class CategoryDAL
    {
        private string? query;
        private MySqlDataReader? reader;

        public List<Category> GetCategoriesByUser(User user)
        {
            query = $"select * from Categories where UserID = {user.UserID}";


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
            category.UserID = reader.GetInt32("UserID");
            category.CategoryName = reader.GetString("CategoryName");
            
            return category;
        }
        
        public void SaveCategory(Category category)
        {
            query = $"Insert into Categories (UserID, CategoryName) value ('{category.UserID}', '{category.CategoryName}')";

            DbHelper.OpenConnection();
            reader = DbHelper.ExecQuery(query);
            DbHelper.CloseConnection();
        }
        public void DeleteCategory(Category category)
        {
            // không xóa được 
            // query = $"Delete from Categories where categoryID = {category.CategoryID}";
            query = $"update Categories set UserID = null where CategoryID = {category.CategoryID}";
            DbHelper.OpenConnection();
            reader = DbHelper.ExecQuery(query);
            DbHelper.CloseConnection();
        }
    }
}
