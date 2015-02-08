using DevDayCFP.Extensions;
using DevDayCFP.Models;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;
using Nancy.Validation;

namespace DevDayCFP.Modules
{
    public class PapersModule : BaseModule
    {
        public PapersModule() : base("papers")
        {
            //this.RequiresAuthentication();

            Get["/"] = _ => View["Index"];

            Get["/add"] = _ => View["Edit", new Paper()];
            Post["/add"] = parameters =>
            {
                var model = this.Bind<Paper>();
                var result = this.Validate(model);

                if (!result.IsValid)
                {
                    var errorViewModels = result.AsErrorViewModels();
                    ViewBag.Page.Value.Errors.AddRange(errorViewModels);

                    return View["add", model];
                }

                string action = Request.Form["action"];

                //TODO Save etc.

                return Response.AsRedirect("/papers");
            };
        }
    }
}