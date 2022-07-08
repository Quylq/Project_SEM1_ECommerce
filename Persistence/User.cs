namespace Persistence
{
    public class User
    {
        public int UserId{get;set;}
        public string UserName{get;set;}
        public string Password{get;set;}
        public string? FullName{get;set;}
        public DateTime Birthday{get;set;}
        public string? Email{get;set;}
        public string? Phone{get;set;}
        public string? Address{get;set;}
        public string? Role{get;set;}
        public User(int _ID, string _UserName, string _Password, string _FullName, DateTime _Birthday, string _Email, string _Phone, string _Address, string _Role)
        {
            UserId = _ID;
            UserName = _UserName;
            Password = _Password;
            FullName = _FullName;
            Birthday = _Birthday;
            Email = _Email;
            Phone = _Phone;
            Address = _Address;
            Role = _Role;
        }

        public User(){}
    }
}