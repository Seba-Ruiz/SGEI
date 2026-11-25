using login_v6.Hubs;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(login_v6.Startup))]
namespace login_v6
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            // Notificaciones en vivo (campana de la barra superior): SignalR en /signalr, después de la autenticación
            // para que el hub sepa quién está conectado.
            GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => new IdUsuarioSignalR());
            app.MapSignalR();
        }
    }
}
