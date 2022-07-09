namespace Persistence
{
    public class Product
    {
        public int ProductID{get;set;}
        public string? ProductName{get;set;}
        public string? Description{get;set;}
        public int Price{get;set;}
        public int Quantity{get;set;}
        public Product(string _ProductName, int _Price, string _Description, int _Quantity)
        {
            ProductName = _ProductName;
            Price = _Price;
            Description = _Description;
            Quantity = _Quantity;
        }
        public Product(int _ProductID, string _ProductName, int _Price, string _Description, int _Quantity)
        {
            ProductID = _ProductID;
            ProductName = _ProductName;
            Price = _Price;
            Description = _Description;
            Quantity = _Quantity;
        }
        public Product()
        {
        }
    }
}