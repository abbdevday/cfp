using System;
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
    public class PapersModule : BaseModule
    {
        public PapersModule(IDataStore dataStore) : base("papers")
        {
            //this.RequiresAuthentication();

            Get["/"] = _ =>
            {
                var papersList = dataStore.GetPapersByUser(Context.CurrentUser.GetId());

                return View["Index", papersList];
            };

            Get["/add"] = _ => View["Edit", new Paper()];
            Post["/add"] = parameters =>
            {
                var model = this.Bind<Paper>();
                var result = this.Validate(model);

                if (!result.IsValid)
                {
                    var errorViewModels = result.AsErrorViewModels();
                    ViewBag.Page.Value.Errors.AddRange(errorViewModels);

                    return View["Edit", model];
                }

                string action = Request.Form["action"];

                //TODO Save etc.

                return Response.AsRedirect("/papers");
            };

            Get["/edit/{id}"] = parameters =>
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

                return View["Edit", paper];
            };

            Post["/edit"] = parameters =>
            {
                var model = this.Bind<Paper>();
                var result = this.Validate(model);

                if (!result.IsValid)
                {
                    var errorViewModels = result.AsErrorViewModels();
                    ViewBag.Page.Value.Errors.AddRange(errorViewModels);

                    return View["Edit", model];
                }

                string action = Request.Form["action"];

                //TODO Save etc.

                return Response.AsRedirect("/papers");
            };
        }
    }
}