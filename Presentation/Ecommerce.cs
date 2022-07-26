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
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("| -------------  ECOMMERCE  ------------- |");
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine();
            Console.WriteLine("1. Login ");
            Console.WriteLine("2. Sign UP ");
            Console.WriteLine("0. Exit");
            Console.Write("Choice: ");
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
                    Console.WriteLine("Confirm you want to exit?");
                    Console.WriteLine("1. Yes       2. No");
                    Console.Write("Choice: ");
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
                            Console.WriteLine("Please choose 1 or 2!");
                            Menu();
                            break;
                    }
                    break;
                default:
                    Console.WriteLine("Please choose 0 - 2!");
                    Menu();
                    break;
            }
        }
        public void Login()
        {
            Console.Clear();
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("|------------- ECOMMERCE -------------|");
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine();
            Console.Write("UserName: ");
            string? _UserName = Console.ReadLine();
            Console.Write("Password: ");
            string? _Password = ReadPassword();

            User user = userBL.GetUserByName(_UserName);

            if (user.UserName != null)
            {
                if (_Password == user.Password)
                {
                    Console.WriteLine($"Login success!");
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
                    Console.WriteLine($"Wrong password!");
                    Menu();
                }
            }
            else
            {
                Console.WriteLine($"Account name does not exist!");
                Console.ReadKey();
                Menu();
            }

        }
        public void SigUp()
        {
            Console.Clear();
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("|------------- ECOMMERCE -------------|");
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine();
            Console.Write("UserName: ");
            string _UserName = Console.ReadLine();
            Console.Write("Password: ");
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

            Console.WriteLine("Sign Up Success!");
            Console.WriteLine("Press any key to continue!");
            Console.ReadKey();
            Menu();
        }
        public void SigUpShop(int _UserID)
        {
            Console.Clear();
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("|------------- ECOMMERCE -------------|");
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine();
            Console.Write("Shop Name: ");
            string _ShopName = Console.ReadLine();
            int _AddressID = ReadAddress();
            int _ShopID = shopBL.ShopIDMax() + 1;
            Shop shop = new Shop(_ShopID, _ShopName, _UserID, _AddressID);
            shopBL.InsertShop(shop);

            Console.WriteLine("Create successful shop!");
            Console.WriteLine("Press any key to enter the shop!");
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
                Console.WriteLine($"Invalid date of birth, please re-enter.");
                ReadBirthDay();
            }
            return _Birthday.ToString(format);
        }
        public int ReadAddress()
        {
            Console.WriteLine("------- Address -------");
            Console.Write("City: ");
            string _City = Console.ReadLine();
            Console.Write("District: ");
            string _District = Console.ReadLine();
            Console.Write("Commune: ");
            string _Commune = Console.ReadLine();
            Console.Write("SpecificAddress: ");
            string _SpecificAddress = Console.ReadLine();
            int _AddressID = addressBL.AddressIDMax() + 1;
            Address address = new Address(_AddressID, _City, _District, _Commune, _SpecificAddress);
            addressBL.InsertAddress(address);

            return _AddressID;
        }
        public void CustomerPage(int _UserID)
        {
            CustomerPage customerPage = new CustomerPage();
            Console.Clear();
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("|------------- ECOMMERCE -------------|");
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine();
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
                Console.WriteLine("5. Create shop.");
            }
            Console.WriteLine("0. Exit.");
            Console.Write("Choice: ");
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
                    Menu();
                    break;
                default:
                    Console.WriteLine("Please choose 0 - 4!");
                    CustomerPage(_UserID);
                    break;
            }
        }
        public void SellerPage(int _ShopID)
        {
            SellerPage sellerPage = new SellerPage();
            Console.Clear();
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("|------------- ECOMMERCE -------------|");
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine();
            Console.WriteLine("1. Order Management.");
            Console.WriteLine("2. Product Management.");
            Console.WriteLine("3. Product catalog management.");
            Console.WriteLine("0. Back");
            Console.Write("Choice: ");
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
                    Console.WriteLine("Please choice 0 - 3!");
                    SellerPage(_ShopID);
                    break;
            }
        }
    }
}