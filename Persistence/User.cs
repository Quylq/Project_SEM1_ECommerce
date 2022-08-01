namespace Persistence
{
    public class User
    {
        public int UserID{get;set;}
        public int AddressID{get;set;}
        public string UserName{get;set;}
        public string Password{get;set;}
        public string FullName{get;set;}
        public string Birthday{get;set;}
        public string Email{get;set;}
        public string Phone{get;set;}
        public string Role{get;set;}
        public User(string _UserName, string _Password, string _FullName, string _Birthday, string _Email, string _Phone, int _AddressID, string _Role)
        {
            UserName = _UserName;
            Password = _Password;
            FullName = _FullName;
            Birthday = _Birthday;
            Email = _Email;
            Phone = _Phone;
            AddressID = _AddressID;
            Role = _Role;
        }
        public User(int _UserID, string _UserName, string _Password, string _FullName, string _Birthday, string _Email, string _Phone, int _AddressID, string _Role)
        {
            UserID = _UserID;
            UserName = _UserName;
            Password = _Password;
            FullName = _FullName;
            Birthday = _Birthday;
            Email = _Email;
            Phone = _Phone;
            AddressID = _AddressID;
            Role = _Role;
        }
        public User()
        {
            UserName = "";
            Password = "";
            FullName = "";
            Birthday = "";
            Email = "";
            Phone = "";
            Role = "";
        }
    }
}