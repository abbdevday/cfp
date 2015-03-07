using System;
using DevDayCFP.Common;
using DevDayCFP.Models;
using DevDayCFP.ViewModels;
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Security;

namespace DevDayCFP.Services
{
    public class UserMapper : IUserMapper
    {
        private readonly IDataStore _dataStore;

        public UserMapper(IDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public IUserIdentity GetUserFromIdentifier(Guid identifier, NancyContext context)
        {
            return _dataStore.GetUserById(identifier);
        }

        public Guid? ValidateUser(string userName, string password)
        {
            var user = _dataStore.GetUserByLoginData(userName, password);

            if (user == null)
            {
                return null;
            }

            return user.Id;
        }

        public User ValidateRegisterNewUser(RegisterViewModel newUser)
        {
            var userRecord = new User()
            {
                Id = Guid.NewGuid(),
                Email = newUser.Email,
                UserName = newUser.UserName,
                Password = Helpers.EncodePassword(newUser.Password),
                ClaimsList = Consts.Claims.User,
                AccountStatus = AccountStatus.PendingVerification,
                RegistrationToken = Guid.NewGuid()
            };

            var existingUser = _dataStore.GetUserByUsernameOrEmail(newUser.UserName, newUser.Email);
            if (existingUser != null)
                return null;

            _dataStore.SaveUser(userRecord);

            return userRecord;           
        }
    }
}