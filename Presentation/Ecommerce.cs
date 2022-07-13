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
        static string ComputeSha256Hash(string rawData)  
        {  
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())  
            {  
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));  
  
                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();  
                for (int i = 0; i < bytes.Length; i++)
                {  
                    builder.Append(bytes[i].ToString("x2"));  
                }  
                return builder.ToString();  
            }  
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
            string _password = ComputeSha256Hash(_Password);
            User user =  userBL.GetUserByName(_UserName);

            if (user.UserName != null)
            {
                if (_password == user.Password)
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
                }
                else
                {
                    Console.WriteLine($"Mật khẩu của bạn không đúng");
                }
            }
            else
            {
                Console.WriteLine($"Tên tài khoản không tồn tại");
            }
        }        
    }
}