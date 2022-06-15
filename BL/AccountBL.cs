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
        account.AccountName = account.AccountName;
        account.AccountPassword = account.AccountPassword;
        return account;
    }

    public void SaveAccount(string acc_name, string acc_pass, string acc_role)
    {
        accountDAL.SaveAccount(acc_name, acc_pass,  acc_role);
    }
}

