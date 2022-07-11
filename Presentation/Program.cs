using BL;
using Persistence;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Text;

Console.OutputEncoding = System.Text.Encoding.Unicode;
Console.InputEncoding = System.Text.Encoding.Unicode;


Ecommerce ecommerce = new Ecommerce();
UserBL userBL = new UserBL();
ProductBL productBL = new ProductBL();
CategoryBL categoryBL = new CategoryBL();

ecommerce.Menu();
// int _ProductID = productBL.ProductIDMax();
// Console.WriteLine($"{_ProductID}");
// ecommerce.SearchProduct();

// User user = userBL.GetUserByName("tuananh");
// ecommerce.CustomerPage(user);

// User user = userBL.GetUserByName("vietanh");
// ecommerce.SellerPage(user);

// Console.WriteLine($"{user.Birthday}");


