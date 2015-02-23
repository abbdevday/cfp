using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevDayCFP.Models;
using Nancy.Security;
using Simple.Data;

namespace DevDayCFP.Services
{
    public class SimpleDataStore : IDataStore
    {
        public IUserIdentity GetUserById(Guid identifier)
        {
            var db = Database.OpenNamedConnection("DefaultConnection");
            var user = db.Users.Get(identifier);
            return user as IUserIdentity;
        }

        public User GetUserByLoginData(string userName, string password)
        {
            var db = Database.OpenNamedConnection("DefaultConnection");
            var user = db.Users.FindByUserName(userName).FirstOrDefault();
            return user;
        }

        public User GetUserByLoginOrEmail(string userName, string email)
        {
            throw new NotImplementedException();
        }

        public void SaveUser(User userRecord)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Paper> GetPapersByUser(string userName)
        {
            throw new NotImplementedException();
        }

        public Paper GetPaperById(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}