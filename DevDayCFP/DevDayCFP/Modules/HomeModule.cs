using Nancy;

namespace DevDayCFP.Modules
{
    public class HomeModule : BaseModule
    {
        public HomeModule()
        {
            Get["/"] = _ => View["Index"];
        }
    }
}