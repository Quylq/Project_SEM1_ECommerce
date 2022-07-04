﻿using DAL;
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

    public void SaveUser(User user)
    {
        userDAL.SaveUser(user);
    }

}
