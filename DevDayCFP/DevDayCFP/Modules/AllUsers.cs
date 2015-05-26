using System;
using System.Collections.Generic;
using DevDayCFP.Services;
using Nancy.Security;

namespace DevDayCFP.Modules
{
    public class AllUsersModule : BaseModule
    {
        public AllUsersModule(IDataStore dataStore)
            : base("allusers", dataStore)
        {
            this.RequiresClaims(new List<string> { Common.Consts.Claims.Admin });

            Get["/"] = _ =>
            {
                var userList = dataStore.GetAllUsers();

                return View["Index", userList];
            };

            Get["/details/{id}"] = parameters =>
            {
                if (!parameters.id.HasValue && String.IsNullOrWhiteSpace(parameters.id))
                {
                    return 404;
                }

                var user = dataStore.GetUserById((Guid)parameters.id);

                if (user == null)
                {
                    return 404;
                }

                return View["Details", user];
            };
        }
    }
}