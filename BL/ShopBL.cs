using DAL;
using Persistence;
using ConsoleTableExt;

namespace BL;
public static class ShopBL
{
    public static string title = "ECOMMERCE";
    public static void OrderManagement(this Shop shop)
    {
        OrderDAL orderDAL = new OrderDAL();
        BLHelper bLHelper = new BLHelper();
        UserDAL userDAL = new UserDAL();
        OrderDetailsDAL orderDetailsDAL = new OrderDetailsDAL();

        Console.Clear();
        List<Order>? orders = orderDAL.GetOrdersByShopID(shop.ShopID);
        if (orders != null)
        {
            List<List<object>> tableData = new List<List<object>>();
            int count = 1;
            for (int i = 0; i < orders.Count; i++)
            {
                int _Quantity = orderDetailsDAL.GetQuantityOfOrderID(orders[i].OrderID);
                if (_Quantity > 0)
                {
                    User user = userDAL.GetUserByID(orders[i].UserID);
                    long total = bLHelper.GetTotalOrder(orders[i].OrderID);
                    List<object> rowData = new List<object>{count++, user.FullName, _Quantity, total.ToString("0,0 vnđ"), orders[i].CreateDate, orders[i].Status};
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
                .WithTitle($"Order Management ", ConsoleColor.Yellow, ConsoleColor.DarkGray)
                .WithColumn("ID", "Customer", "Quantity", "Total", "Time", "Status")
                .WithCharMapDefinition(CharMapDefinition.FrameDoublePipDefinition)
                .ExportAndWriteLine();
            Console.WriteLine("Enter \"ID\" to view corresponding order details");
            Console.WriteLine("0. Back.");
            Console.Write("Choose: ");
            int choice = ReadHelper.ReadInt(0, orders.Count);
            if (choice != 0)
            {
                shop.ViewOrder(orders[choice - 1].OrderID);
            }
        }
        else
        {
            Console.WriteLine("Order Empty!");
            Console.ReadKey();
        }
    }
    public static void ProductManagement(this Shop shop)
    {
        ProductDAL productDAL = new ProductDAL();

        Console.Clear();
        Console.WriteLine("══════════ Product Management ══════════");
        Console.WriteLine("1. Search Product.");
        Console.WriteLine("2. Product All.");
        Console.WriteLine("3. Add Product.");
        Console.WriteLine("0. Back.");
        Console.WriteLine("════════════════════════════════════════");
        Console.Write("Choose: ");
        int choice = ReadHelper.ReadInt(0, 3);
        switch (choice)
        {
            case 1: 
                shop.SearchProductOfShop();
                shop.ProductManagement();
                break; 
            case 2: 
                List<Product>? products = productDAL.GetProductsByShopID(shop.ShopID);
                shop.DisplayProducts(products);
                shop.ProductManagement();
                break;
            case 3: 
                shop.AddProduct();
                break;
            case 0: 
                break;
        }
    }
    public static void CategoryManagement(this Shop shop)
    {
        CategoryDAL categoryDAL = new CategoryDAL();
        ProductDAL productDAL = new ProductDAL();
        Product_CategoryDAL product_CategoryDAL = new Product_CategoryDAL();

        List<Category> categories = categoryDAL.GetCategoriesByShopID(shop.ShopID);
        shop.DisplayCategories(categories);
        Console.WriteLine($"Enter \"View + ID \" to view all products in the respective category.");
        Console.WriteLine($"Enter \"Add + ID \" to add the product to the respective category.");
        Console.WriteLine($"Enter \"Delete + ID \" to delete the corresponding category.");
        Console.WriteLine($"+. Add New Category.");
        Console.WriteLine($"0. Back.");
        Console.Write($"Choose: ");
        try
        {
            string choice = ReadHelper.ReadString();
            if (choice == "0")
            {
                
            }
            else if (choice == "+")
            {
                shop.CreateCategory();
                shop.CategoryManagement();
            }
            else if (IsNumber(choice.ToLower().Replace(" ", "").Replace("view", "")))
            {
                int viewNo = Convert.ToInt32(choice.ToLower().Replace(" ", "").Replace("view", ""));
                List<Product> products = productDAL.GetProductsByCategory(categories[viewNo - 1].CategoryID);
                shop.DisplayProducts(products);
                shop.CategoryManagement();
            }
            else if (IsNumber(choice.ToLower().Replace(" ", "").Replace("add", "")))
            {
                int addNo = Convert.ToInt32(choice.ToLower().Replace(" ", "").Replace("add", ""));
                shop.AddProductsToCategory(categories[addNo - 1].CategoryID);
                shop.CategoryManagement();
            }
            else if (IsNumber(choice.ToLower().Replace(" ", "").Replace("delete", "")))
            {
                int deleteNo = Convert.ToInt32(choice.ToLower().Replace(" ", "").Replace("delete", ""));
                product_CategoryDAL.DeleteProduct_CategoryByCategoryID(categories[deleteNo - 1].CategoryID);
                categoryDAL.DeleteCategoryByID(categories[deleteNo - 1].CategoryID);
                Console.WriteLine("Delete category successfully !");
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
                shop.CategoryManagement();
            }
        }
        catch (System.Exception)
        {
            Console.WriteLine("Invalid selection !");
            Console.ReadKey();
            shop.CategoryManagement();
        }
    }
    public static void SearchProductOfShop(this Shop shop)
    {
        ProductDAL productDAL = new ProductDAL();

        Console.Clear();
        Console.WriteLine(title);
        Console.WriteLine();
        Console.WriteLine($"══════════ Search Product ══════════");
        Console.WriteLine("Enter product name to search or \"0\" to go back. ");
        string? _ProductName = ReadHelper.ReadString();
        if (_ProductName != "0")
        {
            List<Product> products = productDAL.GetProductsByNameAndShopID(_ProductName, shop.ShopID);
            shop.DisplayProducts(products);
        }  
    }
    public static void ProductInformation(this Shop shop, int _ProductID)
    {
        ProductDAL productDAL = new ProductDAL();
        CategoryDAL categoryDAL = new CategoryDAL();
        Product product = productDAL.GetProductByID(_ProductID);
        List<Category> categories = categoryDAL.GetCategoriesByProductID(_ProductID);
        Console.Clear();
        string _ListCategory = "";
        foreach (Category category in categories)
        {
            _ListCategory = String.Concat(_ListCategory, ", ", category.CategoryName);
        }
        char[] charsToTrim = {' ', ','};
        _ListCategory = _ListCategory.Trim(charsToTrim);
        List<List<object>> tableData = new List<List<object>>
        {
            new List<object>{"Product", product.ProductName},
            new List<object>{"Price", product.Price.ToString("0,0 vnđ")},
            new List<object>{"Amount", product.Amount},
            new List<object>{"Category", _ListCategory}
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
        Console.WriteLine("1. Update product description.");
        Console.WriteLine("2. Update product amount.");
        Console.WriteLine("3. Add Product to the category.");
        Console.WriteLine("0. Back.");
        Console.Write("Choose: ");
        int choice = ReadHelper.ReadInt(0, 3);
        if (choice == 1)
        {
            Console.Clear();
            Console.WriteLine($"══════════ Update Description ══════════");
            Console.Write("New Description: ");
            string _Description = ReadHelper.ReadString();
            productDAL.UpdateDescriptionOfProduct(_ProductID, _Description);
            Console.WriteLine("Update successful!");
            Console.WriteLine("Enter any key to continue");
            Console.ReadKey();
            shop.ProductInformation(_ProductID);
        }
        else if (choice == 2)
        {
            Console.Clear();
            Console.WriteLine($"══════════ Update Amount ══════════");
            Console.Write("Amount: ");
            int _Amount = ReadHelper.ReadInt(0, 999);
            productDAL.UpdateAmountOfProduct(_ProductID, _Amount);
            Console.WriteLine("Update successful!");
            Console.WriteLine("Enter any key to continue");
            Console.ReadKey();
            shop.ProductInformation(_ProductID);
        }
        else if (choice == 3)
        {
            shop.AddProductToCategory(_ProductID);
            shop.ProductInformation(_ProductID);
        }
    }
    public static void AddProduct(this Shop shop)
    {
        ProductDAL productDAL = new ProductDAL();

        Console.Clear();
        Console.WriteLine($"══════════ Add Product ══════════");
        Console.WriteLine("Product Name: ");
        string _ProductName = ReadHelper.ReadString(500);
        Console.WriteLine("Price: ");
        int _Price = ReadHelper.ReadInt(1, Int32.MaxValue);
        Console.WriteLine("Amount: ");
        int _Amount = ReadHelper.ReadInt(1, 999);
        Console.WriteLine("Description: ");
        string _Description = ReadHelper.ReadString(2000);
        Product product = new Product(productDAL.ProductIDMax() + 1, shop.ShopID, _ProductName, _Price, _Description, _Amount);
        productDAL.InsertProduct(product);
        shop.AddProductToCategory(product.ProductID);
        Console.WriteLine("Add Product successfully !");
        Console.ReadKey();
        shop.ProductManagement();
    }
    public static void ViewOrder(this Shop shop, int _OrderID)
    {
        UserDAL userDAL = new UserDAL();
        OrderDAL orderDAL = new OrderDAL();
        BLHelper bLHelper   = new BLHelper();
        ProductDAL productDAL = new ProductDAL();
        OrderDetailsDAL orderDetailsDAL = new OrderDetailsDAL();
        AddressDAL addressDAL = new AddressDAL();

        Order order = orderDAL.GetOrderByID(_OrderID);
        List<OrderDetails>? orderDetailsList = orderDetailsDAL.GetOrderDetailsListByOrderID(_OrderID);
        User user = userDAL.GetUserByID(order.UserID);
        Address address = addressDAL.GetAddressByID(user.AddressID);
        Console.Clear();
        Console.WriteLine(title);
        Console.WriteLine();
        Console.WriteLine($"Customer        : {user.FullName}");
        Console.WriteLine($"Customer address: {address}");
        Console.WriteLine($"Invoice date    : {order.CreateDate}");
        List<List<object>> tableData = new List<List<object>>();
        int count = 1;
        long total = bLHelper.GetTotalOrder(_OrderID);
        if (orderDetailsList != null)
        {
            foreach (OrderDetails orderDetails in orderDetailsList)  
            {
                Product product = productDAL.GetProductByID(orderDetails.ProductID);
                List<object> rowData = new List<object>{count++, product.ProductName, product.Price.ToString("0,0 vnđ"), orderDetails.Quantity, bLHelper.GetTotalOrderDetails(orderDetails).ToString("0,0 vnđ")};
                tableData.Add(rowData);
            }
            List<object> rowData1 = new List<object>{"", "", "", "", total.ToString("0,0 vnđ")};
            tableData.Add(rowData1);
        }
        ConsoleTableBuilder
            .From(tableData)
            .WithColumn("ID", "Product Name", "Unit Price", "Quantity", "Status")
            .WithCharMapDefinition(CharMapDefinition.FrameDoublePipDefinition)
            .ExportAndWriteLine();
        if (order.Status == "Processing")
        {
            Console.WriteLine("1. Confirm.");
            Console.WriteLine("2. Refuse.");
            Console.WriteLine("0. Back.");
            Console.Write($"Choose: ");
            int choice = ReadHelper.ReadInt(0, 2);
            if (choice == 1)
            {
                orderDAL.UpdateStatusOfOrder(_OrderID, "ToReceive");
                Console.WriteLine("Successful confirmation");
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
            }
            else if (choice == 2)
            {
                // orderDAL.UpdateStatusOfOrder(_OrderID, "Failed");
                orderDAL.OrderFailed(order.OrderID);
                Console.WriteLine("Canceled order successfully");
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
            }
        }
        else
        {
            Console.WriteLine("Press any key to go back");
            Console.ReadKey();
        }
        shop.OrderManagement();
    }
    public static void DisplayProducts(this Shop shop, List<Product>? products, int page = 1)
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
            foreach (Product product in products1)  
            {
                List<object> rowData = new List<object>{count++, product.ProductName, product.Price.ToString("0,0 vnđ"), product.Amount};
                tableData.Add(rowData);
            }
            ConsoleTableBuilder
                .From(tableData)
                .WithColumn("ID", "Product Name", "Price", "Amount")
                .WithCharMapDefinition(CharMapDefinition.FrameDoublePipDefinition)
                .ExportAndWriteLine();
            Console.WriteLine($"Page {page}/{pages} (total: {products.Count} product)");
            Console.Write("Enter \"ID\" to see the product information or \"0\" to back: ");
            InputKey:
            string choice = ReadHelper.ReadChoice();
            if (choice == "prev")
            {
                if (page > 1)
                {
                    shop.DisplayProducts(products, --page);
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
                    shop.DisplayProducts(products, ++page);
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
                    shop.ProductInformation(product.ProductID);
                    shop.DisplayProducts(products, page);
                }
                else if (temp != 0)
                {
                    Console.WriteLine($"Numbers outside the range [0, {products1.Count}].");
                    goto InputKey;
                }
            }
        }
    }
    public static void AddProductsToCategory(this Shop shop, int _CategoryID)
    {
        Product_CategoryDAL product_CategoryDAL = new Product_CategoryDAL();

        List<Product> products = shop.GetProductsNotCategory( _CategoryID);
        Console.Clear();
        List<List<object>> tableData = new List<List<object>>();
        int count = 1;
        foreach (Product product in products)  
        {
            List<object> rowData = new List<object>{count++, product.ProductName, product.Price.ToString("0,0 vnđ"), product.Amount};
            tableData.Add(rowData);
        }
        ConsoleTableBuilder
            .From(tableData)
            .WithColumn("ID", "Product Name", "Price", "Amount")
            .WithCharMapDefinition(CharMapDefinition.FrameDoublePipDefinition)
            .ExportAndWriteLine();
        Console.Write("Enter \"ID\" add the corresponding product to the category or \"0\" to go back: ");
        int choice = ReadHelper.ReadInt(0, products.Count);
        if (choice != 0)
        {
            product_CategoryDAL.InsertProduct_Category(products[choice - 1].ProductID, _CategoryID);
            Console.WriteLine("successfully added product to category!");
            Console.WriteLine("Enter any key to continue");
            Console.ReadKey();
            shop.AddProductsToCategory(_CategoryID);
        }
        else
        {
            shop.CategoryManagement();
        }
    }
    public static void AddProductToCategory(this Shop shop, int _ProductID)
    {
        CategoryDAL categoryDAL = new CategoryDAL();
        Product_CategoryDAL product_CategoryDAL = new Product_CategoryDAL();

        List<Category> categories = categoryDAL.GetCategoriesByShopID(shop.ShopID);
        for (int i = 0; i < categories.Count; i++)
        {
            if (product_CategoryDAL.CheckProduct_Category(_ProductID, categories[i].CategoryID))
            {
                categories.RemoveAt(i);
                i--;
            }
        }
        shop.DisplayCategories(categories);
        Console.Write("Enter \"ID\" to add the product to the respective category or \"0\" to go back:");
        int choice = ReadHelper.ReadInt(0, categories.Count);
        if (choice != 0)
        {
            product_CategoryDAL.InsertProduct_Category(_ProductID, categories[choice - 1].CategoryID);
            Console.WriteLine("successfully added product to category !");
            Console.ReadKey();
        }
    }
    public static void CreateCategory(this Shop shop)
    {
        CategoryDAL categoryDAL = new CategoryDAL();

        Console.Clear();
        Console.WriteLine($"══════════ Create Category ══════════");
        Console.Write("Category Name: ");
        string _CategoryName = ReadHelper.ReadString();
        Category category = new Category(shop.ShopID, _CategoryName);
        categoryDAL.InsertCategory(category);
        Console.WriteLine("Add successful category");
        Console.ReadKey();
        shop.CategoryManagement();
    }
    public static void DisplayCategories(this Shop shop, List<Category> categories)
    {
        CategoryDAL categoryDAL = new CategoryDAL();

        Console.Clear();
        List<List<object>> tableData = new List<List<object>>();
        int count = 1;
        foreach (Category category in categories)  
        {
            List<object> rowData = new List<object>{count++, category.CategoryName, categoryDAL.GetProductNumberOfCategory(category.CategoryID)};
            tableData.Add(rowData);
        }
        ConsoleTableBuilder
            .From(tableData)
            .WithColumn("ID", "Category", "Quantity")
            .WithCharMapDefinition(CharMapDefinition.FrameDoublePipDefinition)
            .ExportAndWriteLine();
    }
    public static bool IsNumber(string pValue)
    {
        foreach (Char c in pValue)
        {
            if (!Char.IsDigit(c))
            return false;
        }
        return true;
    }
    public static List<Product> GetProductsNotCategory(this Shop shop, int _CategoryID)
    {
        Product_CategoryDAL product_CategoryDAL = new Product_CategoryDAL();
        ProductDAL productDAL = new ProductDAL();
        
        List<Product>? products = productDAL.GetProductsByShopID(shop.ShopID);
        for (int i = 0; i < products!.Count; i++)
        {
            if (product_CategoryDAL.CheckProduct_Category(products[i].ProductID, _CategoryID))
            {
                products.RemoveAt(i);
                i--;
            }
        }

        return products;
    }
}