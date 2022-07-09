using DAL;
using Persistence;

namespace BL;
public class CategoryBL
{
    private CategoryDAL categoryDAL;

    public CategoryBL()
    {
        categoryDAL = new CategoryDAL();
    }
    public List<Category> GetCategoriesByUser(User user)
    {
        List<Category> categories = new List<Category>();
        categories = categoryDAL.GetCategoriesByUser(user);
        return categories;
    }
    public void CreateCategory(User user)
    {
        Console.WriteLine("Tên danh mục: ");
        string _CategoryName = Console.ReadLine();
        Category category = new Category(user.UserID, _CategoryName);
        categoryDAL.SaveCategory(category);
    }

    public void DisplayCategories(List<Category> categories)
    {
        Console.Clear();
        Console.WriteLine("|----------------------------------------------------------|");
        Console.WriteLine("| STT |                     Danh mục                       |");
        Console.WriteLine("|----------------------------------------------------------|");
        int count = 1;
        foreach (Category category in categories) 
        { 
            Console.WriteLine("| {0,3 } | {1,-50} |", count++, category.CategoryName);
            
        }
        Console.WriteLine("|----------------------------------------------------------|");
    }
    public void DisplayCategories(User user)
    {
        List<Category> categories = GetCategoriesByUser(user);
        DisplayCategories(categories);
    }
    public void SaveProduct_Categories(Product product, Category category)
    {
        ProductDAL productDAL = new ProductDAL();
        productDAL.SaveProduct_Categories(product, category);
    }
    public void DeleteCategory(Category category)
    {
        categoryDAL.DeleteCategory(category);
    }
}

