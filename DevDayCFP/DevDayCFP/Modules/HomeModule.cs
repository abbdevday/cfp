using DevDayCFP.Services;

namespace DevDayCFP.Modules
{
    public class HomeModule : BaseModule
    {
        public HomeModule(IDataStore dataStore)
            : base(dataStore)
        {
            Get["/"] = _ => View["Index"];
            Get["/activated"] = _ => View["Activated"];
            Get["/keyfailed"] = _ => View["KeyFailed"];
            Get["/inactive"] = _ => View["Inactive"];
        }
    }
}