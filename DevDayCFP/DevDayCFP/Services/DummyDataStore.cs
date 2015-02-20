using System;
using System.Collections.Generic;
using System.Linq;
using DevDayCFP.Models;
using Nancy.Security;

namespace DevDayCFP.Services
{
    public class DummyDataStore : IDataStore
    {
        private readonly List<User> _usersStore = new List<User>();
        private readonly List<Paper> _papersStore = new List<Paper>(); 

        public IUserIdentity GetUserById(Guid identifier)
        {
            return _usersStore.Single(u => u.Id == identifier);
        }

        public User GetUserByLoginData(string userName, string password)
        {
            return _usersStore.SingleOrDefault(u => u.UserName == userName && u.Password == password);
        }

        public User GetUserByLoginOrEmail(string userName, string email)
        {
            return _usersStore.SingleOrDefault(u => u.UserName == userName || u.Email == email);
        }

        public void SaveUser(User userRecord)
        {
            _usersStore.Add(userRecord);
        }

        public IEnumerable<Paper> GetPapersByUser(string userName)
        {
            return _papersStore;
        }

        public Paper GetPaperById(Guid id)
        {
            return _papersStore.FirstOrDefault(p => p.Id == id);
        }
    }
}