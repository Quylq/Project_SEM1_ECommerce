using BL;
using System.Security.Cryptography;
using System.Text;

namespace Persistence
{
    public class Ecommerce
    {
        private UserBL userBL;
<<<<<<< HEAD
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
=======
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
>>>>>>> 8255db6127fe37ebc1e2dda5a8f2444632a6084f
            Console.WriteLine("1. Quản lý đơn đặt hàng.");
            Console.WriteLine("2. Quản lý sản phẩm.");
            Console.WriteLine("3. Quản lý danh mục sản phẩm.");
            Console.WriteLine("0. Thoát");
            Console.Write("Chọn: ");
            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
<<<<<<< HEAD
                    ManagementOrders(user.UserId);
                    break;
                case "2": 
                    ListOfProduct(user.UserId);
                    break;
                case "3": 
                    CategoryManagement(user.UserId);
=======
                    sellerPage.OrderManagement(user);
                    break;
                case "2": 
                    sellerPage.ProductManagement(user);
                    break;
                case "3": 
                    sellerPage.CategoryManagement(user);
>>>>>>> 8255db6127fe37ebc1e2dda5a8f2444632a6084f
                    break;
                case "0": 
                    Environment.Exit(0);
                    break;
                default:
<<<<<<< HEAD
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
=======
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
>>>>>>> 8255db6127fe37ebc1e2dda5a8f2444632a6084f
            Console.Write("Chọn: ");
            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1": 
<<<<<<< HEAD
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
=======
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
>>>>>>> 8255db6127fe37ebc1e2dda5a8f2444632a6084f
    }
}