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
                    return View["Index", new HomeViewModel()];
                };
            Get["/activated"] = _ => View["Activated"];
            Get["/keyfailed"] = _ => View["KeyFailed"];
            Get["/inactive"] = _ => View["Inactive"];
        }
    }
}