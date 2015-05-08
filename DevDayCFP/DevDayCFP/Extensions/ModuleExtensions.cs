using System.IO;
using Nancy;
using Nancy.ViewEngines;

namespace DevDayCFP.Extensions
{
    public static class ModuleExtensions
    {
        public static string RenderViewToString(this NancyModule module, string viewName)
        {
            return RenderViewToString(module, viewName, null, module.ModulePath);
        }

        public static string RenderViewToString(this NancyModule module, string viewName, object model)
        {
            return RenderViewToString(module, viewName, model, module.ModulePath);
        }

        public static string RenderViewToString(this NancyModule module, string viewName, object model, string modulePath)
        {
            using (var stream = new MemoryStream())
            {
                var viewLocationContext = new ViewLocationContext { Context = module.Context, ModulePath = modulePath };
                module.ViewFactory.RenderView(viewName, model, viewLocationContext).Contents.Invoke(stream);

                var streamWriter = new StreamWriter(stream);
                streamWriter.Flush();
                stream.Position = 0;

                var streamReader = new StreamReader(stream);
                return streamReader.ReadToEnd();
            }
        }
    }
}