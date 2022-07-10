namespace Persistence
{
    public class OrderDetails
    {
        public int OrderID{get;set;}
        public int ProductID{get;set;}
        public string? ProductName{get;set;}
        public int Price{get;set;}
        public int ProductNumber{get;set;}
        public OrderDetails(string _ProductName, int _Price, int _ProductNumber)
        {
            ProductName = _ProductName;
            Price = _Price;
            ProductNumber = _ProductNumber;
        }
        public OrderDetails(int _ProductID, string _ProductName, int _Price, int _ProductNumber)
        {
            ProductID = _ProductID;
            ProductName = _ProductName;
            Price = _Price;
            ProductNumber = _ProductNumber;
        }
        public OrderDetails()
        {
        }
    }
}