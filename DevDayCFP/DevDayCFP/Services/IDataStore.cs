using System;
using System.Collections.Generic;
using DevDayCFP.Models;
using Nancy.Security;

namespace DevDayCFP.Services
{
    public interface IDataStore
    {
        IUserIdentity GetUserById(Guid identifier);
        User GetUserByLoginData(string userName, string password);
        User GetUserByUsernameOrEmail(string username, string email);

        void SaveUser(User userRecord);

        IEnumerable<Paper> GetPapersByUser(Guid userId);
        Paper GetPaperById(Guid id);
    }
}