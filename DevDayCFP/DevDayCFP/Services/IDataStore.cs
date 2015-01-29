using System;
using DevDayCFP.Models;
using Nancy.Security;

namespace DevDayCFP.Services
{
    public interface IDataStore
    {
        IUserIdentity GetUserById(Guid identifier);
        User GetUserByLoginData(string userName, string password);
        User GetUserByLoginOrEmail(string userName, string email);

        void SaveUser(User userRecord);
    }
}