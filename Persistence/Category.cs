namespace Persistence
{
    public class Category
    {
        public int CategoryID{get;set;}
        public int UserID{get;set;}
        public string? CategoryName{get;set;}

        public Category(int _CategoryID, int _UserID, string _CategoryName)
        {
            CategoryID = _CategoryID;
            UserID = _UserID;
            CategoryName = _CategoryName;
        }
        public Category(int _UserID, string _CategoryName)
        {
            UserID = _UserID;
            CategoryName = _CategoryName;
        }
        public Category(){}
    }
}