using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVCPagedList.Startup))]
namespace MVCPagedList
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
