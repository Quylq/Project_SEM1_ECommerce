using BL;
using Persistence;

Console.OutputEncoding = System.Text.Encoding.Unicode;
Console.InputEncoding = System.Text.Encoding.Unicode;


Ecommerce ecommerce = new Ecommerce();
UserBL userBL = new UserBL();
ProductBL productBL = new ProductBL();
CategoryBL categoryBL = new CategoryBL();

// ecommerce.Menu();
// int _ProductID = productBL.ProductIDMax();
// Console.WriteLine($"{_ProductID}");
// ecommerce.SearchProduct();
User user = userBL.GetUserByName("quangquy");
// Console.WriteLine($"{user.Birthday}");
ecommerce.SellerPage(user);
