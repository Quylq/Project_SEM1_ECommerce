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
        ReadHelper readHelper = new ReadHelper();
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
            List<List<object>> tableData = new List<List<object>>();
            int count = 1;
            for (int i = 0; i < orders.Count; i++)
            {
                long total = orderBL.GetTotalOrder(orders[i].OrderID);
                User user = userBL.GetUserByID(orders[i].UserID);
                List<OrderDetails> orderDetailsList = orderDetailsBL.GetOrderDetailsListByOrderID(orders[i].OrderID);
                int _ProductNumber = 0;
                foreach (OrderDetails orderDetails in orderDetailsList)
                {
                    _ProductNumber += orderDetails.ProductNumber;
                }
                List<object> rowData = new List<object>{count++, user.FullName, _ProductNumber, total.ToString("C0"), orders[i].CreateDate, orders[i].Status};
                tableData.Add(rowData);
            }
            ConsoleTableBuilder
                .From(tableData)
                .WithTitle($"Order Management ", ConsoleColor.Yellow, ConsoleColor.DarkGray)
                .WithColumn("ID", "Customer", "Quantity", "Total", "CreateDate", "Status")
                .WithCharMapDefinition(CharMapDefinition.FrameDoublePipDefinition)
                .ExportAndWriteLine();
            Console.WriteLine("Enter \"ID\" to view corresponding order details");
            Console.WriteLine("0. Back.");
            Console.Write("Choose: ");
            int choice = readHelper.ReadInt(0, orders.Count);
            if (choice != 0)
            {
                ViewOrder(_ShopID, orders[choice - 1].OrderID);
            }
            else
            {
                ecommerce.SellerPage(_ShopID);
            }
        }
        public void ProductManagement(int _ShopID)
        {
            Console.Clear();
            Console.WriteLine("══════════ Product Management ══════════");
            Console.WriteLine("1. Search Product.");
            Console.WriteLine("2. Product All.");
            Console.WriteLine("3. Add Product.");
            Console.WriteLine("0. Back.");
            Console.WriteLine("════════════════════════════════════════");
            Console.Write("Choose: ");
            int choice = readHelper.ReadInt(0, 3);
            switch (choice)
            {
                case 1: 
                    SearchProductOfShop(_ShopID);
                    break; 
                case 2: 
                    List<Product> products = productBL.GetProductsByShopID(_ShopID);
                    DisplayProducts(_ShopID, products, "ProductManagement");
                    break;
                case 3: 
                    AddProduct(_ShopID);
                    break;
                case 0: 
                    ecommerce.SellerPage(_ShopID);
                    break;
            }
        }
        public void CategoryManagement(int _ShopID)
        {
            List<Category> categories = categoryBL.GetCategoriesByShopID(_ShopID);
            DisplayCategories(_ShopID, categories);
            Console.WriteLine($"Enter \"View + ID \" to view all products in the respective category.");
            Console.WriteLine($"Enter \"Add + ID \" to add the product to the respective category.");
            Console.WriteLine($"Enter \"Delete + ID \" to delete the corresponding category.");
            Console.WriteLine($"+. Add New Category.");
            Console.WriteLine($"0. Back.");
            Console.Write($"Choose: ");
            try
            {
                string? choice = readHelper.ReadString();
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
                    List<Product> products = productBL.GetProductsByCategory(categories[viewNo - 1].CategoryID);
                    DisplayProducts(_ShopID, products, "CategoryManagement");
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
            }
            catch (System.Exception)
            {
                Console.WriteLine("Invalid selection !");
                Console.ReadKey();
                CategoryManagement(_ShopID);
            }
        }
        public void SearchProductOfShop(int _ShopID)
        {
            Console.Clear();
            Console.WriteLine($"══════════ Search Product ══════════");
            Console.WriteLine("Enter product name to search or \"0\" to go back. ");
            string? _ProductName = readHelper.ReadString();
            if (_ProductName.ToLower() != "0")
            {
                List<Product> products = new List<Product>();
                products = productBL.GetProductsByNameAndShopID(_ProductName, _ShopID);
                DisplayProducts(_ShopID, products, "SearchProduct");
            }
            else
            {
                ProductManagement(_ShopID);
            }   
        }
        public void ProductInformation(int _ShopID, int _ProductID, string navigate, List<Product> products)
        {
            Product product = productBL.GetProductByID(_ProductID);
            List<Category> categories = categoryBL.GetCategoriesByProductID(_ProductID);
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
            Console.WriteLine("2. Update product quantity.");
            Console.WriteLine("3. Add Product to the category.");
            Console.WriteLine("0. Back.");
            Console.Write("Choose: ");
            int choice = readHelper.ReadInt(0, 3);
            if (choice == 1)
            {
                Console.Clear();
                Console.WriteLine($"══════════ Update Description ══════════");
                Console.Write("New Description: ");
                string? _Description = readHelper.ReadString();
                productBL.UpdateDescriptionOfProduct(_ProductID, _Description);
                Console.WriteLine("Update successful!");
                Console.WriteLine("Enter any key to continue");
                Console.ReadKey();
                ProductInformation(_ShopID, _ProductID, navigate, products);
            }
            else if (choice == 2)
            {
                Console.Clear();
                Console.WriteLine($"══════════ Update Quantity ══════════");
                Console.Write("Quantity: ");
                int _Quantity = readHelper.ReadInt(0, 999);
                productBL.UpdateQuantityOfProduct(_ProductID, _Quantity);
                Console.WriteLine("Update successful!");
                Console.WriteLine("Enter any key to continue");
                Console.ReadKey();
                ProductInformation(_ShopID, _ProductID, navigate, products);
            }
            else if (choice == 3)
            {
                AddProductToCategory(_ShopID, _ProductID);
                ProductInformation(_ShopID, _ProductID, navigate, products);
            }
            else
            {
                DisplayProducts(_ShopID, products, navigate);
            }

        }
        public void AddProduct(int _ShopID)
        {
            Console.Clear();
            Console.WriteLine($"══════════ Add Product ══════════");
            Console.WriteLine("Product Name: ");
            string _ProductName = readHelper.ReadString(500);
            Console.WriteLine("Price: ");
            int _Price = readHelper.ReadInt(1, Int32.MaxValue);
            Console.WriteLine("Quantity: ");
            int _Quantity = readHelper.ReadInt(1, 999);
            Console.WriteLine("Description: ");
            string _Description = readHelper.ReadString(2000);
            Product product = new Product(productBL.ProductIDMax() + 1, _ShopID, _ProductName, _Price, _Description, _Quantity);
            productBL.InsertProduct(product);
            AddProductToCategory(_ShopID, product.ProductID);
            Console.WriteLine("Add Product successfully !");
            Console.ReadKey();
            ProductManagement(_ShopID);
        }
        public void ViewOrder(int _ShopID, int _OrderID)
        {
            Order order = orderBL.GetOrderByID(_OrderID);
            List<OrderDetails> orderDetailsList = orderDetailsBL.GetOrderDetailsListByOrderID(_OrderID);
            User user = userBL.GetUserByID(order.UserID);
            Console.Clear();
            List<List<object>> tableData = new List<List<object>>();
            int count = 1;
            long total = orderBL.GetTotalOrder(_OrderID);
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
                .WithColumn("ID", "Product Name", "Price", "Quantity", "Status")
                .WithCharMapDefinition(CharMapDefinition.FrameDoublePipDefinition)
                .ExportAndWriteLine();
            if (order.Status == "Processing")
            {
                Console.WriteLine("1. Confirm.");
                Console.WriteLine("2. Refuse.");
                Console.WriteLine("0. Back.");
                Console.Write($"Choose: ");
                int choice = readHelper.ReadInt(0, 2);
                if (choice == 1)
                {
                    orderBL.UpdateStatusOfOrder(_OrderID, "ToReceive");
                    Console.WriteLine("Successful confirmation");
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                }
                else if (choice == 2)
                {
                    orderBL.UpdateStatusOfOrder(_OrderID, "Failed");
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
            OrderManagement(_ShopID);
        }
        public void DisplayProducts(int _ShopID, List<Product> products, string navigate)
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
                .WithColumn("ID", "Product Name", "Price", "Quantity")
                .WithCharMapDefinition(CharMapDefinition.FrameDoublePipDefinition)
                .ExportAndWriteLine();
            Console.Write("Enter \"ID\" to see the product information or \"0\" to back: ");
            int choice = readHelper.ReadInt(0, products.Count);
            if (choice != 0)
            {
                ProductInformation(_ShopID, products[choice - 1].ProductID, navigate, products);
            }
            else
            {
                if (navigate ==  "SearchProduct")
                {
                    SearchProductOfShop(_ShopID);
                }
                else if (navigate ==  "ProductManagement")
                {
                    ProductManagement(_ShopID);
                }
                else if (navigate ==  "CategoryManagement")
                {
                    CategoryManagement(_ShopID);
                }
            }
        }
        public void AddProductsToCategory(int _ShopID, int _CategoryID)
        {
            List<Product> products = GetProductsNotCategory(_ShopID, _CategoryID);
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
                .WithColumn("ID", "Product Name", "Price", "Quantity")
                .WithCharMapDefinition(CharMapDefinition.FrameDoublePipDefinition)
                .ExportAndWriteLine();
            Console.Write("Enter \"ID\" add the corresponding product to the category or \"0\" to go back: ");
            int choice = readHelper.ReadInt(0, products.Count);
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
        public void AddProductToCategory(int _ShopID, int _ProductID)
        {
            List<Category> categories = categoryBL.GetCategoriesByShopID(_ShopID);
            for (int i = 0; i < categories.Count; i++)
            {
                if (product_CategoryBL.CheckProduct_Category(_ProductID, categories[i].CategoryID))
                {
                    categories.RemoveAt(i);
                    i--;
                }
            }
            DisplayCategories(_ShopID, categories);
            Console.Write("Enter \"ID\" to add the product to the respective category or \"0\" to go back:");
            ProductBL productBL = new ProductBL();
            int choice = readHelper.ReadInt(0, categories.Count);
            if (choice != 0)
            {
                product_CategoryBL.InsertProduct_Category(_ProductID, categories[choice - 1].CategoryID);
                Console.WriteLine("successfully added product to category !");
                Console.ReadKey();
            }
        }
        public void CreateCategory(int _ShopID)
        {
            Console.Clear();
            Console.WriteLine($"══════════ Create Category ══════════");
            Console.Write("Category Name: ");
            string _CategoryName = readHelper.ReadString();
            Category category = new Category(_ShopID, _CategoryName);
            categoryBL.InsertCategory(category);
            Console.WriteLine("Add successful category");
            Console.ReadKey();
            CategoryManagement(_ShopID);
        }
        public void DisplayCategories(int _ShopID, List<Category> categories)
        {
            Console.Clear();
            List<List<object>> tableData = new List<List<object>>();
            int count = 1;
            foreach (Category category in categories)  
            {
                List<object> rowData = new List<object>{count++, category.CategoryName, categoryBL.GetProductNumberOfCategory(category.CategoryID)};
                tableData.Add(rowData);
            }
            ConsoleTableBuilder
                .From(tableData)
                .WithColumn("ID", "Category", "Product Number")
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
        public List<Product> GetProductsNotCategory(int _ShopID, int _CategoryID)
        {
            List<Product> products = productBL.GetProductsByShopID(_ShopID);
            for (int i = 0; i < products.Count; i++)
            {
                if (product_CategoryBL.CheckProduct_Category(products[i].ProductID, _CategoryID))
                {
                    products.RemoveAt(i);
                    i--;
                }
            }

            return products;
        }
    }
    
}