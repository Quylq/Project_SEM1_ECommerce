using NUnit.Framework;
using DAL;
using Persistence;

namespace DALTest;
[TestFixture]
public class AddressDALTest
{
    AddressDAL addressDAL = new AddressDAL();
    [SetUp]
    public void Setup()
    {
    }
    [Test]
    public void GetAddressByIDTest()
    {
        var result = addressDAL.GetAddressByID(1);
        Address address = new Address(1, "Hà Nội", "Quận Đống Đa", "Phường Trung Liệt", "Phố Chùa Bộc");
        Assert.IsTrue(result.DeepEquals1(address));
    }
    [Test]
    public void InsertAddressTest()
    {
        Address address = new Address(15, "Hà Nội", "Quận Cầu Giấy", "Phường Trung Hòa", "Số nhà 99");
        var result = addressDAL.InsertAddress(address);

        Assert.IsTrue(result);
    }
    [Test]
    public void AddressIDMaxTest()
    {
        var result = addressDAL.AddressIDMax();

        Assert.AreEqual(result, 15);
    }
}