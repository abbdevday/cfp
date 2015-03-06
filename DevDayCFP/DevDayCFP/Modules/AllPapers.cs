using System;
using System.Collections.Generic;
using DevDayCFP.Common;
using DevDayCFP.Extensions;
using DevDayCFP.Models;
using DevDayCFP.Services;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;
using Nancy.Validation;

namespace DevDayCFP.Modules
{
    public class AllPapersModule : BaseModule
    {
        public AllPapersModule(IDataStore dataStore)
            : base("allpapers")
        {
            this.RequiresClaims(new List<string> { DevDayCFP.Common.Consts.Claims.Admin });

            Get["/"] = _ =>
            {
                var papersList = dataStore.GetAllPapers();

                return View["Index", papersList];
            };
        }
    }
}