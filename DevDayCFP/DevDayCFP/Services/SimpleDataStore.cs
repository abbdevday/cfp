using System;
using System.Collections.Generic;
using System.Web.Query.Dynamic;
using DevDayCFP.Common;
using DevDayCFP.Extensions;
using DevDayCFP.Models;
using Simple.Data;

namespace DevDayCFP.Services
{
    public class SimpleDataStore : IDataStore
    {
        private readonly dynamic _db = Database.OpenNamedConnection("DefaultConnection");

        public User GetUserById(Guid identifier)
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
            userRecord.EmailHash = Helpers.CalculateMd5(userRecord.Email);
            _db.Users.Upsert(userRecord);
        }

        public void SavePaper(Paper paperRecord)
        {
            paperRecord.LastModificationDate = DateTime.UtcNow;
            paperRecord.EventName = "DevDay 2015"; // TODO: Extract to settings
            dynamic paper = paperRecord.ToDynamic();
            paper.UserId = paper.User.Id;
            _db.Papers.Upsert(paper);
        }

        public IList<Paper> GetPapersByUser(Guid userId)
        {
            IList<Paper> papers = _db.Papers.FindAllByUserIdAndIsActive(userId, true).WithUser().ToList<Paper>();
            return papers;
        }

        public Paper GetPaperById(Guid id)
        {
            Paper paper = _db.Papers.WithUser().Get(id);
            return paper;
        }

        public int GetUsersCount()
        {
            return _db.Users.GetCount();
        }

        public int GetPapersCount()
        {
            return _db.Papers.GetCount(_db.Papers.IsActive == true);
        }

        public IList<Paper> GetAllPapers()
        {
            IList<Paper> papers = _db.Papers.All().WithUser().ToList<Paper>();
            return papers;
        }
    }
}