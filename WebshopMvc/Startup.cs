using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebshopMvc.Startup))]
namespace WebshopMvc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
