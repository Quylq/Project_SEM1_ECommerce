using BL;
using System.Security.Cryptography;
using System.Text;

namespace Persistence
{
    public class CustomerPage
    {
        private UserBL userBL;
        private ProductBL productBL;
        private OrderBL orderBL;
        private CategoryBL categoryBL;
        Ecommerce ecommerce = new Ecommerce();
        JsonUtil jsonUtil ;
        public CustomerPage()
        {
            userBL = new UserBL();
            productBL = new ProductBL();
            orderBL = new OrderBL();
            categoryBL = new CategoryBL();
            jsonUtil = new JsonUtil();
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
            DisplayProducts(user, products);
            
        }
        public void DisplayProducts(User customer, List<Product> products)
        {
            Console.Clear();
            Console.WriteLine("|------------------------------------------------------------------------------------------------------------------------------|");
            Console.WriteLine("| STT | Tên sản phẩm                                       |       Giá       |            Người bán             |  Tình trạng  |");
            Console.WriteLine("|------------------------------------------------------------------------------------------------------------------------------|");
            int count = 1;
            foreach (Product product in products)  
            { 
                User seller = userBL.GetUserByID(product.UserID);
                string status = product.Quantity == 0? "Hết Hàng" : "Còn Hàng";
                Console.WriteLine("| {0,3 } | {1,-50} | {2, 15} | {3,32} | {4,12} |", count++, product.ProductName, product.Price.ToString("C0"), seller.FullName, status);
                
            }
            Console.WriteLine("|------------------------------------------------------------------------------------------------------------------------------|");
            Console.Write("Nhập số thứ tự tương ứng để xem thông tin sản phẩm hoặc \"0\" để tìm sản phẩm khác: ");
            try
            {
                int choice = Convert.ToInt32(Console.ReadLine());
                if (choice != 0)
                {
                    ProductInformation(customer, products[choice - 1]);
                }
                else
                {
                    SearchProduct(customer);
                }
            }
            catch (System.Exception)
            {
                DisplayProducts(customer, products);
            }
        }
        public void ProductInformation(User customer, Product product)
        {
            User seller = userBL.GetUserByID(product.UserID);
            Console.WriteLine($"Người bán: {seller.FullName}");
            Console.WriteLine($"---------------------------------");
            Console.WriteLine($"Sản Phẩm: {product.ProductName}");
            Console.WriteLine($"Giá: {product.Price.ToString("C0")}");
            Console.WriteLine($"Mô tả: {product.Description}");
            string status = product.Quantity == 0? "Hết Hàng" : "Còn Hàng";
            Console.WriteLine($"Tình trạng: {status}");
            Console.WriteLine($"---------------------------------");
            Console.WriteLine($"Nhập \"Add + Số lượng \" để thêm số sản phẩm tương ứng vào giỏ hàng.");
            Console.WriteLine($"Nhập \"Sub + Số lượng \" để bớt số sản phẩm tương ứng ra khỏi giỏ hàng.");
            Console.WriteLine($"1. Đến giỏ hàng.");
            Console.WriteLine($"2. Mua thêm.");
            Console.WriteLine($"0. Tìm sản phẩm khác.");
            Console.Write($"Chọn: ");
            string? choice = Console.ReadLine();
            if (choice == "1")
            {
                Console.WriteLine("Chọn 1 !");
                Console.ReadKey();
            }
            else if (choice == "2")
            {
                Console.WriteLine("Chọn 2 !");
                Console.ReadKey();
            }
            else if (choice == "0")
            {
                Console.WriteLine("Chọn 0 !");
                Console.ReadKey();
                SearchProduct(customer);
            }
            else if (IsNumber(choice.ToLower().Replace(" ", "").Replace("add", "")))
            {
                List<Product> products = jsonUtil.ProductsLoad();
                if (products == null)
                {
                    products = new List<Product>();
                }
                int num = Convert.ToInt32(choice.ToLower().Replace(" ", "").Replace("add", "")) ;
                
                Product selectedProduct ;
                if (CheckProduct(products, product) == null)
                {
                    selectedProduct = new Product(product.ProductID, product.UserID, product.ProductName, product.Price, product.Description, num);
                    products.Add(selectedProduct);
                }
                else
                {
                    products[Convert.ToInt32(CheckProduct(products, product))].Quantity += num;
                }
                jsonUtil.ProductsSave(products);
                Console.WriteLine("Thêm vào giỏ hàng thành công.");
                ProductInformation(customer, product);
            }
            else if (IsNumber(choice.ToLower().Replace(" ", "").Replace("sub", "")))
            {
                List<Product> products = jsonUtil.ProductsLoad();
                if (products == null)
                {
                    products = new List<Product>();
                }
                int num = Convert.ToInt32(choice.ToLower().Replace(" ", "").Replace("sub", "")) ;
                if (CheckProduct(products, product) == null)
                {
                    Console.WriteLine("Sản phẩm không có trong giỏ hàng.");
                }
                else
                {
                    int i = products[Convert.ToInt32(CheckProduct(products, product))].Quantity;
                    products[Convert.ToInt32(CheckProduct(products, product))].Quantity = i > num ? i - num : 0;
                }
                jsonUtil.ProductsSave(products);
                ProductInformation(customer, product);
            }
            else
            {
                Console.WriteLine("Lựa chọn không hợp lệ !");
                Console.ReadKey();
                ProductInformation(customer, product);
            }
            
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
        public void OrderDetails(User customer, List<Product> products)
        {
            User seller = userBL.GetUserByID(products[0].UserID);
            Console.WriteLine($"Người bán: {seller.FullName}");
            Console.WriteLine($"Đia chỉ: {seller.Address}");
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
            Console.WriteLine($"Nhập STT tương ứng để xem thông tin sản phẩm, \"More\" để mua thêm hoặc \"Pay \" để thanh toán ");
            Console.Write($"Chọn: ");
        }
        public bool IsNumber(string pValue)
        {
            foreach (Char c in pValue)
            {
                if (!Char.IsDigit(c))
                return false;
            }
            return true;
        }
        public string CheckProduct(List<Product> products, Product product)
        {
            string result = null;

            for (int i = 0; i < products.Count; i++)
            {
                if (products[i].ProductID == product.ProductID)
                {
                    result = Convert.ToString(i);
                }
            }
            return result;
        }
    }
}