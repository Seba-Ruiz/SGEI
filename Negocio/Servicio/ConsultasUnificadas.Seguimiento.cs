using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Negocio.DTO;

namespace Negocio.Servicio
{
    /// <summary>
    /// Seguimiento de incidentes: observaciones de los usuarios (cada uno edita o elimina las suyas), historial de
    /// cambios e incidentes generados desde tickets. Tablas: BaseDeDatos/2026-11-26_incidentes_observaciones.sql.
    /// Un incidente se identifica con su tipo de equipo (TiposEquipo) y el id en la tabla de incidentes de ese tipo.
    /// </summary>
    public partial class ConsultasUnificadas
    {
        private const int LargoValorHistorial = 200;

        // ------------------------------------------------------------------ Detalle con observaciones e historial

        /// <summary>Incidente con sus observaciones, su historial y el ticket que lo generó (si hay). Null si no existe.</summary>
        public DTO_IncidenteDetalle DetalleIncidente(string tipo, int id)
        {
            var fila = ObtenerIncidenteFila(tipo, id);
            if (fila == null) { return null; }
            using (var ctx = new SGEIContext())
            {
                var usuarios = Tickets.Usuarios(ctx);
                Func<string, DTO_Usuario> usuario = u =>
                {
                    DTO_Usuario x;
                    return u != null && usuarios.TryGetValue(u, out x) ? x : new DTO_Usuario { Id = u, Nombre = "(usuario eliminado)", Iniciales = "?" };
                };

                var actividad = ctx.incidente_comentario.Where(c => c.tipo_equipo == tipo && c.id_incidente == id).ToList()
                    .Select(c => new DTO_TicketActividad
                    {
                        EsComentario = true, Id = c.id_comentario, UsuarioId = c.usuario_id, Usuario = usuario(c.usuario_id).Nombre,
                        Iniciales = usuario(c.usuario_id).Iniciales, Fecha = c.fecha, FechaEdicion = c.fecha_edicion, Texto = c.texto
                    })
                    .Concat(ctx.incidente_historial.Where(h => h.tipo_equipo == tipo && h.id_incidente == id).ToList()
                    .Select(h => new DTO_TicketActividad
                    {
                        Id = h.id_historial, UsuarioId = h.usuario_id, Usuario = usuario(h.usuario_id).Nombre, Iniciales = usuario(h.usuario_id).Iniciales,
                        Fecha = h.fecha, Campo = h.campo, Anterior = h.valor_anterior, Nuevo = h.valor_nuevo
                    }))
                    .OrderBy(a => a.Fecha).ThenBy(a => a.EsComentario).ThenBy(a => a.Id)
                    .ToList();

                var tickets = (from ti in ctx.ticket_incidente
                               join t in ctx.ticket on ti.id_ticket equals t.id_ticket
                               where ti.tipo_equipo == tipo && ti.id_incidente == id
                               select new DTO_Opcion { Id = t.id_ticket, Texto = t.titulo }).ToList();

                return new DTO_IncidenteDetalle { Incidente = fila, Actividad = actividad, Tickets = tickets };
            }
        }

        // ------------------------------------------------------------------ Observaciones

        /// <summary>Agrega una observación (HTML de Summernote). Devuelve su id, o null si el incidente no existe.</summary>
        public int? Observar(string tipo, int idIncidente, string usuarioId, string html)
        {
            using (var ctx = new SGEIContext())
            {
                if (!Incidentes(ctx, tipo, idIncidente).Any()) { return null; }
                var c = new incidente_comentario
                {
                    tipo_equipo = tipo, id_incidente = idIncidente, usuario_id = usuarioId, texto = HtmlSeguro.Limpiar(html), fecha = DateTime.Now
                };
                ctx.incidente_comentario.Add(c);
                ctx.SaveChanges();
                return c.id_comentario;
            }
        }

        /// <summary>Incidente al que pertenece una observación (tipo e id), o null si ya no existe.</summary>
        public Tuple<string, int> IncidenteDeObservacion(int idComentario)
        {
            using (var ctx = new SGEIContext())
            {
                var c = ctx.incidente_comentario.Find(idComentario);
                return c == null ? null : Tuple.Create(c.tipo_equipo, c.id_incidente);
            }
        }

        /// <summary>Edita una observación propia. Devuelve un error, o null si se pudo.</summary>
        public string EditarObservacion(int idComentario, string usuarioId, string html)
        {
            using (var ctx = new SGEIContext())
            {
                var c = ctx.incidente_comentario.Find(idComentario);
                if (c == null) { return "La observación ya no existe."; }
                if (c.usuario_id != usuarioId) { return "Solo podés editar tus observaciones."; }
                c.texto = HtmlSeguro.Limpiar(html);
                c.fecha_edicion = DateTime.Now;
                ctx.SaveChanges();
                return null;
            }
        }

        /// <summary>Elimina una observación propia. Devuelve un error, o null si se pudo.</summary>
        public string EliminarObservacion(int idComentario, string usuarioId)
        {
            using (var ctx = new SGEIContext())
            {
                var c = ctx.incidente_comentario.Find(idComentario);
                if (c == null) { return "La observación ya no existe."; }
                if (c.usuario_id != usuarioId) { return "Solo podés eliminar tus observaciones."; }
                ctx.incidente_comentario.Remove(c);
                ctx.SaveChanges();
                return null;
            }
        }

        // ------------------------------------------------------------------ Historial

        /// <summary>Registra una foto agregada o eliminada en el historial del incidente de impresora.</summary>
        public void RegistrarFoto(int idIncidente, string usuarioId, bool agregada)
        {
            if (string.IsNullOrEmpty(usuarioId)) { return; }
            using (var ctx = new SGEIContext())
            {
                ctx.incidente_historial.Add(Historial(TiposEquipo.Impresora, idIncidente, usuarioId, DateTime.Now, "FOTO", agregada ? null : "eliminada", agregada ? "agregada" : null));
                ctx.SaveChanges();
            }
        }

        /// <summary>Alta (antes == null) o cada dato que cambió entre las dos versiones del incidente.</summary>
        private static void RegistrarCambios(SGEIContext ctx, string tipo, int id, string usuarioId, DTO_IncidenteFila antes, DTO_IncidenteFila despues)
        {
            var ahora = DateTime.Now;
            Func<DateTime?, string> fecha = d => d.HasValue ? d.Value.ToString("dd/MM/yyyy") : null;
            if (antes == null)
            {
                ctx.incidente_historial.Add(Historial(tipo, id, usuarioId, ahora, "ALTA", null, despues.Incidente));
            }
            else
            {
                Action<string, string, string> cambio = (campo, a, d) =>
                {
                    if ((a ?? "") != (d ?? "")) { ctx.incidente_historial.Add(Historial(tipo, id, usuarioId, ahora, campo, a, d)); }
                };
                cambio("EQUIPO", antes.Equipo, despues.Equipo);
                cambio("INCIDENTE", antes.Incidente, despues.Incidente);
                cambio("FECHA_INCIDENTE", fecha(antes.FechaIncidente), fecha(despues.FechaIncidente));
                cambio("ACCION", antes.Accion, despues.Accion);
                cambio("FECHA_ACCION", fecha(antes.FechaAccion), fecha(despues.FechaAccion));
            }
            ctx.SaveChanges();
        }

        private static incidente_historial Historial(string tipo, int id, string usuarioId, DateTime fecha, string campo, string anterior, string nuevo)
        {
            Func<string, string> corto = s => s != null && s.Length > LargoValorHistorial ? s.Substring(0, LargoValorHistorial - 1) + "…" : s;
            return new incidente_historial
            {
                tipo_equipo = tipo, id_incidente = id, usuario_id = usuarioId, fecha = fecha, campo = campo,
                valor_anterior = corto(anterior), valor_nuevo = corto(nuevo)
            };
        }

        // ------------------------------------------------------------------ Incidentes generados desde tickets

        /// <summary>
        /// Crea el incidente de un ticket con equipo (una sola vez por ticket y equipo): fecha de hoy, el tipo de
        /// incidente elegido en el ticket (en impresoras, el título del ticket como descripción) y la acción
        /// "Pendiente · ticket #N". Queda en su historial que lo generó el ticket. Devuelve el id del incidente, o null si
        /// ese ticket ya había generado uno para el equipo.
        /// </summary>
        public int? CrearIncidenteDesdeTicket(int idTicket, string titulo, string tipo, int equipoId, int? catalogoId, string usuario, string usuarioId)
        {
            using (var ctx = new SGEIContext())
            {
                var yaGenerados = ctx.ticket_incidente.Where(x => x.id_ticket == idTicket && x.tipo_equipo == tipo).Select(x => x.id_incidente).ToList();
                if (yaGenerados.Any(i => Incidentes(ctx, tipo, i).Any(x => x.EquipoId == equipoId))) { return null; }
            }

            var texto = (titulo ?? "").Trim();
            if (texto.Length > TiposEquipo.LargoTexto) { texto = texto.Substring(0, TiposEquipo.LargoTexto - 1) + "…"; }
            var f = new DTO_IncidenteFormulario
            {
                Tipo = tipo, EquipoId = equipoId,
                IncidenteId = tipo == TiposEquipo.Impresora ? null : catalogoId,
                IncidenteTexto = tipo == TiposEquipo.Impresora ? texto : null,
                FechaIncidente = DateTime.Today,
                Accion = "Pendiente · ticket #" + idTicket
            };
            string equipo;
            var id = GuardarIncidente(f, usuario, usuarioId, out equipo);

            using (var ctx = new SGEIContext())
            {
                var ahora = DateTime.Now;
                ctx.ticket_incidente.Add(new ticket_incidente { id_ticket = idTicket, tipo_equipo = tipo, id_incidente = id, fecha = ahora });
                ctx.incidente_historial.Add(Historial(tipo, id, usuarioId, ahora, "TICKET", null, "#" + idTicket + " · " + titulo));
                ctx.SaveChanges();
            }
            return id;
        }

        /// <summary>Equipos en los que el ticket ya cargó su incidente, como "tipo:clave" (formulario del ticket).</summary>
        public List<string> EquiposConIncidenteDeTicket(int idTicket)
        {
            using (var ctx = new SGEIContext())
            {
                var r = new List<string>();
                foreach (var c in ctx.ticket_incidente.Where(x => x.id_ticket == idTicket).ToList())
                {
                    var i = Incidentes(ctx, c.tipo_equipo, c.id_incidente).FirstOrDefault();
                    if (i != null && i.EquipoId != null) { r.Add(c.tipo_equipo + ":" + i.EquipoId.Value); }
                }
                return r;
            }
        }

        /// <summary>Incidentes que generó un ticket (para mostrarlos en su detalle).</summary>
        public List<DTO_IncidenteFila> IncidentesDeTicket(int idTicket)
        {
            using (var ctx = new SGEIContext())
            {
                var claves = ctx.ticket_incidente.Where(x => x.id_ticket == idTicket).OrderBy(x => x.fecha).ToList();
                var r = new List<DTO_IncidenteFila>();
                foreach (var grupo in claves.GroupBy(c => c.tipo_equipo))
                {
                    var equipos = Equipos(ctx, grupo.Key);
                    var catalogo = CatalogoIncidentes(ctx, grupo.Key, false);
                    foreach (var c in grupo)
                    {
                        var i = Incidentes(ctx, grupo.Key, c.id_incidente).FirstOrDefault();
                        if (i != null) { r.Add(Fila(grupo.Key, i, equipos, catalogo)); }
                    }
                }
                return r;
            }
        }
    }
}
