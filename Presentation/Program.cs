using BL;
using Persistence;

Console.OutputEncoding = System.Text.Encoding.Unicode;
Console.InputEncoding = System.Text.Encoding.Unicode;


Ecommerce ecommerce = new Ecommerce();
UserBL userBL = new UserBL();
ecommerce.Menu();

// ecommerce.SearchProduct();
User user = userBL.GetUserByName("vietanh");
Console.WriteLine($"{user.Birthday}");
// ecommerce.SellerPage(user);
