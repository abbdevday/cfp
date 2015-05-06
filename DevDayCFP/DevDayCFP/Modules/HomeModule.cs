using DevDayCFP.Common;
using DevDayCFP.Services;
using DevDayCFP.ViewModels;
using DevDayCFP.Extensions;
using Nancy;
using Nancy.Security;

namespace DevDayCFP.Modules
{
    public class HomeModule : BaseModule
    {
        public HomeModule(IDataStore dataStore)
            : base(dataStore)
        {
			Get["/"] = _ =>
			{
				var user = dataStore.GetUserById(Context.CurrentUser.GetId());
			    if (user != null && user.IsAuthenticated())
			    {
                    var papers = dataStore.GetPapersByUser(user.Id).Count;
                    return View["Index", new HomeViewModel()
                    {
                        ProfileComplete = user.ProfileComplete,
                        PapersSubmitted = papers
                    }];
			    }
			    return View["Welcome"];
			};
            Get["/activated"] = _ =>
            {
                this.RequiresAuthentication();

                return View["Activated"];
            };
            Get["/keyfailed"] = _ =>
            {
                this.RequiresAuthentication();

                return View["KeyFailed"];
            };

            Get["/inactive"] = _ =>
            {
                this.RequiresAuthentication();

                return View["Inactive"];
            };
        }
    }
}