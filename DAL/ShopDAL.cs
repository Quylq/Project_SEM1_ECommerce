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
        public void InsertShop(Shop shop)
        {
            query = $"Insert into Shops (ShopID, UserID, AddressID, ShopName) value ('{shop.ShopID}', '{shop.UserID}', '{shop.AddressID}', '{shop.ShopName}')";

            DbHelper.OpenConnection();
            reader = DbHelper.ExecQuery(query);
            DbHelper.CloseConnection();
        }
        public int ShopIDMax()
        {
            query = $"select max(ShopID) from shops";
            DbHelper.OpenConnection();
            reader = DbHelper.ExecQuery(query);
            int _ShopID = 0;

            try
            {
                if (reader.Read())
                {
                    _ShopID = reader.GetInt32("max(ShopID)");
                }
            }
            catch (System.Exception)
            {
                _ShopID = 0;
            }
            DbHelper.CloseConnection();
            return _ShopID;
        }
    }
}