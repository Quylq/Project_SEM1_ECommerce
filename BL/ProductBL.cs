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
    public List<Product> GetProductsByShopID(int _ShopID)
    {
        List<Product> products = new List<Product>();
        products = productDAL.GetProductsByShopID(_ShopID);
        return products;
    }
    public List<Product> GetProductsByNameAndShopID(string _ProductName, int _ShopID)
    {
        List<Product> products = new List<Product>();
        products = productDAL.GetProductsByNameAndShopID(_ProductName, _ShopID);
        return products;
    }
    public List<Product> GetProductsByCategory(int _CategoryID)
    {
        List<Product> products = new List<Product>();
        products = productDAL.GetProductsByCategory(_CategoryID);
        return products;
    }
    public void InsertProduct(Product product)
    {
        productDAL.InsertProduct(product);
    }
    public void UpdateDescriptionOfProduct(int _ProductID, string _Description)
    {
        productDAL.UpdateDescriptionOfProduct(_ProductID, _Description);
    }
    public void UpdateQuantityOfProduct(int _ProductID, int _Quatity)
    {
        productDAL.UpdateQuantityOfProduct(_ProductID, _Quatity);
    }
    public int ProductIDMax()
    {
        int _ProductID = productDAL.ProductIDMax();

        return _ProductID;
    }
    public List<Product> GetProductsByOrderID(int _OrderID)
    {
        List<Product> products = productDAL.GetProductsByOrderID(_OrderID);

        return products;
    }
    public Product GetProductByID(int _ProductID)
    {
        Product product = productDAL.GetProductByID(_ProductID);

        return product;
    }
}

