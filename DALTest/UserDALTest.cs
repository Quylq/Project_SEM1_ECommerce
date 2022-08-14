using NUnit.Framework;
using DAL;
using Persistence;

namespace DALTest;
[TestFixture]
public class UserDALTest
{
    UserDAL userDAL = new UserDAL();
    [SetUp]
    public void Setup()
    {
    }
    // Done
    [Test] 
    public void CheckUserName_valueTrue_ReturnTrue()
    {
        var result = userDAL.CheckUserName("user1");

        Assert.IsTrue(result);
    }
    // Done
    [Test]
    public void CheckUserName_valueFalse_ReturnFalse()
    {
        var result = userDAL.CheckUserName("user1y7");

        Assert.IsFalse(result);
    }
    [Test] // Done
    public void Login_ValueTrue_ReturnInt()
    {
        var result = userDAL.Login("user10", "1234567");
        Assert.AreEqual(10, result);
    }
    // Done
    [TestCase("user1", "matkhausai")]
    [TestCase("user1", null)]
    [TestCase("user1", "")]
    [TestCase("user10", "123 456")]
    [TestCase("taikhoansai", "123456")]
    [TestCase("", "123456")]
    [TestCase(null, "123456")]
    [TestCase("user 10", "123456")]
    public void Login_ValueFalse_ReturnNull(string _UserName, string _Password)
    {
        var result = userDAL.Login(_UserName, _Password);
        Assert.AreEqual(0, result);
    }
    // Done
    [Test]
    public void GetUserByID_ValueTrue_ReturnTrue()
    {
        var result = userDAL.GetUserByID(10);
        User user = new User(10, "User10", "1234567", "Lê văn Tý", "06/07/2000", "Levanty@vtc.edu.vn", "0987654332", 6, "Customer");
        Assert.IsTrue(result!.DeepEquals1(user));
    }
    // Done
    [TestCase(100)]
    public void GetUserByID_ValueFalse_ReturnFalse(int _UserID)
    {
        var result = userDAL.GetUserByID(_UserID);
        
        Assert.IsNull(result);
    }
    // [TestCase(11, "user11", "123456", "lê văn tèo", "2000-9-4", "levanteo@vtc.edu.vn", "0987654321", 5, "Customer")]
    // public void InsertUser_ValueTrue_ReturnTrue(int _UserID, string _UserName, string _Password, string _FullName, string _Birthday, string _Email, string _Phone, int _AddressID, string _Role)
    // {
    //     User user = new User(_UserID, _UserName, _Password, _FullName, _Birthday, _Email, _Phone, _AddressID, _Role);
    //     var result = userDAL.InsertUser(user);
    //     Assert.IsTrue(result);
    // }

    [TestCase(9, "user11", "123456", "lê văn tèo", "2000-9-4", "levanteo@vtc.edu.vn", "0987654321", 5, "Customer")]
    [TestCase(11, "user9", "123456", "lê văn tèo", "2000-9-4", "levanteo@vtc.edu.vn", "0987654321", 5, "Customer")]
    [TestCase(11, "user11", "123456", "lê văn tèo", "2000-9-4", "levanteo@vtc.edu.vn", "0987654321", 11, "Customer")]
    public void InsertUser_ValueFasle_ReturnFalse(int _UserID, string _UserName, string _Password, string _FullName, string _Birthday, string _Email, string _Phone, int _AddressID, string _Role)
    {
        User user = new User(_UserID, _UserName, _Password, _FullName, _Birthday, _Email, _Phone, _AddressID, _Role);
        var result = userDAL.InsertUser(user);
        Assert.IsFalse(result);
    }
    
    [Test]
    public void UpdatePassword_ValueTrue_ReturnTrue()
    {
        var result = userDAL.UpdatePassword(10, "1234567");
        Assert.IsTrue(result);
    }
    [TestCase(10, "123 456")]
    [TestCase(10, null)]
    [TestCase(10, "")]   
    public void UpdatePassword_ValueFalse_ReturnFalse(int _UserID, string _Password)
    {
        var result = userDAL.UpdatePassword(_UserID, _Password);
        Assert.IsFalse(result);
    }

    [Test]
    public void UpdateUser_ValueTrue_returnTrue()
    {
        User user = new User(10, "Lê văn Tý", "2000-7-6", "Levanty@vtc.edu.vn", "0987654332", 6, "Customer");
        var result = userDAL.UpdateUser(user);
        Assert.IsTrue(result);
    }
    [TestCase(10, "Lê văn Tý", "2000-7-60", "Levanty@vtc.edu.vn", "0987654332", 6)] 
    [TestCase(10, "Lê văn Tý", "2000-7-6", "Levanty@vtc.edu.vn", "0987654332", 0)] 
    public void UpdateUser_ValueFalse_returnFalse(int _UserID, string _FullName, string _Birthday, string _Email, string _Phone, int _AddressID)
    {
        User user = new User(_UserID, _FullName, _Birthday, _Email, _Phone, _AddressID, "Customer");
        var result = userDAL.UpdateUser(user);
        Assert.IsFalse(result);
    }
    [Test]
    public void UpdatePassword()
    {
        var result = userDAL.UpdatePassword(10, "1234567");

        Assert.IsTrue(result);
    }
    [Test]
    public void UserIDMax()
    {
        var result = userDAL.UserIDMax();

        Assert.AreEqual(result, 11);
    }
}