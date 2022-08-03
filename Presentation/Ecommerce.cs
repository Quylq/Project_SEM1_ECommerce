using BL;
using System.Text;

namespace Persistence
{
    public class Ecommerce
    {
        private UserBL userBL;
        private ShopBL shopBL;
        private AddressBL addressBL;
        private OrderBL orderBL;
        private ReadHelper readHelper;
        public Ecommerce()
        {
            userBL = new UserBL();
            shopBL = new ShopBL();
            addressBL = new AddressBL();
            orderBL = new OrderBL();
            readHelper = new ReadHelper();
        }
        public void Menu()
        {
            Console.Clear();
            Console.WriteLine("══════════ VTC Shop ══════════");
            Console.WriteLine("1. Login.");
            Console.WriteLine("2. SigUp.");
            Console.WriteLine("0. Exit.");
            Console.WriteLine("══════════════════════════════");
            Console.Write("Choose: ");
            string? choice = readHelper.ReadString();
            switch (choice)
            {
                case "1":
                    Login();
                    break;
                case "2":
                    SigUp();
                    break;
                case "0":
                    Console.WriteLine("You confirm you want to exit?");
                    Console.WriteLine("1. Yes       2. No");
                    Console.Write("Choose: ");
                    string? choice1 = readHelper.ReadString();
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
            Console.WriteLine("══════════ Login ══════════");
            Console.Write("User Name: ");
            string _UserName = ReadUserName();
            User user =  userBL.GetUserByName(_UserName);

            if (user.UserName != "")
            {
                int count = 1;
                Login1:
                Console.Write("Password: ");
                string? _Password = readHelper.ReadPassword();
                if (_Password == user.Password)
                {
                    Console.WriteLine($"Logged in successfully!");
                    if (user.Role == "Customer")
                    {
                        CustomerPage(user.UserID);
                    }
                    else
                    {
                        Console.WriteLine($"Update");
                    }
                }
                else
                {
                    Console.WriteLine($"wrong password!");
                    count++;
                    Console.ReadKey();
                    if (count <= 3)
                    {
                        goto Login1;
                    }
                    else
                    {
                        Console.WriteLine("You enter bad password too 3 times!");
                        Console.ReadKey();
                        Menu();
                    }
                }
            }
            else
            {
                Console.WriteLine($"Account name does not exist");
                Console.WriteLine("Press any key to go back");
                Console.ReadKey();
                Login();
            }

        }
        public void SigUp()
        {
            Console.Clear();
            Console.WriteLine("══════════ SigUp ══════════");
            Console.Write("User Name: ");
            string _UserName = ReadUserName();
            User user1 = userBL.GetUserByName(_UserName);
            if (user1.UserName == "")
            {
                Console.Write("Password: ");
                string _Password = readHelper.ReadPassword();
                Console.Write("You Name: ");
                string _FullName = readHelper.ReadString(100);
                Console.Write("Email: ");
                string _Email = readHelper.ReadEmail();
                Console.Write("Phone: ");
                string _Phone = readHelper.ReadPhone();
                Console.Write("Birthday: ");
                string _Birthday = readHelper.ReadDateOnly();
                int _AddressID = readHelper.ReadAddress().AddressID;
                int _UserID = userBL.UserIDMax() + 1;
                User user = new User(_UserID, _UserName, _Password, _FullName, _Birthday, _Email, _Phone, _AddressID, "Customer");
                userBL.InsertUser(user);
                Console.WriteLine("Sign Up Success!");
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
                CustomerPage(_UserID);
            }
            else
            {
                Console.WriteLine("Username available!");
                Console.ReadKey();
                SigUp();
            }
            
        }
        public void SigUpShop(int _UserID)
        {
            Console.Clear();
            Console.WriteLine("══════════ Register to open a store ══════════");
            Console.Write("Shop Name: ");
            string _ShopName = readHelper.ReadString(50);
            int _AddressID = readHelper.ReadAddress().AddressID;
            int _ShopID = shopBL.ShopIDMax() + 1;
            Shop shop = new Shop(_ShopID, _ShopName, _UserID, _AddressID);
            shopBL.InsertShop(shop);

            Console.WriteLine("Create a successful store.");
            Console.WriteLine("Press any key to enter the store");
            Console.ReadKey();
            SellerPage(_ShopID);

        }
        public string ReadUserName()
        {
            // string temp = "";
            // ConsoleKeyInfo info = Console.ReadKey(true);
            // while (info.Key != ConsoleKey.Enter)
            // {
            //     if (info.Key != ConsoleKey.Backspace && info.Key != ConsoleKey.Spacebar)
            //     {
            //         temp += info.KeyChar;
            //         Console.Write(info.KeyChar);
            //     }
            //     else if (temp.Length > 0 && info.Key == ConsoleKey.Backspace)
            //     {
            //         Console.Write("\b");
            //         temp = temp.Substring(0, temp.Length - 1);
            //     }
            //     info = Console.ReadKey(true);
            // }
            // Console.WriteLine();
            string _UserName = readHelper.ReadString(50);
            bool isSpace = false;
            foreach (char c in _UserName)
            {
                if (c == ' ')
                {
                    Console.WriteLine("UserName cannot contain spaces!");
                    isSpace = true;
                    break;
                }
            }
            if (isSpace)
            {
                return ReadUserName();
            }
            else
            {
                return _UserName;
            }
        }
        public void CustomerPage(int _UserID)
        {
            CustomerPage customerPage = new CustomerPage();
            int ProductNumber = orderBL.GetProductNumberOfCart(_UserID);
            Console.Clear();
            Console.WriteLine($"══════════ {userBL.GetUserByID(_UserID).FullName} ══════════");
            Console.WriteLine("1. Search Product.");
            Console.WriteLine("2. Search Shop.");
            Console.WriteLine($"3. Cart ({ProductNumber} Product).");
            Console.WriteLine("4. My Order.");
            Shop shop = shopBL.GetShopByUserID(_UserID);
            if (shop.UserID == _UserID)
            {
                Console.WriteLine("5. My Shop.");  
            }      
            else
            {
                Console.WriteLine("5. Sales registration."); 
            }
            Console.WriteLine("6. Personal information.");
            Console.WriteLine("0. Log out.");
            for (int i = 0; i < userBL.GetUserByID(_UserID).FullName.Length; i++)
            {
                Console.Write("═");
            }
            Console.WriteLine($"══════════════════════");
            Console.Write("Choose: ");
            string? choice = readHelper.ReadString();
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
                case "6": 
                    customerPage.PersonalInformation(_UserID);
                    break;
                case "0": 
                    Menu();
                    break;
                default:
                    Console.WriteLine("Please select 0 - 4!");
                    CustomerPage(_UserID);
                    break;
            }
        }
        public void SellerPage (int _ShopID)
        {
            SellerPage sellerPage = new SellerPage();
            Console.Clear();
            Console.WriteLine($"══════════ {shopBL.GetShopByID(_ShopID).ShopName} ══════════");
            Console.WriteLine("1. Order Manegement.");
            Console.WriteLine("2. Product Manegement.");
            Console.WriteLine("3. Category Manegement");
            Console.WriteLine("0. Back");
            for (int i = 0; i < shopBL.GetShopByID(_ShopID).ShopName.Length; i++)
            {
                Console.Write("═");
            }
            Console.WriteLine($"══════════════════════");
            Console.Write("Choose: ");
            string? choice = readHelper.ReadString();
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
                    Console.WriteLine("Please select 0 - 3!");
                    SellerPage (_ShopID);
                    break;
            }
        }
    }
}