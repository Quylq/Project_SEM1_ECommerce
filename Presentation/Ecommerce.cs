using BL;
using System.Security.Cryptography;
using System.Text;

namespace Persistence
{
    public class Ecommerce
    {
        private UserBL userBL;
        private BuyerPage buyerPage;

        public Ecommerce()
        {
            userBL = new UserBL();
            buyerPage = new BuyerPage();
        }

        public void Login()
        {
            Console.WriteLine(" --- ECOMMERCE --- ");
            Console.WriteLine();
            Console.Write("Nhập Tên Đăng Nhập: ");
            string _UserName = Console.ReadLine();
            Console.Write("Nhập Mật Khẩu: ");
            string _Password = Console.ReadLine();

            User user =  userBL.GetUserByName(_UserName);

            if (user.UserName != null)
            {
                if (_Password == user.Password)
                {
                    Console.WriteLine($"Đăng nhập thành công");
                    if (user.Role == "Seller")
                    {
                        SellerPage(user.UserId);
                    }
                    else if (user.Role == "Customer")
                    {
                        buyerPage.CustomerPage(user.UserId);
                    }
                    else
                    {
                        Console.WriteLine($"Update");
                    }
                }
                else
                {
                    Console.WriteLine($"Sai mật khẩu");
                }
            }
            else
            {
                Console.WriteLine($"Tài khoản không tồn tại");
            }
        }
        public void SellerPage (int ID)
        {
            User user = userBL.GetUserByID(ID);
            Console.WriteLine(" --- ECOMMERCE --- ");
            Console.WriteLine();
            Console.WriteLine("Chào " + user.FullName);
            Console.WriteLine();
            Console.WriteLine("1. Quản lý đơn đặt hàng.");
            Console.WriteLine("2. Danh sách sản phẩm.");
            Console.WriteLine("3. Quản lý danh mục sản phẩm.");
            Console.WriteLine("4. Thoát");
            Console.Write("Chọn: ");
            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    ManagementOrders(user.UserId);
                    break;
                case "2": 
                    ListOfProduct(user.UserId);
                    break;
                case "3": 
                    CategoryManagement(user.UserId);
                    break;
                case "4": 
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Vui lòng chọn 1, 2, 3 hoặc 4!");
                    SellerPage (user.UserId);
                    break;
            }
        }
        public void ManagementOrders(int ID)
        {
            User user = userBL.GetUserByID(ID);
            Console.WriteLine("1. Đã xác nhận.");
            Console.WriteLine("2. Chờ xác nhận.");
            Console.WriteLine("3. Quay lại.");
            Console.Write("Chọn: ");
            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1": 
                    break; 
                case "2": 
                    break;
                case "3":
                    SellerPage(user.UserId);
                    break;
                default:
                    Console.WriteLine("Vui lòng chọn 1, 2 hoặc 3!");
                    ManagementOrders(user.UserId);
                    break;
            }
        }
        public void ListOfProduct(int ID)
        {
            User user = userBL.GetUserByID(ID);
            Console.WriteLine("1. Tìm kiếm sản phẩm.");
            Console.WriteLine("2. Thông tin sản phẩm.");
            Console.WriteLine("3. Quay lại.");
            Console.Write("Chọn: ");
            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1": 
                    break; 
                case "2": 
                    break;
                case "3": 
                    SellerPage(user.UserId);
                    break;
                default:
                    Console.WriteLine("Vui lòng chọn 1, 2 hoặc 3!");
                    ListOfProduct(user.UserId);
                    break;
            }
        }
        public void CategoryManagement(int ID)
        {
            User user = userBL.GetUserByID(ID);
            Console.WriteLine("1. Xem danh mục.");
            Console.WriteLine("2. Quay lại.");
            Console.Write("Chọn: ");
            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1": 
                    break; 
                case "2": 
                    SellerPage(user.UserId);
                    break; 
                default:
                    Console.WriteLine("Vui lòng chọn 1 hoặc 2!");
                    CategoryManagement(user.UserId);
                    break;
            }
        }   
    }
}