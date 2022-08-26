using DAL;
using Persistence;

namespace BL;
public static class GuestBL
{
    public static string title = "ECOMMERCE";
    public static User? Login()
    {
        UserDAL userDAL = new UserDAL();
        Console.Clear();
        Console.WriteLine(title);
        Console.WriteLine();
        Console.WriteLine("══════════ Login ══════════");
        Console.Write("User Name      : ");
        string _UserName = ReadHelper.ReadUserName();

        if (userDAL.CheckUserName(_UserName))
        {
            int count = 1;
            Login1:
            Console.Write("Password       : ");
            string _Password = ReadHelper.ReadPassword();
            User? user = userDAL.Login(_UserName, _Password);
            if (user != null)
            {
                Console.WriteLine($"Logged in successfully!");
                Console.ReadKey();
                return user;
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
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                    return null;
                }
            }
        }
        else
        {
            Console.WriteLine($"Account name does not exist");
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            return null;
        }
    }
    public static bool SigUp()
    {
        UserDAL userDAL = new UserDAL();
        SigUp1:
        Console.Clear();
        Console.WriteLine(title);
        Console.WriteLine();
        Console.WriteLine("══════════ SigUp ══════════");
        Console.Write("User Name      : ");
        string _UserName = ReadHelper.ReadUserName();
        if (!userDAL.CheckUserName(_UserName))
        {
            Console.Write("Password       : ");
            string _Password = ReadHelper.ReadPassword();
            Console.Write("You Name       : ");
            string _FullName = ReadHelper.ReadString(100);
            Console.Write("Email          : ");
            string _Email = ReadHelper.ReadEmail();
            Console.Write("Phone          : ");
            string _Phone = ReadHelper.ReadPhone();
            Console.Write("Birthday       : ");
            string _Birthday = ReadHelper.ReadDateOnly();
            int _AddressID = ReadHelper.ReadAddress().AddressID;
            int _UserID = userDAL.UserIDMax() + 1;
            User user = new User(_UserID, _UserName, _Password, _FullName, _Birthday, _Email, _Phone, _AddressID, "Customer");
            if (userDAL.InsertUser(user) == true)
            {
                Console.WriteLine("Sign Up Success!");
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
                return true;
            }
            else
            {
                Console.WriteLine("Registration failed!");
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
                return false;
            }
        }
        else
        {
            Console.WriteLine("Username available!");
            Console.ReadKey();
            goto SigUp1;
        }
        
    }
}