using DAL;
using Persistence;

namespace BL;
public class ShopBL
{
    private ShopDAL shopDAL;

    public ShopBL()
    {
        shopDAL = new ShopDAL();
    }
    public Shop GetShopByID(int _ShopID)
    {
        Shop shop = shopDAL.GetShopByID(_ShopID);
        
        return shop;
    }
    public Shop GetShopByUserID(int _UserID)
    {
        Shop shop = shopDAL.GetShopByUserID(_UserID);
        
        return shop;
    }
    public List<Shop> GetShopsByName(string _ShopName)
    {
        List<Shop> shops = shopDAL.GetShopsByName(_ShopName);

        return shops;
    }
}