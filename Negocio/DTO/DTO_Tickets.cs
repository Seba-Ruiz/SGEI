using System;
using System.Collections.Generic;
using System.Linq;

namespace Negocio.DTO
{
    /// <summary>Usuario del sistema para asignar tickets y mostrar autores.</summary>
    public class DTO_Usuario
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Iniciales { get; set; }
        public bool Activo { get; set; }
    }

    /// <summary>Tarjeta del board y fila de la grilla de tickets.</summary>
    public class DTO_TicketTarjeta
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public byte Prioridad { get; set; }
        public byte Estado { get; set; }
        /// <summary>Personas asignadas, por nombre.</summary>
        public List<DTO_Usuario> Asignados { get; set; } = new List<DTO_Usuario>();
        public string CreadoPorId { get; set; }
        public string CreadoPor { get; set; }
        public string CreadoPorIniciales { get; set; }
        public DateTime FechaAlta { get; set; }
        public DateTime FechaPrevista { get; set; }
        public DateTime FechaEstado { get; set; }
        public DateTime? FechaFin { get; set; }
        public string TipoEquipo { get; set; }
        public int? EquipoId { get; set; }
        /// <summary>"HPSISTEMAS05 · 192.168.10.5 · SISTEMAS"</summary>
        public string Equipo { get; set; }
        public int Comentarios { get; set; }

        public bool Terminado { get { return Estado == Negocio.Servicio.EstadosTicket.Terminado; } }

        /// <summary>"Franco Ríos, María Pérez"</summary>
        public string AsignadosTexto { get { return string.Join(", ", Asignados.Select(a => a.Nombre)); } }

        public bool EstaAsignado(string usuarioId) { return usuarioId != null && Asignados.Any(a => a.Id == usuarioId); }

        /// <summary>Días de atraso respecto de la fecha prevista (al día de hoy, o al terminarse). 0 si está en fecha.</summary>
        public int DiasDemora
        {
            get
            {
                var referencia = Terminado && FechaFin.HasValue ? FechaFin.Value.Date : DateTime.Today;
                return Math.Max(0, (referencia - FechaPrevista.Date).Days);
            }
        }

        /// <summary>Días que faltan para la fecha prevista (negativo si ya pasó).</summary>
        public int DiasParaVencer { get { return (FechaPrevista.Date - DateTime.Today).Days; } }

        /// <summary>Días desde el alta hasta hoy (o hasta que se terminó).</summary>
        public int DiasAbierto
        {
            get
            {
                var hasta = Terminado && FechaFin.HasValue ? FechaFin.Value.Date : DateTime.Today;
                return Math.Max(0, (hasta - FechaAlta.Date).Days);
            }
        }

        /// <summary>vencido · por-vencer (hoy o en 1-2 días) · en-plazo · terminado · terminado-tarde</summary>
        public string Semaforo
        {
            get
            {
                if (Terminado) { return DiasDemora > 0 ? "terminado-tarde" : "terminado"; }
                if (DiasDemora > 0) { return "vencido"; }
                return DiasParaVencer <= 2 ? "por-vencer" : "en-plazo";
            }
        }

        /// <summary>"3 días de demora", "Vence hoy", "Vence en 5 días", "Terminado a tiempo".</summary>
        public string TextoPlazo
        {
            get
            {
                if (Terminado) { return DiasDemora > 0 ? "Terminado con " + Dias(DiasDemora) + " de demora" : "Terminado a tiempo"; }
                if (DiasDemora > 0) { return Dias(DiasDemora) + " de demora"; }
                if (DiasParaVencer == 0) { return "Vence hoy"; }
                return DiasParaVencer == 1 ? "Vence mañana" : "Vence en " + Dias(DiasParaVencer);
            }
        }

        private static string Dias(int n) { return n + (n == 1 ? " día" : " días"); }
    }

    /// <summary>Un comentario o un cambio del ticket, en orden cronológico (pantalla de detalle).</summary>
    public class DTO_TicketActividad
    {
        public bool EsComentario { get; set; }
        public int Id { get; set; }
        public string UsuarioId { get; set; }
        public string Usuario { get; set; }
        public string Iniciales { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime? FechaEdicion { get; set; }
        /// <summary>Comentario: HTML saneado.</summary>
        public string Texto { get; set; }
        /// <summary>Cambio: ALTA, ESTADO, ASIGNADO (asignados antes y después), PRIORIDAD, FECHA_PREVISTA, TITULO, EQUIPO, DESCRIPCION.</summary>
        public string Campo { get; set; }
        public string Anterior { get; set; }
        public string Nuevo { get; set; }
    }

    public class DTO_TicketDetalle : DTO_TicketTarjeta
    {
        /// <summary>HTML saneado.</summary>
        public string Descripcion { get; set; }
        public List<DTO_TicketActividad> Actividad { get; set; }

        public IEnumerable<DTO_TicketActividad> SoloComentarios { get { return (Actividad ?? new List<DTO_TicketActividad>()).Where(a => a.EsComentario); } }
    }

    /// <summary>Alta y edición de un ticket (solo administradores).</summary>
    public class DTO_TicketFormulario
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public byte? Prioridad { get; set; }
        /// <summary>Ids de las personas asignadas (al menos una).</summary>
        public List<string> AsignadosIds { get; set; } = new List<string>();
        public DateTime? FechaPrevista { get; set; }
        /// <summary>Opcional: tipo de equipo (TiposEquipo) y su clave.</summary>
        public string TipoEquipo { get; set; }
        public int? EquipoId { get; set; }
        /// <summary>
        /// Tipo de incidente del catálogo de ese equipo, para el incidente que se genera al guardar el ticket con equipo
        /// (no se usa en impresoras ni si el ticket ya generó el incidente de ese equipo).
        /// </summary>
        public int? IncidenteCatalogoId { get; set; }
    }
}
