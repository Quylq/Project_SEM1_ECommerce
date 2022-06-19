namespace Persistence
{
    public class User
    {
        public int CustomerId{get;set;}
        public string? FullName{get;set;}
        public string? Email{get;set;}
        public DateTime Birthday{get;set;}
        public string? Phone{get;set;}
        public string? Address{get;set;}

        public User(string _FullName, string _Email, DateTime _Birthday, string _Phone, string _Address)
        {
            FullName = _FullName;
            Email = _Email;
            Birthday = _Birthday;
            Phone = _Phone;
            Address = _Address;
        }

        public User(){}
    }
}