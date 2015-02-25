using System;
using System.Collections.Generic;
using DevDayCFP.Models;
using Nancy.Security;
using Simple.Data;

namespace DevDayCFP.Services
{
    public class SimpleDataStore : IDataStore
    {
        private readonly dynamic _db = Database.OpenNamedConnection("DefaultConnection");

        public IUserIdentity GetUserById(Guid identifier)
        {
            User user = _db.Users.Get(identifier);
            return user;
        }

        public User GetUserByLoginData(string userName, string password)
        {
            User user = _db.Users.FindAllByUsername(userName).FirstOrDefault();
            return user;
        }

        public User GetUserByUsernameOrEmail(string username, string email)
        {
            User user = _db.Users.FindAll(_db.Users.Username == username || _db.Users.Email == email).FirstOrDefault();
            return user;
        }

        public void SaveUser(User userRecord)
        {
            _db.Users.Upsert(userRecord);
        }

        public IList<Paper> GetPapersByUser(Guid userId)
        {
            IList<Paper> papers = _db.Papers.FindAllByUserId(userId).ToList<Paper>();
            return papers;
        }

        public Paper GetPaperById(Guid id)
        {
            Paper paper = _db.Papers.Get(id);
            return paper;
        }
    }
}