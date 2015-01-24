using Nancy;
using Nancy.Conventions;

namespace DevDayCFP
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureConventions(NancyConventions conventions)
        {
            base.ConfigureConventions(conventions);

            conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("scripts", @"Scripts"));
            conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("images", @"Images"));
            conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("fonts", @"Fonts"));
        }
    }
}