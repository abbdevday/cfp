﻿using System;
using DevDayCFP.Models;
using DevDayCFP.Services;
using DevDayCFP.ViewModels;
using Nancy;

namespace DevDayCFP.Modules
{
    public class BaseModule : NancyModule
    {
        protected readonly IDataStore _dataStore;

        protected readonly DateTime UtcCloseDateTime = new DateTime(2015, 06, 15, 23, 59, 59);

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
                int noOfUsers = _dataStore.GetUsersWithoutAdminPriviligesCount();
                int noOfPapers = _dataStore.GetPapersCount();

                string avatarPath = String.Empty;
                if (ctx.CurrentUser != null)
                {
                    var user = ((User)ctx.CurrentUser);
                    avatarPath = !String.IsNullOrEmpty(user.AvatarPath) 
                                ? String.Format("/Images/Avatars/{0}/{1}", user.Id, user.AvatarPath )
                                : String.Format("https://www.gravatar.com/avatar/{0}?s=40&d=mm", user.EmailHash.ToLower());

                }

                var pageViewModel = new PageViewModel
                {
                    IsAuthenticated = ctx.CurrentUser != null,
                    CurrentUser = ctx.CurrentUser != null ? ctx.CurrentUser.UserName : "",
                    EmailHash = ctx.CurrentUser != null ? ((User)ctx.CurrentUser).EmailHash : String.Empty,
                    AvatarPath = avatarPath,
                    NoOfPapers = noOfPapers,
                    NoOfUsers = noOfUsers
                };

                ViewBag.Page = pageViewModel;

                if (DateTime.UtcNow > UtcCloseDateTime)
                {
                    if (!ctx.Request.Path.Contains("cfpclosed"))
                    {
                        return Response.AsRedirect("/cfpclosed");
                    }
                }

                if (ctx.CurrentUser != null)
                {
                    var user = (User) ctx.CurrentUser;

                    if (user.AccountStatus == AccountStatus.PendingVerification)
                    {
                        if (ctx.Request.Path != "/"
                            && !ctx.Request.Path.Contains("/account/activate")
                            && String.Compare(ctx.Request.Path, "/account/logout", StringComparison.InvariantCultureIgnoreCase) != 0
                            && String.Compare(ctx.Request.Path, "/account/inactive", StringComparison.InvariantCultureIgnoreCase) != 0
                            && String.Compare(ctx.Request.Path, "/keyfailed", StringComparison.InvariantCultureIgnoreCase) != 0
                            && String.Compare(ctx.Request.Path, "/activated", StringComparison.InvariantCultureIgnoreCase) != 0
                            && String.Compare(ctx.Request.Path, "/inactive", StringComparison.InvariantCultureIgnoreCase) != 0)
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