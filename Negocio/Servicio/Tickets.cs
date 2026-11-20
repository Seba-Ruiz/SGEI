using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Negocio.DTO;

namespace Negocio.Servicio
{
    /// <summary>Estados del board, en el orden de sus columnas.</summary>
    public static class EstadosTicket
    {
        public const byte Pendiente = 1;
        public const byte EnCurso = 2;
        public const byte Bloqueado = 3;
        public const byte Terminado = 4;

        public static readonly byte[] Todos = { Pendiente, EnCurso, Bloqueado, Terminado };

        public static bool EsValido(byte estado) { return estado >= Pendiente && estado <= Terminado; }

        public static string Nombre(byte estado)
        {
            switch (estado)
            {
                case Pendiente: return "Pendiente";
                case EnCurso: return "En curso";
                case Bloqueado: return "Bloqueado";
                case Terminado: return "Terminado";
                default: return "?";
            }
        }
    }

    public static class PrioridadesTicket
    {
        public const byte Baja = 1;
        public const byte Media = 2;
        public const byte Alta = 3;
        public const byte Urgente = 4;

        public static readonly byte[] Todas = { Baja, Media, Alta, Urgente };

        public static bool EsValida(byte prioridad) { return prioridad >= Baja && prioridad <= Urgente; }

        public static string Nombre(byte prioridad)
        {
            switch (prioridad)
            {
                case Baja: return "Baja";
                case Media: return "Media";
                case Alta: return "Alta";
                case Urgente: return "Urgente";
                default: return "?";
            }
        }
    }

    /// <summary>
    /// Tickets de trabajo (menú Tickets): board por estado, alta y edición (administradores), cambio de estado (el
    /// asignado o un administrador) y comentarios (cualquiera; cada uno edita o elimina los suyos). Cada cambio queda en
    /// ticket_historial con quién y cuándo. Tablas: BaseDeDatos/2026-11-20_tickets.sql.
    /// </summary>
    public class Tickets
    {
        public const int LargoTitulo = 150;
        /// <summary>Días que un ticket terminado sigue en el board.</summary>
        public const int DiasTerminadosEnBoard = 30;

        private const int LargoValorHistorial = 200;

        // ------------------------------------------------------------------ Usuarios

        /// <summary>Usuarios activos para asignar (más el que ya tenía el ticket, aunque esté dado de baja).</summary>
        public List<DTO_Usuario> UsuariosParaAsignar(string incluirId = null)
        {
            using (var ctx = new SGEIContext())
            {
                return Usuarios(ctx).Values.Where(u => u.Activo || u.Id == incluirId).OrderBy(u => u.Nombre).ToList();
            }
        }

        public DTO_Usuario Usuario(string id)
        {
            using (var ctx = new SGEIContext())
            {
                DTO_Usuario u;
                return id != null && Usuarios(ctx).TryGetValue(id, out u) ? u : null;
            }
        }

        private static Dictionary<string, DTO_Usuario> Usuarios(SGEIContext ctx)
        {
            return ctx.AspNetUsers
                .Select(u => new { u.Id, u.UserName, u.nombre, u.apellido, u.fecha_baja })
                .ToList()
                .ToDictionary(u => u.Id, u =>
                {
                    var nombre = ((u.nombre ?? "").Trim() + " " + (u.apellido ?? "").Trim()).Trim();
                    if (nombre.Length == 0) { nombre = u.UserName; }
                    return new DTO_Usuario { Id = u.Id, Nombre = nombre, Iniciales = Iniciales(nombre), Activo = u.fecha_baja == null };
                });
        }

        private static string Iniciales(string nombre)
        {
            var partes = (nombre ?? "").Split(new[] { ' ', '.', '_', '-', '@' }, StringSplitOptions.RemoveEmptyEntries);
            if (partes.Length == 0) { return "?"; }
            var r = partes.Length == 1 ? partes[0].Substring(0, Math.Min(2, partes[0].Length)) : partes[0].Substring(0, 1) + partes[partes.Length - 1].Substring(0, 1);
            return r.ToUpperInvariant();
        }

        // ------------------------------------------------------------------ Listados

        /// <summary>Tickets del board: los abiertos y los terminados en los últimos <see cref="DiasTerminadosEnBoard"/> días.</summary>
        public List<DTO_TicketTarjeta> Board()
        {
            var desde = DateTime.Today.AddDays(-DiasTerminadosEnBoard);
            using (var ctx = new SGEIContext())
            {
                return Tarjetas(ctx, ctx.ticket.Where(t => t.estado != EstadosTicket.Terminado || t.fecha_fin >= desde));
            }
        }

        /// <summary>Todos los tickets (grilla "Todos los tickets").</summary>
        public List<DTO_TicketTarjeta> Listar()
        {
            using (var ctx = new SGEIContext())
            {
                return Tarjetas(ctx, ctx.ticket);
            }
        }

        public DTO_TicketTarjeta Tarjeta(int id)
        {
            using (var ctx = new SGEIContext())
            {
                return Tarjetas(ctx, ctx.ticket.Where(t => t.id_ticket == id)).FirstOrDefault();
            }
        }

        private static List<DTO_TicketTarjeta> Tarjetas(SGEIContext ctx, IQueryable<ticket> consulta)
        {
            var filas = consulta
                .Select(t => new
                {
                    t.id_ticket, t.titulo, t.prioridad, t.estado, t.asignado_id, t.creado_por_id, t.fecha_alta, t.fecha_prevista,
                    t.fecha_estado, t.fecha_fin, t.tipo_equipo, t.id_equipo, comentarios = t.ticket_comentario.Count()
                })
                .ToList();
            var usuarios = Usuarios(ctx);
            var equipos = EquiposDe(ctx, filas.Select(f => Tuple.Create(f.tipo_equipo, f.id_equipo)));

            return filas.Select(f => new DTO_TicketTarjeta
            {
                Id = f.id_ticket,
                Titulo = f.titulo,
                Prioridad = f.prioridad,
                Estado = f.estado,
                AsignadoId = f.asignado_id,
                Asignado = NombreDe(usuarios, f.asignado_id),
                AsignadoIniciales = usuarios.ContainsKey(f.asignado_id) ? usuarios[f.asignado_id].Iniciales : "?",
                CreadoPorId = f.creado_por_id,
                CreadoPor = NombreDe(usuarios, f.creado_por_id),
                FechaAlta = f.fecha_alta,
                FechaPrevista = f.fecha_prevista,
                FechaEstado = f.fecha_estado,
                FechaFin = f.fecha_fin,
                TipoEquipo = f.tipo_equipo,
                EquipoId = f.id_equipo,
                Equipo = TextoEquipo(equipos, f.tipo_equipo, f.id_equipo),
                Comentarios = f.comentarios
            })
            // Más urgente primero; a igual prioridad, el que vence antes.
            .OrderByDescending(t => t.Prioridad).ThenBy(t => t.FechaPrevista).ThenBy(t => t.Id)
            .ToList();
        }

        /// <summary>Equipos de los tickets, por tipo (solo se leen los tipos que aparecen).</summary>
        private static Dictionary<string, Dictionary<int, DTO_Equipo>> EquiposDe(SGEIContext ctx, IEnumerable<Tuple<string, int?>> claves)
        {
            return claves
                .Where(c => c.Item2 != null && TiposEquipo.EsValido(c.Item1))
                .Select(c => c.Item1)
                .Distinct()
                .ToDictionary(t => t, t => ConsultasUnificadas.Equipos(ctx, t));
        }

        private static string TextoEquipo(Dictionary<string, Dictionary<int, DTO_Equipo>> equipos, string tipo, int? id)
        {
            if (id == null || tipo == null) { return null; }
            Dictionary<int, DTO_Equipo> delTipo;
            DTO_Equipo e;
            if (equipos.TryGetValue(tipo, out delTipo) && delTipo.TryGetValue(id.Value, out e)) { return e.Texto; }
            return TiposEquipo.Nombre(tipo) + " #" + id;
        }

        private static string NombreDe(Dictionary<string, DTO_Usuario> usuarios, string id)
        {
            DTO_Usuario u;
            return id != null && usuarios.TryGetValue(id, out u) ? u.Nombre : "(usuario eliminado)";
        }

        // ------------------------------------------------------------------ Detalle

        public DTO_TicketDetalle Detalle(int id)
        {
            using (var ctx = new SGEIContext())
            {
                var t = ctx.ticket.Find(id);
                if (t == null) { return null; }
                var tarjeta = Tarjetas(ctx, ctx.ticket.Where(x => x.id_ticket == id)).First();
                var usuarios = Usuarios(ctx);
                Func<string, string> iniciales = u => usuarios.ContainsKey(u) ? usuarios[u].Iniciales : "?";

                var actividad = ctx.ticket_comentario.Where(c => c.id_ticket == id).ToList()
                    .Select(c => new DTO_TicketActividad
                    {
                        EsComentario = true, Id = c.id_comentario, UsuarioId = c.usuario_id, Usuario = NombreDe(usuarios, c.usuario_id),
                        Iniciales = iniciales(c.usuario_id), Fecha = c.fecha, FechaEdicion = c.fecha_edicion, Texto = c.texto
                    })
                    .Concat(ctx.ticket_historial.Where(h => h.id_ticket == id).ToList()
                    .Select(h => new DTO_TicketActividad
                    {
                        Id = h.id_historial, UsuarioId = h.usuario_id, Usuario = NombreDe(usuarios, h.usuario_id), Iniciales = iniciales(h.usuario_id),
                        Fecha = h.fecha, Campo = h.campo, Anterior = h.valor_anterior, Nuevo = h.valor_nuevo
                    }))
                    .OrderBy(a => a.Fecha).ThenBy(a => a.EsComentario).ThenBy(a => a.Id)
                    .ToList();

                var detalle = new DTO_TicketDetalle { Descripcion = t.descripcion, Actividad = actividad };
                foreach (var p in typeof(DTO_TicketTarjeta).GetProperties().Where(p => p.CanWrite))
                {
                    p.SetValue(detalle, p.GetValue(tarjeta));
                }
                return detalle;
            }
        }

        public DTO_TicketFormulario Formulario(int id)
        {
            using (var ctx = new SGEIContext())
            {
                var t = ctx.ticket.Find(id);
                return t == null ? null : new DTO_TicketFormulario
                {
                    Id = t.id_ticket, Titulo = t.titulo, Descripcion = t.descripcion, Prioridad = t.prioridad, AsignadoId = t.asignado_id,
                    FechaPrevista = t.fecha_prevista, TipoEquipo = t.tipo_equipo, EquipoId = t.id_equipo
                };
            }
        }

        // ------------------------------------------------------------------ Escritura

        /// <summary>Alta o edición (ya validada). Deja en el historial el alta o cada dato que cambió.</summary>
        public int Guardar(DTO_TicketFormulario f, string usuarioId)
        {
            var ahora = DateTime.Now;
            using (var ctx = new SGEIContext())
            {
                var usuarios = Usuarios(ctx);
                var tipo = f.EquipoId != null && TiposEquipo.EsValido(f.TipoEquipo) ? f.TipoEquipo : null;
                var idEquipo = tipo != null ? f.EquipoId : null;
                var descripcion = HtmlSeguro.EstaVacio(f.Descripcion) ? null : HtmlSeguro.Limpiar(f.Descripcion);

                ticket t;
                if (f.Id == 0)
                {
                    t = new ticket
                    {
                        estado = EstadosTicket.Pendiente, creado_por_id = usuarioId, fecha_alta = ahora, fecha_estado = ahora
                    };
                    ctx.ticket.Add(t);
                    t.ticket_historial.Add(Historial(usuarioId, ahora, "ALTA", null, NombreDe(usuarios, f.AsignadoId)));
                }
                else
                {
                    t = ctx.ticket.Find(f.Id);
                    if (t == null) { throw new InvalidOperationException("No existe el ticket " + f.Id + "."); }
                    var equipos = EquiposDe(ctx, new[] { Tuple.Create(t.tipo_equipo, t.id_equipo), Tuple.Create(tipo, idEquipo) });
                    Action<string, string, string> cambio = (campo, antes, despues) =>
                    {
                        if (antes != despues) { t.ticket_historial.Add(Historial(usuarioId, ahora, campo, antes, despues)); }
                    };
                    cambio("TITULO", t.titulo, f.Titulo);
                    cambio("ASIGNADO", NombreDe(usuarios, t.asignado_id), NombreDe(usuarios, f.AsignadoId));
                    cambio("PRIORIDAD", PrioridadesTicket.Nombre(t.prioridad), PrioridadesTicket.Nombre(f.Prioridad.Value));
                    cambio("FECHA_PREVISTA", t.fecha_prevista.ToString("dd/MM/yyyy"), f.FechaPrevista.Value.ToString("dd/MM/yyyy"));
                    cambio("EQUIPO", TextoEquipo(equipos, t.tipo_equipo, t.id_equipo), TextoEquipo(equipos, tipo, idEquipo));
                    if ((t.descripcion ?? "") != (descripcion ?? "")) { t.ticket_historial.Add(Historial(usuarioId, ahora, "DESCRIPCION", null, null)); }
                }

                t.titulo = f.Titulo;
                t.descripcion = descripcion;
                t.prioridad = f.Prioridad.Value;
                t.asignado_id = f.AsignadoId;
                t.fecha_prevista = f.FechaPrevista.Value.Date;
                t.tipo_equipo = tipo;
                t.id_equipo = idEquipo;
                ctx.SaveChanges();
                return t.id_ticket;
            }
        }

        /// <summary>Cambia el estado. Devuelve un mensaje de error, o null si se pudo.</summary>
        public string CambiarEstado(int id, byte estado, string usuarioId, bool esAdministrador)
        {
            if (!EstadosTicket.EsValido(estado)) { return "Estado desconocido."; }
            using (var ctx = new SGEIContext())
            {
                var t = ctx.ticket.Find(id);
                if (t == null) { return "El ticket ya no existe."; }
                if (!esAdministrador && t.asignado_id != usuarioId) { return "Solo la persona asignada o un administrador pueden cambiar el estado."; }
                if (t.estado == estado) { return null; }

                var ahora = DateTime.Now;
                t.ticket_historial.Add(Historial(usuarioId, ahora, "ESTADO", EstadosTicket.Nombre(t.estado), EstadosTicket.Nombre(estado)));
                t.estado = estado;
                t.fecha_estado = ahora;
                t.fecha_fin = estado == EstadosTicket.Terminado ? ahora : (DateTime?)null;
                ctx.SaveChanges();
                return null;
            }
        }

        /// <summary>Elimina el ticket con sus comentarios e historial. Devuelve el título, o null si no existía.</summary>
        public string Eliminar(int id, string nombreUsuario)
        {
            using (var ctx = new SGEIContext())
            {
                var t = ctx.ticket.Find(id);
                if (t == null) { return null; }
                var titulo = t.titulo;
                ctx.ticket.Remove(t);
                ctx.Auditoria.Add(new Auditoria { fecha_hora = DateTime.Now, tipo_equipo = "TICKET", id_equipo = id, accion = "ELIMINAR_TICKET", usuario = nombreUsuario });
                ctx.SaveChanges();
                return titulo;
            }
        }

        // ------------------------------------------------------------------ Comentarios

        /// <summary>Agrega un comentario (HTML de Summernote). Devuelve su id, o null si el ticket no existe.</summary>
        public int? Comentar(int idTicket, string usuarioId, string html)
        {
            using (var ctx = new SGEIContext())
            {
                if (!ctx.ticket.Any(t => t.id_ticket == idTicket)) { return null; }
                var c = new ticket_comentario { id_ticket = idTicket, usuario_id = usuarioId, texto = HtmlSeguro.Limpiar(html), fecha = DateTime.Now };
                ctx.ticket_comentario.Add(c);
                ctx.SaveChanges();
                return c.id_comentario;
            }
        }

        public int? TicketDeComentario(int idComentario)
        {
            using (var ctx = new SGEIContext())
            {
                return ctx.ticket_comentario.Where(c => c.id_comentario == idComentario).Select(c => (int?)c.id_ticket).FirstOrDefault();
            }
        }

        /// <summary>Edita un comentario propio. Devuelve un error, o null si se pudo; idTicket queda con su ticket.</summary>
        public string EditarComentario(int idComentario, string usuarioId, string html, out int idTicket)
        {
            idTicket = 0;
            using (var ctx = new SGEIContext())
            {
                var c = ctx.ticket_comentario.Find(idComentario);
                if (c == null) { return "El comentario ya no existe."; }
                idTicket = c.id_ticket;
                if (c.usuario_id != usuarioId) { return "Solo podés editar tus comentarios."; }
                c.texto = HtmlSeguro.Limpiar(html);
                c.fecha_edicion = DateTime.Now;
                ctx.SaveChanges();
                return null;
            }
        }

        /// <summary>Elimina un comentario propio. Devuelve un error, o null si se pudo; idTicket queda con su ticket.</summary>
        public string EliminarComentario(int idComentario, string usuarioId, out int idTicket)
        {
            idTicket = 0;
            using (var ctx = new SGEIContext())
            {
                var c = ctx.ticket_comentario.Find(idComentario);
                if (c == null) { return "El comentario ya no existe."; }
                idTicket = c.id_ticket;
                if (c.usuario_id != usuarioId) { return "Solo podés eliminar tus comentarios."; }
                ctx.ticket_comentario.Remove(c);
                ctx.SaveChanges();
                return null;
            }
        }

        private static ticket_historial Historial(string usuarioId, DateTime fecha, string campo, string anterior, string nuevo)
        {
            Func<string, string> corto = s => s != null && s.Length > LargoValorHistorial ? s.Substring(0, LargoValorHistorial - 1) + "…" : s;
            return new ticket_historial { usuario_id = usuarioId, fecha = fecha, campo = campo, valor_anterior = corto(anterior), valor_nuevo = corto(nuevo) };
        }
    }
}
