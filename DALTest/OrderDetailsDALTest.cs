using NUnit.Framework;
using DAL;
using Persistence;

namespace DALTest;
[TestFixture]
public class OrderDetailsDALTest
{
    OrderDetailsDAL orderDetailsDAL = new OrderDetailsDAL();
    [SetUp]
    public void Setup()
    {
    }
    [Test]
    public void CheckProductOfCartTest()
    {
        var result = orderDetailsDAL.CheckProductOfCart(1, 110);

        Assert.IsTrue(result);
    }
    [Test]
    public void GetQuantityInCartTest()
    {
        var result = orderDetailsDAL.GetQuantityInCart(1, 107);

        Assert.AreEqual(result, 15);
    }
    [Test]
    public void GetOrderIDOfCartTest()
    {
        var result = orderDetailsDAL.GetOrderIDOfCart(1, 4);

        Assert.AreEqual(result, 6);
    }
    [Test]
    public void GetProductNumberOfCartTest()
    {
        var result = orderDetailsDAL.GetProductNumberOfCart(1);

        Assert.AreEqual(result, 4);
    }
    [Test]
    public void GetQuantityOfOrderIDTest()
    {
        var result = orderDetailsDAL.GetQuantityOfOrderID(1);

        Assert.AreEqual(result, 22);
    }
    
    [Test]
    public void InsertOrderDetailsTest()
    {
        OrderDetails orderDetails = new OrderDetails(50, 1, 2);
        var result = orderDetailsDAL.InsertOrderDetails(orderDetails);

        Assert.IsTrue(result);
    }
    [Test]
    public void UpdateOrderDetailsTest()
    {
        OrderDetails orderDetails = new OrderDetails(50, 1, 6);
        var result = orderDetailsDAL.UpdateOrderDetails(orderDetails);

        Assert.IsTrue(result);
    }
}