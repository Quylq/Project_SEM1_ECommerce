namespace Persistence
{
    public class Account
    {
        public int? AccountId{get;set;}
        public string? AccountName{get;set;}
        public string? AccountPassword{get;set;}
        public string? Role{get;set;}

        public Account(string _accountName, string _accountPassword, string _Role)
        {
            AccountName = _accountName;
            AccountPassword = _accountPassword;
            Role = _Role;
        }

        public Account()
        {
        }
    }
}
