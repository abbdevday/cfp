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
            this.RequiresAuthentication();

            Get["/"] = _ =>
            {
                var papersList = dataStore.GetPapersByUser(Context.CurrentUser.GetId());

                return View["Index", papersList];
            };

            Get["/add"] = _ => View["Edit", new Paper()];
            Post["/add"] = parameters =>
            {
                var paper = this.Bind<Paper>();
                var result = this.Validate(paper);

                if (!result.IsValid)
                {
                    var errorViewModels = result.AsErrorViewModels();
                    ViewBag.Page.Value.Errors.AddRange(errorViewModels);

                    return View["Edit", paper];
                }

                paper.UserId = Context.CurrentUser.GetId();
                paper.Id = Guid.NewGuid();
                dataStore.SavePaper(paper);

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

            Post["/edit/{id}"] = parameters =>
            {
                var paper = this.Bind<Paper>();
                var result = this.Validate(paper);

                if (!result.IsValid)
                {
                    var errorViewModels = result.AsErrorViewModels();
                    ViewBag.Page.Value.Errors.AddRange(errorViewModels);

                    return View["Edit", paper];
                }

                dataStore.SavePaper(paper);

                return Response.AsRedirect("/papers");
            };
        }
    }
}