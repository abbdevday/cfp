using System;
using System.Collections.Generic;
using DevDayCFP.Services;
using Nancy.Security;

namespace DevDayCFP.Modules
{
    public class AllPapersModule : BaseModule
    {
        public AllPapersModule(IDataStore dataStore)
            : base("allpapers", dataStore)
        {
            this.RequiresClaims(new List<string> { Common.Consts.Claims.Admin });

            Get["/"] = _ =>
            {
                var papersList = dataStore.GetAllPapers();

                return View["Index", papersList];
            };

            Get["/details/{id}"] = parameters =>
            {
                if (!parameters.id.HasValue && String.IsNullOrWhiteSpace(parameters.id))
                {
                    return 404;
                }

                var paper = dataStore.GetPaperById((Guid)parameters.id);

                if (paper == null)
                {
                    return 404;
                }

                return View["Details", paper];
            };
        }
    }
}