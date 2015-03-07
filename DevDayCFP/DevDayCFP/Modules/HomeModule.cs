using DevDayCFP.Services;
using Nancy;

namespace DevDayCFP.Modules
{
    public class HomeModule : BaseModule
    {
        public HomeModule(IDataStore dataStore) : base(dataStore)
        {
            Get["/"] = _ => View["Index"];
        }
    }
}