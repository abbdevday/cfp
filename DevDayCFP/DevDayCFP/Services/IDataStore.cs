using System;
using System.Collections.Generic;
using DevDayCFP.Models;
using Nancy.Security;

namespace DevDayCFP.Services
{
    public interface IDataStore
    {
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

        Token GetLastToken(User user, TokenType type);
    }
}