using BL;
using Persistence;

namespace Persistence
{
    public class GUI
    {
        private AccountBL accountBL;

        public GUI()
        {
            accountBL = new AccountBL();
        }
        public void Login()
        {
            Console.Write("Nhập Tên Đăng Nhập: ");
            string acc_name = Console.ReadLine();
            Console.Write("Nhập Mật Khẩu: ");
            string acc_pass = Console.ReadLine();
            Account account =  accountBL.GetAccountByName(acc_name);

            if (account != null)
            {
                if (acc_pass == account.AccountPassword)
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
        {
            string acc_role = "";
            Console.WriteLine("Bạn muốn ?");
            Console.WriteLine("1. Mua hàng.");
            Console.WriteLine("2. Bán hàng.");
            Console.Write("Chọn: ");
            int choice = Convert.ToInt32(Console.ReadLine());
            switch (choice)
            {
                case 1: 
                    acc_role = "Customer";
                    break;
                case 2: 
                    acc_role = "Seller";
                    break;
                default:
                    break;
            }
            string acc_name;
            Account account1 = new Account();
            do
            {
                Console.Write("Nhập Tên Đăng Nhập: ");
                acc_name = Console.ReadLine();
                account1 = accountBL.GetAccountByName(acc_name);
                Console.WriteLine($"{account1.AccountName} {account1.AccountPassword} {account1.Role}");
                if (account1.AccountName != null)
                {
                    Console.WriteLine("Tên Đăng Nhập đã tồn tại, vui lòng chọn tên khác! ");
                }
            } while (account1.AccountName != null);
            
            Console.Write("Nhập Mật Khẩu: ");
            string acc_pass = Console.ReadLine();
            Account account = new Account(acc_name, acc_pass, acc_role);
            accountBL.SaveAccount(account);
            account.AccountId = accountBL.GetAccountByName(acc_name).AccountId;

            Console.Write("Tên của bạn: ");
            string name = Console.ReadLine();
            Console.Write("Email: ");
            string email = Console.ReadLine();
            Console.Write("Ngày sinh: ");
            DateTime birthday = Convert.ToDateTime(Console.ReadLine());
            Console.Write("Số điện thoại: ");
            string phone = Console.ReadLine();
            Console.Write("Địa chỉ: ");
            string address = Console.ReadLine();

            User user = new User(name, email, birthday, phone, address);
            accountBL.SaveUser(user, account);
        }
    }
}