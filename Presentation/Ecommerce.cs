using BL;
using System.Security.Cryptography;
using System.Text;

namespace Persistence
{
    public class Ecommerce
    {
        private UserBL userBL;
        private ShopBL shopBL;
        private AddressBL addressBL;
        public Ecommerce()
        {
            userBL = new UserBL();
            shopBL = new ShopBL();
            addressBL = new AddressBL();
        }
        public void Menu()
        {
            Console.WriteLine("1. Đăng Nhập. ");
            Console.WriteLine("2. Đăng Ký. ");
            Console.WriteLine("0. Thoát");
            Console.Write("Chọn: ");
            string? choice = Console.ReadLine();
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
                    Console.WriteLine("Vui lòng chọn 0, 1, 2 !");
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
            string? _Password = ReadPassword();

            User user =  userBL.GetUserByName(_UserName);

            if (user.UserName != null)
            {
                if (_Password == user.Password)
                {
                    Console.WriteLine($"Đăng nhập thành công!");
                    if (user.Role == "Customer")
                    {
                        CustomerPage(user.UserID);
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
                Console.WriteLine("Nhấn phím bất kỳ để quay lại");
                Console.ReadKey();
                Menu();
            }

        }
        public void SigUp()
        {
            Console.Write("Tên đăng nhập: ");
            string _UserName = Console.ReadLine();
            Console.Write("Mật Khẩu: ");
            string _Password = ReadPassword();
            Console.Write("You Name: ");
            string _FullName = Console.ReadLine();
            Console.Write("Email: ");
            string _Email = Console.ReadLine();
            Console.Write("Phone: ");
            string _Phone = Console.ReadLine();
            string _Birthday = ReadBirthDay();
            int _AddressID = ReadAddress();
            int _UserID = userBL.UserIDMax() + 1;
            User user = new User(_UserID, _UserName, _Password, _FullName, _Birthday, _Email, _Phone, _AddressID, "Customer");
            userBL.InsertUser(user);

            Console.WriteLine("Đăng ký thành công!");
            Console.WriteLine("Nhấn phím bất kỳ để tiếp tục");
            Console.ReadKey();
            CustomerPage(_UserID);
        }
        public void SigUpShop(int _UserID)
        {
            Console.Write("Nhập tên Shop: ");
            string _ShopName = Console.ReadLine();
            int _AddressID = ReadAddress();
            int _ShopID = shopBL.ShopIDMax() + 1;
            Shop shop = new Shop(_ShopID, _ShopName, _UserID, _AddressID);
            shopBL.InsertShop(shop);

            Console.WriteLine("Tạo cửa hàng thành công");
            Console.WriteLine("Nhấn phím bất kỳ để vào cửa hàng");
            Console.ReadKey();
            SellerPage(_ShopID);

        }
        public string ReadPassword()
        {
            string temp = "";
            ConsoleKeyInfo info = Console.ReadKey(true);
            while (info.Key != ConsoleKey.Enter)
            {
                if (info.Key != ConsoleKey.Backspace && info.Key != ConsoleKey.Spacebar)
                {
                    temp += info.KeyChar;
                    Console.Write("*");
                }
                else if (temp.Length > 0 && info.Key == ConsoleKey.Backspace)
                {
                    Console.Write("\b");
                    temp = temp.Substring(0, temp.Length - 1);
                }
                info = Console.ReadKey(true);
            }
            Console.WriteLine();
            return Sha256Hash(temp);
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
        public string ReadBirthDay()
        {
            DateOnly _Birthday;
            string format = "yyyy-MM-dd";
            try
            {           
            Console.WriteLine("Birthday (dd/MM/yyyy): ");  
            string s = Console.ReadLine();
            string[] result = s.Replace(" ", "").Split('/', StringSplitOptions.None);
            _Birthday = new DateOnly(Convert.ToInt32(result[2]), Convert.ToInt32(result[1]), Convert.ToInt32(result[0]));
            }
            catch (System.Exception)
            {
                Console.WriteLine($"Ngày sinh không hợp lệ, vui lòng nhập lại.");
                ReadBirthDay();
            }
            return _Birthday.ToString(format);
        }
        public int ReadAddress()
        {
            Console.WriteLine("--- Địa chỉ ---");
            Console.Write("City: ");
            string _City = Console.ReadLine();
            Console.Write("District: ");
            string _District = Console.ReadLine();
            Console.Write("Commune: ");
            string _Commune = Console.ReadLine();
            Console.Write("SpecificAddress: ");
            string _SpecificAddress = Console.ReadLine();
            int _AddressID = addressBL.AddressIDMax() + 1;
            Address address =  new Address(_AddressID, _City, _District, _Commune, _SpecificAddress);
            addressBL.InsertAddress(address);

            return _AddressID;
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
                        SigUpShop(_UserID);
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
        public void SellerPage (int _ShopID)
        {
            SellerPage sellerPage = new SellerPage();
            Console.Clear();
            Console.WriteLine("1. Quản lý đơn đặt hàng.");
            Console.WriteLine("2. Quản lý sản phẩm.");
            Console.WriteLine("3. Quản lý danh mục sản phẩm.");
            Console.WriteLine("0. Quay lại");
            Console.Write("Chọn: ");
            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    sellerPage.OrderManagement(_ShopID);
                    break;
                case "2": 
                    sellerPage.ProductManagement(_ShopID);
                    break;
                case "3": 
                    sellerPage.CategoryManagement(_ShopID);
                    break;
                case "0": 
                    Shop shop = shopBL.GetShopByID(_ShopID);
                    CustomerPage(shop.UserID);
                    break;
                default:
                    Console.WriteLine("Vui lòng chọn 0 - 3 !");
                    SellerPage (_ShopID);
                    break;
            }
        }
    }
}