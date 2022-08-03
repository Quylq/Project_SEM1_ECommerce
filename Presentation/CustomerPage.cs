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
        public ReadHelper readHelper = new ReadHelper();
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
            Console.WriteLine($"══════════ Search Product ══════════");
            Console.WriteLine("Enter product name to search or \"0\" to go back. ");
            string? _ProductName = readHelper.ReadString(500);
            if (_ProductName != "0")
            {
                List<Product> products = productBL.GetProductsByName(_ProductName);
                if (products.Count > 0)
                {
                    DisplayProducts(_UserID, products, "SearchProduct");
                }
                else
                {
                    Console.WriteLine("Product not found, would you like to find it again? (Y/N)");
                    string choice = readHelper.ReadString();
                    if (choice.ToLower() == "y")
                    {
                        SearchProduct(_UserID);
                    }
                    else
                    {
                        ecommerce.CustomerPage(_UserID);
                    }
                }
            }
            else
            {
                ecommerce.CustomerPage(_UserID);
            }     
        }
        public void DisplayProducts(int _UserID, List<Product> products, string navigate)
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
                .WithColumn("ID", "Product Name", "Price", "Status")
                .WithTextAlignment(new Dictionary < int, TextAligntment>
                    {
                        {2, TextAligntment.Right },
                        {3, TextAligntment.Right }
                    })
                .WithCharMapDefinition(CharMapDefinition.FrameDoublePipDefinition)
                .ExportAndWriteLine();
            Console.Write("Enter \"ID\" to see the product information or \"0\" to back:");
            int choice = readHelper.ReadInt(0, products.Count);
            if (choice != 0)
            {
                Product product = productBL.GetProductByID(products[choice - 1].ProductID);
                ProductInformation(_UserID, product.ProductID, navigate, products);
            }
            else
            {
                if (navigate == "SearchProduct")
                {
                    SearchProduct(_UserID);
                }
                else if (navigate == "SearchShop")
                {
                    viewShop(_UserID, products[0].ShopID);
                }
            }
        }
        public void ProductInformation(int _UserID, int _ProductID, string navigate, List<Product> products)
        {
            Product product = productBL.GetProductByID(_ProductID);
            Shop shop = shopBL.GetShopByID(product.ShopID);
            string status = product.Quantity == 0? "Out of stock" : "Stocking";
            List<List<object>> tableData = new List<List<object>>
            {
                new List<object>{"Shop", shop.ShopName},
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
            List<Order> orders = orderBL.GetOrdersByStatusAndUserID("Shopping", _UserID);
            List<OrderDetails> oDList = orderDetailsBL.GetOrderDetailsListByUserIDAndStatus(_UserID, "Shopping");
            int ProductNumberOfCart = 0;
            if (CheckProductOfOrderDetails(oDList, _ProductID) != -1)
            {
                ProductNumberOfCart = oDList[CheckProductOfOrderDetails(oDList, _ProductID)].ProductNumber;
            }
            Console.WriteLine($"Enter Quantity of products to add to cart.");
            Console.WriteLine($"0. Back.");
            Console.Write($"Choose: ");
            int choice = readHelper.ReadInt(0 - ProductNumberOfCart, product.Quantity);
            if (choice != 0)
            {
                int i = CheckShopOfCart(orders, product.ShopID);
                if (i == -1)
                {
                    string format = "yyyy-MM-dd HH:mm:ss";
                    DateTime now = DateTime.Now;
                    Order order = new Order(orderBL.OrderIDMax() + 1, _UserID, product.ShopID, now.ToString(format), "Shopping");
                    orderBL.InsertOrder(order);
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
                if (choice > 0)
                {
                    Console.WriteLine($"Successfully added {choice} products from the cart.");
                }
                else
                {
                    Console.WriteLine($"Successfully removed {choice} products from cart.");
                }
                Console.WriteLine("Press any key to continue.");
                Console.ReadKey();
            }
            if (navigate == "ViewCart")
            {
                ViewCart(_UserID);
            }
            else
            {
                DisplayProducts(_UserID, products, navigate);
            }
        }
        public void ViewOrder(int _UserID, int _OrderID)
        {
            Console.Clear();
            List<List<object>> tableData = new List<List<object>>();
            int count = 1;
            Order order = orderBL.GetOrderByID(_OrderID);
            List<OrderDetails> orderDetailsList = orderDetailsBL.GetOrderDetailsListByOrderID(_OrderID);
            Shop shop = shopBL.GetShopByID(order.ShopID);
            long total = orderBL.GetTotalOrder(_OrderID);
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
                .WithColumn("Shop", "ID", "Product", "Price", "Quantity", "Total")
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
                Console.WriteLine("2. Reject orders.");
                Console.WriteLine("0. Back.");
                string? choice = readHelper.ReadString();
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
            Console.WriteLine($"══════════ {shopBL.GetShopByID(_ShopID).ShopName} ══════════");           
            Console.WriteLine("1. Product All");
            List<Category> categories = categoryBL.GetCategoriesByShopID(_ShopID);
            int count = 2;
            foreach (Category category in categories)
            {
                Console.WriteLine($"{count++}. {category.CategoryName}");
            }
            Console.WriteLine("0. Back.");
            for (int i = 0; i < shopBL.GetShopByID(_ShopID).ShopName.Length; i++)
            {
                Console.Write("═");
            }
            Console.WriteLine($"══════════════════════");
            Console.Write("Choose: ");
            try
            {
                int choice = readHelper.ReadInt(0, categories.Count + 1);
                if (choice == 0)
                {
                    SearchShop(_UserID);
                }
                else
                {
                    List<Product> products = new List<Product>();
                    if (choice == 1)
                    {
                        products = productBL.GetProductsByShopID(_ShopID);
                    }
                    else
                    {
                        products = productBL.GetProductsByCategory(categories[choice - 2].CategoryID);
                    }
                    DisplayProducts(_UserID, products, "SearchShop");
                }
            }
            catch (System.Exception)
            {
                viewShop(_UserID, _ShopID);
            }
        }
        public void ViewCart(int _UserID)
        {
            List<Order> orders = orderBL.GetOrdersByStatusAndUserID("Shopping", _UserID);
            List<OrderDetails> orderDetailsList = orderDetailsBL.GetOrderDetailsListByUserIDAndStatus(_UserID, "Shopping");
            Console.Clear();
            if (orders.Count > 0 && orderDetailsList.Count > 0)
            {
                List<List<object>> tableData = new List<List<object>>();
                int count = 1;
                long total = orderBL.GetTotalCart(_UserID);
                foreach (OrderDetails orderDetails in orderDetailsList)  
                { 
                    if (orderDetails.ProductNumber > 0)
                    {
                        Order order = orderBL.GetOrderByID(orderDetails.OrderID);
                        Shop shop = shopBL.GetShopByID(order.ShopID);
                        Product product = productBL.GetProductByID(orderDetails.ProductID);
                        List<object> rowData = new List<object>{shop.ShopName, count++, product.ProductName, product.Price.ToString("C0"), orderDetails.ProductNumber, orderDetailsBL.GetTotalOrderDetails(orderDetails).ToString("C0")};
                        tableData.Add(rowData);
                    }
                }
                List<object> rowTotal = new List<object>{"", "", "", "", "", total.ToString("C0")};
                tableData.Add(rowTotal);
                ConsoleTableBuilder
                    .From(tableData)
                    .WithColumn("Shop", "ID", "Product", "Price", "Quantity", "Total")
                    .WithTextAlignment(new Dictionary < int, TextAligntment>
                        {
                            {4, TextAligntment.Right },
                            {3, TextAligntment.Right },
                            {5, TextAligntment.Right }
                        })
                    .WithCharMapDefinition(CharMapDefinition.FrameDoublePipDefinition)
                    .ExportAndWriteLine();
                Console.WriteLine("Enter ID to view product information, 0 to return or \"Order\" to order");
                Console.Write($"Choose: ");
                string? choice = readHelper.ReadString();
                if ( readHelper.IsNumber(choice))
                {
                    int num = Convert.ToInt32(choice);
                    if (num == 0)
                    {
                        ecommerce.CustomerPage(_UserID);
                    }
                    else if (num > 0 && num <= orderDetailsList.Count)
                    {
                        List<Product> products = new List<Product>();
                        ProductInformation(_UserID, orderDetailsList[num - 1].ProductID, "ViewCart", products);
                    }
                }
                if (choice.ToLower() == "order")
                {
                    foreach (Order order in orders)
                    {
                        List<OrderDetails> orderDetailsList1 = orderDetailsBL.GetOrderDetailsListByOrderID(order.OrderID);
                        if (orderDetailsList1.Count > 0)
                        {
                            string format = "yyyy-MM-dd HH:mm:ss";
                            DateTime now = DateTime.Now;
                            orderBL.UpdateCreateDateOfOrder(order.OrderID, now.ToString(format));
                            orderBL.UpdateStatusOfOrder(order.OrderID, "Processing");
                            Console.WriteLine("Order Success");
                            Console.WriteLine("Press any key to continue.");
                            Console.ReadKey();
                        }
                    }
                    ecommerce.CustomerPage(_UserID);
                }
                else
                {
                    Console.WriteLine("Invalid selection!");
                    Console.ReadKey();
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
        public int CheckShopOfCart(List<Order> orders, int _ShopID)
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
            Console.WriteLine("══════════ Search Shop ══════════");
            Console.WriteLine("Enter Shop name to search or \"0\" to go back: ");
            string? _ShopName = readHelper.ReadString(50);
            if (_ShopName.ToLower() != "0")
            {
                List<Shop> shops = new List<Shop>();
                shops = shopBL.GetShopsByName(_ShopName);
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
                int choice = readHelper.ReadInt(0, shops.Count);
                if (choice != 0)
                {
                    viewShop(_UserID, shops[choice - 1].ShopID);
                }
                else
                {
                    SearchShop(_UserID);
                }
            }
            else
            {
                ecommerce.CustomerPage(_UserID);
            }
        }
        public void MyOrder(int _UserID)
        {
            List<Order> orders = orderBL.GetOrdersByUserID(_UserID);
            Console.Clear();
            List<List<object>> tableData = new List<List<object>>();
            int count = 1;
            for (int i = 0; i < orders.Count; i++)
            {
                long total = orderBL.GetTotalOrder(orders[i].OrderID);
                Shop shop = shopBL.GetShopByID(orders[i].ShopID);
                List<OrderDetails> orderDetailsList = orderDetailsBL.GetOrderDetailsListByOrderID(orders[i].OrderID);
                int _ProductNumber = 0;
                foreach (OrderDetails orderDetails in orderDetailsList)
                {
                    _ProductNumber += orderDetails.ProductNumber;
                }
                if (_ProductNumber != 0)
                {
                    List<object> rowData = new List<object>{count++, shop.ShopName, _ProductNumber, total.ToString("C0"), orders[i].Status};
                    tableData.Add(rowData);
                }
                else
                {
                    orders.RemoveAt(i);
                    i--;
                }
            }
            ConsoleTableBuilder
                .From(tableData)
                .WithColumn("ID", "Shop Name", "Quantity", "Total", "Status")
                .WithCharMapDefinition(CharMapDefinition.FrameDoublePipDefinition)
                .ExportAndWriteLine();
            Console.WriteLine("Enter ID to view order information.");
            Console.WriteLine("0. Back.");
            int choice = readHelper.ReadInt(0 , orders.Count);
            if (choice != 0)
            {
                ViewOrder(_UserID, orders[choice - 1].OrderID);
            }
            else
            {
                ecommerce.CustomerPage(_UserID);
            }
        }
        public void PersonalInformation(int _UserID)
        {
            Console.Clear();
            User user = userBL.GetUserByID(_UserID);
            Address address = addressBL.GetAddressByID(user.AddressID);
            List<List<object>> tableData = new List<List<object>>
            {
                new List<object>{"Full Name", user.FullName},
                new List<object>{"Birthday", user.Birthday},
                new List<object>{"Email", user.Email},
                new List<object>{"Phone", user.Phone},
                new List<object>{"Address", address.ToString()},
            };
            ConsoleTableBuilder
                    .From(tableData)
                    .WithTitle("Personal Information ", ConsoleColor.Yellow, ConsoleColor.DarkGray)
                    .WithCharMapDefinition(CharMapDefinition.FrameDoublePipDefinition)
                    .ExportAndWriteLine();
            Console.WriteLine("1. Change Information");
            Console.WriteLine("2. Change Password");
            Console.WriteLine("0. Back");
            int choice = readHelper.ReadInt(0, 2);
            if (choice == 0)
            {
                ecommerce.CustomerPage(_UserID);
            }
            else if (choice == 1)
            {
                ChangeInformation(_UserID);
            }
            else
            {
                ChangePassword(_UserID);
            }
        }
        public void ChangeInformation(int _UserID)
        {
            Console.Clear();
            Console.WriteLine("══════════ Change Information ══════════");
            Console.Write("You Name: ");
            string _FullName = readHelper.ReadString(100);
            Console.Write("Email: ");
            string _Email = readHelper.ReadEmail();
            Console.Write("Phone: ");
            string _Phone = readHelper.ReadPhone();
            Console.Write("Birthday: ");
            string _Birthday = readHelper.ReadDateOnly();
            int _AddressID = readHelper.ReadAddress().AddressID;
            User user = new User(_UserID, _FullName, _Birthday, _Email, _Phone, _AddressID, "Customer");
            userBL.UpdateUser(user);
            Console.WriteLine("Change Information Success!");
            Console.ReadKey();
            PersonalInformation(_UserID);
        }
        public void ChangePassword(int _UserID)
        {
            Console.Clear();
            Console.WriteLine("══════════ Change Password ══════════");
            int count = 1;
            ChangePassword1:
            Console.Write("Old Password: ");
            string OldPassword = readHelper.ReadPassword();
            User user = userBL.GetUserByID(_UserID);
            if (user.Password == OldPassword)
            {
                Console.Write("New Password: ");
                string NewPassword = readHelper.ReadPassword();
                userBL.UpdatePassword(_UserID, NewPassword);
                Console.WriteLine("Change Password Success!");
                Console.ReadKey();
                ecommerce.Menu();
            }
            else
            {
                Console.WriteLine($"wrong password!");
                count++;
                Console.ReadKey();
                if (count <= 3)
                {
                    goto ChangePassword1;
                }
                else
                {
                    Console.WriteLine("You enter bad password too 3 times!");
                    Console.ReadKey();
                    PersonalInformation(_UserID);
                }
            }
            
        }
    }
}