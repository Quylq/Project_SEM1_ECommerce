namespace Persistence
{
    public class OrderDetails
    {
        public int OrderID{get;set;}
        public int ProductID{get;set;}
        public int ProductNumber{get;set;}
        public OrderDetails(int _OrderID, int _ProductID, int _ProductNumber)
        {
            OrderID = _OrderID;
            ProductID = _ProductID;
            ProductNumber = _ProductNumber;
        }
        public OrderDetails(){}
    }
}