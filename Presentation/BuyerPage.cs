using BL;

namespace Persistence
{
    public class BuyerPage
    {
        private UserBL userBL;

        public BuyerPage()
        {
            userBL = new UserBL();
        }
        public void CustomerPage(int ID)
        {
            User user = userBL.GetUserByID(ID);
            Console.WriteLine(" --- ECOMMERCE --- ");
            Console.WriteLine();
            Console.WriteLine("Chào " + user.FullName);
            Console.WriteLine();
            Console.WriteLine("1. Đơn hàng của tôi.");
            Console.WriteLine("2. Giỏ hàng.");
            Console.WriteLine("3. Danh mục.");
            Console.WriteLine("4. Tìm kiếm sản phẩm.");
            Console.WriteLine("5. Thoát.");
            Console.Write("Chọn: ");
            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1": 
                    break;
                case "2": 
                    break;
                case "3": 
                    break; 
                case "4": 
                    break;
                case "5": 
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Vui lòng chọn 1, 2, 3, 4 hoặc 5!");
                    CustomerPage(user.UserId);
                    break;
            }
        }
        public void Cart(int ID)
        {
            User user = userBL.GetUserByID(ID);
            Console.WriteLine("1. Loại bỏ khỏi giỏ hàng.");
            Console.WriteLine("2. Thanh Toán.");
            Console.WriteLine("3. Quay lại.");
            Console.Write("Chọn: ");
            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1": 
                    break; 
                case "2": 
                    break;
                case "3":
                    CustomerPage(user.UserId);
                    break;
                default:
                    Console.WriteLine("Vui lòng chọn 1, 2 hoặc 3!");
                    Cart(user.UserId);
                    break;
            }
        }
        public void ShowCategory(int ID)
        {
            User user = userBL.GetUserByID(ID);
            Console.WriteLine("1. Xem thông tin sản phẩm.");
            Console.WriteLine("2. Quay lại.");
            Console.Write("Chọn: ");
            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1": 
                    break;
                case "2":
                    CustomerPage(user.UserId);
                    break; 
                default:
                    Console.WriteLine("Vui lòng chọn 1 hoặc 2!");
                    ShowCategory(user.UserId);
                    break;
            }
        }
        public void SearchProduct()
        {
            
        }
        public void Myorder()
        {
            
        }
    }
}