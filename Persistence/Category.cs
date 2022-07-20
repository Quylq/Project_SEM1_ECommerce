namespace Persistence
{
    public class Category
    {
        public int CategoryID{get;set;}
        public int ShopID{get;set;}
        public string? CategoryName{get;set;}

        public Category(int _CategoryID, int _ShopID, string _CategoryName)
        {
            CategoryID = _CategoryID;
            ShopID = _ShopID;
            CategoryName = _CategoryName;
        }
        public Category(int _ShopID, string _CategoryName)
        {
            ShopID = _ShopID;
            CategoryName = _CategoryName;
        }
        public Category(){}
    }
}