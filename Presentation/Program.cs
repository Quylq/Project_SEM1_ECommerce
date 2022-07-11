using BL;
using Persistence;

Console.OutputEncoding = System.Text.Encoding.Unicode;
Console.InputEncoding = System.Text.Encoding.Unicode;


Ecommerce ecommerce = new Ecommerce();
<<<<<<< HEAD
ecommerce.Login();
=======
UserBL userBL = new UserBL();
ProductBL productBL = new ProductBL();
CategoryBL categoryBL = new CategoryBL();

// ecommerce.Menu();
// int _ProductID = productBL.ProductIDMax();
// Console.WriteLine($"{_ProductID}");
// ecommerce.SearchProduct();
User user = userBL.GetUserByName("quangquy");
ecommerce.SellerPage(user);

// Console.WriteLine($"{user.Birthday}");
>>>>>>> 8255db6127fe37ebc1e2dda5a8f2444632a6084f
