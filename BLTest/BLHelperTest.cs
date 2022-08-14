using NUnit.Framework;
using BL;
using Persistence;

namespace BLTest;
[TestFixture]
public class BLHelperTests
{
    BLHelper bLHelper = new BLHelper();
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void GetTotalOrderDetailsTest()
    {
        OrderDetails orderDetails = new OrderDetails(1, 2, 7);
        var result = bLHelper.GetTotalOrderDetails(orderDetails);

        Assert.AreEqual(result, 1400000);
    }
    [Test]
    public void GetTotalOrderTest()
    {
        var result = bLHelper.GetTotalOrder(1);

        Assert.AreEqual(result, 4900000);
    }
    [Test]
    public void GetTotalCartTest()
    {
        var result = bLHelper.GetTotalCart(1);

        Assert.AreEqual(result, 3720000);
    }
}