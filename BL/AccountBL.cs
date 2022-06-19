using DAL;
using Persistence;

namespace BL;
public class AccountBL
{
    private AccountDAL accountDAL;

    public AccountBL()
    {
        accountDAL = new AccountDAL();
    }
    public Account GetAccountByName(string acc_name)
    {
        Account account = accountDAL.GetAccountByName(acc_name);
        return account;
    }

    public void SaveAccount(Account account)
    {
        accountDAL.SaveAccount(account);
    }

    public void SaveUser(User user, Account account)
    {
        accountDAL.SaveUser(user, account);
    }
}

