using BL;
using System.Security.Cryptography;
using System.Text;

namespace Persistence
{
    public class Ecommerce
    {
        private UserBL userBL;
        private ProductBL productBL;

        public Ecommerce()
        {
            userBL = new UserBL();
            productBL = new ProductBL();
        }
        
        public void Login()
        {
            Console.Write("Nhập Tên Đăng Nhập: ");
            string _UserName = Console.ReadLine();
            Console.Write("Nhập Mật Khẩu: ");
            string _Password = Console.ReadLine();

            User user =  userBL.GetUserByName(_UserName);

            if (user.UserName != null)
            {
                if (_Password == user.Password)
                {
                    Console.WriteLine($"Đăng nhập thành công {user.UserId}");
                    if (user.Role == "Seller")
                    {
                        SellerPage(user);
                    }
                    else if (user.Role == "Customer")
                    {
                        CustomerPage();
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
        public void SigUp()
        {}
        public void Menu()
        {
            Ecommerce ecommerce = new Ecommerce();
            Console.WriteLine("1. Đăng Nhập: ");
            Console.WriteLine("2. Đăng Ký: ");
            Console.WriteLine("3. Thoát");
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
                    case "3":
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
                            Console.WriteLine("Vui lòng chọn 1, 2 hoặc 3!");
                            throw;
                        }
                        break;
                    default:
                        Console.WriteLine("Vui lòng chọn 1, 2 hoặc 3!");
                        Menu();
                        break;
                }
            }
            catch (System.Exception)
            {
                Console.WriteLine("Vui lòng chọn 1, 2 hoặc 3!");
                Menu();
                throw;
            }
        }
        public void SellerPage (User user)
        {
            Console.WriteLine("1. Quản lý đơn đặt hàng.");
            Console.WriteLine("2. Danh sách sản phẩm.");
            Console.WriteLine("3. Quản lý danh mục sản phẩm.");
            Console.WriteLine("4. Thoát");
            Console.Write("Chọn: ");
            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    ManagementOrders();
                    break;
                case "2": 
                    ListOfProduct(user);
                    break;
                case "3": 
                    CategoryManagement();
                    break;
                case "4": 
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Vui lòng chọn 1, 2, 3 hoặc 4!");
                    SellerPage (user);
                    break;
            }
        }

        public void CustomerPage()
        {
            Console.WriteLine("1. Đơn hàng của tôi.");
            Console.WriteLine("2. Giỏ hàng.");
            Console.WriteLine("3. Danh mục.");
            Console.WriteLine("4. Tìm kiếm sản phẩm.");
            Console.WriteLine("5. Thoát.");
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
                case "5": 
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Vui lòng chọn 1, 2, 3, 4 hoặc 5!");
                    CustomerPage();
                    break;
            }
        }
        public void ManagementOrders()
        {
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
                    // SellerPage();
                    break;
                default:
                    Console.WriteLine("Vui lòng chọn 1, 2 hoặc 3!");
                    ManagementOrders();
                    break;
            }
        }
        public void ListOfProduct(User user)
        {
            Console.WriteLine("1. Tìm kiếm sản phẩm.");
            Console.WriteLine("2. Thông tin sản phẩm.");
            Console.WriteLine("3. Quay lại.");
            Console.Write("Chọn: ");
            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1": 
                    SearchProductOfShop(user);
                    break; 
                case "2": 
                    break;
                case "3": 
                    SellerPage(user);
                    break;
                default:
                    Console.WriteLine("Vui lòng chọn 1, 2 hoặc 3!");
                    ListOfProduct(user);
                    break;
            }
        }
        public void CategoryManagement()
        {
            Console.WriteLine("1. Xem danh mục.");
            Console.WriteLine("2. Quay lại.");
            Console.Write("Chọn: ");
            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1": 
                    break; 
                case "2": 
                    // SellerPage();
                    break; 
                default:
                    Console.WriteLine("Vui lòng chọn 1 hoặc 2!");
                    CategoryManagement();
                    break;
            }
        }
        public void Cart()
        {
            Console.WriteLine("1. Loại bỏ khỏi giỏ hàng.");
            Console.WriteLine("2. Thanh Toán.");
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
                    CustomerPage();
                    break;
                default:
                    Console.WriteLine("Vui lòng chọn 1, 2 hoặc 3!");
                    Cart();
                    break;
            }
        }
        public void ShowCategory()
        {
            Console.WriteLine("1. Xem thông tin sản phẩm.");
            Console.WriteLine("2. Quay lại.");
            Console.Write("Chọn: ");
            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1": 
                    break;
                case "2":
                    CustomerPage();
                    break; 
                default:
                    Console.WriteLine("Vui lòng chọn 1 hoặc 2!");
                    ShowCategory();
                    break;
            }
        }
        public void SearchProductOfShop(User user)
        {
            Console.WriteLine("1. Nhập sản phẩm bạn muốn tìm: ");
            string _ProductName = Console.ReadLine();
            List<Product> productList = new List<Product>();
            productList = productBL.GetProductListByName(_ProductName, user);
            Console.Clear();
            Console.WriteLine("|----------------------------------------------------------------------|");
            Console.WriteLine("| Tên sản Phẩm                                       |       Giá       |");
            Console.WriteLine("|----------------------------------------------------------------------|");
            foreach (Product product in productList) 
            { 
                Console.WriteLine("| {0,-50} | {1, 15} |", product.ProductName, product.Price.ToString("C0"));
            }
            Console.WriteLine("|----------------------------------------------------------------------|");

        }

        public void SearchProduct()
        {
            Console.WriteLine("1. Nhập sản phẩm bạn muốn tìm: ");
            string _ProductName = Console.ReadLine();
            List<Product> productList = new List<Product>();
            productList = productBL.GetProductListByName(_ProductName);
            Console.Clear();
            Console.WriteLine("|----------------------------------------------------------------------|");
            Console.WriteLine("| Tên sản Phẩm                                       |       Giá       |");
            Console.WriteLine("|----------------------------------------------------------------------|");
            foreach (Product product in productList) 
            { 
                Console.WriteLine("| {0,-50} | {1, 15} |", product.ProductName, product.Price.ToString("C0"));
            }
            Console.WriteLine("|----------------------------------------------------------------------|");

        }

        public void Myorder()
        {
            
        }
        
    }
}