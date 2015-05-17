using DevDayCFP.Services;

namespace DevDayCFP.Modules
{
    public class ErrorCodes : BaseModule
    {
        public ErrorCodes(IDataStore dataStore)
            : base("errorcodes", dataStore)
        {
            Get["/{errorcode}"] = parameters =>
            {
                string errorcode = parameters.errorcode;
                return View[errorcode];
            };
        }
    }
}