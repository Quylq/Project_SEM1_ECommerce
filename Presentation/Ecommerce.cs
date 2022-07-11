using BL;
using System.Security.Cryptography;
using System.Text;

namespace Persistence
{
    public class Ecommerce
    {
        private UserBL userBL;
        private ProductBL productBL;
        private OrderBL orderBL;
        private CategoryBL categoryBL;
        public Ecommerce()
        {
            userBL = new UserBL();
            productBL = new ProductBL();
            orderBL = new OrderBL();
            categoryBL = new CategoryBL();
        }
        public void Login()
        {
            SellerPage sellerPage = new SellerPage();
            CustomerPage customerPage = new CustomerPage();
            Console.Clear();
            Console.Write("Nhập Tên Đăng Nhập: ");
            string? _UserName = Console.ReadLine();
            Console.Write("Nhập Mật Khẩu: ");
            string? _Password = Console.ReadLine();

            User user =  userBL.GetUserByName(_UserName);

            if (user.UserName != null)
            {
                if (_Password == user.Password)
                {
                    Console.WriteLine($"Đăng nhập thành công {user.UserID}");
                    if (user.Role == "Seller")
                    {
                        sellerPage.Seller(user);
                    }
                    else if (user.Role == "Customer")
                    {
                        customerPage.Customer(user);
                    }
                    else
                    {
                        Console.WriteLine($"Update");
                    }
                }
            }
            else
            {
                Console.WriteLine($"Sai tài khoản hoặc mật khẩu");
            }

        }        
    }
}