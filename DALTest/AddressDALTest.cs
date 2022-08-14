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

    // [TestCase(10, "Hà Nội", "Cầu Giấy", "Trung Hòa", "A14A1")]
    // public void InsertAddress_ValueTrue_ReturnTrue(int _AddressID, string _City, string _District, string _Commune, string _SpecificAddress)
    // {
    //     Address address = new Address(_AddressID, _City, _District, _Commune, _SpecificAddress);
    //     var result = addressDAL.InsertAddress(address);
    //     Assert.IsTrue(result);
    // }

    [TestCase(8, "Hà Nội", "Cầu Giấy", "Trung Hòa", "A14A1")]
    public void InsertAddress_ValueFasle_ReturnFalse(int _AddressID, string _City, string _District, string _Commune, string _SpecificAddress)
    {
        Address address = new Address(_AddressID, _City, _District, _Commune, _SpecificAddress);
        var result = addressDAL.InsertAddress(address);
        Assert.IsFalse(result);
    }
}