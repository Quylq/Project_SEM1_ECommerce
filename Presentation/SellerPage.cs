using BL;
using System.Security.Cryptography;
using System.Text;

namespace Persistence
{
    public class SellerPage
    {
        private UserBL userBL;
        private ProductBL productBL;
        private OrderBL orderBL;
        Ecommerce ecommerce = new Ecommerce();
        public SellerPage()
        {
            userBL = new UserBL();
            productBL = new ProductBL();
            orderBL = new OrderBL();
        }
        
        public void OrderManagement(User user)
        {
            Console.Clear();
            Console.WriteLine("1. Chờ xác nhận.");
            Console.WriteLine("2. Đã xác nhận.");
            Console.WriteLine("3. Từ chối.");
            Console.WriteLine("0. Quay lại.");
            Console.Write("Chọn: ");
            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1": 
                    orderBL.GetOrderByStatus("Processing", user);
                    break; 
                case "2": 
                    break;
                case "0":
                    ecommerce.SellerPage(user);
                    break;
                default:
                    Console.WriteLine("Vui lòng chọn 0 - 2 !");
                    OrderManagement(user);
                    break;
            }
        }
        public void ProductManagement(User user)
        {
            Console.Clear();
            Console.WriteLine("1. Tìm kiếm sản phẩm.");
            Console.WriteLine("2. Hiển thị tất cả sản phẩm.");
            Console.WriteLine("3. Thêm sản phẩm.");
            Console.WriteLine("0. Quay lại.");
            Console.Write("Chọn: ");
            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1": 
                    SearchProductOfShop(user);
                    break; 
                case "2": 
                    List<Product> productList = productBL.GetProductListByName(user);
                    DisplayProductList(user, productList);
                    break;
                case "3": 
                    
                    break;
                case "0": 
                    ecommerce.SellerPage(user);
                    break;
                default:
                    Console.WriteLine("Vui lòng chọn 0 - 3 !");
                    ProductManagement(user);
                    break;
            }
        }
        public void CategoryManagement()
        {
            Console.Clear();
            Console.WriteLine("1. Xem danh mục.");
            Console.WriteLine("0. Quay lại.");
            Console.Write("Chọn: ");
            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1": 
                    break; 
                case "0": 
                    // SellerPage();
                    break; 
                default:
                    Console.WriteLine("Vui lòng chọn 0 - 1 !");
                    CategoryManagement();
                    break;
            }
        }
    
        public void SearchProductOfShop(User user)
        {
            Console.Clear();
            Console.WriteLine("Nhập sản phẩm bạn muốn tìm: ");
            string? _ProductName = Console.ReadLine();
            List<Product> productList = new List<Product>();
            productList = productBL.GetProductListByName(_ProductName, user);
            Console.Clear();
            DisplayProductList(user, productList);

        }

        public void DisplayProductList(User user, List<Product> productList)
        {
            Console.Clear();
            Console.WriteLine("|----------------------------------------------------------------------------|----------|");
            Console.WriteLine("| STT | Tên sản phẩm                                       |       Giá       | Số lượng |");
            Console.WriteLine("|----------------------------------------------------------------------------|----------|");
            int count = 1;
            foreach (Product product in productList) 
            { 
                Console.WriteLine("| {0,3 } | {1,-50} | {2, 15} | {3,8} |", count++, product.ProductName, product.Price.ToString("C0"), product.Quantity);
                
            }
            Console.WriteLine("|----------------------------------------------------------------------------|----------|");
            
            Console.Write("Nhập số thứ tự để xem thông tin chi tiết hoặc \"0\" để quay lại: ");
            try
            {
                int choice = Convert.ToInt32(Console.ReadLine());
                if (choice != 0)
                {
                    ProductInformation(user, productList[choice - 1]);
                }
                else
                {
                    ProductManagement(user);
                }
            }
            catch (System.Exception)
            {
                Console.Clear();
                DisplayProductList(user, productList);
            }
            
        }
        
        public void ProductInformation(User user, Product product)
        {
            Console.Clear();
            Console.WriteLine($"Tên sản Phẩm: {product.ProductName}");
            Console.WriteLine($"Giá: {product.Price.ToString("C0")}");
            Console.WriteLine($"Mô tả: {product.Description}");
            Console.WriteLine($"Số lượng: {product.Quantity}");
            Console.WriteLine($"-----------------------------------------");
            ProductInformation:
            Console.WriteLine("1. Cập nhật mô tả sản phẩm.");
            Console.WriteLine("2. Cập nhật số lượng sản phẩm.");
            Console.WriteLine("0. Quay lại.");
            Console.Write("Chọn: ");
            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1": 
                    UpdateDescription(user, product);
                    break;
                case "2":
                    UpdateQuantity(user, product);
                    break; 
                case "0":
                    ProductManagement(user);
                    break;
                default:
                    Console.WriteLine("Vui lòng chọn 0 - 2 !");
                    goto ProductInformation;
            }
        }

        public void UpdateDescription(User user, Product product)
        {
            Console.Clear();
            Console.Write("Mô tả mới: ");
            string? _Description = Console.ReadLine();
            productBL.UpdateDescription(product, _Description);
            product.Description = _Description;
            Console.WriteLine("Cập nhật thành công!");
            Console.WriteLine("Nhập phím bất kỳ để tiếp tục");
            Console.ReadKey();
            ProductInformation(user, product);
        }

        public void UpdateQuantity(User user, Product product)
        {
            Console.Clear();
            Console.Write("Số lượng sản phẩm còn: ");
            int _Quantity = Convert.ToInt32(Console.ReadLine());
            productBL.UpdateQuantity(product, _Quantity);
            product.Quantity = _Quantity;
            Console.WriteLine("Cập nhật thành công!");
            Console.WriteLine("Nhập phím bất kỳ để tiếp tục");
            Console.ReadKey();
            ProductInformation(user, product);
        }
        
        public void DisplayOrderList(User user, List<Order> orderList)
        {
            Console.Clear();
            Console.WriteLine("|----------------------------------------------------------------------------|----------|");
            Console.WriteLine("| STT |   Khách Hàng     | Thời gian đặt hàng |              Địa chỉ         |  Tổng tiền  |");
            Console.WriteLine("|----------------------------------------------------------------------------|----------|");
            int count = 1;
            foreach (Order order in orderList) 
            { 
                Console.WriteLine("| {0,3 } | {1,-50} | {2, 15} | {3,8} |", count++, order.CustomerID, order.CreateDate, order.Address);
                
            }
            Console.WriteLine("|----------------------------------------------------------------------------|----------|");
            
            Console.Write("Nhập số thứ tự để xem thông tin sản phẩm hoặc \"0\" để quay lại: ");
            try
            {
                int choice = Convert.ToInt32(Console.ReadLine());
                if (choice != 0)
                {
                    OrderInformation(user, orderList[choice - 1]);
                }
                else
                {
                    OrderManagement(user);
                }
            }
            catch (System.Exception)
            {
                Console.Clear();
                DisplayOrderList(user, orderList);
            }
            
        }
        public void OrderInformation(User user, Order order)
        {

        }
    }
}