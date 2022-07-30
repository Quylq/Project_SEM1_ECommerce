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
    public List<Category> GetCategoriesByShopID(int _ShopID)
    {
        List<Category> categories = new List<Category>();
        categories = categoryDAL.GetCategoriesByShopID(_ShopID);
        return categories;
    }
    public void InsertCategory(Category category)
    {
        categoryDAL.InsertCategory(category);
    }
    public void DeleteCategoryByID(int _CategoryID)
    {
        categoryDAL.DeleteCategoryByID(_CategoryID);
    }
    public List<Category> GetCategoriesByProductID(int _ProductID)
    {
        List<Category> categories = categoryDAL.GetCategoriesByProductID(_ProductID);
        
        return categories;
    }
}
