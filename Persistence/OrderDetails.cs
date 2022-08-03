namespace Persistence
{
    public class OrderDetails
    {
        public int OrderID{get;set;}
        public int ProductID{get;set;}
        public int Quantity{get;set;}
        public OrderDetails(int _OrderID, int _ProductID, int _Quantity)
        {
            OrderID = _OrderID;
            ProductID = _ProductID;
            Quantity = _Quantity;
        }
        public OrderDetails(){}
    }
}