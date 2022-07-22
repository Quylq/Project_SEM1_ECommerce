using DAL;
using Persistence;

namespace BL;
public class UserBL
{
    private UserDAL userDAL;

    public UserBL()
    {
        userDAL = new UserDAL();
    }
    public User GetUserByName(string _UserName)
    {
        User user = userDAL.GetUserByName(_UserName);
        return user;
    }
    public User GetUserByID(int _UserID)
    {
        User user = userDAL.GetUserByID(_UserID);
        return user;
    }
    public void InsertUser(User user)
    {
        userDAL.InsertUser(user);
    }
    public int UserIDMax()
    {
        int _UserID = userDAL.UserIDMax();

        return _UserID;
    }
}

