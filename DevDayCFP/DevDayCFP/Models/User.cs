using System;
using System.Collections.Generic;
using System.Linq;
using Nancy.Security;

namespace DevDayCFP.Models
{
    public class User : IUserIdentity
    {
        public Guid Id { get; set; }

        public AccountStatus AccountStatus { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }

        public string Email { get; set; }
        public string EmailHash { get; set; }

        public string Name { get; set; }
        public string Location { get; set; }
        public string Bio { get; set; }
        public string AvatarPath { get; set; }
        public string TwitterHandle { get; set; }
        public string Website { get; set; }

        public string ClaimsList { get; set; }

        public IEnumerable<string> Claims
        {
            get { return ClaimsList == null ? new List<string>() : ClaimsList.Split(',').ToList(); }
        }

        public User()
        {
            AccountStatus = AccountStatus.PendingVerification;
        }
    }

    public enum AccountStatus
    {
        PendingVerification,
        Active
    }
}