using DAL;
using Persistence;

namespace BL;
public class ProductBL
{
    private ProductDAL productDAL;

    public ProductBL()
    {
        productDAL = new ProductDAL();
    }
    public List<Product> GetProductsByName(string _ProductName)
    {
        List<Product> products = new List<Product>();
        products = productDAL.GetProductsByName(_ProductName);
        return products;
    }

    public List<Product> GetProductsByNameAndUser(string _ProductName, User user)
    {
        List<Product> products = new List<Product>();
        products = productDAL.GetProductsByNameAndUser(_ProductName, user);
        return products;
    }

    public List<Product> GetProductsByUser(User user)
    {
        List<Product> products = new List<Product>();
        products = productDAL.GetProductsByUser(user);
        return products;
    }
    public List<Product> GetProductsByCategory(Category category)
    {
        List<Product> products = new List<Product>();
        products = productDAL.GetProductsByCategory(category);
        return products;
    }
    public void SaveProduct(Product product)
    {
        productDAL.SaveProduct(product);
    }
    public void UpdateDescription(Product product, string _Description)
    {
        productDAL.UpdateDescription(product, _Description);
    }

    public void UpdateQuantity(Product product, int _Quatity)
    {
        productDAL.UpdateQuantity(product, _Quatity);
    }

    public int ProductIDMax()
    {
        int _ProductID = productDAL.ProductIDMax();

        return _ProductID;
    }
}

