using BL;
using System.Security.Cryptography;
using System.Text;

namespace Persistence
{
    public class Ecommerce
    {
        private UserBL userBL;
        private ShopBL shopBL;
        public Ecommerce()
        {
            userBL = new UserBL();
            shopBL = new ShopBL();
        }
        public void Menu()
        {
            Console.WriteLine("1. Đăng Nhập: ");
            Console.WriteLine("0. Thoát");
            Console.Write("Chọn: ");
            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    Login();
                    break;
                case "0":
                    Console.WriteLine("Bạn xác nhận muốn thoát?");
                    Console.WriteLine("1. Yes       2. No");
                    Console.Write("Chọn: ");
                    string? choice1 = Console.ReadLine();
                    switch (choice1)
                    {
                        case "1":
                            Console.WriteLine(" You Are Exit");
                            Environment.Exit(0);
                            break;
                        case "2":
                            Menu();
                            break;
                        default:
                            Console.WriteLine("Vui lòng chọn 1 hoặc 2!");
                            Menu();
                            break;
                    }
                    break;
                default:
                    Console.WriteLine("Vui lòng chọn 0 hoặc 1!");
                    Menu();
                    break;
            }
        }
        public void Login()
        {
            Console.Clear();
            Console.Write("Nhập Tên Đăng Nhập: ");
            string? _UserName = Console.ReadLine();
            Console.Write("Nhập Mật Khẩu: ");
            string? _Password = Console.ReadLine();
            string? _Pass = Sha256Hash(_Password);

            User user =  userBL.GetUserByName(_UserName);

            if (user.UserName != null)
            {
                if (_Pass == user.Password)
                {
                    Console.WriteLine($"Đăng nhập thành công!");
                    if (user.Role == "Seller")
                    {
                        SellerPage(user.UserID);
                    }
                    else if (user.Role == "Customer")
                    {
                        CustomerPage(user.UserID);
                    }
                }
                else
                {
                    Console.WriteLine($"Mật khẩu của bạn không đúng");
                    Menu();
                }
            }
            else
            {
                Console.WriteLine($"Tên tài khoản không tồn tại");
                Menu();
            }
        }
        static string Sha256Hash(string rawData)  
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
        public void CustomerPage(int _UserID)
        {
            CustomerPage customerPage = new CustomerPage();
            Console.Clear();
            Console.WriteLine("1. Search Product.");
            Console.WriteLine("2. Search Shop.");
            Console.WriteLine("3. Cart.");
            Console.WriteLine("4. My Order.");
            Shop shop = shopBL.GetShopByUserID(_UserID);
            if (shop.UserID == _UserID)
            {
                Console.WriteLine("5. My Shop.");  
            }      
            else
            {
                Console.WriteLine("5. Đăng ký bán hàng."); 
            }
            Console.WriteLine("0. Exit.");
            Console.Write("Chọn: ");
            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    customerPage.SearchProduct(_UserID);
                    break;
                case "2":
                    customerPage.SearchShop(_UserID);
                    break;
                case "3":
                    customerPage.ViewCart(_UserID);
                    break;
                case "4": 
                    customerPage.MyOrder(_UserID);
                    break; 
                case "5": 
                    if (shop.UserID == _UserID)
                    {
                        SellerPage(shop.ShopID);
                    }
                    else
                    {
                        Console.WriteLine("Đang cập nhật");
                        Console.ReadKey();
                        CustomerPage(_UserID);
                    }
                    break; 
                case "0": 
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Vui lòng chọn 0 - 4 !");
                    CustomerPage(_UserID);
                    break;
            }
        }
        public void SellerPage (int _UserID)
        {
            SellerPage sellerPage = new SellerPage();
            Console.Clear();
            Console.WriteLine("1. Quản lý đơn đặt hàng.");
            Console.WriteLine("2. Quản lý sản phẩm.");
            Console.WriteLine("3. Quản lý danh mục sản phẩm.");
            Console.WriteLine("0. Thoát");
            Console.Write("Chọn: ");
            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    sellerPage.OrderManagement(_UserID);
                    break;
                case "2": 
                    sellerPage.ProductManagement(_UserID);
                    break;
                case "3": 
                    sellerPage.CategoryManagement(_UserID);
                    break;
                case "0": 
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Vui lòng chọn 0 - 3 !");
                    SellerPage (_UserID);
                    break;
            }
        }
    }
}