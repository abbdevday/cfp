using System;
using System.Collections.Generic;
using DevDayCFP.Models;
using DevDayCFP.Services;
using DevDayCFP.ViewModels;
using Nancy;

namespace DevDayCFP.Modules
{
    public class BaseModule : NancyModule
    {
        private readonly IDataStore _dataStore;

        public BaseModule(IDataStore dataStore)
        {
            _dataStore = dataStore;
            SetupModelDefaults();
        }

        public BaseModule(string modulepath, IDataStore dataStore)
            : base(modulepath)
        {
            _dataStore = dataStore;
            SetupModelDefaults();
        }

        private void SetupModelDefaults()
        {
            Before += ctx =>
            {
                int noOfUsers = _dataStore.GetUsersCount();
                int noOfPapers = _dataStore.GetPapersCount();

                var pageViewModel = new PageViewModel
                {
                    IsAuthenticated = ctx.CurrentUser != null,
                    CurrentUser = ctx.CurrentUser != null ? ctx.CurrentUser.UserName : "",
                    EmailHash = ctx.CurrentUser != null ? ((User)ctx.CurrentUser).EmailHash : String.Empty,
                    NoOfPapers = noOfPapers,
                    NoOfUsers = noOfUsers
                };

                ViewBag.Page = pageViewModel;

                if (ctx.CurrentUser != null)
                {
                    var user = (User) ctx.CurrentUser;

                    if (user.AccountStatus == AccountStatus.PendingVerification)
                    {
                        if (ctx.Request.Path != "/"
                            && !ctx.Request.Path.Contains("/account/activate")
                            && ctx.Request.Path != "/keyfailed"
                            && ctx.Request.Path != "/activated"
                            && ctx.Request.Path != "/inactive")
                        {
                            return Response.AsRedirect("/inactive");
                        }
                    }
                }

                return null;
            };
        }
    }
}