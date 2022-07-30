using BL;
using ConsoleTableExt;


namespace Persistence
{
    public class SellerPage
    {
        private UserBL userBL;
        private ProductBL productBL;
        private OrderBL orderBL;
        private CategoryBL categoryBL;
        private OrderDetailsBL orderDetailsBL;
        private Product_CategoryBL product_CategoryBL;
        private ShopBL shopBL;
        private AddressBL addressBL;
        Ecommerce ecommerce = new Ecommerce();
        public SellerPage()
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
        public void OrderManagement(int _ShopID)
        {
            Console.Clear();
            List<Order> orders = orderBL.GetOrdersByShopIDAndNotStatus(_ShopID, "Shopping");
            DisplayOrders(_ShopID, orders);
            Console.WriteLine("Enter Order Number to view corresponding order details");
            Console.WriteLine("0. Back.");
            Console.Write("Choose: ");
            try
            {
                int choice = Convert.ToInt32(Console.ReadLine());
                if (choice != 0)
                {
                    ViewOrder(_ShopID, orders[choice - 1].OrderID);
                }
                else
                {
                    ecommerce.SellerPage(_ShopID);
                }
            }
            catch (System.Exception)
            {
                OrderManagement(_ShopID);
            }
        }
        public void ProductManagement(int _ShopID)
        {
            Console.Clear();
            Console.WriteLine("------------- Product Management -----------------");
            Console.WriteLine("1. Search Product.");
            Console.WriteLine("2. Product All.");
            Console.WriteLine("3. Add Product.");
            Console.WriteLine("0. Back.");
            Console.Write("Choose: ");
            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1": 
                    SearchProductOfShop(_ShopID);
                    break; 
                case "2": 
                    ViewAllProducts(_ShopID);
                    break;
                case "3": 
                    AddProduct(_ShopID);
                    break;
                case "0": 
                    ecommerce.SellerPage(_ShopID);
                    break;
                default:
                    Console.WriteLine("Please choose 0 - 3 !");
                    ProductManagement(_ShopID);
                    break;
            }
        }
        public void CategoryManagement(int _ShopID)
        {
            DisplayCategories(_ShopID);
            List<Category> categories = categoryBL.GetCategoriesByShopID(_ShopID);
            Console.WriteLine($"Enter \"View + Serial Number \" to view all products in the respective category.");
            Console.WriteLine($"Enter \"Add + Serial Number \" to add the product to the respective category.");
            Console.WriteLine($"Enter \"Delete + Serial Number \" to delete the corresponding category.");
            Console.WriteLine($"+. Add New Category.");
            Console.WriteLine($"0. Back.");
            Console.Write($"Choose: ");
            string? choice = Console.ReadLine();
            if (choice == "0")
            {
                ecommerce.SellerPage(_ShopID);
            }
            else if (choice == "+")
            {
                CreateCategory(_ShopID);
            }
            else if (IsNumber(choice.ToLower().Replace(" ", "").Replace("view", "")))
            {
                int viewNo = Convert.ToInt32(choice.ToLower().Replace(" ", "").Replace("view", ""));
                ViewProductsOfCategory(_ShopID, categories[viewNo - 1].CategoryID);
            }
            else if (IsNumber(choice.ToLower().Replace(" ", "").Replace("add", "")))
            {
                int addNo = Convert.ToInt32(choice.ToLower().Replace(" ", "").Replace("add", ""));
                AddProductsToCategory(_ShopID, categories[addNo - 1].CategoryID);
            }
            else if (IsNumber(choice.ToLower().Replace(" ", "").Replace("delete", "")))
            {
                int deleteNo = Convert.ToInt32(choice.ToLower().Replace(" ", "").Replace("delete", ""));
                product_CategoryBL.DeleteProduct_CategoryByCategoryID(categories[deleteNo - 1].CategoryID);
                categoryBL.DeleteCategoryByID(categories[deleteNo - 1].CategoryID);
                Console.WriteLine("Delete category successfully !");
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
                CategoryManagement(_ShopID);
            }
            else
            {
                Console.WriteLine("Invalid selection !");
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
                CategoryManagement(_ShopID);
            }
        }
        public void SearchProductOfShop(int _ShopID)
        {
            Console.Clear();
            Console.WriteLine("Enter the product you want to find: ");
            string? _ProductName = Console.ReadLine();
            List<Product> products = new List<Product>();
            products = productBL.GetProductsByNameAndShopID(_ProductName, _ShopID);
            Console.Clear();
            DisplayProducts(_ShopID, products);

        }
        
        public void ProductInformation(int _ShopID, int _ProductID)
        {
            Product product = productBL.GetProductByID(_ProductID);
            List<Category> categories = categoryBL.GetCategoriesByProductID(_ProductID);
            Console.Clear();
            string _ListCategory = "";
            foreach (Category category in categories)
            {
                _ListCategory = String.Concat(_ListCategory, " ", category.CategoryName);
            }
            List<List<object>> tableData = new List<List<object>>
            {
                new List<object>{"Product", product.ProductName},
                new List<object>{"Price", product.Price.ToString("C0")},
                new List<object>{"Quantity", product.Quantity},
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
            ProductInformation:
            Console.WriteLine("1. Update product description.");
            Console.WriteLine("2. Update product quantity.");
            Console.WriteLine("3. Add Product to the category.");
            Console.WriteLine("0. Back.");
            Console.Write("Choose: ");
            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1": 
                    UpdateDescription(_ShopID, _ProductID);
                    break;
                case "2":
                    UpdateQuantity(_ShopID, _ProductID);
                    break; 
                case "3":
                    AddProductToCategory(_ShopID, _ProductID);
                    ProductManagement(_ShopID);
                    break; 
                case "0":
                    ProductManagement(_ShopID);
                    break;
                default:
                    Console.WriteLine("Please choose 0 - 2 !");
                    goto ProductInformation;
            }
        }

        public void UpdateDescription(int _ShopID, int _ProductID)
        {
            Console.Clear();
            Console.Write("New Description: ");
            string? _Description = Console.ReadLine();
            productBL.UpdateDescriptionOfProduct(_ProductID, _Description);
            Console.WriteLine("Update successful!");
            Console.WriteLine("Enter any key to continue");
            Console.ReadKey();
            ProductInformation(_ShopID, _ProductID);
        }

        public void UpdateQuantity(int _ShopID, int _ProductID)
        {
            Console.Clear();
            Console.Write("Quantity: ");
            int _Quantity = Convert.ToInt32(Console.ReadLine());
            productBL.UpdateQuantityOfProduct(_ProductID, _Quantity);
            Console.WriteLine("Update successful!");
            Console.WriteLine("Enter any key to continue");
            Console.ReadKey();
            ProductInformation(_ShopID, _ProductID);
        }
        
        public void AddProduct(int _ShopID)
        {
            try
            {
                Console.Clear();
                Console.WriteLine("Product Name: ");
                string? _ProductName = Console.ReadLine();
                Console.WriteLine("Price: ");
                int _Price = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Quantity: ");
                int _Quantity = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Description: ");
                string? _Description = Console.ReadLine();
                Product product = new Product(productBL.ProductIDMax() + 1, _ShopID, _ProductName, _Price, _Description, _Quantity);
                productBL.InsertProduct(product);
                AddProductToCategory(_ShopID, product.ProductID);
                Console.WriteLine("Add Product successfully !");
                Console.WriteLine("Press any key to continue !");
                Console.ReadKey();
                ProductManagement(_ShopID);
            }
            catch (System.Exception)
            {
                Console.WriteLine("Invalid data !");
                Console.ReadKey();
                AddProduct(_ShopID);
            }
        }
        public void DisplayOrders(int _ShopID, List<Order> orders)
        {
            Console.Clear();
            List<List<object>> tableData = new List<List<object>>();
            int count = 1;
            for (int i = 0; i < orders.Count; i++)
            {
                int total = orderBL.GetTotalOrder(orders[i].OrderID);
                User user = userBL.GetUserByID(orders[i].UserID);
                List<OrderDetails> orderDetailsList = orderDetailsBL.GetOrderDetailsListByOrderID(orders[i].OrderID);
                List<object> rowData = new List<object>{count++, user.FullName, orderDetailsList.Count, total.ToString("C0"), orders[i].Status};
                tableData.Add(rowData);
            }
            ConsoleTableBuilder
                .From(tableData)
                .WithColumn("STT", "Customer", "Quantity", "Total", "Status")
                .WithCharMapDefinition(CharMapDefinition.FrameDoublePipDefinition)
                .ExportAndWriteLine();
        }
        public void ViewOrder(int _ShopID, int _OrderID)
        {
            Order order = orderBL.GetOrderByID(_OrderID);
            List<OrderDetails> orderDetailsList = orderDetailsBL.GetOrderDetailsListByOrderID(_OrderID);
            User user = userBL.GetUserByID(order.UserID);
            Console.Clear();
            List<List<object>> tableData = new List<List<object>>();
            int count = 1;
            int total = orderBL.GetTotalOrder(_OrderID);
            foreach (OrderDetails orderDetails in orderDetailsList)  
            {
                if (orderDetails.ProductNumber > 0)
                {
                    Product product = productBL.GetProductByID(orderDetails.ProductID);
                    List<object> rowData = new List<object>{count++, product.ProductName, product.Price.ToString("C0"), orderDetails.ProductNumber, (product.Price * orderDetails.ProductNumber).ToString("C0")};
                    tableData.Add(rowData);
                }
            }
            List<object> rowData1 = new List<object>{"", "", "", "", total.ToString("C0")};
            tableData.Add(rowData1);
            ConsoleTableBuilder
                .From(tableData)
                .WithTitle($"Customer: {user.FullName} ", ConsoleColor.Yellow, ConsoleColor.DarkGray)
                .WithColumn("STT", "Product Name", "Price", "Quantity", "Status")
                .WithCharMapDefinition(CharMapDefinition.FrameDoublePipDefinition)
                .ExportAndWriteLine();
            if (order.Status == "Processing")
            {
                Console.WriteLine("1. Confirm.");
                Console.WriteLine("2. Refuse.");
                Console.WriteLine("0. Back.");
                Console.Write($"Choose: ");
                string? choice = Console.ReadLine();
                if (choice.ToLower() == "2")
                {
                    orderBL.UpdateStatusOfOrder(_OrderID, "Failed");
                    Console.WriteLine("Canceled order successfully");
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                    OrderManagement(_ShopID);
                }
                else if (choice.ToLower() == "1")
                {
                    orderBL.UpdateStatusOfOrder(_OrderID, "ToReceive");
                    Console.WriteLine("Successful confirmation");
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                    OrderManagement(_ShopID);
                }
                else if (choice == "0")
                {
                    OrderManagement(_ShopID);
                }
                else
                {
                    Console.WriteLine("Please choose 0 - 2");
                    Console.ReadKey();
                    ViewOrder(_ShopID, _OrderID);
                }
            }
            else
            {
                Console.WriteLine("Press any key to go back");
                Console.ReadKey();
                OrderManagement(_ShopID);
            }
        }
        public void DisplayProducts(int _ShopID, List<Product> products)
        {
            Console.Clear();
            List<List<object>> tableData = new List<List<object>>();
            int count = 1;
            foreach (Product product in products)  
            {
                List<object> rowData = new List<object>{count++, product.ProductName, product.Price.ToString("C0"), product.Quantity};
                tableData.Add(rowData);
            }
            ConsoleTableBuilder
                .From(tableData)
                .WithColumn("STT", "Product Name", "Price", "Quantity")
                .WithCharMapDefinition(CharMapDefinition.FrameDoublePipDefinition)
                .ExportAndWriteLine();
            Console.Write("Enter the sequence number to view details or \"0\" to go back: ");
            try
            {
                int choice = Convert.ToInt32(Console.ReadLine());
                if (choice != 0)
                {
                    ProductInformation(_ShopID, products[choice - 1].ProductID);
                }
                else
                {
                    ProductManagement(_ShopID);
                }
            }
            catch (System.Exception)
            {
                Console.Clear();
                DisplayProducts(_ShopID, products);
            }
        }
        public void ViewAllProducts(int _ShopID)
        {
            List<Product> products = productBL.GetProductsByShopID(_ShopID);
            DisplayProducts(_ShopID, products);
        }
        public void ViewProductsOfCategory(int _ShopID, int _CategoryID)
        {
            List<Product> products = productBL.GetProductsByCategory(_CategoryID);
            DisplayProducts(_ShopID, products);
        }
        public void AddProductsToCategory(int _ShopID, int _CategoryID)
        {
            List<Product> products = GetProductsByShopIDAndCategoryID(_ShopID, _CategoryID);
            Console.Clear();
            List<List<object>> tableData = new List<List<object>>();
            int count = 1;
            foreach (Product product in products)  
            {
                List<object> rowData = new List<object>{count++, product.ProductName, product.Price.ToString("C0"), product.Quantity};
                tableData.Add(rowData);
            }
            ConsoleTableBuilder
                .From(tableData)
                .WithColumn("STT", "Product Name", "Price", "Quantity")
                .WithCharMapDefinition(CharMapDefinition.FrameDoublePipDefinition)
                .ExportAndWriteLine();
            Console.Write("Enter the corresponding serial number to add the product to the Category or \"0\" to go back: ");
            try
            {
                int choice = Convert.ToInt32(Console.ReadLine());
                if (choice != 0)
                {
                    product_CategoryBL.InsertProduct_Category(products[choice - 1].ProductID, _CategoryID);
                    Console.WriteLine("successfully added product to category!");
                    Console.WriteLine("Enter any key to continue");
                    Console.ReadKey();
                    AddProductsToCategory(_ShopID, _CategoryID);
                }
                else
                {
                    CategoryManagement(_ShopID);
                }
            }
            catch (System.Exception)
            {
                Console.Clear();
                AddProductsToCategory(_ShopID, _CategoryID);
            }
        }
        public void DisplayProductsOfCategory(int _ShopID)
        {
            List<Category> categories = categoryBL.GetCategoriesByShopID(_ShopID);
            DisplayCategories(_ShopID);
            Console.Write("Enter the order number to view the product or \"0\" to go back: ");
            try
            {
                int choice = Convert.ToInt32(Console.ReadLine());
                if (choice != 0)
                {
                    List<Product> products = productBL.GetProductsByCategory(categories[choice - 1].CategoryID);
                    DisplayProducts(_ShopID, products);
                }
                else
                {
                    CategoryManagement(_ShopID);
                }
            }
            catch (System.Exception)
            {
                Console.Clear();
                DisplayProductsOfCategory(_ShopID);
            }
        }
        public void AddProductToCategory(int _ShopID, int _ProductID)
        {
            List<Category> categories = categoryBL.GetCategoriesByShopID(_ShopID);
            DisplayCategories(_ShopID);
            Console.Write("Enter the order number to add the product to an existing catalog or 0 to go back : ");
            try
            {
                ProductBL productBL = new ProductBL();
                int choice = Convert.ToInt32(Console.ReadLine());
                if (choice != 0)
                {
                    product_CategoryBL.InsertProduct_Category(_ProductID, categories[choice - 1].CategoryID);
                    Console.WriteLine("successfully added product to category !");
                    Console.WriteLine("Enter any key to continue");
                    Console.ReadKey();
                }
                else
                {
                    ProductManagement(_ShopID);
                }
            }
            catch (System.Exception)
            {
                Console.Clear();
                AddProductToCategory(_ShopID, _ProductID);
            }
        }
        public void CreateCategory(int _ShopID)
        {
            Console.WriteLine("Category Name: ");
            string? _CategoryName = Console.ReadLine();
            Category category = new Category(_ShopID, _CategoryName);
            categoryBL.InsertCategory(category);
            Console.WriteLine("Add successful category");
            Console.WriteLine("Press any key to continue ");
            Console.ReadKey();
            CategoryManagement(_ShopID);
        }
        public void DisplayCategories(int _ShopID)
        {
            Console.Clear();
            List<List<object>> tableData = new List<List<object>>();
            List<Category> categories = categoryBL.GetCategoriesByShopID(_ShopID);
            int count = 1;
            foreach (Category category in categories)  
            {
                List<object> rowData = new List<object>{count++, category.CategoryName};
                tableData.Add(rowData);
            }
            ConsoleTableBuilder
                .From(tableData)
                .WithColumn("STT", "Category")
                .WithCharMapDefinition(CharMapDefinition.FrameDoublePipDefinition)
                .ExportAndWriteLine();
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
        public bool CheckProductOfCategory(int _ProductID, int _CategoryID)
        {
            bool result = false;
            List<Category> categories = categoryBL.GetCategoriesByProductID(_ProductID);
            for (int i = 0; i < categories.Count; i++)
            {
                if (categories[i].CategoryID == _CategoryID)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }
        public List<Product> GetProductsByShopIDAndCategoryID(int _ShopID, int _CategoryID)
        {
            List<Product> products = productBL.GetProductsByShopID(_ShopID);
            List<Product> productsNew = new List<Product>();
            foreach (Product product in products)  
            {
                if (!CheckProductOfCategory(product.ProductID, _CategoryID))
                {
                    productsNew.Add(product);
                }
                
            }
            return productsNew;
        }
    }
}