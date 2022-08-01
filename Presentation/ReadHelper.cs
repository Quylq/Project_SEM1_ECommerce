using BL;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace Persistence
{
    public class ReadHelper
    {
        public bool IsNumber(string? value)
        {
            if (value != null && value != "")
            {
                for (int i = 0; i < value.Length; i++)
                {
                    if (i == 0)
                    {
                        if (!Char.IsDigit(value[i]) && value[i] != '-')
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
        public int ReadInt()
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
        public int ReadInt(int min, int max)
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
        public string ReadString()
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
        public string ReadString(int length)
        {
            string? value = Console.ReadLine();
            if (value != null && value != "" && value.Length <= length)
            {
                return value;
            }
            else if (value != "")
            {
                Console.WriteLine($"The number of characters cannot exceed {length}.");
                return ReadString();
            }
            else
            {
                Console.WriteLine("Not empty, Please Retype.");
                return ReadString();
            }
        }
        public string ReadDateOnly()
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
        public string ReadPhone()
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
        public string ReadEmail()
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
    }
}