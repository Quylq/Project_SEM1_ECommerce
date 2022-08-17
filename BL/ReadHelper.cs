using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using DAL;
using Persistence;

namespace BL
{
    public static class ReadHelper
    {
        public static string ReadPassword()
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
            if (temp != "")
            {
                return Sha256Hash(temp);
            }
            else
            {
                Console.WriteLine("Password can not be blank");
                return ReadPassword();
            }
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
        public static bool IsNumber(string? value)
        {
            if (value != null && value != "")
            {
                for (int i = 0; i < value.Length; i++)
                {
                    if (i == 0)
                    {
                        if (!Char.IsDigit(value[i]) && (value[i] != '-' || value.Length <= 1))
                        return false;
                    }
                    else
                    {
                        if (!Char.IsDigit(value[i]))
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
        public static int ReadInt()
        {
            string? value = ReadString();
            if (IsNumber(value))
            {
                try
                {
                    return Convert.ToInt32(value);
                }
                catch (System.OverflowException)
                {
                    Console.WriteLine("Value is too small or too large, Please Retype.");
                    return ReadInt();
                }
            }
            else
            {
                Console.WriteLine("Invalid number, Please Retype.");
                return ReadInt();
            }
        }
        public static int ReadInt(int min, int max)
        {
            string? value = ReadString();
            if (IsNumber(value))
            {
                try
                {
                    int result = Convert.ToInt32(value);
                    if (result >= min && result <= max)
                    {
                        return result;
                    }
                    else
                    {
                        Console.WriteLine($"numbers outside the range [{min}, {max}].");
                        return ReadInt(min, max);
                    }
                }
                catch (System.OverflowException)
                {
                    Console.WriteLine("Value is too small or too large, Please Retype.");
                    return ReadInt(min, max);
                }
            }
            else
            {
                Console.WriteLine("Invalid number, Please Retype.");
                return ReadInt(min, max);
            }
        }
        public static string ReadString()
        {
            string? value = Console.ReadLine();
            if (value != null && value != "")
            {
                return value;
            }
            else
            {
                Console.WriteLine("Not empty, Please Retype.");
                return ReadString();
            }
        }
        public static string ReadString(int lengthMax)
        {
            string value = ReadString();
            if (value.Length <= lengthMax)
            {
                return value;
            }
            else
            {
                Console.WriteLine($"The number of characters cannot exceed {lengthMax}.");
                return ReadString(lengthMax);
            }
        }
        public static string ReadString(int lengthMin, int lengthMax)
        {
            string value = ReadString();
            if (value.Length >= lengthMin && value.Length <= lengthMax)
            {
                return value;
            }
            else if (value!.Length < lengthMin)
            {
                Console.WriteLine($"Character number is not less than {lengthMin}.");
                return ReadString(lengthMin, lengthMax);
            }
            else if (value.Length > lengthMax)
            {
                Console.WriteLine($"The number of characters cannot exceed {lengthMax}.");
                return ReadString(lengthMin, lengthMax);
            }
            else
            {
                return ReadString(lengthMin, lengthMax);
            }
        }
        public static string ReadDateOnly()
        {
            try
            {       
                string format = "yyyy-MM-dd";    
                string value = ReadString();
                string[] result = value.Replace(" ", "").Split('/', StringSplitOptions.None);
                DateOnly dateOnly = new DateOnly(Convert.ToInt32(result[2]), Convert.ToInt32(result[1]), Convert.ToInt32(result[0]));
                return dateOnly.ToString(format);
            }
            catch (System.Exception)
            {
                Console.WriteLine($"Invalid Date, Please Retype.");
                return ReadDateOnly();
            }
        }
        public static string ReadPhone()
        {
            string _Phone = ReadString();
            Regex isValidInput = new Regex(@"^\d{3,11}$");
            if(!isValidInput.IsMatch(_Phone))
            {
                Console.WriteLine($"Invalid Phone, Please Retype.");
                return ReadPhone();
            }
            return _Phone;
        }
        public static string ReadEmail()
        {
            string _Email = ReadString(100);
            try
            {
                MailAddress m = new MailAddress(_Email);
                return _Email;
            }
            catch (FormatException)
            {
                Console.WriteLine($"Invalid Email, Please Retype.");
                return ReadEmail();
            }
        }
        public static Address ReadAddress()
        {
            AddressDAL addressDAL = new AddressDAL();
            Console.WriteLine("══════════ Address ══════════");
            Console.Write("City: ");
            string _City = ReadString(30);
            Console.Write("District: ");
            string _District = ReadString(30);
            Console.Write("Commune: ");
            string _Commune = ReadString(30);
            Console.Write("SpecificAddress: ");
            string _SpecificAddress = ReadString(110);
            int _AddressID = addressDAL.AddressIDMax() + 1;
            Address address =  new Address(_AddressID, _City, _District, _Commune, _SpecificAddress);
            addressDAL.InsertAddress(address);

            return address;
        }
        public static string ReadUserName()
        {
            string _UserName = ReadString(50);
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
    }
}