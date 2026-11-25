using System;
using System.Collections.Generic;

namespace Negocio.DTO
{
    /// <summary>Notificación tal como se muestra en la campana y en la pantalla Notificaciones.</summary>
    public class DTO_Notificacion
    {
        public int Id { get; set; }
        public string Tipo { get; set; }
        public string Titulo { get; set; }
        public string Mensaje { get; set; }
        /// <summary>Ruta dentro del sistema ("/Tickets/Ver/15"), o null.</summary>
        public string Url { get; set; }
        /// <summary>Quién la generó ("Seba Ruiz"), o null si la generó el sistema.</summary>
        public string Origen { get; set; }
        public string OrigenIniciales { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime? FechaLectura { get; set; }
        public DateTime? FechaMail { get; set; }
        public string ErrorMail { get; set; }

        public bool Leida { get { return FechaLectura.HasValue; } }
        /// <summary>Ícono de Bootstrap Icons según el tipo.</summary>
        public string Icono { get { return Negocio.Servicio.TiposNotificacion.Icono(Tipo); } }
    }

    /// <summary>Una página del listado de notificaciones de un usuario.</summary>
    public class DTO_PaginaNotificaciones
    {
        public List<DTO_Notificacion> Items { get; set; }
        public int Pagina { get; set; }
        public int PorPagina { get; set; }
        /// <summary>Cantidad que cumple el filtro (para paginar).</summary>
        public int Total { get; set; }
        public int TotalSinLeer { get; set; }
        public bool SoloSinLeer { get; set; }

        public int Paginas { get { return Math.Max(1, (Total + PorPagina - 1) / PorPagina); } }
    }

    /// <summary>Datos para generar una notificación. Solo UsuarioId, Tipo y Titulo son obligatorios.</summary>
    public class NotificacionNueva
    {
        /// <summary>Destinatario (AspNetUsers.Id).</summary>
        public string UsuarioId { get; set; }
        /// <summary>Quién la generó (AspNetUsers.Id), o null si es del sistema.</summary>
        public string OrigenUsuarioId { get; set; }
        /// <summary>Ver <see cref="Negocio.Servicio.TiposNotificacion"/>.</summary>
        public string Tipo { get; set; }
        public string Titulo { get; set; }
        public string Mensaje { get; set; }
        /// <summary>Ruta dentro del sistema, relativa a la raíz de la aplicación ("/Tickets/Ver/15").</summary>
        public string Url { get; set; }
        public string ReferenciaTipo { get; set; }
        public int? ReferenciaId { get; set; }

        /// <summary>Datos que se listan en el mail ("Prioridad" → "Alta"). No se guardan en la base.</summary>
        public List<KeyValuePair<string, string>> Detalle { get; set; }
        /// <summary>Texto del botón del mail ("Ver ticket").</summary>
        public string TextoBoton { get; set; }

        public NotificacionNueva()
        {
            Detalle = new List<KeyValuePair<string, string>>();
        }

        public NotificacionNueva Dato(string nombre, string valor)
        {
            if (!string.IsNullOrWhiteSpace(valor)) { Detalle.Add(new KeyValuePair<string, string>(nombre, valor)); }
            return this;
        }
    }

    /// <summary>A quién y a qué dirección se manda el mail de una notificación.</summary>
    public class DTO_DestinatarioMail
    {
        public string Nombre { get; set; }
        public string Email { get; set; }
        public bool Activo { get; set; }
    }
}
