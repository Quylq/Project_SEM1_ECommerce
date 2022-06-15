using BL;
using Persistence;

Console.OutputEncoding = System.Text.Encoding.Unicode;
Console.InputEncoding = System.Text.Encoding.Unicode;

Console.WriteLine("1. Đăng Nhập: ");
Console.WriteLine("2. Đăng Ký: ");
Console.Write("Chọn: ");
int choice = Convert.ToInt32(Console.ReadLine());
switch (choice)
{
    case 1: 
        Login();
        break;
    case 2: 
        SigUp();
        break;
    default:
        break;
}


static void Login()
{
    Console.Write("Nhập Tên Đăng Nhập: ");
    string acc_name = Console.ReadLine();
    Console.Write("Nhập Mật Khẩu: ");
    string acc_pass = Console.ReadLine();

    AccountBL accountBL = new AccountBL();
    Account acc =  accountBL.GetAccountByName(acc_name);

    if (acc != null)
    {
        if (acc_pass == acc.AccountPassword)
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

static void SigUp()
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
    Console.Write("Nhập Tên Đăng Nhập: ");
    string acc_name = Console.ReadLine();
    Console.Write("Nhập Mật Khẩu: ");
    string acc_pass = Console.ReadLine();

    AccountBL accountBL = new AccountBL();
    accountBL.SaveAccount(acc_name, acc_pass, acc_role);
}