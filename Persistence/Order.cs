namespace Persistence
{
    public class Order
    {
        public int OrderID{get;set;}
        public int SellerID{get;set;}
        public int CustomerID{get;set;}
        public DateTime CreateDate{get;set;}
        public string? Status{get;set;}
        public string? Address{get;set;}

        public Order(int _SellerID, int _CustomerID, DateTime _CreateDate, string _Status, string _Address)
        {
            SellerID = _SellerID;
            CustomerID = _CustomerID;
            CreateDate = _CreateDate;
            Status = _Status;
            Address = _Address;
        }
        public Order(int _OrderID, int _SellerID, int _CustomerID, DateTime _CreateDate, string _Status, string _Address)
        {
            OrderID = _OrderID;
            SellerID = _SellerID;
            CustomerID = _CustomerID;
            CreateDate = _CreateDate;
            Status = _Status;
            Address = _Address;
        }
        public Order(){}
    }
}