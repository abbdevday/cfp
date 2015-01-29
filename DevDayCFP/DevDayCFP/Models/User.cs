using System;
using System.Collections.Generic;
using Nancy.Security;

namespace DevDayCFP.Models
{
    public class User : IUserIdentity
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public IEnumerable<string> Claims { get; set; }
    }
}