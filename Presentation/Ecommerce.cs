using BL;
using System.Security.Cryptography;
using System.Text;

namespace Persistence
{
    public class Ecommerce
    {
        private UserBL userBL;

        public Ecommerce()
        {
            userBL = new UserBL();
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
                    Console.WriteLine($"Đăng nhập thành công");
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
            int choice = Convert.ToInt32(Console.ReadLine());
            try
            {
                switch (choice)
                {
                    case 1:
                        Login();
                        break;
                    case 2:
                        SigUp();
                        break;
                    case 3:
                        Console.WriteLine("Bạn xác nhận muốn thoát?");
                        Console.WriteLine("1. Yes       2. No");
                        Console.Write("Chọn: ");
                        choice = Convert.ToInt32(Console.ReadLine());
                        try
                        {
                            switch (choice)
                            {
                                case 1:
                                    Console.WriteLine(" You Are Exit");
                                    Environment.Exit(0);
                                    break;
                                case 2:
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
        public void SellerPage ()
        {
            Console.Write("1. Management Orders.");
            Console.Write("2. List of products.");
            Console.Write("3. Category Management.");
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
                default:
                    break;
            }
        }

        public void CustomerPage()
        {
            Console.Write("1. My order.");
            Console.Write("2. Cart.");
            Console.Write("3. Show Category.");
            Console.Write("4. Search product.");
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
                default:
                    break;
            }
        }
        public void ManagementOrders()
        {
            Console.Write("1. Confirmed.");
            Console.Write("2. Wait for confirmation.");
            Console.Write("Chọn: ");
            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1": 
                    break; 
                case "2": 
                    break;
                default:
                    break;
            }
        }
        public void ListOfProduct()
        {
            Console.Write("1. SearchProduct.");
            Console.Write("2. View product information.");
            Console.Write("Chọn: ");
            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1": 
                    break; 
                case "2": 
                    break;
                default:
                    break;
            }
        }
        public void CategoryManagement()
        {
            Console.Write("1. View category.");
            Console.Write("Chọn: ");
            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1": 
                    break; 
                default:
                    break;
            }
        }
        public void Cart()
        {
            Console.Write("1. Remove from cart.");
            Console.Write("2. Pay the bill.");
            Console.Write("Chọn: ");
            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1": 
                    break; 
                case "2": 
                    break;
                default:
                    break;
            }
        }
        public void ShowCategory()
        {
            Console.Write("1. View product information.");
            Console.Write("Chọn: ");
            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1": 
                    break; 
                default:
                    break;
            }
        }
        public void SearchProduct()
        {
            
        }
        public void Myorder()
        {
            
        }
    }
}