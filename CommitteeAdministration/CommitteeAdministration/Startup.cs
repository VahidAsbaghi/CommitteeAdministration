using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CommitteeAdministration.Startup))]
namespace CommitteeAdministration
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
