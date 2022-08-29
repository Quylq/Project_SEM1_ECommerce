using DAL;
using Persistence;
using ConsoleTableExt;

namespace BL;
public static class CustomerBL
{
    public static string title = "ECOMMERCE";
    public static Shop? SigUpShop(this User user)
    {
        ShopDAL shopDAL = new ShopDAL();
        Console.Clear();
        Console.WriteLine(title);
        Console.WriteLine();
        Console.WriteLine("══════════ Register to open a store ══════════");
        Console.Write("Shop Name      : ");
        string _ShopName = ReadHelper.ReadString(50);
        int _AddressID = ReadHelper.ReadAddress().AddressID;
        int _ShopID = shopDAL.ShopIDMax() + 1;
        Shop shop = new Shop(_ShopID, _ShopName, user.UserID, _AddressID);
        if (shopDAL.InsertShop(shop))
        {
            Console.WriteLine("Create a successful store.");
            Console.WriteLine("Press any key to enter the store");
            Console.ReadKey();
            return shop;
        }
        else
        {
            Console.WriteLine("Store registration failed.");
            Console.WriteLine("Press any key to back.");
            Console.ReadKey();
            return null;
        }
    }
    public static void SearchProduct(this User user)
    {
        ProductDAL productDAL = new ProductDAL();
        Console.Clear();
        Console.WriteLine(title);
        Console.WriteLine();
        Console.WriteLine($"══════════ Search Product ══════════");
        Console.WriteLine("Enter product name to search or \"0\" to go back. ");
        string _ProductName = ReadHelper.ReadString(500);
        if (_ProductName != "0")
        {
            List<Product>? products = productDAL.GetProductsByName(_ProductName);
            if (products != null)
            {
                user.DisplayProducts(products);
            }
            else
            {
                Console.WriteLine("Product not found, Press any key to continue!");
                Console.ReadKey();
                user.SearchProduct();
            }
        }  
    }
    public static void DisplayProducts(this User user, List<Product>? products, int page = 1)
    {
        ProductDAL productDAL = new ProductDAL();
        Console.Clear();
        Console.WriteLine(title);
        Console.WriteLine();
        if (products != null)
        {
            int size = 10;
            int pages = (int)Math.Ceiling((double)products.Count / size);
            var products1 = products.Skip((page - 1) * size).Take(size).ToList();
            List<List<object>> tableData = new List<List<object>>();

            int count = 1;
            for (int i = 0; i < products1.Count; i++)
            {
                string status = products1[i].Amount == 0? "Out of stock" : "Stocking";
                List<object> rowData = new List<object>{count++, products1[i].ProductName, products1[i].Price.ToString("0,0 vnđ"), status};
                tableData.Add(rowData);
            }
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
            Console.WriteLine($"Page {page}/{pages} (total: {products.Count} product)");
            Console.WriteLine("← : previous page\n→ : next page");
            Console.Write("Enter \"ID\" to see the product information or \"0\" to back:");
            InputKey:
            // ConsoleKeyInfo info = Console.ReadKey(true);
            // if (info.Key == ConsoleKey.RightArrow)
            // {
            //     if (page < pages)
            //     {
            //         user.DisplayProducts(products, ++page);
            //     }
            //     else
            //     {
            //         goto InputKey;
            //     }
            // }
            // else if (info.Key == ConsoleKey.LeftArrow)
            // {
            //     if (page > 1)
            //     {
            //         user.DisplayProducts(products, --page);
            //     }
            //     else
            //     {
            //         goto InputKey;
            //     }
            // }
            // else
            // {
            //     int choice = ReadHelper.ReadInt(0, products1.Count);
            //     if (choice != 0)
            //     {
            //         Product product = productDAL.GetProductByID(products1[choice - 1].ProductID);
            //         user.ProductInformation(product);
            //         user.DisplayProducts(products, page);
            //     }
            // }
            string choice = ReadHelper.ReadChoice();
            if (choice == "prev")
            {
                if (page > 1)
                {
                    user.DisplayProducts(products, --page);
                }
                else
                {
                    goto InputKey;
                }
            }
            else if (choice == "next")
            {
                if (page < pages)
                {
                    user.DisplayProducts(products, ++page);
                }
                else
                {
                    goto InputKey;
                }
            }
            else
            {
                int temp = Convert.ToInt32(choice);
                if (temp > 0 && temp <= products1.Count)
                {
                    Product product = productDAL.GetProductByID(products1[temp - 1].ProductID);
                    user.ProductInformation(product);
                    user.DisplayProducts(products, page);
                }
                else if (temp != 0)
                {
                    Console.WriteLine($"Numbers outside the range [0, {products1.Count}].");
                    goto InputKey;
                }
            }
        }
    }
    public static void ProductInformation(this User user, Product product)
    {
        ShopDAL shopDAL = new ShopDAL();
        OrderDAL orderDAL = new OrderDAL();
        OrderDetailsDAL orderDetailsDAL = new OrderDetailsDAL();
        Shop shop = shopDAL.GetShopByID(product.ShopID);
        string status = product.Amount == 0? "Out of stock" : "Stocking";
        List<List<object>> tableData = new List<List<object>>
        {
            new List<object>{"Shop", shop.ShopName},
            new List<object>{"Product Name", product.ProductName},
            new List<object>{"Price", product.Price.ToString("0,0 vnđ")},
            new List<object>{"Amount", product.Amount}
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
        int QuantityInCart = orderDetailsDAL.GetQuantityInCart(user.UserID, product.ProductID);
        Console.Write($"Enter Amount of products to add to cart ");
        Console.WriteLine($"(Shopping cart: {QuantityInCart})");
        Console.WriteLine($"0. Back.");
        Console.Write($"Choose: ");
        int choice = ReadHelper.ReadInt(0 - QuantityInCart, product.Amount - QuantityInCart);
        if (choice != 0)
        {
            // Nếu trong giỏ hàng chưa có sản phẩm của shop này, tạo order mới
            if (orderDetailsDAL.GetOrderIDOfCart(user.UserID, product.ShopID) == 0)
            {
                string format = "yyyy-MM-dd HH:mm:ss";
                DateTime now = DateTime.Now;
                Order order = new Order(orderDAL.OrderIDMax() + 1, user.UserID, product.ShopID, now.ToString(format), "Shopping");
                OrderDetails orderDetails = new OrderDetails(order.OrderID, product.ProductID, choice);
                
                Console.WriteLine($"Successfully added {choice} products from the cart.");
                orderDAL.InsertOrder(order);
                orderDetailsDAL.InsertOrderDetails(orderDetails);
            }
            else
            {          
                int _OrderID =  orderDetailsDAL.GetOrderIDOfCart(user.UserID, product.ShopID);
                OrderDetails orderDetails = new OrderDetails(_OrderID, product.ProductID, QuantityInCart + choice);
                // Nếu trong giỏ hàng chưa có sản phẩm này
                if (!orderDetailsDAL.CheckProductOfCart(user.UserID, product.ProductID))
                {
                    Console.WriteLine($"Successfully added {choice} products from the cart.");
                    orderDetailsDAL.InsertOrderDetails(orderDetails);
                }
                else
                {
                    if (choice > 0)
                    {
                        Console.WriteLine($"Successfully added {choice} products from the cart.");
                    }
                    else
                    {
                        Console.WriteLine($"Successfully removed {-choice} products from cart.");
                    }
                    orderDetailsDAL.UpdateOrderDetails(orderDetails);
                }
            }
            Console.ReadKey();
        }
    }
    public static void SearchShop(this User user)
    {
        ShopDAL shopDAL = new ShopDAL();
        AddressDAL addressDAL = new AddressDAL();

        Console.Clear();
        Console.WriteLine(title);
        Console.WriteLine();
        Console.WriteLine("══════════ Search Shop ══════════");
        Console.WriteLine("Enter Shop name to search or \"0\" to go back: ");
        string _ShopName = ReadHelper.ReadString(50);
        if (_ShopName != "0")
        {
            List<Shop>? shops = shopDAL.GetShopsByName(_ShopName);
            if (shops != null)
            {
                user.DisplayShops(shops);
            }
            else
            {
                Console.WriteLine("Shop not found, Press any key to continue!");
                Console.ReadKey();
                user.SearchShop();
            }
        }     
    }
    public static void DisplayShops(this User user, List<Shop>? shops)
    {
        AddressDAL addressDAL = new AddressDAL();

        if (shops != null)
        {
            Console.Clear();
            List<List<object>> tableData = new List<List<object>>();
            int count = 1;
            foreach (Shop shop in shops)  
            { 
                Address address = addressDAL.GetAddressByID(shop.AddressID);
                List<object> rowData = new List<object>{count++, shop.ShopName, address.City};
                tableData.Add(rowData);
            }
            ConsoleTableBuilder
                .From(tableData)
                .WithColumn("ID", "Shop Name", "Address")
                .WithCharMapDefinition(CharMapDefinition.FrameDoublePipDefinition)
                .ExportAndWriteLine();
            Console.Write("Enter ID to view store or \"0\" to find another shop: ");
            int choice = ReadHelper.ReadInt(0, shops.Count);
            if (choice != 0)
            {
                user.ViewShop(shops[choice - 1].ShopID);
                user.DisplayShops(shops);
            }
        }
    }
    public static void ViewShop(this User user, int _ShopID)
    {
        ShopDAL shopDAL = new ShopDAL();
        CategoryDAL categoryDAL = new CategoryDAL();
        ProductDAL productDAL = new ProductDAL();

        Shop shop = shopDAL.GetShopByID(_ShopID);
        Console.Clear();
        Console.WriteLine($"══════════ {shop.ShopName} ══════════");           
        Console.WriteLine("1. All Products");
        List<Category> categories = categoryDAL.GetCategoriesByShopID(_ShopID);
        int count = 2;
        foreach (Category category in categories)
        {
            Console.WriteLine($"{count++}. {category.CategoryName}");
        }
        Console.WriteLine("0. Back.");
        for (int i = 0; i < shop.ShopName.Length; i++)
        {
            Console.Write("═");
        }
        Console.WriteLine($"══════════════════════");
        Console.Write("Choose: ");
        int choice = ReadHelper.ReadInt(0, categories.Count + 1);
        if (choice != 0)
        {
            List<Product>? products = new List<Product>();
            if (choice == 1)
            {
                products = productDAL.GetProductsByShopID(_ShopID);
            }
            else
            {
                products = productDAL.GetProductsByCategory(categories[choice - 2].CategoryID);
            }
            user.DisplayProducts(products);
            user.ViewShop(_ShopID);
        }
    }
    public static void ViewCart(this User user)
    {
        OrderDAL orderDAL = new OrderDAL();
        OrderDetailsDAL orderDetailsDAL = new OrderDetailsDAL();
        ShopDAL shopDAL = new ShopDAL();
        ProductDAL productDAL = new ProductDAL();
        BLHelper bLHelper = new BLHelper();

        List<Order>? orders = orderDAL.GetOrdersByStatusAndUserID("Shopping", user.UserID);
        List<OrderDetails>? orderDetailsList = orderDetailsDAL.GetOrderDetailsListByUserIDAndStatus(user.UserID, "Shopping");
        Console.Clear();
        Console.WriteLine(title);
        Console.WriteLine();
        if ( orderDetailsList != null)
        {
            List<List<object>> tableData = new List<List<object>>();
            int count = 1;
            long total = bLHelper.GetTotalCart(user.UserID);
            foreach (OrderDetails orderDetails in orderDetailsList)  
            { 
                Order order = orderDAL.GetOrderByID(orderDetails.OrderID);
                Shop shop = shopDAL.GetShopByID(order.ShopID);
                Product product = productDAL.GetProductByID(orderDetails.ProductID);
                List<object> rowData = new List<object>{shop.ShopName, count++, product.ProductName, product.Price.ToString("0,0 vnđ"), orderDetails.Quantity, bLHelper.GetTotalOrderDetails(orderDetails).ToString("0,0 vnđ")};
                tableData.Add(rowData);
            }
            List<object> rowTotal = new List<object>{"", "", "", "", "", total.ToString("0,0 vnđ")};
            tableData.Add(rowTotal);
            ConsoleTableBuilder
                .From(tableData)
                .WithTitle("Shopping Cart", ConsoleColor.Yellow, ConsoleColor.DarkGray)
                .WithColumn("Shop", "ID", "Product", "Price", "Amount", "Total")
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
            string choice = ReadHelper.ReadString();
            if ( ReadHelper.IsNumber(choice))
            {
                int num = Convert.ToInt32(choice);
                if (num > 0 && num <= orderDetailsList.Count)
                {
                    Product product = productDAL.GetProductByID(orderDetailsList[num - 1].ProductID);
                    user.ProductInformation(product);
                    user.ViewCart();
                }
                else if (num < 0 || num > orderDetailsList.Count)
                {
                    Console.WriteLine("Invalid selection!");
                    Console.ReadKey();
                    user.ViewCart();
                }
            }
            else if (choice.ToLower() == "order")
            {
                foreach (Order order in orders!)
                {
                    string format = "yyyy-MM-dd HH:mm:ss";
                    DateTime now = DateTime.Now;
                    orderDAL.OrderingProcess(order.OrderID, now.ToString(format));
                }
                Console.WriteLine("Order Success");
                Console.ReadKey();
                foreach (Order order in orders!)
                {
                    user.Invoice(order);
                }
            }
            else
            {
                Console.WriteLine("Invalid selection!");
                Console.ReadKey();
                user.ViewCart();
            }
        }
        else
        {
            Console.WriteLine("Cart is empty");
            Console.WriteLine("Press any key to return to Menu.");
            Console.ReadKey();
        }
    }
    public static void Invoice(this User user, Order order)
    {
        OrderDetailsDAL orderDetailsDAL = new OrderDetailsDAL();
        BLHelper bLHelper = new BLHelper();
        ShopDAL shopDAL = new ShopDAL();
        ProductDAL productDAL = new ProductDAL();
        AddressDAL addressDAL = new AddressDAL();

        Console.Clear();
        Console.WriteLine(title);
        Console.WriteLine();
        List<OrderDetails>? orderDetailsList = orderDetailsDAL.GetOrderDetailsListByOrderID(order.OrderID);
        Shop shop = shopDAL.GetShopByID(order.ShopID);
        Address addressShop = addressDAL.GetAddressByID(shop.AddressID);
        Address addressCustomer = addressDAL.GetAddressByID(user.AddressID);
        Console.WriteLine($"Invoice");
        Console.WriteLine($"════════════════");
        Console.WriteLine($"Shop            : {shop.ShopName}");
        Console.WriteLine($"Shop address    : {addressShop}");
        Console.WriteLine($"Customer        : {user.FullName}");
        Console.WriteLine($"Customer address: {addressCustomer}");
        Console.WriteLine($"Invoice date    : {order.CreateDate}");

        List<List<object>> tableData = new List<List<object>>();
        int count = 1;
        for (int i = 0; i < orderDetailsList!.Count; i++)
        {
            Product product = productDAL.GetProductByID(orderDetailsList[i].ProductID);
            long total = bLHelper.GetTotalOrderDetails(orderDetailsList[i]);
            List<object> rowData = new List<object>{count++, product.ProductName, product.Price.ToString("0,0 vnđ"), orderDetailsList[i].Quantity, total.ToString("0,0 vnđ")};
            tableData.Add(rowData);
        }
        long subTotal = bLHelper.GetTotalOrder(order.OrderID);
        List<object> rowTotal = new List<object>{"", "", "", "", subTotal.ToString("0,0 vnđ")};
        tableData.Add(rowTotal);
        ConsoleTableBuilder
            .From(tableData)
            .WithColumn("ID", "Product Name", "Unit Price", "Quantity", "Total")
            .WithCharMapDefinition(CharMapDefinition.FrameDoublePipDefinition)
            .ExportAndWriteLine();
        Console.ReadKey();
    }
    public static void MyOrder(this User user)
    {
        OrderDAL orderDAL = new OrderDAL();
        OrderDetailsDAL orderDetailsDAL = new OrderDetailsDAL();
        BLHelper bLHelper = new BLHelper();
        ShopDAL shopDAL = new ShopDAL();

        List<Order>? orders = orderDAL.GetOrdersByUserID(user.UserID);
        Console.Clear();
        Console.WriteLine(title);
        Console.WriteLine();
        List<List<object>> tableData = new List<List<object>>();
        int count = 1;
        for (int i = 0; i < orders.Count; i++)
        {
            long total = bLHelper.GetTotalOrder(orders[i].OrderID);
            Shop shop = shopDAL.GetShopByID(orders[i].ShopID);
            List<OrderDetails>? orderDetailsList = orderDetailsDAL.GetOrderDetailsListByOrderID(orders[i].OrderID);
            if (orderDetailsList != null)
            {
                List<object> rowData = new List<object>{count++, shop.ShopName, orderDetailsList.Count, total.ToString("0,0 vnđ"), orders[i].CreateDate, orders[i].Status};
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
            .WithTitle("My Order", ConsoleColor.Yellow, ConsoleColor.DarkGray)
            .WithColumn("ID", "Shop Name", "Quantity", "Total", "Time", "Status")
            .WithCharMapDefinition(CharMapDefinition.FrameDoublePipDefinition)
            .ExportAndWriteLine();
        Console.WriteLine("Enter ID to view order information.");
        Console.WriteLine("0. Back.");
        int choice = ReadHelper.ReadInt(0 , orders.Count);
        if (choice != 0)
        {
            user.ViewOrder(orders[choice - 1].OrderID);
        }
    }
    public static void ViewOrder(this User user, int _OrderID)
    {
        OrderDAL orderDAL = new OrderDAL();
        OrderDetailsDAL orderDetailsDAL = new OrderDetailsDAL();
        ShopDAL shopDAL = new ShopDAL();
        BLHelper bLHelper = new BLHelper();
        ProductDAL productDAL = new ProductDAL();
        AddressDAL addressDAL = new AddressDAL();

        Console.Clear();
        Order order = orderDAL.GetOrderByID(_OrderID);
        Shop shop = shopDAL.GetShopByID(order.ShopID);
        Address addressShop = addressDAL.GetAddressByID(shop.AddressID);
        Console.WriteLine($"Shop            : {shop.ShopName}");
        Console.WriteLine($"Shop address    : {addressShop}");
        Console.WriteLine($"Invoice date    : {order.CreateDate}");
        List<List<object>> tableData = new List<List<object>>();
        int count = 1;
        
        List<OrderDetails>? orderDetailsList = orderDetailsDAL.GetOrderDetailsListByOrderID(_OrderID);
        long total = bLHelper.GetTotalOrder(_OrderID);
        if (orderDetailsList != null)
        {
            foreach (OrderDetails orderDetails in orderDetailsList)  
            { 
                Product product = productDAL.GetProductByID(orderDetails.ProductID);
                List<object> rowData = new List<object>{count++, product.ProductName, product.Price.ToString("0,0 vnđ"), orderDetails.Quantity, bLHelper.GetTotalOrderDetails(orderDetails).ToString("0,0 vnđ")};
                tableData.Add(rowData);
            }
        }
        List<object> rowTotal = new List<object>{"", "", "", "", total.ToString("0,0 vnđ")};
        tableData.Add(rowTotal);
        ConsoleTableBuilder
            .From(tableData)
            .WithColumn("ID", "Product Name", "Unit Price", "Quantity", "Total")
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
            string? choice = ReadHelper.ReadString();
            if (choice == "1")
            {
                orderDAL.UpdateStatusOfOrder(order.OrderID, "Finished");
                Console.WriteLine("Pick up successfully");
                Console.WriteLine("Press any key to continue.");
                Console.ReadKey();
            }
            else if (choice == "2")
            {
                // orderDAL.UpdateStatusOfOrder(order.OrderID, "Failed");
                orderDAL.OrderFailed(order.OrderID);
                Console.WriteLine("Successfully refused order");
                Console.WriteLine("Press any key to continue.");
                Console.ReadKey();
            }
            user.MyOrder();
        }
        else
        {
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
            user.MyOrder();
        }
    }
    public static void PersonalInformation(this User user)
    {
        UserDAL userDAL = new UserDAL();
        AddressDAL addressDAL = new AddressDAL();

        Console.Clear();
        Console.WriteLine(title);
        Console.WriteLine();
        Address address = addressDAL.GetAddressByID(user.AddressID);
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
        int choice = ReadHelper.ReadInt(0, 2);
        if (choice == 1)
        {
            user.ChangeInformation();
        }
        else if (choice == 2)
        {
            user.ChangePassword();
        }
    }
    public static void ChangeInformation(this User user)
    {
        UserDAL userDAL = new UserDAL();

        Console.Clear();
        Console.WriteLine("══════════ Change Information ══════════");
        Console.Write("You Name       : ");
        string _FullName = ReadHelper.ReadString(100);
        Console.Write("Email          : ");
        string _Email = ReadHelper.ReadEmail();
        Console.Write("Phone          : ");
        string _Phone = ReadHelper.ReadPhone();
        Console.Write("Birthday       : ");
        string _Birthday = ReadHelper.ReadDateOnly();
        int _AddressID = ReadHelper.ReadAddress().AddressID;
        user = new User(user.UserID, user.UserName, user.Password, _FullName, _Birthday, _Email, _Phone, _AddressID, "Customer");
        userDAL.UpdateUser(user);
        Console.WriteLine("Change Information Success!");
        Console.ReadKey();
        user.PersonalInformation();
    }
    public static void ChangePassword(this User user)
    {
        UserDAL userDAL = new UserDAL();

        Console.Clear();
        Console.WriteLine("══════════ Change Password ══════════");
        int count = 1;
        ChangePassword1:
        Console.Write("Old Password   : ");
        string OldPassword = ReadHelper.ReadPassword(); 
        if (user.Password == OldPassword)
        {
            Console.Write("New Password   : ");
            string NewPassword = ReadHelper.ReadPassword();
            user.Password = NewPassword;
            userDAL.UpdatePassword(user.UserID, user.Password);
            Console.WriteLine("Change Password Success!");
            Console.ReadKey();
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
                user.PersonalInformation();
            }
        }
    }
    
}