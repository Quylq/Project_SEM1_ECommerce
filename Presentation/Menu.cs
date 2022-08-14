using BL;
using Persistence;

public static class Menu
{
    public static void MainMenu()
    {
        MainMenu1:
        Console.Clear();
        Console.WriteLine("══════════ VTC Shop ══════════");
        Console.WriteLine("1. Login.");
        Console.WriteLine("2. SigUp.");
        Console.WriteLine("0. Exit.");
        Console.WriteLine("══════════════════════════════");
        Console.Write("Choose: ");
        int choice = ReadHelper.ReadInt(0, 2);
        switch (choice)
        {
            case 1:
                User? user = GuestBL.Login();
                if (user != null)
                {
                    user.CustomerMenu();
                }
                else
                {
                    goto MainMenu1;
                }
                break;
            case 2:
                User? user1 = GuestBL.SigUp();
                if (user1 != null)
                {
                    user1.CustomerMenu();
                }
                else
                {
                    goto MainMenu1;
                }
                break;
            case 0:
                Console.WriteLine("You confirm you want to exit?");
                Console.WriteLine("1. Yes       2. No");
                Console.Write("Choose: ");
                int choice1 = ReadHelper.ReadInt(1, 2);
                switch (choice1)
                {
                    case 1:
                        Console.WriteLine(" You Are Exit");
                        Environment.Exit(0);
                        break;
                    case 2:
                        MainMenu();
                        break;
                }
                break;
        }
    }
    public static void CustomerMenu(this User user)
    {
        BLHelper bLHelper = new BLHelper();
        int ProductNumber = bLHelper.GetProductNumberOfCart(user.UserID);
        Shop? shop = bLHelper.GetShopByUserID(user.UserID);
        Console.Clear();
        Console.WriteLine($"══════════ {user.FullName} ══════════");
        Console.WriteLine("1. Search Product.");
        Console.WriteLine("2. Search Shop.");
        Console.WriteLine($"3. Cart ({ProductNumber} Product).");
        Console.WriteLine("4. My Order.");

        if (shop != null)
        {
            Console.WriteLine("5. My Shop.");  
        }      
        else
        {
            Console.WriteLine("5. Sales registration."); 
        }
        Console.WriteLine("6. Personal information.");
        Console.WriteLine("0. Log out.");
        for (int i = 0; i < user.FullName.Length; i++)
        {
            Console.Write("═");
        }
        Console.WriteLine($"══════════════════════");
        Console.Write("Choose: ");
        int choice = ReadHelper.ReadInt(0, 6);
        switch (choice)
        {
            case 1:
                user.SearchProduct();
                user.CustomerMenu();
                break;
            case 2:
                user.SearchShop();
                user.CustomerMenu();
                break;
            case 3:
                user.ViewCart();
                user.CustomerMenu();
                break;
            case 4: 
                user.MyOrder();
                user.CustomerMenu();
                break; 
            case 5: 
                if (shop != null)
                {
                    shop.ShopMenu();
                    user.CustomerMenu();
                }
                else
                {
                    user.SigUpShop();
                    user.CustomerMenu();
                }
                break;
            case 6: 
                user.PersonalInformation();
                user.CustomerMenu();
                break;
            case 0: 
                MainMenu();
                break;
        }
    }
    public static void ShopMenu(this Shop shop)
    {
        BLHelper bLHelper = new BLHelper();

        Console.Clear();
        Console.WriteLine($"══════════ {shop.ShopName} ══════════");
        Console.WriteLine("1. Order Manegement.");
        Console.WriteLine("2. Product Manegement.");
        Console.WriteLine("3. Category Manegement");
        Console.WriteLine("0. Back");
        for (int i = 0; i < shop.ShopName.Length; i++)
        {
            Console.Write("═");
        }
        Console.WriteLine($"══════════════════════");
        Console.Write("Choose: ");
        int choice = ReadHelper.ReadInt(0, 3);
        switch (choice)
        {
            case 1:
                shop.OrderManagement();
                shop.ShopMenu();
                break;
            case 2: 
                shop.ProductManagement();
                shop.ShopMenu();
                break;
            case 3: 
                shop.CategoryManagement();
                shop.ShopMenu();
                break;
            case 0: 
                break;
        }
    }
}
