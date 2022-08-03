namespace Persistence
{
    public class Product
    {
        public int ProductID{get;set;}
        public int ShopID{get;set;}
        public string ProductName{get;set;}
        public string Description{get;set;}
        public int Price{get;set;}
        public int Amount{get;set;}
        public Product(int _ProductID, int _ShopID, string _ProductName, int _Price, string _Description, int _Amount)
        {
            ProductID = _ProductID;
            ShopID = _ShopID;
            ProductName = _ProductName;
            Price = _Price;
            Description = _Description;
            Amount = _Amount;
        }
        public Product()
        {
            ProductName = "";
            Description = "";
        }
    }
}