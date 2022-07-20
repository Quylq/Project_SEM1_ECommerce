using MySql.Data.MySqlClient;
using Persistence;

namespace DAL
{
    public class ShopDAL
    {
        private string? query;
        private MySqlDataReader? reader;
        private Shop GetShopInfo(MySqlDataReader reader)
        {
            Shop shop = new Shop();

            shop.ShopID = reader.GetInt32("ShopID");
            shop.UserID = reader.GetInt32("UserID");
            shop.AddressID = reader.GetInt32("AddressID");
            shop.ShopName = reader.GetString("ShopName");
            return shop;
        }
        public Shop GetShopByID(int _ShopID)
        {
            query = $"select * from Shops where ShopID = '{_ShopID}'";

            DbHelper.OpenConnection();
            reader = DbHelper.ExecQuery(query);

            Shop shop = new Shop();
            if (reader.Read())
            {
                shop = GetShopInfo(reader);
            }
            DbHelper.CloseConnection();
            return shop;
        }
        public Shop GetShopByUserID(int _UserID)
        {
            query = $"select * from Shops where UserID = '{_UserID}'";

            DbHelper.OpenConnection();
            reader = DbHelper.ExecQuery(query);

            Shop shop = new Shop();
            if (reader.Read())
            {
                shop = GetShopInfo(reader);
            }
            DbHelper.CloseConnection();
            return shop;
        }
        public List<Shop> GetShopsByName(string _ShopName)
        {
            query = $"select * from Shops where ShopName like '%{_ShopName}%'";

            DbHelper.OpenConnection();
            reader = DbHelper.ExecQuery(query);

            List<Shop>? shops = new List<Shop>();

            while (reader.Read())
            {
                Shop shop = GetShopInfo(reader);
                shops.Add(shop);
            }
            DbHelper.CloseConnection();
            return shops;
        }
    }
}
