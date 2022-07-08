using BL;
using System.Security.Cryptography;
using System.Text;

namespace Persistence
{
    public class BuyerPage
    {
        private UserBL userBL;

        public BuyerPage()
        {
            userBL = new UserBL();
        }
    }
}