using NUnit.Framework;
using BL;
using Persistence;

namespace BLTest;
[TestFixture]
public class UserBLTests
{
    [SetUp]
    public void Setup()
    {
    }

    [TestCase("user1676", "dhfjksad")]
    [TestCase("user1", "dhfjksad")]
    public void Login_ValueFalse_ReturnNull(string _UserName, string _Password)
    {
    }
    
}