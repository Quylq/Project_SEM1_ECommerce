namespace Persistence
{
    public class Order
    {
        public int OrderID{get;set;}
        public int UserID{get;set;}
        public int ShopID{get;set;}
        public string? CreateDate{get;set;}
        public string? Status{get;set;}

        public Order(int _OrderID, int _UserID, int _ShopID, string _CreateDate, string _Status)
        {
            OrderID = _OrderID;
            UserID = _UserID;
            ShopID = _ShopID;
            CreateDate = _CreateDate;
            Status = _Status;
        }
        public Order(int _OrderID, int _UserID, int _ShopID, string _Status)
        {
            OrderID = _OrderID;
            UserID = _UserID;
            ShopID = _ShopID;
            Status = _Status;
        }
        public Order(){}
    }
}