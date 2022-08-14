using NUnit.Framework;
using DAL;
using Persistence;

namespace DALTest;
[TestFixture]
public class CategoryDALTest
{
    CategoryDAL categoryDAL = new CategoryDAL();
    [SetUp]
    public void Setup()
    {
    }
    [Test]
    public void InsertCategoryTest()
    {
        Category category = new Category(50, 1, "Quần Áo");
        var result = categoryDAL.InsertCategory(category);

        Assert.IsTrue(result);
    }
    [Test]
    public void DeleteCategoryByIDTest()
    {
        var result = categoryDAL.DeleteCategoryByID(50);

        Assert.IsTrue(result);
    }
    [Test]
    public void GetProductNumberOfCategoryTest()
    {
        var result = categoryDAL.GetProductNumberOfCategory(1);

        Assert.AreEqual(result, 10);
    }
    
}