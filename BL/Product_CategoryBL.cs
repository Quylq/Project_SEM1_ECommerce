using DAL;
using Persistence;

namespace BL;
public class Product_CategoryBL
{
    private Product_CategoryDAL product_CategoryDAL;

    public Product_CategoryBL()
    {
        product_CategoryDAL = new Product_CategoryDAL();
    }
    public void InsertProduct_Category(int _ProductID, int _CategoryID)
    {
        product_CategoryDAL.InsertProduct_Category(_ProductID, _CategoryID);
    }
    public void DeleteProduct_CategoryByCategoryID(int _CategoryID)
    {
        product_CategoryDAL.DeleteProduct_CategoryByCategoryID(_CategoryID);
    }
}