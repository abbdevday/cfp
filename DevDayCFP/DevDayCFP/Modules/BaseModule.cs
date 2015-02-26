using System;
using System.Collections.Generic;
using DevDayCFP.Models;
using DevDayCFP.ViewModels;
using Nancy;

namespace DevDayCFP.Modules
{
    public class BaseModule : NancyModule
    {
        public BaseModule()
        {
            SetupModelDefaults();
        }

        public BaseModule(string modulepath)
            : base(modulepath)
        {
            SetupModelDefaults();
        }

        private void SetupModelDefaults()
        {
            Before += ctx =>
            {
                var pageViewModel = new PageViewModel
                {
                    IsAuthenticated = ctx.CurrentUser != null,
                    CurrentUser = ctx.CurrentUser != null ? ctx.CurrentUser.UserName : "",
                    EmailHash = ctx.CurrentUser != null ? ((User)ctx.CurrentUser).EmailHash : String.Empty
                };

                ViewBag.Page = pageViewModel;

                return null;
            };
        }
    }
}