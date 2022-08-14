using NUnit.Framework;
using DAL;
using Persistence;

namespace DALTest;
[TestFixture]
public class Product_CategoryDALTest
{
    Product_CategoryDAL product_CategoryDAL = new Product_CategoryDAL();
    [SetUp]
    public void Setup()
    {
    }
    [Test]
    public void CheckProduct_Category()
    {
        var result = product_CategoryDAL.CheckProduct_Category(1, 1);

        Assert.IsTrue(result);
    }
    [Test]
    public void InsertProduct_Category()
    {
        var result = product_CategoryDAL.InsertProduct_Category(100, 1);

        Assert.IsTrue(result);
    }
    [Test]
    public void DeleteProduct_CategoryByCategoryID()
    {
        var result = product_CategoryDAL.DeleteProduct_CategoryByCategoryID(12);

        Assert.IsTrue(result);
    }
}