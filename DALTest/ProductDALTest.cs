using NUnit.Framework;
using DAL;
using Persistence;

namespace DALTest;
[TestFixture]
public class ProductDALTest
{
    ProductDAL productDAL = new ProductDAL();
    [SetUp]
    public void Setup()
    {
    }
    [Test]
    public void InsertProduct()
    {
        Product product = new Product(200, 1, "Áo thun", 100000, "Chất liệu: 80% Cotton - 20% Recycle Polyester", 100);
        var result = productDAL.InsertProduct(product);

        Assert.IsTrue(result);
    }
    [Test]
    public void UpdateDescriptionOfProduct()
    {
        var result = productDAL.UpdateDescriptionOfProduct(200, "Chất liệu: 100% Cotton");

        Assert.IsTrue(result);
    }
    [Test]
    public void UpdateAmountOfProduct()
    {
        var result = productDAL.UpdateAmountOfProduct(200, 9);

        Assert.IsTrue(result);
    }
    [Test]
    public void ProductIDMax()
    {
        var result = productDAL.ProductIDMax();

        Assert.AreEqual(result, 200);
    }
    [Test]
    public void GetProductByID()
    {
        var result = productDAL.GetProductByID(200);
        Product product = new Product(200, 1, "Áo thun", 100000, "Chất liệu: 100% Cotton", 9);

        Assert.IsTrue(result.DeepEquals1(product));
    }
}