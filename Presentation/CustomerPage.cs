using BL;
using ConsoleTableExt;


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
            Console.WriteLine("Nhập sản phẩm bạn muốn tìm hoặc \"Menu\" để quay lại: ");
            string? _ProductName = Console.ReadLine();
            if (_ProductName.ToLower() != "menu")
            {
                List<Product> products = new List<Product>();
                products = productBL.GetProductsByName(_ProductName);
                DisplayProducts(_UserID, products);
            }
            else
            {
                ecommerce.CustomerPage(_UserID);
            }     
        }
        public void DisplayProducts(int _UserID, List<Product> products)
        {
            List<List<object>> tableData = new List<List<object>>();
            int count = 1;
            foreach (Product product in products)  
            { 
                string status = product.Quantity == 0? "Hết Hàng" : "Còn Hàng";
                List<object> rowData = new List<object>{count++, product.ProductName, product.Price.ToString("C0"), status};
                tableData.Add(rowData);
            }
            Console.Clear();
            ConsoleTableBuilder
                .From(tableData)
                .WithColumn("STT", "Tên Sản Phẩm", "Giá", "Tình Trạng")
                .WithTextAlignment(new Dictionary < int, TextAligntment>
                    {
                        {2, TextAligntment.Right },
                        {3, TextAligntment.Right }
                    })
                .WithCharMapDefinition(CharMapDefinition.FrameDoublePipDefinition)
                .ExportAndWriteLine();
            Console.Write("Nhập số thứ tự tương ứng để xem thông tin sản phẩm hoặc \"0\" để tìm sản phẩm khác: ");
            try
            {
                int choice = Convert.ToInt32(Console.ReadLine());
                if (choice != 0)
                {
                    ProductInformation(_UserID, products[choice - 1].ProductID);
                }
                else
                {
                    SearchProduct(_UserID);
                }
            }
            catch (System.Exception)
            {
                DisplayProducts(_UserID, products);
            }
        }
        public void ProductInformation(int _UserID, int _ProductID)
        {
            Product product = productBL.GetProductByID(_ProductID);
            Shop shop = shopBL.GetShopByID(product.ShopID);
            string status = product.Quantity == 0? "Hết Hàng" : "Còn Hàng";
            List<List<object>> tableData = new List<List<object>>
            {
                new List<object>{"Cửa Hàng", shop.ShopName},
                new List<object>{"Sản Phẩm", product.ProductName},
                new List<object>{"Giá", product.Price.ToString("C0")},
                new List<object>{"Hàng còn", product.Quantity}
            };
            for (int i = 0; i < product.Description.Split('\n').Length; i++)
            {
                List<object> rowData;
                if (i == 0)
                {
                    rowData = new List<object>{"Mô tả", product.Description.Split('\n')[i]};
                }
                else
                {
                    rowData = new List<object>{"", product.Description.Split('\n')[i]};
                }
                tableData.Add(rowData);
            } 
            Console.Clear();
            ConsoleTableBuilder
                .From(tableData)
                .WithTitle("Thông Tin sản phẩm ", ConsoleColor.Yellow, ConsoleColor.DarkGray)
                .WithCharMapDefinition(new Dictionary<CharMapPositions, char> {
                    {CharMapPositions.BottomLeft, '═' },
                    {CharMapPositions.BottomCenter, '═' },
                    {CharMapPositions.BottomRight, '═' },
                    {CharMapPositions.BorderTop, '═' },
                    {CharMapPositions.BorderBottom, '═' },
                    {CharMapPositions.BorderLeft, '│' },
                    {CharMapPositions.BorderRight, '│' },
                    {CharMapPositions.DividerY, '│' },
                })
                .WithHeaderCharMapDefinition(new Dictionary<HeaderCharMapPositions, char> {
                    {HeaderCharMapPositions.TopLeft, '═' },
                    {HeaderCharMapPositions.TopCenter, '═' },
                    {HeaderCharMapPositions.TopRight, '═' },
                    {HeaderCharMapPositions.BottomLeft, '│' },
                    {HeaderCharMapPositions.BottomRight, '│' },
                    {HeaderCharMapPositions.Divider, '│' },
                    {HeaderCharMapPositions.BorderTop, '═' },
                    {HeaderCharMapPositions.BorderLeft, '│' },
                    {HeaderCharMapPositions.BorderRight, '│' },
                })
                .ExportAndWriteLine();
            Console.WriteLine($"Nhập Số lượng sản phẩm cần thêm vào giỏ hàng.");
            Console.WriteLine($"0. Quay lại.");
            Console.Write($"Chọn: ");
            int choice = Convert.ToInt32(Console.ReadLine());
            try
            {
                if (choice != 0)
                {
                    List<Order> orders = orderBL.GetOrdersByStatusAndUserID("Shopping", _UserID);
                    int i = CheckShopOfOrders(orders, product.ShopID);
                    if (i == -1)
                    {
                        string format = "yyyy-MM-dd HH:mm:ss";
                        DateTime now = DateTime.Now;
                        Order order = new Order(orderBL.OrderIDMax() + 1, _UserID, product.ShopID, now.ToString(format), "Shopping");
                        orderBL.InsertOrder(order);
                        Console.ReadKey();
                        OrderDetails orderDetails = new OrderDetails(order.OrderID, product.ProductID, choice);
                        orderDetailsBL.InsertOrderDetails(orderDetails);
                    }
                    else
                    {                   
                        Order order = orderBL.GetOrderByID(orders[i].OrderID);
                        List<OrderDetails> orderDetailsList = orderDetailsBL.GetOrderDetailsListByOrderID(order.OrderID);
                        if (CheckProductOfOrderDetails(orderDetailsList, product.ProductID) == -1)
                        {
                            OrderDetails orderDetails = new OrderDetails(order.OrderID, product.ProductID, choice);
                            orderDetailsBL.InsertOrderDetails(orderDetails);
                        }
                        else
                        {
                            int j = CheckProductOfOrderDetails(orderDetailsList, product.ProductID);
                            orderDetailsList[j].ProductNumber += choice;
                            orderDetailsBL.UpdateProductNumberOfOrderDetails(orderDetailsList[j]);
                        }
                    }
                    Console.WriteLine($"Đã thêm {choice} sản phẩm vào giỏ hàng thành công.");
                    Console.WriteLine("Nhấn phím bất kỳ để tiếp tục.");
                    Console.ReadKey();
                    ProductInformation(_UserID, _ProductID);
                }
                else
                {
                    SearchProduct(_UserID);
                }
            }
            catch (System.Exception)
            {
                ProductInformation(_UserID, _ProductID);
            }
        }
        public void ProductInformationOfCart(int _UserID, int _ProductID)
        {
            Product product = productBL.GetProductByID(_ProductID);
            Shop shop = shopBL.GetShopByID(product.ShopID);
            Console.WriteLine($"Cửa hàng: {shop.ShopName}");
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
                SearchProduct(_UserID);
            }
            else if (IsNumber(choice.ToLower().Replace(" ", "").Replace("add", "")))
            {
                List<Order> orders = orderBL.GetOrdersByStatusAndUserID("Shopping", _UserID);
                int addNo = Convert.ToInt32(choice.ToLower().Replace(" ", "").Replace("add", "")) ;
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
                ProductInformation(_UserID, _ProductID);
            }
            else if (IsNumber(choice.ToLower().Replace(" ", "").Replace("sub", "")))
            {
                List<Order> orders = orderBL.GetOrdersByStatusAndUserID("Shopping", _UserID);
                if (orders == null)
                {
                    Console.WriteLine("Sản phẩm không có trong giỏ hàng.");
                    Console.WriteLine("Nhấn phím bất kỳ để tiếp tục.");
                    Console.ReadKey();
                    ProductInformation(_UserID, _ProductID);
                }
                int num = Convert.ToInt32(choice.ToLower().Replace(" ", "").Replace("sub", ""));
                int i = CheckShopOfOrders(orders, product.ShopID);
                if (i == -1)
                {
                    Console.WriteLine("Sản phẩm không có trong giỏ hàng.");
                    Console.WriteLine("Nhấn phím bất kỳ để tiếp tục.");
                    Console.ReadKey();
                    ProductInformation(_UserID, _ProductID);
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
                        ProductInformation(_UserID, _ProductID);
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
                        ProductInformation(_UserID, _ProductID);
                    }
                }
            }
            else
            {
                Console.WriteLine("Lựa chọn không hợp lệ !");
                Console.ReadKey();
                ProductInformation(_UserID, _ProductID);
            }  
        }
        public void ViewOrder(int _UserID, int _OrderID)
        {
            List<List<object>> tableData = new List<List<object>>();
            int count = 1;
            Order order = orderBL.GetOrderByID(_OrderID);
            List<OrderDetails> orderDetailsList = orderDetailsBL.GetOrderDetailsListByOrderID(_OrderID);
            Shop shop = shopBL.GetShopByID(order.ShopID);
            int total = orderBL.GetTotalOrder(_OrderID);
            foreach (OrderDetails orderDetails in orderDetailsList)  
            { 
                if (orderDetails.ProductNumber > 0)
                {
                    Product product = productBL.GetProductByID(orderDetails.ProductID);
                    List<object> rowData = new List<object>{shop.ShopName, count++, product.ProductName, product.Price.ToString("C0"), orderDetails.ProductNumber, (product.Price * orderDetails.ProductNumber).ToString("C0")};
                    tableData.Add(rowData);
                }
            }
            List<object> rowTotal = new List<object>{"", "", "", "", "", total.ToString("C0")};
            tableData.Add(rowTotal);
            ConsoleTableBuilder
                .From(tableData)
                .WithColumn("Cửa Hàng", "STT", "Sản Phẩm", "Giá", "Số lượng", "Thành Tiền")
                .WithTextAlignment(new Dictionary < int, TextAligntment>
                    {
                        {4, TextAligntment.Right },
                        {3, TextAligntment.Right },
                        {5, TextAligntment.Right }
                    })
                .WithCharMapDefinition(CharMapDefinition.FrameDoublePipDefinition)
                .ExportAndWriteLine();
            if (order.Status == "ToReceive")
            {
                Console.WriteLine("1. Xác nhận lấy hàng.");
                Console.WriteLine("2. Từ chối lấy hàng.");
                Console.WriteLine("0. Quay lại.");
                string? choice = Console.ReadLine();
                if (choice == "1")
                {
                    orderBL.UpdateStatusOfOrder(order.OrderID, "Finished");
                    Console.WriteLine("Lấy hàng thành công");
                    Console.WriteLine("Nhấn phím bất kỳ để tiếp tục");
                    Console.ReadKey();
                }
                else if (choice == "2")
                {
                    orderBL.UpdateStatusOfOrder(order.OrderID, "Failed");
                    Console.WriteLine("Từ chối nhận hàng thành công");
                    Console.WriteLine("Nhấn phím bất kỳ để tiếp tục");
                    Console.ReadKey();

                }
                MyOrder(_UserID);
            }
            else
            {
                Console.WriteLine("Nhấn phím bất kỳ để tiếp tục");
                Console.ReadKey();
                MyOrder(_UserID);
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
                    DisplayProducts(_UserID, products);
                }
                else if (choice == 0)
                {
                    ecommerce.CustomerPage(_UserID);
                }
                else
                {
                    List<Product> products = productBL.GetProductsByShopID(_ShopID);
                    DisplayProducts(_UserID, products);
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
            Console.Clear();
            if (orders.Count > 0)
            {
                List<List<object>> tableData = new List<List<object>>();
                int count = 1;
                
                List<OrderDetails> orderDetailsList = orderDetailsBL.GetOrderDetailsListByUserIDAndStatus(_UserID, "Shopping");
                
                int total = orderBL.GetTotalCart(_UserID);
                foreach (OrderDetails orderDetails in orderDetailsList)  
                { 
                    if (orderDetails.ProductNumber > 0)
                    {
                        Order order = orderBL.GetOrderByID(orderDetails.OrderID);
                        Shop shop = shopBL.GetShopByID(order.ShopID);
                        Product product = productBL.GetProductByID(orderDetails.ProductID);
                        List<object> rowData = new List<object>{shop.ShopName, count++, product.ProductName, product.Price.ToString("C0"), orderDetails.ProductNumber, (product.Price * orderDetails.ProductNumber).ToString("C0")};
                        tableData.Add(rowData);
                    }
                }
                List<object> rowTotal = new List<object>{"", "", "", "", "", total.ToString("C0")};
                tableData.Add(rowTotal);
                ConsoleTableBuilder
                    .From(tableData)
                    .WithColumn("Cửa Hàng", "STT", "Sản Phẩm", "Giá", "Số lượng", "Thành Tiền")
                    .WithTextAlignment(new Dictionary < int, TextAligntment>
                        {
                            {4, TextAligntment.Right },
                            {3, TextAligntment.Right },
                            {5, TextAligntment.Right }
                        })
                    .WithCharMapDefinition(CharMapDefinition.FrameDoublePipDefinition)
                    .ExportAndWriteLine();
                Console.WriteLine("1. Đặt hàng");
                Console.WriteLine("0. Quay lại");
                Console.Write($"Chọn: ");
                string? choice = Console.ReadLine();
                if (choice == "1")
                {
                    foreach (Order order in orders)
                    {
                        orderBL.UpdateStatusOfOrder(order.OrderID, "Processing");
                    }
                    Console.WriteLine("Đặt hàng thành công");
                    Console.WriteLine("Nhấn phím bất kỳ để tiếp tục");
                    Console.ReadKey();
                    ecommerce.CustomerPage(_UserID);
                }
                else if (choice == "0")
                {
                    ecommerce.CustomerPage(_UserID);
                }
                else
                {
                    ViewCart(_UserID);
                }
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
            List<List<object>> tableData = new List<List<object>>();
            int count = 1;
            foreach (Shop shop in shops)  
            { 
                Address address = addressBL.GetAddressByID(shop.AddressID);
                List<object> rowData = new List<object>{count++, shop.ShopName, address.City};
                tableData.Add(rowData);
            }
            ConsoleTableBuilder
                .From(tableData)
                .WithColumn("STT", "Tên cửa hàng", "Địa chỉ")
                .WithCharMapDefinition(CharMapDefinition.FrameDoublePipDefinition)
                .ExportAndWriteLine();
            Console.Write("Nhập số thứ tự tương ứng để xem cửa hàng hoặc \"0\" để tìm shop khác: ");
            try
            {
                int choice = Convert.ToInt32(Console.ReadLine());
                if (choice != 0)
                {
                    viewShop(_UserID, shops[choice - 1].ShopID);
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
            List<Order> orders = orderBL.GetOrdersByUserID(_UserID);
            DisplayOrders(_UserID, orders);
            Console.WriteLine("Nhập số thự tự tương ứng để xem thông tin order.");
            Console.WriteLine("0. Quay lại.");
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
                MyOrder(_UserID);
            }
            
        }
        // {
        //     Console.WriteLine("1. Chờ xác nhận.");
        //     Console.WriteLine("2. Chờ lấy hàng");
        //     Console.WriteLine("3. Đã mua.");
        //     Console.WriteLine("4. Đơn hàng hủy.");       
        //     Console.WriteLine("0. Quay lại.");
        //     Console.Write("Chọn: ");
        //     string? choice = Console.ReadLine();
        //     switch (choice)
        //     {
        //         case "1":
        //             ViewOrdersProcessing(_UserID);
        //             break;
        //         case "2":
        //             ViewOrdersToReceive(_UserID);
        //             break;
        //         case "3":
        //             ViewOrdersFinished(_UserID);
        //             break;
        //         case "4":
        //             ViewOrdersFailed(_UserID);
        //             break;
        //         case "0": 
        //             ecommerce.CustomerPage(_UserID);
        //             break;
        //         default:
        //             Console.WriteLine("Vui lòng chọn 0 - 4 !");
        //             MyOrder(_UserID);
        //             break;
        //     }
        // }
        
        public void DisplayOrders(int _UserID, List<Order> orders)
        {
            Console.Clear();
            List<List<object>> tableData = new List<List<object>>();
            int count = 1;
            for (int i = 0; i < orders.Count; i++)
            {
                int total = orderBL.GetTotalOrder(orders[i].OrderID);
                Shop shop = shopBL.GetShopByID(orders[i].ShopID);
                List<OrderDetails> orderDetailsList = orderDetailsBL.GetOrderDetailsListByOrderID(orders[i].OrderID);
                List<object> rowData = new List<object>{count++, shop.ShopName, orderDetailsList.Count, total.ToString("C0"), orders[i].Status};
                tableData.Add(rowData);
            }
            ConsoleTableBuilder
                .From(tableData)
                .WithColumn("STT", "Tên cửa hàng", "Số lượng sản phẩm", "Tổng tiền", "Trạng Thái")
                .WithCharMapDefinition(CharMapDefinition.FrameDoublePipDefinition)
                .ExportAndWriteLine();
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
                    orderBL.UpdateStatusOfOrder(orders[confirmNo-1].OrderID, "Finished");
                    Console.WriteLine("Xác nhận thành công");
                    Console.WriteLine("Nhấn phím bất kỳ để tiếp tục");
                    Console.ReadKey();
                    ViewOrdersToReceive(_UserID);
                }
                else if (IsNumber(choice.ToLower().Replace(" ", "").Replace("reject", "")))
                {
                    int rejectNo = Convert.ToInt32(choice.ToLower().Replace(" ", "").Replace("reject", ""));
                    orderBL.UpdateStatusOfOrder(orders[rejectNo-1].OrderID, "Failed");
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