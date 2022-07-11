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
        private CategoryBL categoryBL;
        Ecommerce ecommerce = new Ecommerce();
        public SellerPage()
        {
            userBL = new UserBL();
            productBL = new ProductBL();
            orderBL = new OrderBL();
            categoryBL = new CategoryBL();
        }
        public void Seller (User user)
        {
            
            Console.Clear();
            Console.WriteLine("1. Quản lý đơn đặt hàng.");
            Console.WriteLine("2. Quản lý sản phẩm.");
            Console.WriteLine("3. Quản lý danh mục sản phẩm.");
            Console.WriteLine("0. Thoát");
            Console.Write("Chọn: ");
            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    OrderManagement(user);
                    break;
                case "2": 
                    ProductManagement(user);
                    break;
                case "3": 
                    CategoryManagement(user);
                    break;
                case "0": 
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Vui lòng chọn 0 - 3 !");
                    Seller (user);
                    break;
            }
        }
        public void OrderManagement(User user)
        {
            Console.Clear();
            Console.WriteLine("1. Chờ xác nhận.");
            Console.WriteLine("2. Đang giao.");
            Console.WriteLine("3. Hoàn thành.");
            Console.WriteLine("4. Thất bại.");
            Console.WriteLine("0. Quay lại.");
            Console.Write("Chọn: ");
            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1": 
                    ViewOrdersProcessing(user);
                    break; 
                case "2":
                    ViewOrders("Shipping", user);
                    Console.WriteLine("Nhấn phím bất kỳ để tiếp tục !");
                    Console.ReadKey();
                    OrderManagement(user);
                    break;
                case "3":
                    ViewOrders("Finished", user);
                    Console.WriteLine("Nhấn phím bất kỳ để tiếp tục !");
                    Console.ReadKey();
                    OrderManagement(user);
                    break;
                case "4":
                    ViewOrders("Failed", user);
                    Console.WriteLine("Nhấn phím bất kỳ để tiếp tục !");
                    Console.ReadKey();
                    OrderManagement(user);
                    break;
                case "0":
                    Seller(user);
                    break;
                default:
                    Console.WriteLine("Vui lòng chọn 0 - 3 !");
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
                    List<Product> products = productBL.GetProductsByUser(user);
                    ViewProducts(user, products);
                    break;
                case "3": 
                    MoreProduct(user);
                    break;
                case "0": 
                    Seller(user);
                    break;
                default:
                    Console.WriteLine("Vui lòng chọn 0 - 3 !");
                    ProductManagement(user);
                    break;
            }
        }
        public void CategoryManagement(User user)
        {
            Console.Clear();
            Console.WriteLine("1. Xem sản phẩm theo danh mục.");
            Console.WriteLine("2. Tạo danh mục mới.");
            Console.WriteLine("3. Xóa danh mục.");
            Console.WriteLine("0. Quay lại.");
            Console.Write("Chọn: ");
            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1": 
                    ViewProductsOfCategory(user);
                    break; 
                case "2": 
                    categoryBL.CreateCategory(user);
                    break;
                case "3":
                    DeleteCategory(user);
                    break;
                case "0": 
                    Seller(user);
                    break; 
                default:
                    Console.WriteLine("Vui lòng chọn 0 - 1 !");
                    CategoryManagement(user);
                    break;
            }
        }
    
        public void SearchProductOfShop(User user)
        {
            Console.Clear();
            Console.WriteLine("Nhập sản phẩm bạn muốn tìm: ");
            string? _ProductName = Console.ReadLine();
            List<Product> products = new List<Product>();
            products = productBL.GetProductsByNameAndUser(_ProductName, user);
            Console.Clear();
            ViewProducts(user, products);

        }
        public void DisplayProducts(List<Product> products)
        {
            Console.Clear();
            Console.WriteLine("|---------------------------------------------------------------------------------------|");
            Console.WriteLine("| STT | Tên sản phẩm                                       |       Giá       | Số lượng |");
            Console.WriteLine("|---------------------------------------------------------------------------------------|");
            int count = 1;
            foreach (Product product in products)  
            { 
                Console.WriteLine("| {0,3 } | {1,-50} | {2, 15} | {3,8} |", count++, product.ProductName, product.Price.ToString("C0"), product.Quantity);
                
            }
            Console.WriteLine("|---------------------------------------------------------------------------------------|");
        }
        public void ViewProducts(User user, List<Product> products)
        {
            
            DisplayProducts(products);
            Console.Write("Nhập số thứ tự để xem thông tin chi tiết hoặc \"0\" để quay lại: ");
            try
            {
                int choice = Convert.ToInt32(Console.ReadLine());
                if (choice != 0)
                {
                    ProductInformation(user, products[choice - 1]);
                }
                else
                {
                    ProductManagement(user);
                }
            }
            catch (System.Exception)
            {
                Console.Clear();
                ViewProducts(user, products);
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
            Console.WriteLine("3. Thêm sản phẩm vào danh mục.");
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
                case "3":
                    List<Category> categories = new List<Category>();
                    categories = categoryBL.GetCategoriesByUser(user);
                    AddCategories(product, user);
                    Console.WriteLine("Nhấn phím bất kỳ để tiếp tục !");
                    Console.ReadKey();
                    ProductManagement(user);
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
        
        public void MoreProduct(User user)
        {
            Console.Clear();
            Console.WriteLine("Tên sản phẩm: ");
            string? _ProductName = Console.ReadLine();
            Console.WriteLine("Giá sản phẩm: ");
            int _Price = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Số lượng sản phẩm: ");
            int _Quantity = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Mô tả: ");
            string? _Description = Console.ReadLine();
            int _ProductID = productBL.ProductIDMax() + 1;
            Product product = new Product(_ProductID, _ProductName, _Price, _Description, _Quantity);
            // Lưu sản phẩm
            productBL.SaveProduct(product);
            // Lưu Users_product
            productBL.SaveUsers_Product(user, product);
            List<Category> categories = categoryBL.GetCategoriesByUser(user);
            // Lưu category
            AddCategories(product, user);
            Console.WriteLine("Thêm sản phẩm thành công !");
            Console.WriteLine("Nhấn phím bất kỳ để tiếp tục !");
            Console.ReadKey();
            ProductManagement(user);
        }
        public void DisplayOrders(List<User> customers, List<Order> orders)
        {
            Console.WriteLine("|----------------------------------------------------------------------------------------------------------------------|");
            Console.WriteLine("| STT |           Khách Hàng           |    Thời gian đặt hàng    |                     Địa chỉ                        |");
            Console.WriteLine("|----------------------------------------------------------------------------------------------------------------------|");
            int count = 1;
            for (int i = 0; i < customers.Count; i++)
            {
                Console.WriteLine("| {0,3 } | {1,-30} | {2, 24} | {3, -50} |", count++, customers[i].FullName, orders[i].CreateDate, customers[i].Address);
            }
            Console.WriteLine("|----------------------------------------------------------------------------------------------------------------------|");
        }
        public void ViewOrders(string status, User seller)
        {  
            List<User> customers = new List<User>();
            List<Order> orders = new List<Order>();
            customers = orderBL.GetUsersByStatusOfSeller(status, seller);
            orders = orderBL.GetOrdersByStatusOfSeller(status, seller);
            Console.Clear();
            DisplayOrders(customers, orders);
            Console.Write("Nhập số thứ tự để xem thông tin đơn hàng hoặc \"0\" để quay lại: ");
            try
            {
                int choice = Convert.ToInt32(Console.ReadLine());
                if (choice != 0)
                {
                    OrderDetails(customers[choice - 1], orders[choice - 1]);
                }
                else
                {
                    OrderManagement(seller);
                }
            }
            catch (System.Exception)
            {
                Console.Clear();
                ViewOrders(status, seller);
            }
            
        }
        public void OrderDetails(User customer, Order order)
        {
            Console.Clear();
            Console.WriteLine($"Khách hàng: {customer.FullName}");
            Console.WriteLine($"Đia chỉ: {customer.Address}");
            Console.WriteLine($"Số điện thoại: {customer.Phone}");

            List<Product> products = orderBL.GetOrderDetails(order);
            Console.WriteLine("|--------------------------------------------------------------------------------------------|");
            Console.WriteLine("| STT | Tên sản phẩm                        |       Giá       | Số lượng |    Thành Tiền     |");
            Console.WriteLine("|--------------------------------------------------------------------------------------------|");
            int count = 1;
            int total = 0;
            foreach (Product product in products)  
            { 
                Console.WriteLine("| {0,3 } | {1,-35} | {2, 15} | {3,8} | {4, 17} |", count++, product.ProductName, product.Price.ToString("C0"), product.Quantity, (product.Price * product.Quantity).ToString("C0"));
                total += product.Price * product.Quantity;
            }
            Console.WriteLine("|--------------------------------------------------------------------------------------------|");
            Console.WriteLine("Tổng Tiền:  {0, 17} ", total.ToString("C0"));
        }
        
        public void ViewProductsOfCategory(User user)
        {
            List<Category> categories = categoryBL.GetCategoriesByUser(user);
            categoryBL.DisplayCategories(user);
            Console.Write("Nhập số thứ tự để xem sản phẩm hoặc \"0\" để quay lại: ");
            try
            {
                int choice = Convert.ToInt32(Console.ReadLine());
                if (choice != 0)
                {
                    List<Product> products = productBL.GetProductsByCategory(categories[choice - 1]);
                    ViewProducts(user,products);
                }
                else
                {
                    CategoryManagement(user);
                }
            }
            catch (System.Exception)
            {
                Console.Clear();
                ViewProductsOfCategory(user);
            }
        }
        public void AddCategories(Product product, User user)
        {
            List<Category> categories = categoryBL.GetCategoriesByUser(user);
            categoryBL.DisplayCategories(categories);
            Console.Write("Nhập số thứ tự để thêm sản phẩm vào danh mục có sẵn hoặc 0 để tạo danh mục mới : ");
            try
            {
                ProductBL productBL = new ProductBL();
                int choice = Convert.ToInt32(Console.ReadLine());
                if (choice != 0)
                {
                    categoryBL.SaveProduct_Categories(product, categories[choice - 1]);
                    Console.WriteLine("Thêm sản phẩm vào danh mục thành công !");   
                }
                else
                {
                    categoryBL.CreateCategory(user);
                    AddCategories(product, user);
                }
            }
            catch (System.Exception)
            {
                Console.Clear();
                AddCategories(product, user);
            }
        }
        public void DeleteCategory(User user)
        {
            List<Category> categories = categoryBL.GetCategoriesByUser(user);
            categoryBL.DisplayCategories(categories);
            Console.Write("Nhập số thứ tự tương ứng để xóa danh mục 0 để quay lại: ");
            try
            {
                int choice = Convert.ToInt32(Console.ReadLine());
                if (choice != 0)
                {
                    categoryBL.DeleteCategory(categories[choice - 1]);
                    Console.WriteLine("Xóa danh mục thành công !");
                    Console.WriteLine("Nhấn phím bất kỳ để tiếp tục !");
                    Console.ReadKey();
                    DeleteCategory(user);
                }
                else
                {
                    CategoryManagement(user);
                }
            }
            catch (System.Exception)
            {
                Console.Clear();
                DeleteCategory(user);
            }
        }
        public void ViewOrdersProcessing(User user)
        {
            ViewOrders("Processing", user);
            Console.WriteLine("1. Xác nhận đơn hàng. ");
            Console.WriteLine("2. Từ chối đơn hàng. ");
            Console.WriteLine("0. Quay lại.");
            Console.Write("Chọn: ");
            try
            {
                int choice = Convert.ToInt32(Console.ReadLine());
                if (choice == 0)
                {
                    ViewOrdersProcessing(user);
                }
                else if (choice == 1)
                {
                    List<Order> orders = orderBL.GetOrdersByStatusOfSeller("Processing", user);
                    orderBL.UpdateStatus(orders[choice - 1], "Shipping");
                    Console.WriteLine("Nhập phím bất kỳ để tiếp tục");
                    Console.ReadKey();
                    ViewOrdersProcessing(user);
                }
                else if (choice == 2)
                {
                    List<Order> orders = orderBL.GetOrdersByStatusOfSeller("Processing", user);
                    orderBL.UpdateStatus(orders[choice - 1], "Failed");
                    Console.WriteLine("Nhập phím bất kỳ để tiếp tục");
                    Console.ReadKey();
                    ViewOrdersProcessing(user);
                }
            }
            catch (System.Exception)
            {
                Console.Clear();
                ViewOrdersProcessing(user);
            }
        }

    }
    
}