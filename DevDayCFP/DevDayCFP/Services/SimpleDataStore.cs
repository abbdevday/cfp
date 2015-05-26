using System;
using System.Linq;
using System.Collections.Generic;
using DevDayCFP.Common;
using DevDayCFP.Extensions;
using DevDayCFP.Models;
using Simple.Data;

namespace DevDayCFP.Services
{
    public class SimpleDataStore : IDataStore
    {
        private readonly dynamic _db = Database.OpenNamedConnection("DefaultConnection");

        public dynamic DatabaseObject
        {
            get { return _db; }
        }

        public User GetUserById(Guid identifier)
        {
            User user = _db.Users.Get(identifier);
            return user;
        }

        public User GetUserByLoginData(string userName, string password)
        {
            User user = _db.Users.FindAllByUsernameAndPassword(userName, Helpers.EncodePassword(password)).FirstOrDefault();
            return user;
        }

        public User GetUserByUsernameOrEmail(string username, string email)
        {
            User user = _db.Users.FindAll(_db.Users.Username == username || _db.Users.Email == email).FirstOrDefault();
            return user;
        }

        public IList<User> GetAdmins()
        {
            return _db.Users.FindAllByClaimsList("User,Admin").ToList<User>();
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
            paper.UserId = paperRecord.User.Id;
            _db.Papers.Upsert(paper);
        }

        public IList<Paper> GetPapersByUser(Guid userId)
        {
            IList<Paper> papers = _db.Papers.FindAllByUserIdAndIsActive(userId, true).OrderByTitle().WithUser().ToList<Paper>();
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

        public int GetUsersWithoutAdminPriviligesCount()
        {
            return
                _db.Users.GetCountByClaimsList("User");
        }

        public int GetPapersCount()
        {
            return _db.Papers.GetCount(_db.Papers.IsActive == true);
        }

        public Token GetLastToken(User user, TokenType type)
        {
            IList<Token> matchingTokens = _db.Tokens.WithUser().FindAllByUserIdAndTokenTypeAndIsActive(user.Id, type, true).ToList<Token>();
            return matchingTokens.OrderByDescending(x => x.CreateDate).FirstOrDefault();
        }

        public Token FindTokenByContent(Guid id)
        {
            IList<Token> tokens = _db.Tokens.FindAllByTokenGuid(id).WithUser().ToList<Token>();
            return tokens.FirstOrDefault();
        }

        public Token GetTokenById(Guid id)
        {
            Token tokens = _db.Tokens.WithUser().Get(id);
            return tokens;
        }

        public void SaveToken(Token token)
        {
            token.CreateDate = DateTime.UtcNow;
            dynamic tokenRecord = token.ToDynamic();
            tokenRecord.UserId = token.User.Id;
            _db.Tokens.Upsert(tokenRecord);
        }

        public IList<Paper> GetAllPapers()
        {
            IList<Paper> papers = _db.Papers.All().WithUser().ToList<Paper>();
            return papers;
        }
    }
}