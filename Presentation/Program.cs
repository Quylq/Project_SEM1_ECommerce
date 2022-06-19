using BL;
using Persistence;

Console.OutputEncoding = System.Text.Encoding.Unicode;
Console.InputEncoding = System.Text.Encoding.Unicode;
GUI gUI = new GUI();


Console.WriteLine("1. Đăng Nhập: ");
Console.WriteLine("2. Đăng Ký: ");
Console.Write("Chọn: ");
int choice = Convert.ToInt32(Console.ReadLine());
switch (choice)
{
    case 1: 
        gUI.Login();
        break;
    case 2: 
        gUI.SigUp();
        break;
    default:
        break;
}