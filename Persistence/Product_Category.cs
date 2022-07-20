namespace Persistence
{
    public class Product_Category
    {
        public int CategoryID{get;set;}
        public int ProductID{get;set;}
        
        public Product_Category(int _CategoryID, int _ProductID)
        {
            ProductID = _ProductID;
            CategoryID = _CategoryID;
        }
        public Product_Category(){}
    }
}