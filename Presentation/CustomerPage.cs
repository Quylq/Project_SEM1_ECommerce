using BL;
using System.Security.Cryptography;
using System.Text;

namespace Persistence
{
    public class Customer
    {
        private UserBL userBL;
        private ProductBL productBL;
        private OrderBL orderBL;
        Ecommerce ecommerce = new Ecommerce();
        public Customer()
        {
            userBL = new UserBL();
            productBL = new ProductBL();
            orderBL = new OrderBL();
        }
        
        public void Cart(User user)
        {
            Console.Clear();
            Console.WriteLine("1. Loại bỏ khỏi giỏ hàng.");
            Console.WriteLine("2. Thanh Toán.");
            Console.WriteLine("0. Quay lại.");
            Console.Write("Chọn: ");
            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1": 
                    break; 
                case "2": 
                    break;
                case "0":
                    ecommerce.CustomerPage(user);
                    break;
                default:
                    Console.WriteLine("Vui lòng chọn 0 - 2!");
                    Cart(user);
                    break;
            }
        }
        public void ShowCategory(User user)
        {
            Console.Clear();
            Console.WriteLine("1. Xem thông tin sản phẩm.");
            Console.WriteLine("2. Quay lại.");
            Console.Write("Chọn: ");
            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1": 
                    break;
                case "2":
                    ecommerce.CustomerPage(user);
                    break; 
                default:
                    Console.WriteLine("Vui lòng chọn 1 hoặc 2!");
                    ShowCategory(user);
                    break;
            }
        }
        
        public void SearchProduct(User user)
        {
            Console.Clear();
            Console.WriteLine("Nhập sản phẩm bạn muốn tìm: ");
            string? _ProductName = Console.ReadLine();
            List<Product> products = new List<Product>();
            products = productBL.GetProductsByName(_ProductName);
            Console.Clear();
            
        }
    }
}