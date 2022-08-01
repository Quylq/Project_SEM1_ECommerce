namespace Persistence
{
    public class Shop
    {
        public int ShopID{get;set;}
        public int UserID{get;set;}
        public int AddressID{get;set;}
        public string ShopName{get;set;}
        public Shop(int _ShopID, string _ShopName, int _UserID, int _AddressID)
        {
            ShopID = _ShopID;
            ShopName = _ShopName;
            UserID = _UserID;
            AddressID = _AddressID;
        }
        public Shop(string _ShopName, int _UserID, int _AddressID)
        {
            ShopName = _ShopName;
            UserID = _UserID;
            AddressID = _AddressID;
        }
        public Shop()
        {
            ShopName = "";
        }
    }
}