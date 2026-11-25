using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;

namespace login_v6.Hubs
{
    /// <summary>
    /// Canal en vivo de la campana de notificaciones (SignalR, en /signalr). Solo el servidor manda mensajes:
    ///  - nueva(notificacion, cantidadSinLeer): llegó una notificación.
    ///  - cantidad(cantidadSinLeer): cambió la cantidad sin leer (por ejemplo, se marcó como leída en otra pestaña).
    /// Los mensajes se dirigen con Clients.User(AspNetUsers.Id): ver <see cref="IdUsuarioSignalR"/>.
    /// </summary>
    [Authorize]
    public class NotificacionesHub : Hub
    {
    }

    /// <summary>
    /// SignalR identifica por defecto a cada usuario por su nombre (el mail). Las notificaciones se guardan por
    /// AspNetUsers.Id, así que se identifica por el Id. Se registra en Startup.
    /// </summary>
    public class IdUsuarioSignalR : IUserIdProvider
    {
        public string GetUserId(IRequest request)
        {
            var usuario = request.User;
            return usuario != null && usuario.Identity != null && usuario.Identity.IsAuthenticated
                ? usuario.Identity.GetUserId()
                : null;
        }
    }
}
