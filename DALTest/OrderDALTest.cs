using NUnit.Framework;
using DAL;
using Persistence;

namespace DALTest;
[TestFixture]
public class OrderDALTest
{
    OrderDAL orderDAL = new OrderDAL();
    [SetUp]
    public void Setup()
    {
    }
    [Test]
    public void UpdateStatusOfOrderTest()
    {
        var result = orderDAL.UpdateStatusOfOrder(8, "Processing");

        Assert.IsTrue(result);
    }
    [Test]
    public void UpdateCreateDateOfOrderTest()
    {
        var result = orderDAL.UpdateCreateDateOfOrder(8, "2022-08-14 13:17:16");

        Assert.IsTrue(result);
    }
    [Test]
    public void InsertOrderTest()
    {
        Order order = new Order(50, 1, 2, "2022-08-14 13:17:16", "Shopping");
        var result = orderDAL.InsertOrder(order);

        Assert.IsTrue(result);
    }
    [Test]
    public void OrderIDMaxTest()
    {
        var result = orderDAL.OrderIDMax();

        Assert.AreEqual(result, 50);
    }
    [Test]
    public void GetOrderByIDTest()
    {
        var result = orderDAL.GetOrderByID(50);
        Order order = new Order(50, 1, 2, "2022-08-14 13:17:16", "Shopping");

        Assert.IsTrue(result.DeepEquals1(order));
    }
    
}