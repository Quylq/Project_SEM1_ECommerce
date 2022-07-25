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
        private OrderDetailsBL orderDetailsBL;
        private Product_CategoryBL product_CategoryBL;
        private ShopBL shopBL;
        private AddressBL addressBL;
        public Ecommerce ecommerce = new Ecommerce();
        public CustomerPage()
        {
            userBL = new UserBL();
            productBL = new ProductBL();
            orderBL = new OrderBL();
            categoryBL = new CategoryBL();
            orderDetailsBL = new OrderDetailsBL();
            product_CategoryBL = new Product_CategoryBL();
            shopBL = new ShopBL();
            addressBL = new AddressBL();
        }

        public void SearchProduct(int _UserID)
        {
            Console.Clear();
            Console.WriteLine("Enter the product name you want to find or enter \"Menu\" to go back! ");
            string? _ProductName = Console.ReadLine();
            if (_ProductName.ToLower() != "menu")
            {
                List<Product> products = new List<Product>();
                products = productBL.GetProductsByName(_ProductName);
                DisplayProductsInSearchProduct(_UserID, products);
            }
            else
            {
                ecommerce.CustomerPage(_UserID);
            }
        }
        public void DisplayProductsInSearchProduct(int _UserID, List<Product> products)
        {
            Console.Clear();
            Console.WriteLine("|-------------------------------------------------------------------------------------------|");
            Console.WriteLine("| STT | Tên sản phẩm                                       |       Giá       |  Tình trạng  |");
            Console.WriteLine("|-------------------------------------------------------------------------------------------|");
            int count = 1;
            foreach (Product product in products)
            {
                string status = product.Quantity == 0 ? "Hết Hàng" : "Còn Hàng";
                Console.WriteLine("| {0,3 } | {1,-50} | {2,15} | {3,12} |", count++, product.ProductName, product.Price.ToString("C0"), status);
            }
            Console.WriteLine("|--------------------------------------------------------------------------------------------|");
            Console.Write("Nhập số thứ tự tương ứng để xem thông tin sản phẩm hoặc \"0\" để tìm sản phẩm khác: ");
            // try
            // {
            int choice = Convert.ToInt32(Console.ReadLine());
            if (choice != 0)
            {
                ProductInformationInSearchProduct(_UserID, products[choice - 1].ProductID, products);
            }
            else
            {
                SearchProduct(_UserID);
            }
            // }
            // catch (System.Exception)
            // {
            // DisplayProducts(_UserID, products);
            // }
        }
        public void DisplayProductsInShop(int _UserID, List<Product> products, int _ShopID)
        {
            Console.Clear();
            Console.WriteLine("|-------------------------------------------------------------------------------------------|");
            Console.WriteLine("| STT | Tên sản phẩm                                       |       Giá       |  Tình trạng  |");
            Console.WriteLine("|-------------------------------------------------------------------------------------------|");
            int count = 1;
            foreach (Product product in products)
            {
                string status = product.Quantity == 0 ? "Hết Hàng" : "Còn Hàng";
                Console.WriteLine("| {0,3 } | {1,-50} | {2,15} | {3,12} |", count++, product.ProductName, product.Price.ToString("C0"), status);
            }
            Console.WriteLine("|--------------------------------------------------------------------------------------------|");
            Console.Write("Nhập số thứ tự tương ứng để xem thông tin sản phẩm hoặc \"0\" để quay lại ");
            int choice = Convert.ToInt32(Console.ReadLine());
            if (choice != 0)
            {
                ProductInformationInShop(_UserID, products[choice - 1].ProductID, products);
            }
            else
            {
                viewShop(_UserID, _ShopID);
            }
        }
        public void ProductInformationInSearchProduct(int _UserID, int _ProductID, List<Product> products)
        {
            Product product = productBL.GetProductByID(_ProductID);
            Shop shop = shopBL.GetShopByID(product.ShopID);
            Console.WriteLine($"Cửa hàng: {shop.ShopName}");
            Console.WriteLine($"---------------------------------");
            Console.WriteLine($"Sản Phẩm: {product.ProductName}");
            Console.WriteLine($"Giá: {product.Price.ToString("C0")}");
            Console.WriteLine($"Mô tả: {product.Description}");
            string status = product.Quantity == 0 ? "Hết Hàng" : "Còn Hàng";
            Console.WriteLine($"Tình trạng: {status}");
            Console.WriteLine($"---------------------------------");
            Console.WriteLine($"Nhập \"Add + Số lượng \" để thêm số sản phẩm tương ứng vào giỏ hàng.");
            Console.WriteLine($"Nhập \"Sub + Số lượng \" để bớt số sản phẩm tương ứng ra khỏi giỏ hàng.");
            Console.WriteLine($"1. Đến giỏ hàng.");
            Console.WriteLine($"2. Vào cửa hàng {shop.ShopName}.");
            Console.WriteLine($"0. Quay lại.");
            Console.Write($"Chọn: ");
            string? choice = Console.ReadLine();
            if (choice == "1")
            {
                ViewCart(_UserID);
            }
            else if (choice == "2")
            {
                viewShop(_UserID, shop.ShopID);
            }
            else if (choice == "0")
            {
                DisplayProductsInSearchProduct(_UserID, products);
            }
            else if (IsNumber(choice.ToLower().Replace(" ", "").Replace("add", "")))
            {
                List<Order> orders = orderBL.GetOrdersByStatusAndUserID("Shopping", _UserID);
                int addNo = Convert.ToInt32(choice.ToLower().Replace(" ", "").Replace("add", ""));
                int i = CheckShopOfOrders(orders, product.ShopID);
                Console.WriteLine($"{i}");
                Console.ReadKey();
                if (i == -1)
                {
                    string format = "yyyy-MM-dd HH:mm:ss";
                    DateTime now = DateTime.Now;
                    Order order = new Order(orderBL.OrderIDMax() + 1, _UserID, product.ShopID, now.ToString(format), "Shopping");
                    orderBL.InsertOrder(order);
                    Console.ReadKey();
                    OrderDetails orderDetails = new OrderDetails(order.OrderID, product.ProductID, addNo);
                    orderDetailsBL.InsertOrderDetails(orderDetails);
                }
                else
                {
                    Order order = orderBL.GetOrderByID(orders[i].OrderID);
                    List<OrderDetails> orderDetailsList = orderDetailsBL.GetOrderDetailsListByOrderID(order.OrderID);
                    if (CheckProductOfOrderDetails(orderDetailsList, product.ProductID) == -1)
                    {
                        OrderDetails orderDetails = new OrderDetails(order.OrderID, product.ProductID, addNo);
                        orderDetailsBL.InsertOrderDetails(orderDetails);
                    }
                    else
                    {
                        int j = CheckProductOfOrderDetails(orderDetailsList, product.ProductID);
                        orderDetailsList[j].ProductNumber += addNo;
                        orderDetailsBL.UpdateProductNumberOfOrderDetails(orderDetailsList[j]);
                    }
                }
                Console.Clear();
                Console.WriteLine($"---------------------------------");
                Console.WriteLine("Thêm vào giỏ hàng thành công.");
                Console.WriteLine("Nhấn phím bất kỳ để tiếp tục.");
                Console.ReadKey();
                ProductInformationInSearchProduct(_UserID, _ProductID, products);
            }
            else if (IsNumber(choice.ToLower().Replace(" ", "").Replace("sub", "")))
            {
                List<Order> orders = orderBL.GetOrdersByStatusAndUserID("Shopping", _UserID);
                if (orders == null)
                {
                    Console.WriteLine("Sản phẩm không có trong giỏ hàng.");
                    Console.WriteLine("Nhấn phím bất kỳ để tiếp tục.");
                    Console.ReadKey();
                    ProductInformationInSearchProduct(_UserID, _ProductID, products);
                }
                int num = Convert.ToInt32(choice.ToLower().Replace(" ", "").Replace("sub", ""));
                int i = CheckShopOfOrders(orders, product.ShopID);
                if (i == -1)
                {
                    Console.WriteLine("Sản phẩm không có trong giỏ hàng.");
                    Console.WriteLine("Nhấn phím bất kỳ để tiếp tục.");
                    Console.ReadKey();
                    ProductInformationInSearchProduct(_UserID, _ProductID, products);
                }
                else
                {
                    Order order = orderBL.GetOrderByID(orders[i].OrderID);
                    List<OrderDetails> orderDetailsList = orderDetailsBL.GetOrderDetailsListByOrderID(order.OrderID);
                    if (CheckProductOfOrderDetails(orderDetailsList, product.ProductID) == -1)
                    {
                        Console.WriteLine("Sản phẩm không có trong giỏ hàng.");
                        Console.WriteLine("Nhấn phím bất kỳ để tiếp tục.");
                        Console.ReadKey();
                        ProductInformationInSearchProduct(_UserID, _ProductID, products);
                    }
                    else
                    {
                        int j = CheckProductOfOrderDetails(orderDetailsList, product.ProductID);
                        orderDetailsList[j].ProductNumber -= num;
                        if (orderDetailsList[j].ProductNumber < 0)
                        {
                            orderDetailsList[j].ProductNumber = 0;
                        }
                        orderDetailsBL.UpdateProductNumberOfOrderDetails(orderDetailsList[j]);
                        Console.WriteLine("Bỏ khỏi giỏ hàng thành công.");
                        Console.WriteLine("Nhấn phím bất kỳ để tiếp tục.");
                        Console.ReadKey();
                        ProductInformationInSearchProduct(_UserID, _ProductID, products);
                    }
                }
            }
            else
            {
                Console.WriteLine("Lựa chọn không hợp lệ !");
                Console.ReadKey();
                ProductInformationInSearchProduct(_UserID, _ProductID, products);
            }
        }
        public void ProductInformationInShop(int _UserID, int _ProductID, List<Product> products)
        {
            Product product = productBL.GetProductByID(_ProductID);
            Shop shop = shopBL.GetShopByID(product.ShopID);
            Console.WriteLine($"Cửa hàng: {shop.ShopName}");
            Console.WriteLine($"---------------------------------");
            Console.WriteLine($"Sản Phẩm: {product.ProductName}");
            Console.WriteLine($"Giá: {product.Price.ToString("C0")}");
            Console.WriteLine($"Mô tả: {product.Description}");
            string status = product.Quantity == 0 ? "Hết Hàng" : "Còn Hàng";
            Console.WriteLine($"Tình trạng: {status}");
            Console.WriteLine($"---------------------------------");
            Console.WriteLine($"Nhập \"Add + Số lượng \" để thêm số sản phẩm tương ứng vào giỏ hàng.");
            Console.WriteLine($"Nhập \"Sub + Số lượng \" để bớt số sản phẩm tương ứng ra khỏi giỏ hàng.");
            Console.WriteLine($"1. Đến giỏ hàng.");
            Console.WriteLine($"0. Quay lại.");
            Console.Write($"Chọn: ");
            string? choice = Console.ReadLine();
            if (choice == "1")
            {
                ViewCart(_UserID);
            }
            else if (choice == "0")
            {
                viewShop(_UserID, shop.ShopID);
            }
            else if (IsNumber(choice.ToLower().Replace(" ", "").Replace("add", "")))
            {
                List<Order> orders = orderBL.GetOrdersByStatusAndUserID("Shopping", _UserID);
                int addNo = Convert.ToInt32(choice.ToLower().Replace(" ", "").Replace("add", ""));
                int i = CheckShopOfOrders(orders, product.ShopID);
                Console.WriteLine($"{i}");
                Console.ReadKey();
                if (i == -1)
                {
                    string format = "yyyy-MM-dd HH:mm:ss";
                    DateTime now = DateTime.Now;
                    Order order = new Order(orderBL.OrderIDMax() + 1, _UserID, product.ShopID, now.ToString(format), "Shopping");
                    orderBL.InsertOrder(order);
                    Console.ReadKey();
                    OrderDetails orderDetails = new OrderDetails(order.OrderID, product.ProductID, addNo);
                    orderDetailsBL.InsertOrderDetails(orderDetails);
                }
                else
                {
                    Order order = orderBL.GetOrderByID(orders[i].OrderID);
                    List<OrderDetails> orderDetailsList = orderDetailsBL.GetOrderDetailsListByOrderID(order.OrderID);
                    if (CheckProductOfOrderDetails(orderDetailsList, product.ProductID) == -1)
                    {
                        OrderDetails orderDetails = new OrderDetails(order.OrderID, product.ProductID, addNo);
                        orderDetailsBL.InsertOrderDetails(orderDetails);
                    }
                    else
                    {
                        int j = CheckProductOfOrderDetails(orderDetailsList, product.ProductID);
                        orderDetailsList[j].ProductNumber += addNo;
                        orderDetailsBL.UpdateProductNumberOfOrderDetails(orderDetailsList[j]);
                    }
                }
                Console.Clear();
                Console.WriteLine($"---------------------------------");
                Console.WriteLine("Thêm vào giỏ hàng thành công.");
                Console.WriteLine("Nhấn phím bất kỳ để tiếp tục.");
                Console.ReadKey();
                ProductInformationInShop(_UserID, _ProductID, products);
            }
            else if (IsNumber(choice.ToLower().Replace(" ", "").Replace("sub", "")))
            {
                List<Order> orders = orderBL.GetOrdersByStatusAndUserID("Shopping", _UserID);
                if (orders == null)
                {
                    Console.WriteLine("Sản phẩm không có trong giỏ hàng.");
                    Console.WriteLine("Nhấn phím bất kỳ để tiếp tục.");
                    Console.ReadKey();
                    ProductInformationInShop(_UserID, _ProductID, products);
                }
                int num = Convert.ToInt32(choice.ToLower().Replace(" ", "").Replace("sub", ""));
                int i = CheckShopOfOrders(orders, product.ShopID);
                if (i == -1)
                {
                    Console.WriteLine("Sản phẩm không có trong giỏ hàng.");
                    Console.WriteLine("Nhấn phím bất kỳ để tiếp tục.");
                    Console.ReadKey();
                    ProductInformationInShop(_UserID, _ProductID, products);
                }
                else
                {
                    Order order = orderBL.GetOrderByID(orders[i].OrderID);
                    List<OrderDetails> orderDetailsList = orderDetailsBL.GetOrderDetailsListByOrderID(order.OrderID);
                    if (CheckProductOfOrderDetails(orderDetailsList, product.ProductID) == -1)
                    {
                        Console.WriteLine("Sản phẩm không có trong giỏ hàng.");
                        Console.WriteLine("Nhấn phím bất kỳ để tiếp tục.");
                        Console.ReadKey();
                        ProductInformationInShop(_UserID, _ProductID, products);
                    }
                    else
                    {
                        int j = CheckProductOfOrderDetails(orderDetailsList, product.ProductID);
                        orderDetailsList[j].ProductNumber -= num;
                        if (orderDetailsList[j].ProductNumber < 0)
                        {
                            orderDetailsList[j].ProductNumber = 0;
                        }
                        orderDetailsBL.UpdateProductNumberOfOrderDetails(orderDetailsList[j]);
                        Console.WriteLine("Bỏ khỏi giỏ hàng thành công.");
                        Console.WriteLine("Nhấn phím bất kỳ để tiếp tục.");
                        Console.ReadKey();
                        ProductInformationInShop(_UserID, _ProductID, products);
                    }
                }
            }
            else
            {
                Console.WriteLine("Lựa chọn không hợp lệ !");
                Console.ReadKey();
                ProductInformationInShop(_UserID, _ProductID, products);
            }
        }
        public void ViewOrder(int _UserID, int _OrderID)
        {
            Order order = orderBL.GetOrderByID(_OrderID);
            List<OrderDetails> orderDetailsList = orderDetailsBL.GetOrderDetailsListByOrderID(_OrderID);
            Shop shop = shopBL.GetShopByID(order.ShopID);
            Address address = addressBL.GetAddressByID(shop.AddressID);
            Console.WriteLine($"Cửa Hàng: {shop.ShopName}");
            Console.WriteLine($"Đia chỉ: {address.City}");
            Console.WriteLine("|--------------------------------------------------------------------------------------------|");
            Console.WriteLine("| STT | Tên sản phẩm                        |       Giá       | Số lượng |    Thành Tiền     |");
            Console.WriteLine("|--------------------------------------------------------------------------------------------|");
            int count = 1;
            int total = 0;
            foreach (OrderDetails orderDetails in orderDetailsList)
            {
                if (orderDetails.ProductNumber > 0)
                {
                    Product product = productBL.GetProductByID(orderDetails.ProductID);
                    Console.WriteLine("| {0,3 } | {1,-35} | {2, 15} | {3,8} | {4, 17} |", count++, product.ProductName, product.Price.ToString("C0"), orderDetails.ProductNumber, (product.Price * orderDetails.ProductNumber).ToString("C0"));
                }
            }
            Console.WriteLine("|--------------------------------------------------------------------------------------------|");
            total = orderBL.GetTotalOrder(_OrderID);
            Console.WriteLine("Tổng Tiền:  {0, 17} ", total.ToString("C0"));
            Console.WriteLine("1. Đặt hàng.");
            Console.WriteLine("2. Xem shop.");
            Console.WriteLine("0. Quay lại.");
            Console.Write($"Chọn: ");
            string? choice = Console.ReadLine();
            if (choice.ToLower() == "2")
            {
                viewShop(_UserID, shop.ShopID);
            }
            else if (choice.ToLower() == "1")
            {
                orderBL.UpdateStatusOfOrder(_OrderID, "Processing");
                Console.WriteLine("Đặt hàng thành công");
                Console.WriteLine("Nhấn phím bất kỳ để tiếp tục");
                Console.ReadKey();
                MyOrder(_UserID);
            }
            else if (choice == "0")
            {
                ecommerce.CustomerPage(_UserID);
            }
            else
            {
                Console.WriteLine("Vui lòng chọn 0 - 2");
                ViewOrder(_UserID, _OrderID);
            }
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
        public int CheckProductOfOrderDetails(List<OrderDetails> orderDetailsList, int _ProductID)
        {
            int result = -1;

            for (int i = 0; i < orderDetailsList.Count; i++)
            {
                if (orderDetailsList[i].ProductID == _ProductID)
                {
                    result = i;
                    break;
                }
            }
            return result;
        }
        public void viewShop(int _UserID, int _ShopID)
        {
            Console.Clear();
            Console.WriteLine("1. Tất cả sản phẩm");
            List<Category> categories = categoryBL.GetCategoriesByShopID(_ShopID);
            int count = 2;
            foreach (Category category in categories)
            {
                Console.WriteLine($"{count++}. {category.CategoryName}");
            }
            Console.WriteLine("0. Quay lại.");
            Console.Write("Chọn: ");
            try
            {
                int choice = Convert.ToInt32(Console.ReadLine());
                if (choice != 0 && choice != 1)
                {
                    List<Product> products = productBL.GetProductsByCategory(categories[choice - 2].CategoryID);
                    DisplayProductsInShop(_UserID, products, _ShopID);
                }
                else if (choice == 0)
                {
                    ecommerce.CustomerPage(_UserID);
                }
                else
                {
                    List<Product> products = productBL.GetProductsByShopID(_ShopID);
                    DisplayProductsInShop(_UserID, products, _ShopID);
                }
            }
            catch (System.Exception)
            {
                Console.Clear();
                viewShop(_UserID, _ShopID);
            }
        }
        public void viewShopInSearch(int _UserID, int _ShopID, List<Shop> shops)
        {
            Console.Clear();
            Console.WriteLine("1. Tất cả sản phẩm");
            List<Category> categories = categoryBL.GetCategoriesByShopID(_ShopID);
            int count = 2;
            foreach (Category category in categories)
            {
                Console.WriteLine($"{count++}. {category.CategoryName}");
            }
            Console.WriteLine("0. Quay lại.");
            Console.Write("Chọn: ");
            try
            {
                int choice = Convert.ToInt32(Console.ReadLine());
                if (choice != 0 && choice != 1)
                {
                    List<Product> products = productBL.GetProductsByCategory(categories[choice - 2].CategoryID);
                    DisplayProductsInShop(_UserID, products, _ShopID);
                }
                else if (choice == 0)
                {
                    DisplayShops(_UserID, shops);
                }
                else
                {
                    List<Product> products = productBL.GetProductsByShopID(_ShopID);
                    DisplayProductsInShop(_UserID, products, _ShopID);
                }
            }
            catch (System.Exception)
            {
                Console.Clear();
                viewShop(_UserID, _ShopID);
            }
        }
        public void ViewCart(int _UserID)
        {
            List<Order> orders = orderBL.GetOrdersByStatusAndUserID("Shopping", _UserID);
            if (orders.Count > 1)
            {
                Console.Clear();
                Console.WriteLine("|----------------------------------------------------------|");
                Console.WriteLine("| STT | Người Bán                                          |");
                Console.WriteLine("|----------------------------------------------------------|");
                int count = 1;
                for (int i = 0; i < orders.Count; i++)
                {
                    Shop shop = shopBL.GetShopByID(orders[i].ShopID);
                    Console.WriteLine("| {0,3 } | {1,-50} |", count++, shop.ShopName);
                }
                Console.WriteLine("|----------------------------------------------------------|");
                Console.Write("Nhập số thứ tự tương ứng để xem thông tin đơn hàng hoặc \"0\" để quay lại: ");
                try
                {
                    int choice = Convert.ToInt32(Console.ReadLine());
                    if (choice != 0)
                    {
                        ViewOrder(_UserID, orders[choice - 1].OrderID);
                    }
                    else
                    {
                        ecommerce.CustomerPage(_UserID);
                    }
                }
                catch (System.Exception)
                {
                    SearchProduct(_UserID);
                }
            }
            else if (orders.Count == 1)
            {
                ViewOrder(_UserID, orders[0].OrderID);
            }
            else
            {
                Console.WriteLine("Giỏ hàng trống");
                Console.WriteLine("Nhấn phím bất kỳ để về Menu");
                Console.ReadKey();
                ecommerce.CustomerPage(_UserID);
            }
        }
        public int CheckShopOfOrders(List<Order> orders, int _ShopID)
        {
            int result = -1;
            for (int i = 0; i < orders.Count; i++)
            {
                if (orders[i].ShopID == _ShopID)
                {
                    result = i;
                    break;
                }
            }
            return result;
        }
        public void SearchShop(int _UserID)
        {
            Console.Clear();
            Console.WriteLine("Nhập Shop bạn muốn tìm hoặc \"Menu\" để quay lại: ");
            string? _ShopName = Console.ReadLine();
            if (_ShopName.ToLower() != "menu")
            {
                List<Shop> shops = new List<Shop>();
                shops = shopBL.GetShopsByName(_ShopName);
                DisplayShops(_UserID, shops);
            }
            else
            {
                ecommerce.CustomerPage(_UserID);
            }
        }
        public void DisplayShops(int _UserID, List<Shop> shops)
        {
            Console.Clear();
            Console.WriteLine("|--------------------------------------------------------------------------------------------|");
            Console.WriteLine("| STT | Tên cửa hàng                                       |          Địa Chỉ                |");
            Console.WriteLine("|--------------------------------------------------------------------------------------------|");
            int count = 1;
            foreach (Shop shop in shops)
            {
                Address address = addressBL.GetAddressByID(shop.AddressID);
                Console.WriteLine("| {0,3 } | {1,-50} | {2,31} |", count++, shop.ShopName, address.City);
            }
            Console.WriteLine("|--------------------------------------------------------------------------------------------|");
            Console.Write("Nhập số thứ tự tương ứng để xem cửa hàng hoặc \"0\" để tìm shop khác: ");
            try
            {
                int choice = Convert.ToInt32(Console.ReadLine());
                if (choice != 0)
                {
                    viewShopInSearch(_UserID, shops[choice - 1].ShopID, shops);
                }
                else
                {
                    SearchShop(_UserID);
                }
            }
            catch (System.Exception)
            {
                DisplayShops(_UserID, shops);
            }
        }
        public void MyOrder(int _UserID)
        {
            Console.WriteLine("1. Chờ xác nhận.");
            Console.WriteLine("2. Chờ lấy hàng");
            Console.WriteLine("3. Đã mua.");
            Console.WriteLine("4. Đơn hàng hủy.");
            Console.WriteLine("0. Quay lại.");
            Console.Write("Chọn: ");
            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    ViewOrdersProcessing(_UserID);
                    break;
                case "2":
                    ViewOrdersToReceive(_UserID);
                    break;
                case "3":
                    ViewOrdersFinished(_UserID);
                    break;
                case "4":
                    ViewOrdersFailed(_UserID);
                    break;
                case "0":
                    ecommerce.CustomerPage(_UserID);
                    break;
                default:
                    Console.WriteLine("Vui lòng chọn 0 - 4 !");
                    MyOrder(_UserID);
                    break;
            }
        }
        public void DisplayOrders(int _UserID, List<Order> orders)
        {
            Console.Clear();
            Console.WriteLine("|--------------------------------------------------------------------------------------------------|");
            Console.WriteLine("| STT | Tên cửa hàng                    | Số lượng sản phẩm |       Tổng tiền     |   Trạng Thái   |");
            Console.WriteLine("|--------------------------------------------------------------------------------------------------|");
            int count = 1;
            for (int i = 0; i < orders.Count; i++)
            {
                int total = orderBL.GetTotalOrder(orders[i].OrderID);
                Shop shop = shopBL.GetShopByID(orders[i].ShopID);
                List<OrderDetails> orderDetailsList = orderDetailsBL.GetOrderDetailsListByOrderID(orders[i].OrderID);
                Console.WriteLine("| {0,3 } | {1,-31} | {2,17} | {3,19} | {4,14} |", count++, shop.ShopName, orderDetailsList.Count, total.ToString("C0"), orders[i].Status);
            }
            Console.WriteLine("|--------------------------------------------------------------------------------------------------|");
        }
        public void ViewOrdersProcessing(int _UserID)
        {
            List<Order> orders = orderBL.GetOrdersByStatusAndUserID("Processing", _UserID);
            DisplayOrders(_UserID, orders);
            Console.WriteLine("Nhấn phím bất kỳ để quay lại");
            Console.ReadKey();
            MyOrder(_UserID);
        }
        public void ViewOrdersToReceive(int _UserID)
        {
            List<Order> orders = orderBL.GetOrdersByStatusAndUserID("ToReceive", _UserID);
            if (orders.Count > 0)
            {
                DisplayOrders(_UserID, orders);
                Console.WriteLine($"Nhập \"Confirm + Số thứ tự \" để xác nhận lấy hàng.");
                Console.WriteLine($"Nhập \"Reject + Số thứ tự \" để từ chối lấy hàng.");
                Console.WriteLine($"0. Quay lại.");
                Console.Write($"Chọn: ");
                string? choice = Console.ReadLine();
                if (choice == "0")
                {
                    MyOrder(_UserID);
                }
                else if (IsNumber(choice.ToLower().Replace(" ", "").Replace("confirm", "")))
                {
                    int confirmNo = Convert.ToInt32(choice.ToLower().Replace(" ", "").Replace("confirm", ""));
                    orderBL.UpdateStatusOfOrder(orders[confirmNo - 1].OrderID, "Finished");
                    Console.WriteLine("Xác nhận thành công");
                    Console.WriteLine("Nhấn phím bất kỳ để tiếp tục");
                    Console.ReadKey();
                    ViewOrdersToReceive(_UserID);
                }
                else if (IsNumber(choice.ToLower().Replace(" ", "").Replace("reject", "")))
                {
                    int rejectNo = Convert.ToInt32(choice.ToLower().Replace(" ", "").Replace("reject", ""));
                    orderBL.UpdateStatusOfOrder(orders[rejectNo - 1].OrderID, "Failed");
                    Console.WriteLine("Từ chối thành công");
                    Console.WriteLine("Nhấn phím bất kỳ để tiếp tục");
                    Console.ReadKey();
                    ViewOrdersToReceive(_UserID);
                }
                else
                {
                    Console.WriteLine("Lựa chọn không hợp lệ !");
                    Console.ReadKey();
                    ViewOrdersToReceive(_UserID);
                }
            }
            else
            {
                Console.WriteLine("Không có đơn hàng ");
                Console.WriteLine("Nhấn phím bất kỳ để tiếp tục");
                Console.ReadKey();
                MyOrder(_UserID);
            }
        }
        public void ViewOrdersFinished(int _UserID)
        {
            List<Order> orders = orderBL.GetOrdersByStatusAndUserID("Finished", _UserID);
            DisplayOrders(_UserID, orders);
            Console.WriteLine("Nhấn phím bất kỳ để quay lại");
            Console.ReadKey();
            MyOrder(_UserID);
        }
        public void ViewOrdersFailed(int _UserID)
        {
            List<Order> orders = orderBL.GetOrdersByStatusAndUserID("Finished", _UserID);
            DisplayOrders(_UserID, orders);
            Console.WriteLine("Nhấn phím bất kỳ để quay lại");
            Console.ReadKey();
            MyOrder(_UserID);
        }
    }
}