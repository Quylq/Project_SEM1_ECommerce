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
                    ViewOrdersProcessing(_ShopID);
                    break; 
                case "2":
                    ViewOrdersToReceive(_ShopID);
                    break;
                case "3":
                    ViewOrdersFinished(_ShopID);
                    break;
                case "4":
                    ViewOrdersFailed(_ShopID);
                    break;
                case "0":
                    ecommerce.SellerPage(_ShopID);
                    break;
                default:
                    Console.WriteLine("Vui lòng chọn 0 - 4 !");
                    OrderManagement(_ShopID);
                    break;
            }
        }
        public void ProductManagement(int _ShopID)
        {
            Console.Clear();
            Console.WriteLine("1. Tìm kiếm sản phẩm.");
            Console.WriteLine("2. Tất cả sản phẩm.");
            Console.WriteLine("3. Thêm sản phẩm.");
            Console.WriteLine("0. Quay lại.");
            Console.Write("Chọn: ");
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
                    Console.WriteLine("Vui lòng chọn 0 - 3 !");
                    ProductManagement(_ShopID);
                    break;
            }
        }
        public void CategoryManagement(int _ShopID)
        {
            DisplayCategories(_ShopID);
            List<Category> categories = categoryBL.GetCategoriesByShopID(_ShopID);
            Console.WriteLine($"Nhập \"View + Số thứ tự \" xem tất cả sản phẩm trong danh mục tương ứng.");
            Console.WriteLine($"Nhập \"Add + Số thứ tự \" để thêm sản phẩm vào danh mục tương ứng.");
            Console.WriteLine($"Nhập \"Delete + Số thứ tự \" để xóa danh mục tương ứng.");
            Console.WriteLine($"+. Thêm Danh mục mới.");
            Console.WriteLine($"0. Quay lại.");
            Console.Write($"Chọn: ");
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
                Console.WriteLine("Xóa danh mục thành công !");
                Console.WriteLine("Nhấn phím bất kỳ để tiếp tục");
                Console.ReadKey();
                CategoryManagement(_ShopID);
            }
            else
            {
                Console.WriteLine("Lựa chọn không hợp lệ !");
                Console.WriteLine("Nhấn phím bất kỳ để tiếp tục");
                Console.ReadKey();
                CategoryManagement(_ShopID);
            }
        }
        public void SearchProductOfShop(int _ShopID)
        {
            Console.Clear();
            Console.WriteLine("Nhập sản phẩm bạn muốn tìm: ");
            string? _ProductName = Console.ReadLine();
            List<Product> products = new List<Product>();
            products = productBL.GetProductsByNameAndShopID(_ProductName, _ShopID);
            Console.Clear();
            DisplayProducts(_ShopID, products);

        }
        
        public void ProductInformation(int _ShopID, int _ProductID)
        {
            Product product = productBL.GetProductByID(_ProductID);
            Console.Clear();
            Console.WriteLine($"Tên sản Phẩm: {product.ProductName}");
            Console.WriteLine($"Giá: {product.Price.ToString("C0")}");
            Console.WriteLine($"Mô tả: {product.Description}");
            Console.WriteLine($"Số lượng: {product.Quantity}");
            List<Category> categories = categoryBL.GetCategoriesByProductID(_ProductID);
            Console.Write($"Danh mục: ");
            foreach (Category category in categories)
            {
                Console.Write($"{category.CategoryName} ");
            }
            Console.WriteLine("\n-----------------------------------------");
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
                    Console.WriteLine("Vui lòng chọn 0 - 2 !");
                    goto ProductInformation;
            }
        }

        public void UpdateDescription(int _ShopID, int _ProductID)
        {
            Console.Clear();
            Console.Write("Mô tả mới: ");
            string? _Description = Console.ReadLine();
            productBL.UpdateDescriptionOfProduct(_ProductID, _Description);
            Console.WriteLine("Cập nhật thành công!");
            Console.WriteLine("Nhập phím bất kỳ để tiếp tục");
            Console.ReadKey();
            ProductInformation(_ShopID, _ProductID);
        }

        public void UpdateQuantity(int _ShopID, int _ProductID)
        {
            Console.Clear();
            Console.Write("Số lượng sản phẩm còn: ");
            int _Quantity = Convert.ToInt32(Console.ReadLine());
            productBL.UpdateQuantityOfProduct(_ProductID, _Quantity);
            Console.WriteLine("Cập nhật thành công!");
            Console.WriteLine("Nhập phím bất kỳ để tiếp tục");
            Console.ReadKey();
            ProductInformation(_ShopID, _ProductID);
        }
        
        public void AddProduct(int _ShopID)
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
            Product product = new Product(productBL.ProductIDMax() + 1, _ShopID, _ProductName, _Price, _Description, _Quantity);
            productBL.InsertProduct(product);
            AddProductToCategory(_ShopID, product.ProductID);
            Console.WriteLine("Thêm sản phẩm thành công !");
            Console.WriteLine("Nhấn phím bất kỳ để tiếp tục !");
            Console.ReadKey();
            ProductManagement(_ShopID);
        }
        public void DisplayOrders(int _ShopID, List<Order> orders)
        {
            Console.Clear();
            Console.WriteLine("|-----------------------------------------------------------------------------------------------|");
            Console.WriteLine("| STT | Tên khách hàng                  | Số lượng sản phẩm |       Tổng tiền     |  Trạng Thái |");
            Console.WriteLine("|-----------------------------------------------------------------------------------------------|");
            int count = 1;
            for (int i = 0; i < orders.Count; i++)
            {
                int total = orderBL.GetTotalOrder(orders[i].OrderID);
                User user = userBL.GetUserByID(orders[i].UserID);
                List<OrderDetails> orderDetailsList = orderDetailsBL.GetOrderDetailsListByOrderID(orders[i].OrderID);
                Console.WriteLine("| {0,3 } | {1,-31} | {2,17} | {3,19} | {4,11} |", count++, user.FullName, orderDetailsList.Count, total.ToString("C0"), orders[i].Status);
            }
            Console.WriteLine("|-----------------------------------------------------------------------------------------------|");
        }
        public void ViewOrdersToReceive(int _ShopID)
        {
            List<Order> orders = orderBL.GetOrdersByStatusAndShopID("ToReceive", _ShopID);
            DisplayOrders(_ShopID, orders);
            Console.WriteLine("Nhấn phím bất kỳ để quay lại");
            Console.ReadKey();
            OrderManagement(_ShopID);
        }
        public void ViewOrdersFinished(int _ShopID)
        {
            List<Order> orders = orderBL.GetOrdersByStatusAndShopID("Finished", _ShopID);
            DisplayOrders(_ShopID, orders);
            Console.WriteLine("Nhấn phím bất kỳ để quay lại");
            Console.ReadKey();
            OrderManagement(_ShopID);
        }
        public void ViewOrdersFailed(int _ShopID)
        {
            List<Order> orders = orderBL.GetOrdersByStatusAndShopID("Failed", _ShopID);
            DisplayOrders(_ShopID, orders);
            Console.WriteLine("Nhấn phím bất kỳ để quay lại");
            Console.ReadKey();
            OrderManagement(_ShopID);
        }
        public void ViewOrdersProcessing(int _ShopID)
        {
            List<Order> orders = orderBL.GetOrdersByStatusAndShopID("Processing", _ShopID);
            if (orders.Count > 0)
            {
                DisplayOrders(_ShopID, orders);
                Console.WriteLine($"Nhập Số thứ tự tương ứng để xem chi tiết đơn hàng.");
                Console.WriteLine($"0. Quay lại.");
                Console.Write($"Chọn: ");
                int choice = Convert.ToInt32(Console.ReadLine()) ;
                try
                {
                    if (choice == 0)
                    {
                        OrderManagement(_ShopID);
                    }
                    else if (choice != 0)
                    {
                        ViewOrder(_ShopID, orders[choice - 1].OrderID);
                    }
                }
                catch (System.Exception)
                {
                    ViewOrdersProcessing(_ShopID);
                }
            }
            else
            {
                Console.WriteLine("Không có đơn hàng");
                Console.WriteLine("Nhập phím bất kỳ để tiếp tục");
                Console.ReadKey();
                OrderManagement(_ShopID);
            }
        }
        public void ViewOrder(int _ShopID, int _OrderID)
        {
            Order order = orderBL.GetOrderByID(_OrderID);
            List<OrderDetails> orderDetailsList = orderDetailsBL.GetOrderDetailsListByOrderID(_OrderID);
            User user = userBL.GetUserByID(order.UserID);
            Address address = addressBL.GetAddressByID(user.AddressID);
            Console.WriteLine($"Cửa Hàng: {user.FullName}");
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
            Console.WriteLine("1. Xác nhận.");
            Console.WriteLine("2. Từ chối.");
            Console.WriteLine("0. Quay lại.");
            Console.Write($"Chọn: ");
            string? choice = Console.ReadLine();
            if (choice.ToLower() == "2")
            {
                orderBL.UpdateStatusOfOrder(_OrderID, "Failed");
                Console.WriteLine("Hủy đơn hàng thành công");
                Console.WriteLine("Nhấn phím bất kỳ để tiếp tục");
                Console.ReadKey();
                ViewOrdersProcessing(_ShopID);
            }
            else if (choice.ToLower() == "1")
            {
                orderBL.UpdateStatusOfOrder(_OrderID, "ToReceive");
                Console.WriteLine("Xác nhận thành công");
                Console.WriteLine("Nhấn phím bất kỳ để tiếp tục");
                Console.ReadKey();
                ViewOrdersProcessing(_ShopID);
            }
            else if (choice == "0")
            {
                ViewOrdersProcessing(_ShopID);
            }
            else
            {
                Console.WriteLine("Vui lòng chọn 0 - 2");
                ViewOrder(_ShopID, _OrderID);
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
                .WithColumn("STT", "Tên sản phẩm", "Giá", "Số lượng")
                .WithCharMapDefinition(CharMapDefinition.FrameDoublePipDefinition)
                .ExportAndWriteLine();
            Console.Write("Nhập số thứ tự để xem thông tin chi tiết hoặc \"0\" để quay lại: ");
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
            Console.WriteLine("|---------------------------------------------------------------------------------------|");
            Console.WriteLine("| STT | Tên sản phẩm                                       |       Giá       | Số lượng |");
            Console.WriteLine("|---------------------------------------------------------------------------------------|");
            int count = 1;
            foreach (Product product in products)  
            {
                Console.WriteLine("| {0,3 } | {1,-50} | {2, 15} | {3,8} |", count++, product.ProductName, product.Price.ToString("C0"), product.Quantity);
                
            }
            Console.WriteLine("|---------------------------------------------------------------------------------------|");
            Console.Write("Nhập số thứ tự tương ứng để thêm sản phẩm vào Danh mục hoặc \"0\" để quay lại: ");
            try
            {
                int choice = Convert.ToInt32(Console.ReadLine());
                if (choice != 0)
                {
                    product_CategoryBL.InsertProduct_Category(products[choice - 1].ProductID, _CategoryID);
                    Console.WriteLine("Thêm sản phẩm vào danh mục thành công !");
                    Console.WriteLine("Nhập phím bất kỳ để tiếp tục");
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
            Console.Write("Nhập số thứ tự để xem sản phẩm hoặc \"0\" để quay lại: ");
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
            Console.Write("Nhập số thứ tự để thêm sản phẩm vào danh mục có sẵn hoặc 0 để quay lại : ");
            // try
            // {
                ProductBL productBL = new ProductBL();
                int choice = Convert.ToInt32(Console.ReadLine());
                if (choice != 0)
                {
                    // xử lý TH đã có sẵn trong category
                    product_CategoryBL.InsertProduct_Category(_ProductID, categories[choice - 1].CategoryID);
                    Console.WriteLine("Thêm sản phẩm vào danh mục thành công !");
                    Console.WriteLine("Nhập phím bất kỳ để tiếp tục");
                    Console.ReadKey();
                }
                else
                {
                    ProductManagement(_ShopID);
                }
            // }
            // catch (System.Exception)
            // {
            //     Console.Clear();
            //     AddProductToCategory(_ShopID, _ProductID);
            // }
        }
        public void CreateCategory(int _ShopID)
        {
            Console.WriteLine("Tên danh mục: ");
            string? _CategoryName = Console.ReadLine();
            Category category = new Category(_ShopID, _CategoryName);
            categoryBL.InsertCategory(category);
            Console.WriteLine("Thêm danh mục thành công");
            Console.WriteLine("Nhấn phím bất kỳ để tiếp tục ");
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
                .WithColumn("STT", "Danh Mục")
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