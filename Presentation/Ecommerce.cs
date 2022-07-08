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
        
        public void Menu()
        {
            // Console.Clear();
            Ecommerce ecommerce = new Ecommerce();
            Console.WriteLine("1. Đăng Nhập: ");
            Console.WriteLine("2. Đăng Ký: ");
            Console.WriteLine("0. Thoát");
            Console.Write("Chọn: ");
            string? choice = Console.ReadLine();
            try
            {
                switch (choice)
                {
                    case "1":
                        Login();
                        break;
                    case "2":
                        SigUp();
                        break;
                    case "0":
                        Console.WriteLine("Bạn xác nhận muốn thoát?");
                        Console.WriteLine("1. Yes       2. No");
                        Console.Write("Chọn: ");
                        string? choice1 = Console.ReadLine();
                        try
                        {
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
                        }
                        catch (System.Exception)
                        {
                            Console.WriteLine("Vui lòng chọn 0, 1, 2 !");
                            throw;
                        }
                        break;
                    default:
                        Console.WriteLine("Vui lòng chọn 0, 1, 2 !");
                        Menu();
                        break;
                }
            }
            catch (System.Exception)
            {
                Console.WriteLine("Vui lòng chọn 0, 1, 2 !");
                Menu();
                throw;
            }
        }
        public void Login()
        {
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
                        SellerPage(user);
                    }
                    else if (user.Role == "Customer")
                    {
                        CustomerPage(user);
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
        public void SigUp()
        {}
        public void SellerPage (User user)
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
                    sellerPage.OrderManagement(user);
                    break;
                case "2": 
                    sellerPage.ProductManagement(user);
                    break;
                case "3": 
                    sellerPage.CategoryManagement();
                    break;
                case "0": 
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Vui lòng chọn 0 - 3 !");
                    SellerPage (user);
                    break;
            }
        }

        public void CustomerPage(User user)
        {
            Console.Clear();
            Console.WriteLine("1. Đơn hàng của tôi.");
            Console.WriteLine("2. Giỏ hàng.");
            Console.WriteLine("3. Danh mục.");
            Console.WriteLine("4. Tìm kiếm sản phẩm.");
            Console.WriteLine("0. Thoát.");
            Console.Write("Chọn: ");
            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1": 
                    break;
                case "2": 
                    break;
                case "3": 
                    break; 
                case "4": 
                    break;
                case "0": 
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Vui lòng chọn 0 - 4 !");
                    CustomerPage(user);
                    break;
            }
        }
    }
}