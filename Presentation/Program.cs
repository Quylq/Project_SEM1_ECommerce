using BL;
using Persistence;

Console.OutputEncoding = System.Text.Encoding.Unicode;
Console.InputEncoding = System.Text.Encoding.Unicode;

static void Page()
{
    GUI gUI = new GUI();
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
                gUI.Login();
                break;
            case 2:
                gUI.SigUp();
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
                            Page();
                            break;
                        case 2:
                            Console.WriteLine(" You Are Exit");
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("Vui lòng chọn 1 hoặc 2!");
                            Page();
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
                Page();
                break;
        }
    }
    catch (System.Exception)
    {
        Console.WriteLine("Vui lòng chọn 1, 2 hoặc 3!");
        Page();
        throw;
    }
}