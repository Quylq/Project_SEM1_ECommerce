using NUnit.Framework;
using DAL;
using Persistence;

namespace DALTest;
[TestFixture]
public class ShopDALTest
{
    ShopDAL shopDAL = new ShopDAL();
    [SetUp]
    public void Setup()
    {
    }
    [Test]
    public void GetShopByIDTest()
    {
        Shop shop = new Shop(1, "Shop Đồ Gia Dụng", 1, 1);
        var result = shopDAL.GetShopByID(1);

        Assert.IsTrue(result.DeepEquals1(shop));
    }
    [Test]
    public void GetShopByUserIDTest()
    {
        Shop shop = new Shop(1, "Shop Đồ Gia Dụng", 1, 1);
        var result = shopDAL.GetShopByUserID(1);

        Assert.IsTrue(result!.DeepEquals1(shop));
    }
    [Test]
    public void InsertShopTest()
    {
        Shop shop = new Shop(10, "Shop Đồ Điện Tử", 7, 2);
        var result = shopDAL.InsertShop(shop);

        Assert.IsTrue(result);
    }
}