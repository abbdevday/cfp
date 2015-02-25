using System;
using DevDayCFP.Models;
using Nancy.Security;

namespace DevDayCFP.Common
{
    public static class UserIdentityExtensions
    {
        public static Guid GetId(this IUserIdentity userIdentity)
        {
            if (userIdentity == null)
            {
                return Guid.Empty;
            }
            return ((User) userIdentity).Id;
        }
    }
}