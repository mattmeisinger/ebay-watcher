using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EbayWatcher.Startup))]
namespace EbayWatcher
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}