using System;
using System.Collections.Generic;
using DevDayCFP.Models;

namespace DevDayCFP.Services
{
    public interface IDataStore
    {
        dynamic DatabaseObject { get; }

        User GetUserById(Guid identifier);
        User GetUserByLoginData(string userName, string password);
        User GetUserByUsernameOrEmail(string username, string email);

        void SaveUser(User userRecord);
        void SavePaper(Paper paperRecord);

        IList<Paper> GetPapersByUser(Guid userId);
        IList<Paper> GetAllPapers();
        Paper GetPaperById(Guid id);

        int GetUsersCount();
        int GetPapersCount();

        Token GetTokenById(Guid id);
        Token FindTokenByContent(Guid id);
        void SaveToken(Token token);
    }
}