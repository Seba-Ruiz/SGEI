using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Model;

namespace Negocio.Servicio
{
    /// <summary>Una serie de un gráfico: nombre, color y un valor por cada etiqueta.</summary>
    public class SerieGrafico
    {
        public string Nombre { get; set; }
        public string Color { get; set; }
        public List<int> Valores { get; set; }
    }

    /// <summary>Datos de un gráfico: etiquetas del eje y sus series.</summary>
    public class DatosGrafico
    {
        public List<string> Etiquetas { get; set; }
        public List<SerieGrafico> Series { get; set; }
        public int Total { get { return Series == null ? 0 : Series.Sum(s => s.Valores.Sum()); } }
    }

    /// <summary>
    /// Datos del Dashboard: incidentes por mes (por tipo de equipo), tickets por estado y por usuario
    /// (los mismos que muestra el board) y computadoras activas por sistema operativo.
    /// Colores: paleta categórica validada para daltonismo (tipos de equipo); estados de tickets en gris, azul,
    /// naranja y verde agua (Pendiente, En curso, Bloqueado, Terminado).
    /// </summary>
    public class Dashboard
    {
        public const int MesesIncidentes = 12;

        private static readonly Dictionary<string, string> ColoresTipo = new Dictionary<string, string>
        {
            { TiposEquipo.Computadora, "#2a78d6" },
            { TiposEquipo.Impresora, "#eb6834" },
            { TiposEquipo.Telefono, "#1baf7a" },
            { TiposEquipo.Camara, "#eda100" },
            { TiposEquipo.Switch, "#e87ba4" },
            { TiposEquipo.Escaner, "#008300" }
        };

        public static readonly Dictionary<byte, string> ColoresEstado = new Dictionary<byte, string>
        {
            { EstadosTicket.Pendiente, "#8a96a3" },
            { EstadosTicket.EnCurso, "#2a78d6" },
            { EstadosTicket.Bloqueado, "#eb6834" },
            { EstadosTicket.Terminado, "#1baf7a" }
        };

        /// <summary>Incidentes de los últimos 12 meses (incluido el actual), una serie por tipo de equipo.</summary>
        public DatosGrafico IncidentesPorMes()
        {
            var primerMes = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-(MesesIncidentes - 1));
            var meses = Enumerable.Range(0, MesesIncidentes).Select(i => primerMes.AddMonths(i)).ToList();
            var fechas = new ConsultasUnificadas().FechasDeIncidentes(primerMes);
            var cultura = new CultureInfo("es-AR");

            return new DatosGrafico
            {
                Etiquetas = meses.Select(m => Capitalizar(m.ToString("MMMM", cultura).Substring(0, 3)) + " " + m.ToString("yy")).ToList(),
                Series = TiposEquipo.Todos.Select(tipo => new SerieGrafico
                {
                    Nombre = TiposEquipo.Plural(tipo),
                    Color = ColoresTipo[tipo],
                    Valores = meses.Select(m => fechas[tipo].Count(f => f.Year == m.Year && f.Month == m.Month)).ToList()
                }).ToList()
            };
        }

        /// <summary>Tickets del board por estado (una serie por estado, con un solo valor).</summary>
        public DatosGrafico TicketsPorEstado(List<Negocio.DTO.DTO_TicketTarjeta> tickets)
        {
            return new DatosGrafico
            {
                Etiquetas = EstadosTicket.Todos.Select(EstadosTicket.Nombre).ToList(),
                Series = new List<SerieGrafico>
                {
                    new SerieGrafico
                    {
                        Nombre = "Tickets",
                        Valores = EstadosTicket.Todos.Select(e => tickets.Count(t => t.Estado == e)).ToList()
                    }
                }
            };
        }

        /// <summary>Tickets del board por persona asignada, una serie por estado. Más cargados primero.</summary>
        public DatosGrafico TicketsPorUsuario(List<Negocio.DTO.DTO_TicketTarjeta> tickets)
        {
            var personas = tickets.GroupBy(t => t.Asignado)
                .OrderByDescending(g => g.Count()).ThenBy(g => g.Key)
                .Select(g => g.Key).ToList();
            return new DatosGrafico
            {
                Etiquetas = personas,
                Series = EstadosTicket.Todos.Select(e => new SerieGrafico
                {
                    Nombre = EstadosTicket.Nombre(e),
                    Color = ColoresEstado[e],
                    Valores = personas.Select(p => tickets.Count(t => t.Asignado == p && t.Estado == e)).ToList()
                }).ToList()
            };
        }

        /// <summary>Computadoras activas por sistema operativo (según su último detalle). Más usados primero.</summary>
        public DatosGrafico ComputadorasPorSistemaOperativo()
        {
            using (var ctx = new SGEIContext())
            {
                var activas = new HashSet<int>(ctx.pc.Where(p => p.fecha_baja == null).Select(p => p.id_pc));
                var sistemas = ctx.soPC.ToDictionary(s => s.id_so, s => (s.descripcion ?? "").Trim());
                var ultimoDetalle = ctx.detallePC
                    .Where(d => d.pc_id != null)
                    .Select(d => new { d.id_detalle, d.pc_id, d.so_id })
                    .ToList()
                    .GroupBy(d => d.pc_id.Value)
                    .ToDictionary(g => g.Key, g => g.OrderByDescending(d => d.id_detalle).First().so_id);

                var conteo = activas
                    .Select(id =>
                    {
                        int? so;
                        string nombre;
                        return ultimoDetalle.TryGetValue(id, out so) && so.HasValue && sistemas.TryGetValue(so.Value, out nombre) && nombre.Length > 0
                            ? nombre : "Sin dato";
                    })
                    .GroupBy(n => n, StringComparer.OrdinalIgnoreCase)
                    .Select(g => new { Nombre = g.First(), Cantidad = g.Count() })
                    // "Sin dato" siempre al final.
                    .OrderBy(x => x.Nombre == "Sin dato").ThenByDescending(x => x.Cantidad).ThenBy(x => x.Nombre)
                    .ToList();

                return new DatosGrafico
                {
                    Etiquetas = conteo.Select(x => x.Nombre).ToList(),
                    Series = new List<SerieGrafico>
                    {
                        new SerieGrafico { Nombre = "Computadoras", Color = "#2a78d6", Valores = conteo.Select(x => x.Cantidad).ToList() }
                    }
                };
            }
        }

        private static string Capitalizar(string s)
        {
            return string.IsNullOrEmpty(s) ? s : char.ToUpperInvariant(s[0]) + s.Substring(1);
        }
    }
}
