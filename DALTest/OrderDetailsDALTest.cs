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
    public void CheckProductOfCart()
    {
        var result = orderDetailsDAL.CheckProductOfCart(1, 110);

        Assert.IsTrue(result);
    }
    [Test]
    public void GetQuantityInCart()
    {
        var result = orderDetailsDAL.GetQuantityInCart(1, 107);

        Assert.AreEqual(result, 15);
    }
    [Test]
    public void GetOrderIDOfCart()
    {
        var result = orderDetailsDAL.GetOrderIDOfCart(1, 4);

        Assert.AreEqual(result, 6);
    }
    [Test]
    public void GetProductNumberOfCart()
    {
        var result = orderDetailsDAL.GetProductNumberOfCart(1);

        Assert.AreEqual(result, 4);
    }
    [Test]
    public void GetQuantityOfOrderID()
    {
        var result = orderDetailsDAL.GetQuantityOfOrderID(1);

        Assert.AreEqual(result, 22);
    }
    
    [Test]
    public void InsertOrderDetails()
    {
        OrderDetails orderDetails = new OrderDetails(50, 1, 2);
        var result = orderDetailsDAL.InsertOrderDetails(orderDetails);

        Assert.IsTrue(result);
    }
    [Test]
    public void UpdateOrderDetails()
    {
        OrderDetails orderDetails = new OrderDetails(50, 1, 6);
        var result = orderDetailsDAL.UpdateOrderDetails(orderDetails);

        Assert.IsTrue(result);
    }
}