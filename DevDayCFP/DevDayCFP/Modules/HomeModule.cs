using Nancy;

namespace DevDayCFP.Modules
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = _ => View["Index"];
            Get["/papers"] = _ => View["Papers"];
        }
    }
}