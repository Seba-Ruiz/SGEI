using System.Linq;
using System.Web.Mvc;
using Negocio.Servicio;

namespace login_v6.Controllers
{
    /// <summary>
    /// Dashboard (antes "Inicio"): cantidad de equipos activos por tipo y gráficos de incidentes, tickets y sistemas
    /// operativos (Negocio/Servicio/Dashboard.cs). Queda en /Inicio/Index para que sigan andando todos los links.
    /// </summary>
    [Authorize]
    public class InicioController : Controller
    {
        servicio serv = new servicio();
        stores store = new stores();

        public ActionResult Index()
        {
            ViewBag.impresoras = serv.sumaImpresora();
            ViewBag.computadoras = serv.sumaComputadora();
            ViewBag.telefonos = serv.sumaTelefono();
            ViewBag.camaras = serv.sumaCamara();
            ViewBag.sw = serv.sumaSW();
            ViewBag.es = serv.sumaES();

            var dashboard = new Negocio.Servicio.Dashboard();
            var tickets = new Tickets().Board();
            ViewBag.Graficos = new
            {
                incidentes = dashboard.IncidentesPorMes(),
                estados = dashboard.TicketsPorEstado(tickets),
                coloresEstado = EstadosTicket.Todos.Select(e => Negocio.Servicio.Dashboard.ColoresEstado[e]).ToList(),
                usuarios = dashboard.TicketsPorUsuario(tickets),
                sistemas = dashboard.ComputadorasPorSistemaOperativo()
            };
            return View();
        }

        /// <summary>El Dashboard se unificó con Inicio.</summary>
        public ActionResult Dashboard()
        {
            return RedirectToAction("Index");
        }

        public ActionResult Sugeridos()
        {
            var a_mantenimiento = store.mantenimiento();
            return View(a_mantenimiento);
        }
    }
}
