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
            Console.WriteLine("Enter product name to search or \"0\" to go back: ");
            string? _ProductName = Console.ReadLine();
            if (_ProductName.ToLower() != "0")
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
                string status = product.Quantity == 0? "Out of stock" : "Stocking";
                List<object> rowData = new List<object>{count++, product.ProductName, product.Price.ToString("C0"), status};
                tableData.Add(rowData);
            }
            Console.Clear();
            ConsoleTableBuilder
                .From(tableData)
                .WithColumn("ID", "Product's name", "Price", "Status")
                .WithTextAlignment(new Dictionary < int, TextAligntment>
                    {
                        {2, TextAligntment.Right },
                        {3, TextAligntment.Right }
                    })
                .WithCharMapDefinition(CharMapDefinition.FrameDoublePipDefinition)
                .ExportAndWriteLine();
            Console.Write("Enter ID to view product information or \"0\" to find other products: ");
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
            string status = product.Quantity == 0? "Out of stock" : "Stocking";
            List<List<object>> tableData = new List<List<object>>
            {
                new List<object>{"Shop Name", shop.ShopName},
                new List<object>{"Product Name", product.ProductName},
                new List<object>{"Price", product.Price.ToString("C0")},
                new List<object>{"Quantity", product.Quantity}
            };
            for (int i = 0; i < product.Description.Split('\n').Length; i++)
            {
                List<object> rowData;
                if (i == 0)
                {
                    rowData = new List<object>{"Description", product.Description.Split('\n')[i]};
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
                .WithTitle("Product Information ", ConsoleColor.Yellow, ConsoleColor.DarkGray)
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
            Console.WriteLine($"Enter Quantity of products to add to cart.");
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
                    Console.WriteLine($"Added {choice} products to cart successfully.");
                    Console.WriteLine("Press any key to continue.");
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
                .WithColumn("Shop Name", "ID", "Product's Name", "Price", "Quantity", "Into Money")
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
                Console.WriteLine("1. Confirm pick up.");
                Console.WriteLine("2. Reject orders");
                Console.WriteLine("0. Back.");
                string? choice = Console.ReadLine();
                if (choice == "1")
                {
                    orderBL.UpdateStatusOfOrder(order.OrderID, "Finished");
                    Console.WriteLine("Pick up successfully");
                    Console.WriteLine("Press any key to continue.");
                    Console.ReadKey();
                }
                else if (choice == "2")
                {
                    orderBL.UpdateStatusOfOrder(order.OrderID, "Failed");
                    Console.WriteLine("Successfully refused order");
                    Console.WriteLine("Press any key to continue.");
                    Console.ReadKey();
                }
                MyOrder(_UserID);
            }
            else
            {
                Console.WriteLine("Press any key to continue.");
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
            Console.WriteLine("1. All products");
            List<Category> categories = categoryBL.GetCategoriesByShopID(_ShopID);
            int count = 2;
            foreach (Category category in categories)
            {
                Console.WriteLine($"{count++}. {category.CategoryName}");
            }
            Console.WriteLine("0. Back.");
            Console.Write("Choice: ");
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
                    .WithColumn("Shop Name", "ID", "Product's Name", "Price", "Quantity", "Into Money")
                    .WithTextAlignment(new Dictionary < int, TextAligntment>
                        {
                            {4, TextAligntment.Right },
                            {3, TextAligntment.Right },
                            {5, TextAligntment.Right }
                        })
                    .WithCharMapDefinition(CharMapDefinition.FrameDoublePipDefinition)
                    .ExportAndWriteLine();
                Console.WriteLine("1. Payment");
                Console.WriteLine("0. Back");
                Console.Write($"Choice: ");
                string? choice = Console.ReadLine();
                if (choice == "1")
                {
                    foreach (Order order in orders)
                    {
                        orderBL.UpdateStatusOfOrder(order.OrderID, "Processing");
                    }
                    Console.WriteLine("Order Success");
                    Console.WriteLine("Press any key to continue.");
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
                Console.WriteLine("Cart is empty");
                Console.WriteLine("Press any key to return to Menu.");
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
            Console.WriteLine("Enter Shop name to search or \"0\" to go back: ");
            string? _ShopName = Console.ReadLine();
            if (_ShopName.ToLower() != "0")
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
                .WithColumn("ID", "Shop Name", "Address")
                .WithCharMapDefinition(CharMapDefinition.FrameDoublePipDefinition)
                .ExportAndWriteLine();
            Console.Write("Enter ID to view store or \"0\" to find another shop: ");
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
            Console.WriteLine("Enter ID to view order information.");
            Console.WriteLine("0. Back.");
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
                .WithColumn("ID", "Shop Name", "Quantity", "Into Money", "Status")
                .WithCharMapDefinition(CharMapDefinition.FrameDoublePipDefinition)
                .ExportAndWriteLine();
        }
        public void ViewOrdersToReceive(int _UserID)
        {
            List<Order> orders = orderBL.GetOrdersByStatusAndUserID("ToReceive", _UserID);
            if (orders.Count > 0)
            {
                DisplayOrders(_UserID, orders);
                Console.WriteLine($"Enter \"Confirm + ID \" to confirm pick up.");
                Console.WriteLine($"Enter \"Reject + ID \" to reject the order.");
                Console.WriteLine($"0. Back.");
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
                    Console.WriteLine("Successful confirmation");
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                    ViewOrdersToReceive(_UserID);
                }
                else if (IsNumber(choice.ToLower().Replace(" ", "").Replace("reject", "")))
                {
                    int rejectNo = Convert.ToInt32(choice.ToLower().Replace(" ", "").Replace("reject", ""));
                    orderBL.UpdateStatusOfOrder(orders[rejectNo-1].OrderID, "Failed");
                    Console.WriteLine("Successful refusal");
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                    ViewOrdersToReceive(_UserID);
                }
                else
                {
                    Console.WriteLine("Invalid selection !");
                    Console.ReadKey();
                    ViewOrdersToReceive(_UserID);
                }
            }
            else
            {
                Console.WriteLine("No orders ");
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
                MyOrder(_UserID);
            }
        }
    }
}