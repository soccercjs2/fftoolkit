using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(fftoolkit.Web.Startup))]
namespace fftoolkit.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
