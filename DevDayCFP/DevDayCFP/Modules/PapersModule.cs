using Nancy;
using Nancy.Security;

namespace DevDayCFP.Modules
{
    public class PapersModule : NancyModule
    {
        public PapersModule() : base("papers")
        {
            this.RequiresAuthentication();

            Get["/"] = _ => View["Index"];           
        }
    }
}