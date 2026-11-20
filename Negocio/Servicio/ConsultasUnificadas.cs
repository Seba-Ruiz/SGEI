using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Negocio.DTO;

namespace Negocio.Servicio
{
    /// <summary>Tipos de equipo de SGEI: clave usada en URLs y formularios, nombre para mostrar y nombre en Auditoria.</summary>
    public static class TiposEquipo
    {
        public const string Computadora = "computadora";
        public const string Impresora = "impresora";
        public const string Telefono = "telefono";
        public const string Camara = "camara";
        public const string Switch = "switch";
        public const string Escaner = "escaner";

        private static readonly Dictionary<string, string[]> datos = new Dictionary<string, string[]>
        {
            // clave => [singular, plural, auditoría, "el/la"]
            { Computadora, new[] { "Computadora", "Computadoras", "COMPUTADORA", "la" } },
            { Impresora,   new[] { "Impresora",   "Impresoras",   "IMPRESORA",   "la" } },
            { Telefono,    new[] { "Teléfono",    "Teléfonos",    "TELEFONO",    "el" } },
            { Camara,      new[] { "Cámara",      "Cámaras",      "CAMARA",      "la" } },
            { Switch,      new[] { "Switch",      "Switches",     "SWITCH",      "el" } },
            { Escaner,     new[] { "Escáner",     "Escáneres",    "ESCANER",     "el" } },
        };

        /// <summary>Todos los tipos, en el orden en que se muestran.</summary>
        public static readonly string[] Todos = { Computadora, Impresora, Telefono, Camara, Switch, Escaner };

        /// <summary>Tipos que registran insumos colocados.</summary>
        public static readonly string[] ConInsumos = { Computadora, Impresora };

        public static bool EsValido(string tipo) { return tipo != null && datos.ContainsKey(tipo); }
        public static string Nombre(string tipo) { return EsValido(tipo) ? datos[tipo][0] : tipo; }
        public static string Plural(string tipo) { return EsValido(tipo) ? datos[tipo][1] : tipo; }
        public static string Auditoria(string tipo) { return datos[tipo][2]; }

        /// <summary>"la computadora", "el teléfono".</summary>
        public static string ConArticulo(string tipo) { return datos[tipo][3] + " " + datos[tipo][0].ToLowerInvariant(); }

        /// <summary>Largo máximo de la acción realizada de un incidente (columna de la base de cada tipo).</summary>
        public static int LargoAccion(string tipo) { return tipo == Camara || tipo == Switch || tipo == Escaner ? 150 : 50; }

        /// <summary>Largo máximo de la descripción libre de un incidente de impresora y del n° de serie de un insumo de PC.</summary>
        public const int LargoTexto = 50;
    }

    /// <summary>
    /// Incidentes, insumos colocados y bajas de todos los equipos en un solo lugar (menú Consultas).
    /// Cada tipo de equipo guarda sus datos en tablas propias; acá se leen y escriben con una forma común.
    ///
    /// Clave del equipo en cada tipo (la que guardan sus incidentes, insumos y bajas):
    ///   computadora: pc.id_pc (insumos: detallePC.id_detalle) · impresora: ubicacion_impresora.id ·
    ///   teléfono: detalle_tel.id_detalle · cámara: camara.id_camara · switch: swicth.id_switch · escáner: escaner.id_escaner
    /// </summary>
    public class ConsultasUnificadas
    {
        // ------------------------------------------------------------------ Equipos

        /// <summary>Equipos de un tipo (activos y dados de baja), por su clave.</summary>
        internal static Dictionary<int, DTO_Equipo> Equipos(SGEIContext ctx, string tipo)
        {
            var lista = new List<DTO_Equipo>();
            switch (tipo)
            {
                case TiposEquipo.Computadora:
                {
                    var ubis = ctx.ubicacionPC.ToDictionary(u => u.id_ubicacion, u => u.nombre);
                    var rel = UltimoPor(ctx.pc_ubicacionpc.Where(x => x.pc_id != null).ToList(), x => x.pc_id.Value, x => x.id_upc);
                    foreach (var p in ctx.pc.ToList())
                    {
                        var u = rel.ContainsKey(p.id_pc) ? rel[p.id_pc].ubicacionpc_id : null;
                        lista.Add(new DTO_Equipo { Id = p.id_pc, EquipoId = p.id_pc, Nombre = p.nombre, Ip = p.ip, UbicacionId = u, Ubicacion = Nombre(ubis, u), FechaBaja = p.fecha_baja });
                    }
                    break;
                }
                case TiposEquipo.Impresora:
                {
                    var ubis = ctx.ubicacion.ToDictionary(u => u.id, u => u.nombreUbicacion);
                    var modelos = ctx.impresora.ToDictionary(i => i.id, i => i.marca_modelo);
                    var det = UltimoPor(ctx.detalle_impresora_ubicacion.Where(x => x.ubicacion_impresora_id != null).ToList(), x => x.ubicacion_impresora_id.Value, x => x.id);
                    foreach (var ui in ctx.ubicacion_impresora.ToList())
                    {
                        var modelo = Nombre(modelos, ui.impresora_id);
                        var nombre = string.IsNullOrWhiteSpace(ui.descripcion) ? modelo
                                   : string.IsNullOrWhiteSpace(modelo) ? ui.descripcion : ui.descripcion + " (" + modelo + ")";
                        lista.Add(new DTO_Equipo
                        {
                            Id = ui.id, EquipoId = ui.id, Nombre = nombre, Ip = det.ContainsKey(ui.id) ? det[ui.id].ip : null,
                            UbicacionId = ui.ubicacion_id, Ubicacion = Nombre(ubis, ui.ubicacion_id), FechaBaja = ui.fecha_baja
                        });
                    }
                    break;
                }
                case TiposEquipo.Telefono:
                {
                    var ubis = ctx.ubicacion_tel.ToDictionary(u => u.id_ubicacion_tel, u => u.nombre_ubi);
                    var tels = ctx.telefono.ToDictionary(t => t.id_telefono);
                    var rel = UltimoPor(ctx.ubicacion_tel_tel.Where(x => x.detalle_id != null).ToList(), x => x.detalle_id.Value, x => x.id_ubicacion_tel_tel);
                    foreach (var d in ctx.detalle_tel.ToList())
                    {
                        telefono t;
                        if (d.telefono_id == null || !tels.TryGetValue(d.telefono_id.Value, out t)) { continue; }
                        var u = rel.ContainsKey(d.id_detalle) ? rel[d.id_detalle].ubicacion_tel_id : null;
                        lista.Add(new DTO_Equipo
                        {
                            Id = d.id_detalle, EquipoId = t.id_telefono, DetalleId = d.id_detalle, Nombre = t.descripcion,
                            Ip = string.IsNullOrWhiteSpace(d.nro_interno) ? null : "Int. " + d.nro_interno,
                            UbicacionId = u, Ubicacion = Nombre(ubis, u), FechaBaja = t.fecha_baja
                        });
                    }
                    break;
                }
                case TiposEquipo.Camara:
                {
                    var ubis = ctx.ubicacion_camara.ToDictionary(u => u.id_ubicacion_camara, u => u.nombre);
                    var det = UltimoPor(ctx.detalle_camara.Where(x => x.camara_id != null).ToList(), x => x.camara_id.Value, x => x.id_detalle);
                    var rel = UltimoPor(ctx.ubicacion_camara_camara.Where(x => x.detalle_camara_id != null).ToList(), x => x.detalle_camara_id.Value, x => x.id_ubicacion_camara_camara);
                    foreach (var c in ctx.camara.ToList())
                    {
                        var d = det.ContainsKey(c.id_camara) ? det[c.id_camara] : null;
                        var u = d != null && rel.ContainsKey(d.id_detalle) ? rel[d.id_detalle].ubicacion_camara_id : null;
                        lista.Add(new DTO_Equipo
                        {
                            Id = c.id_camara, EquipoId = c.id_camara, DetalleId = d != null ? d.id_detalle : (int?)null, Nombre = c.descripcion,
                            Ip = d != null ? d.nroip : null, UbicacionId = u, Ubicacion = Nombre(ubis, u), FechaBaja = c.fecha_baja
                        });
                    }
                    break;
                }
                case TiposEquipo.Switch:
                {
                    var ubis = ctx.ubicacion_switch.ToDictionary(u => u.id_ubicacion_switch, u => u.nombre);
                    var det = UltimoPor(ctx.switch_detalle.Where(x => x.switch_id != null).ToList(), x => x.switch_id.Value, x => x.id_detalle_switch);
                    var rel = UltimoPor(ctx.switch_ubicacion_switch.Where(x => x.detalle_swich_id != null).ToList(), x => x.detalle_swich_id.Value, x => x.id_switch_ubicacion_switch);
                    foreach (var s in ctx.swicth.ToList())
                    {
                        var d = det.ContainsKey(s.id_switch) ? det[s.id_switch] : null;
                        var u = d != null && rel.ContainsKey(d.id_detalle_switch) ? rel[d.id_detalle_switch].ubicacion_switch_id : null;
                        lista.Add(new DTO_Equipo
                        {
                            Id = s.id_switch, EquipoId = s.id_switch, DetalleId = d != null ? d.id_detalle_switch : (int?)null, Nombre = s.descripcion,
                            Ip = d != null ? d.nroip : null, UbicacionId = u, Ubicacion = Nombre(ubis, u), FechaBaja = s.fecha_baja
                        });
                    }
                    break;
                }
                case TiposEquipo.Escaner:
                {
                    var ubis = ctx.ubicacion_escaner.ToDictionary(u => u.id_ubicacion_escaner, u => u.nombre);
                    var det = UltimoPor(ctx.detalle_escaner.Where(x => x.escaner_id != null).ToList(), x => x.escaner_id.Value, x => x.id_detalle_escaner);
                    var rel = UltimoPor(ctx.escaner_ubicacion_escaner.Where(x => x.detalle_escaner_id != null).ToList(), x => x.detalle_escaner_id.Value, x => x.id_escaner_ubicacion_escaner);
                    foreach (var e in ctx.escaner.ToList())
                    {
                        var d = det.ContainsKey(e.id_escaner) ? det[e.id_escaner] : null;
                        var u = d != null && rel.ContainsKey(d.id_detalle_escaner) ? rel[d.id_detalle_escaner].ubicacion_escaner_id : null;
                        lista.Add(new DTO_Equipo
                        {
                            Id = e.id_escaner, EquipoId = e.id_escaner, DetalleId = d != null ? d.id_detalle_escaner : (int?)null, Nombre = e.descripcion,
                            Ip = d != null ? d.nroip : null, UbicacionId = u, Ubicacion = Nombre(ubis, u), FechaBaja = e.fecha_baja
                        });
                    }
                    break;
                }
                default:
                    throw new ArgumentException("Tipo de equipo desconocido: " + tipo);
            }
            return lista.GroupBy(e => e.Id).ToDictionary(g => g.Key, g => g.First());
        }

        /// <summary>Computadoras por id de detalle (clave que usan sus insumos).</summary>
        private static Dictionary<int, DTO_Equipo> ComputadorasPorDetalle(SGEIContext ctx)
        {
            var pcs = Equipos(ctx, TiposEquipo.Computadora);
            var r = new Dictionary<int, DTO_Equipo>();
            foreach (var d in ctx.detallePC.Where(x => x.pc_id != null).ToList())
            {
                DTO_Equipo p;
                if (!pcs.TryGetValue(d.pc_id.Value, out p)) { continue; }
                r[d.id_detalle] = new DTO_Equipo
                {
                    Id = d.id_detalle, EquipoId = p.EquipoId, DetalleId = d.id_detalle, Nombre = p.Nombre, Ip = p.Ip,
                    UbicacionId = p.UbicacionId, Ubicacion = p.Ubicacion, FechaBaja = p.FechaBaja
                };
            }
            return r;
        }

        /// <summary>Equipos activos de un tipo para elegir en un formulario (más el que ya tenía el registro, aunque esté de baja).</summary>
        public List<DTO_Equipo> EquiposParaElegir(string tipo, bool paraInsumos, int? incluirId = null)
        {
            using (var ctx = new SGEIContext())
            {
                var equipos = paraInsumos && tipo == TiposEquipo.Computadora ? ComputadorasPorDetalle(ctx) : Equipos(ctx, tipo);
                return equipos.Values
                    .Where(e => e.FechaBaja == null || e.Id == incluirId)
                    .OrderBy(e => e.Nombre)
                    .ToList();
            }
        }

        // ------------------------------------------------------------------ Incidentes

        /// <summary>Catálogo de incidentes de un tipo (las impresoras no tienen: el incidente se escribe).</summary>
        public List<DTO_Opcion> CatalogoIncidentes(string tipo)
        {
            using (var ctx = new SGEIContext())
            {
                return CatalogoIncidentes(ctx, tipo, true).Select(kv => new DTO_Opcion { Id = kv.Key, Texto = kv.Value }).OrderBy(o => o.Texto).ToList();
            }
        }

        private static Dictionary<int, string> CatalogoIncidentes(SGEIContext ctx, string tipo, bool soloActivos)
        {
            switch (tipo)
            {
                case TiposEquipo.Computadora: return ctx.incidentePC.Where(x => !soloActivos || x.fecha_baja == null).ToDictionary(x => x.id_incidentepc, x => x.nombre);
                case TiposEquipo.Telefono: return ctx.incidente_tel.Where(x => !soloActivos || x.fecha_baja == null).ToDictionary(x => x.id_incidente_tel, x => x.nombre_incidente);
                case TiposEquipo.Camara: return ctx.incidente_camara.Where(x => !soloActivos || x.fecha_baja == null).ToDictionary(x => x.id_incidente, x => x.descripcion);
                case TiposEquipo.Switch: return ctx.incidente_switch.Where(x => !soloActivos || x.fecha_baja == null).ToDictionary(x => x.id_incidente_switch, x => x.descripcion);
                case TiposEquipo.Escaner: return ctx.incidente_escaner.Where(x => !soloActivos || x.fecha_baja == null).ToDictionary(x => x.id_incidente_escaner, x => x.descripcion);
                default: return new Dictionary<int, string>();
            }
        }

        /// <summary>Incidentes de todos los equipos, del más reciente al más viejo.</summary>
        public List<DTO_IncidenteFila> ListarIncidentes()
        {
            var filas = new List<DTO_IncidenteFila>();
            using (var ctx = new SGEIContext())
            {
                foreach (var tipo in TiposEquipo.Todos)
                {
                    var equipos = Equipos(ctx, tipo);
                    var catalogo = CatalogoIncidentes(ctx, tipo, false);
                    filas.AddRange(Incidentes(ctx, tipo).Select(i => Fila(tipo, i, equipos, catalogo)));
                }
                var fotos = ctx.imagen.Where(x => x.incidente_id != null).GroupBy(x => x.incidente_id.Value)
                               .Select(g => new { g.Key, Cantidad = g.Count() }).ToDictionary(x => x.Key, x => x.Cantidad);
                foreach (var f in filas.Where(f => f.Tipo == TiposEquipo.Impresora && fotos.ContainsKey(f.Id)))
                {
                    f.Fotos = fotos[f.Id];
                }
            }
            return filas.OrderByDescending(f => f.FechaIncidente).ThenByDescending(f => f.Id).ToList();
        }

        /// <summary>Fechas de los incidentes de cada tipo de equipo desde una fecha (gráfico del Dashboard).</summary>
        public Dictionary<string, List<DateTime>> FechasDeIncidentes(DateTime desde)
        {
            using (var ctx = new SGEIContext())
            {
                return TiposEquipo.Todos.ToDictionary(
                    tipo => tipo,
                    tipo => Incidentes(ctx, tipo)
                        .Where(i => i.FechaIncidente.HasValue && i.FechaIncidente.Value >= desde)
                        .Select(i => i.FechaIncidente.Value)
                        .ToList());
            }
        }

        /// <summary>Un incidente con todos sus datos (para el detalle).</summary>
        public DTO_IncidenteFila ObtenerIncidenteFila(string tipo, int id)
        {
            using (var ctx = new SGEIContext())
            {
                var i = Incidentes(ctx, tipo, id).FirstOrDefault();
                if (i == null) { return null; }
                var fila = Fila(tipo, i, Equipos(ctx, tipo), CatalogoIncidentes(ctx, tipo, false));
                fila.Fotos = tipo == TiposEquipo.Impresora ? ctx.imagen.Count(x => x.incidente_id == id) : 0;
                return fila;
            }
        }

        /// <summary>Ids de las fotos de un incidente de impresora.</summary>
        public List<int> FotosDeIncidente(int idIncidente)
        {
            using (var ctx = new SGEIContext())
            {
                return ctx.imagen.Where(x => x.incidente_id == idIncidente).OrderBy(x => x.id).Select(x => x.id).ToList();
            }
        }

        public byte[] Foto(int idImagen)
        {
            using (var ctx = new SGEIContext())
            {
                var img = ctx.imagen.Find(idImagen);
                return img != null ? img.imagen1 : null;
            }
        }

        public void AgregarFoto(int idIncidente, byte[] contenido)
        {
            using (var ctx = new SGEIContext())
            {
                ctx.imagen.Add(new imagen { incidente_id = idIncidente, imagen1 = contenido });
                ctx.SaveChanges();
            }
        }

        /// <summary>Elimina una foto; devuelve el id del incidente al que pertenecía.</summary>
        public int? EliminarFoto(int idImagen)
        {
            using (var ctx = new SGEIContext())
            {
                var img = ctx.imagen.Find(idImagen);
                if (img == null) { return null; }
                var incidente = img.incidente_id;
                ctx.imagen.Remove(img);
                ctx.SaveChanges();
                return incidente;
            }
        }

        /// <summary>Datos de un incidente para editarlo, o null si no existe.</summary>
        public DTO_IncidenteFormulario ObtenerIncidente(string tipo, int id)
        {
            using (var ctx = new SGEIContext())
            {
                var i = Incidentes(ctx, tipo, id).FirstOrDefault();
                if (i == null) { return null; }
                return new DTO_IncidenteFormulario
                {
                    Tipo = tipo, Id = i.Id, EquipoId = i.EquipoId, IncidenteId = i.CatalogoId, IncidenteTexto = i.Texto,
                    FechaIncidente = i.FechaIncidente, Accion = i.Accion, FechaAccion = i.FechaAccion
                };
            }
        }

        /// <summary>Guarda un incidente nuevo o editado. Devuelve su id y el nombre del equipo.</summary>
        public int GuardarIncidente(DTO_IncidenteFormulario f, string usuario, out string equipo)
        {
            using (var ctx = new SGEIContext())
            {
                var nuevo = f.Id == 0;
                int id;
                switch (f.Tipo)
                {
                    case TiposEquipo.Impresora:
                    {
                        var x = nuevo ? ctx.incidente.Add(new incidente()) : ctx.incidente.Find(f.Id);
                        x.ubicacion_impresora_id = f.EquipoId; x.incidente1 = f.IncidenteTexto;
                        x.fecha_incidente = f.FechaIncidente; x.accion = f.Accion; x.fecha_accion = f.FechaAccion;
                        ctx.SaveChanges(); id = x.id;
                        break;
                    }
                    case TiposEquipo.Computadora:
                    {
                        var x = nuevo ? ctx.pc_incidentepc.Add(new pc_incidentepc()) : ctx.pc_incidentepc.Find(f.Id);
                        x.pc_id = f.EquipoId; x.incidentepc_id = f.IncidenteId;
                        x.fecha_incidente = f.FechaIncidente; x.reparacion = f.Accion; x.fecha_reparacion = f.FechaAccion;
                        ctx.SaveChanges(); id = x.id_pc_incidente;
                        break;
                    }
                    case TiposEquipo.Telefono:
                    {
                        var x = nuevo ? ctx.incidente_tel_tel.Add(new incidente_tel_tel()) : ctx.incidente_tel_tel.Find(f.Id);
                        x.detalle_id = f.EquipoId; x.incidentetel_id = f.IncidenteId;
                        x.fecha_incidente = f.FechaIncidente; x.reparacion = f.Accion; x.fecha_reparacion = f.FechaAccion;
                        ctx.SaveChanges(); id = x.id_incidente_tel_tel;
                        break;
                    }
                    case TiposEquipo.Camara:
                    {
                        var x = nuevo ? ctx.camara_incidente_camara.Add(new camara_incidente_camara()) : ctx.camara_incidente_camara.Find(f.Id);
                        x.camara_id = f.EquipoId; x.incidente_id = f.IncidenteId;
                        x.fecha_incidente = f.FechaIncidente; x.reparacion = f.Accion; x.fecha_reparacion = f.FechaAccion;
                        ctx.SaveChanges(); id = x.id_camara_incidente_camara;
                        break;
                    }
                    case TiposEquipo.Switch:
                    {
                        var x = nuevo ? ctx.switch_incidente_switch.Add(new switch_incidente_switch()) : ctx.switch_incidente_switch.Find(f.Id);
                        x.switch_id = f.EquipoId; x.incidente_switch_id = f.IncidenteId;
                        x.fecha_incidente = f.FechaIncidente; x.reparacion = f.Accion; x.fecha_reparacion = f.FechaAccion;
                        ctx.SaveChanges(); id = x.id_switch_incidente_switch;
                        break;
                    }
                    case TiposEquipo.Escaner:
                    {
                        var x = nuevo ? ctx.escaner_incidente_escaner.Add(new escaner_incidente_escaner()) : ctx.escaner_incidente_escaner.Find(f.Id);
                        x.escaner_id = f.EquipoId; x.incidente_escaner_id = f.IncidenteId;
                        x.fecha_incidente = f.FechaIncidente; x.reparacion = f.Accion; x.fecha_reparacion = f.FechaAccion;
                        ctx.SaveChanges(); id = x.id_escaner_incidente_escaner;
                        break;
                    }
                    default:
                        throw new ArgumentException("Tipo de equipo desconocido: " + f.Tipo);
                }

                var eq = Equipos(ctx, f.Tipo);
                DTO_Equipo e = null;
                if (f.EquipoId != null) { eq.TryGetValue(f.EquipoId.Value, out e); }
                equipo = e != null ? e.Nombre : null;
                Auditar(ctx, f.Tipo, e != null ? e.EquipoId : f.EquipoId, nuevo ? "INCIDENTE" : "EDITAR_INCIDENTE", usuario);
                return id;
            }
        }

        /// <summary>Elimina un incidente (y sus fotos, si es de impresora). Devuelve el nombre del equipo, o null si no existía.</summary>
        public string EliminarIncidente(string tipo, int id, string usuario)
        {
            using (var ctx = new SGEIContext())
            {
                var i = Incidentes(ctx, tipo, id).FirstOrDefault();
                if (i == null) { return null; }
                switch (tipo)
                {
                    case TiposEquipo.Impresora:
                        ctx.imagen.RemoveRange(ctx.imagen.Where(x => x.incidente_id == id));
                        ctx.incidente.Remove(ctx.incidente.Find(id));
                        break;
                    case TiposEquipo.Computadora: ctx.pc_incidentepc.Remove(ctx.pc_incidentepc.Find(id)); break;
                    case TiposEquipo.Telefono: ctx.incidente_tel_tel.Remove(ctx.incidente_tel_tel.Find(id)); break;
                    case TiposEquipo.Camara: ctx.camara_incidente_camara.Remove(ctx.camara_incidente_camara.Find(id)); break;
                    case TiposEquipo.Switch: ctx.switch_incidente_switch.Remove(ctx.switch_incidente_switch.Find(id)); break;
                    case TiposEquipo.Escaner: ctx.escaner_incidente_escaner.Remove(ctx.escaner_incidente_escaner.Find(id)); break;
                }
                ctx.SaveChanges();

                DTO_Equipo e = null;
                if (i.EquipoId != null) { Equipos(ctx, tipo).TryGetValue(i.EquipoId.Value, out e); }
                Auditar(ctx, tipo, e != null ? e.EquipoId : i.EquipoId, "ELIMINAR_INCIDENTE", usuario);
                return e != null ? e.Nombre ?? "" : "";
            }
        }

        /// <summary>Incidente de cualquier tabla con nombres comunes.</summary>
        private class IncidenteComun
        {
            public int Id;
            public int? EquipoId;
            public int? CatalogoId;
            public string Texto;
            public DateTime? FechaIncidente;
            public string Accion;
            public DateTime? FechaAccion;
        }

        private static List<IncidenteComun> Incidentes(SGEIContext ctx, string tipo, int? id = null)
        {
            switch (tipo)
            {
                case TiposEquipo.Impresora:
                    return ctx.incidente.Where(x => id == null || x.id == id).ToList()
                        .Select(x => new IncidenteComun { Id = x.id, EquipoId = x.ubicacion_impresora_id, Texto = x.incidente1, FechaIncidente = x.fecha_incidente, Accion = x.accion, FechaAccion = x.fecha_accion }).ToList();
                case TiposEquipo.Computadora:
                    return ctx.pc_incidentepc.Where(x => id == null || x.id_pc_incidente == id).ToList()
                        .Select(x => new IncidenteComun { Id = x.id_pc_incidente, EquipoId = x.pc_id, CatalogoId = x.incidentepc_id, FechaIncidente = x.fecha_incidente, Accion = x.reparacion, FechaAccion = x.fecha_reparacion }).ToList();
                case TiposEquipo.Telefono:
                    return ctx.incidente_tel_tel.Where(x => id == null || x.id_incidente_tel_tel == id).ToList()
                        .Select(x => new IncidenteComun { Id = x.id_incidente_tel_tel, EquipoId = x.detalle_id, CatalogoId = x.incidentetel_id, FechaIncidente = x.fecha_incidente, Accion = x.reparacion, FechaAccion = x.fecha_reparacion }).ToList();
                case TiposEquipo.Camara:
                    return ctx.camara_incidente_camara.Where(x => id == null || x.id_camara_incidente_camara == id).ToList()
                        .Select(x => new IncidenteComun { Id = x.id_camara_incidente_camara, EquipoId = x.camara_id, CatalogoId = x.incidente_id, FechaIncidente = x.fecha_incidente, Accion = x.reparacion, FechaAccion = x.fecha_reparacion }).ToList();
                case TiposEquipo.Switch:
                    return ctx.switch_incidente_switch.Where(x => id == null || x.id_switch_incidente_switch == id).ToList()
                        .Select(x => new IncidenteComun { Id = x.id_switch_incidente_switch, EquipoId = x.switch_id, CatalogoId = x.incidente_switch_id, FechaIncidente = x.fecha_incidente, Accion = x.reparacion, FechaAccion = x.fecha_reparacion }).ToList();
                case TiposEquipo.Escaner:
                    return ctx.escaner_incidente_escaner.Where(x => id == null || x.id_escaner_incidente_escaner == id).ToList()
                        .Select(x => new IncidenteComun { Id = x.id_escaner_incidente_escaner, EquipoId = x.escaner_id, CatalogoId = x.incidente_escaner_id, FechaIncidente = x.fecha_incidente, Accion = x.reparacion, FechaAccion = x.fecha_reparacion }).ToList();
                default:
                    return new List<IncidenteComun>();
            }
        }

        private static DTO_IncidenteFila Fila(string tipo, IncidenteComun i, Dictionary<int, DTO_Equipo> equipos, Dictionary<int, string> catalogo)
        {
            DTO_Equipo e = null;
            if (i.EquipoId != null) { equipos.TryGetValue(i.EquipoId.Value, out e); }
            return new DTO_IncidenteFila
            {
                Tipo = tipo, TipoNombre = TiposEquipo.Nombre(tipo), Id = i.Id,
                Equipo = e != null ? e.Nombre : null, Ip = e != null ? e.Ip : null, Ubicacion = e != null ? e.Ubicacion : null,
                Incidente = tipo == TiposEquipo.Impresora ? i.Texto : Nombre(catalogo, i.CatalogoId),
                FechaIncidente = i.FechaIncidente, Accion = i.Accion, FechaAccion = i.FechaAccion
            };
        }

        // ------------------------------------------------------------------ Insumos colocados

        /// <summary>Insumos de un tipo con su stock (Extra), para elegir al colocar uno.</summary>
        public List<DTO_Opcion> CatalogoInsumos(string tipo)
        {
            using (var ctx = new SGEIContext())
            {
                switch (tipo)
                {
                    case TiposEquipo.Impresora:
                        return ctx.insumo.Where(x => x.fecha_baja == null).OrderBy(x => x.nombre).ToList()
                            .Select(x => new DTO_Opcion { Id = x.id, Texto = x.nombre, Extra = (x.cantidad ?? 0).ToString() }).ToList();
                    case TiposEquipo.Computadora:
                        return ctx.insumo_pc.Where(x => x.fecha_baja == null).OrderBy(x => x.nombre).ToList()
                            .Select(x => new DTO_Opcion { Id = x.id_insumo, Texto = x.nombre, Extra = (x.cantidad ?? 0).ToString() }).ToList();
                    default:
                        return new List<DTO_Opcion>();
                }
            }
        }

        /// <summary>Insumos colocados en impresoras y computadoras, del más reciente al más viejo.</summary>
        public List<DTO_InsumoFila> ListarInsumos()
        {
            var filas = new List<DTO_InsumoFila>();
            using (var ctx = new SGEIContext())
            {
                var impresoras = Equipos(ctx, TiposEquipo.Impresora);
                var insumosImp = ctx.insumo.ToDictionary(x => x.id, x => x.nombre);
                filas.AddRange(ctx.detalle_insumo_impresora.ToList().Select(x => FilaInsumo(TiposEquipo.Impresora, x.id, x.ubicacion_impresora_id, Nombre(insumosImp, x.insumo_id), null, x.fechacambioinsumo, impresoras)));

                var pcs = ComputadorasPorDetalle(ctx);
                var insumosPc = ctx.insumo_pc.ToDictionary(x => x.id_insumo, x => x.nombre);
                filas.AddRange(ctx.pc_insumo_pc.ToList().Select(x => FilaInsumo(TiposEquipo.Computadora, x.id_ipc, x.detalle_pc_id, Nombre(insumosPc, x.insumo_id), x.nro_serie, x.fecha_insumo, pcs)));
            }
            return filas.OrderByDescending(f => f.Fecha).ThenByDescending(f => f.Id).ToList();
        }

        public DTO_InsumoFila ObtenerInsumoFila(string tipo, int id)
        {
            return ListarInsumos().FirstOrDefault(f => f.Tipo == tipo && f.Id == id);
        }

        private static DTO_InsumoFila FilaInsumo(string tipo, int id, int? equipoId, string insumo, string serie, DateTime? fecha, Dictionary<int, DTO_Equipo> equipos)
        {
            DTO_Equipo e = null;
            if (equipoId != null) { equipos.TryGetValue(equipoId.Value, out e); }
            return new DTO_InsumoFila
            {
                Tipo = tipo, TipoNombre = TiposEquipo.Nombre(tipo), Id = id, Insumo = insumo, NroSerie = serie, Fecha = fecha,
                Equipo = e != null ? e.Nombre : null, Ip = e != null ? e.Ip : null, Ubicacion = e != null ? e.Ubicacion : null
            };
        }

        public DTO_InsumoFormulario ObtenerInsumo(string tipo, int id)
        {
            using (var ctx = new SGEIContext())
            {
                if (tipo == TiposEquipo.Impresora)
                {
                    var x = ctx.detalle_insumo_impresora.Find(id);
                    return x == null ? null : new DTO_InsumoFormulario { Tipo = tipo, Id = x.id, EquipoId = x.ubicacion_impresora_id, InsumoId = x.insumo_id, Fecha = x.fechacambioinsumo };
                }
                if (tipo == TiposEquipo.Computadora)
                {
                    var x = ctx.pc_insumo_pc.Find(id);
                    return x == null ? null : new DTO_InsumoFormulario { Tipo = tipo, Id = x.id_ipc, EquipoId = x.detalle_pc_id, InsumoId = x.insumo_id, NroSerie = x.nro_serie, Fecha = x.fecha_insumo };
                }
                return null;
            }
        }

        /// <summary>
        /// Guarda un insumo colocado y ajusta el stock: uno nuevo descuenta 1 del insumo; si al editar cambia el insumo,
        /// devuelve 1 al anterior y descuenta 1 del nuevo. Devuelve null si salió bien, o el motivo por el que no se guardó.
        /// </summary>
        public string GuardarInsumo(DTO_InsumoFormulario f, string usuario, out string equipo, out string insumo)
        {
            equipo = null; insumo = null;
            using (var ctx = new SGEIContext())
            {
                int? anterior = null;
                if (f.Tipo == TiposEquipo.Impresora)
                {
                    var x = f.Id == 0 ? null : ctx.detalle_insumo_impresora.Find(f.Id);
                    if (f.Id != 0 && x == null) { return "El registro ya no existe."; }
                    if (x != null) { anterior = x.insumo_id; }
                    if (anterior != f.InsumoId)
                    {
                        var nuevoInsumo = ctx.insumo.Find(f.InsumoId);
                        if (nuevoInsumo == null) { return "El insumo elegido no existe."; }
                        if ((nuevoInsumo.cantidad ?? 0) <= 0) { return "No hay stock de «" + nuevoInsumo.nombre + "». Cargá un ingreso antes de colocarlo."; }
                        nuevoInsumo.cantidad = (nuevoInsumo.cantidad ?? 0) - 1;
                        if (anterior != null) { var viejo = ctx.insumo.Find(anterior); if (viejo != null) { viejo.cantidad = (viejo.cantidad ?? 0) + 1; } }
                    }
                    if (x == null) { x = ctx.detalle_insumo_impresora.Add(new detalle_insumo_impresora()); }
                    x.ubicacion_impresora_id = f.EquipoId; x.insumo_id = f.InsumoId; x.fechacambioinsumo = f.Fecha;
                    ctx.SaveChanges();
                    var i = ctx.insumo.Find(f.InsumoId); insumo = i != null ? i.nombre : null;
                }
                else if (f.Tipo == TiposEquipo.Computadora)
                {
                    var x = f.Id == 0 ? null : ctx.pc_insumo_pc.Find(f.Id);
                    if (f.Id != 0 && x == null) { return "El registro ya no existe."; }
                    if (x != null) { anterior = x.insumo_id; }
                    if (anterior != f.InsumoId)
                    {
                        var nuevoInsumo = ctx.insumo_pc.Find(f.InsumoId);
                        if (nuevoInsumo == null) { return "El insumo elegido no existe."; }
                        if ((nuevoInsumo.cantidad ?? 0) <= 0) { return "No hay stock de «" + nuevoInsumo.nombre + "». Cargá un ingreso antes de colocarlo."; }
                        nuevoInsumo.cantidad = (nuevoInsumo.cantidad ?? 0) - 1;
                        if (anterior != null) { var viejo = ctx.insumo_pc.Find(anterior); if (viejo != null) { viejo.cantidad = (viejo.cantidad ?? 0) + 1; } }
                    }
                    if (x == null) { x = ctx.pc_insumo_pc.Add(new pc_insumo_pc()); }
                    x.detalle_pc_id = f.EquipoId; x.insumo_id = f.InsumoId; x.nro_serie = f.NroSerie; x.fecha_insumo = f.Fecha;
                    ctx.SaveChanges();
                    var i = ctx.insumo_pc.Find(f.InsumoId); insumo = i != null ? i.nombre : null;
                }
                else
                {
                    return "Ese tipo de equipo no registra insumos.";
                }

                var equipos = f.Tipo == TiposEquipo.Computadora ? ComputadorasPorDetalle(ctx) : Equipos(ctx, f.Tipo);
                DTO_Equipo e = null;
                if (f.EquipoId != null) { equipos.TryGetValue(f.EquipoId.Value, out e); }
                equipo = e != null ? e.Nombre : null;
                Auditar(ctx, f.Tipo, e != null ? e.EquipoId : f.EquipoId, f.Id == 0 ? "INSUMO" : "EDITAR_INSUMO", usuario);
                return null;
            }
        }

        /// <summary>Elimina un insumo colocado y devuelve 1 unidad al stock. Devuelve el nombre del insumo, o null si no existía.</summary>
        public string EliminarInsumo(string tipo, int id, string usuario)
        {
            using (var ctx = new SGEIContext())
            {
                string nombre = null;
                int? equipoAuditado = null;
                if (tipo == TiposEquipo.Impresora)
                {
                    var x = ctx.detalle_insumo_impresora.Find(id);
                    if (x == null) { return null; }
                    var i = ctx.insumo.Find(x.insumo_id);
                    if (i != null) { i.cantidad = (i.cantidad ?? 0) + 1; nombre = i.nombre; }
                    equipoAuditado = x.ubicacion_impresora_id;
                    ctx.detalle_insumo_impresora.Remove(x);
                }
                else if (tipo == TiposEquipo.Computadora)
                {
                    var x = ctx.pc_insumo_pc.Find(id);
                    if (x == null) { return null; }
                    var i = ctx.insumo_pc.Find(x.insumo_id);
                    if (i != null) { i.cantidad = (i.cantidad ?? 0) + 1; nombre = i.nombre; }
                    var d = x.detalle_pc_id != null ? ctx.detallePC.Find(x.detalle_pc_id) : null;
                    equipoAuditado = d != null ? d.pc_id : null;
                    ctx.pc_insumo_pc.Remove(x);
                }
                else
                {
                    return null;
                }
                ctx.SaveChanges();
                Auditar(ctx, tipo, equipoAuditado, "ELIMINAR_INSUMO", usuario);
                return nombre ?? "";
            }
        }

        // ------------------------------------------------------------------ Bajas

        /// <summary>Equipos dados de baja de todos los tipos, de la baja más reciente a la más vieja.</summary>
        public List<DTO_BajaFila> ListarBajas()
        {
            var filas = new List<DTO_BajaFila>();
            using (var ctx = new SGEIContext())
            {
                foreach (var tipo in TiposEquipo.Todos)
                {
                    var deBaja = Equipos(ctx, tipo).Values.Where(e => e.FechaBaja != null)
                        .GroupBy(e => e.EquipoId).Select(g => g.OrderByDescending(e => e.Id).First());   // teléfonos: un detalle por equipo
                    foreach (var e in deBaja)
                    {
                        filas.Add(new DTO_BajaFila
                        {
                            Tipo = tipo, TipoNombre = TiposEquipo.Nombre(tipo), Id = e.Id, Equipo = e.Nombre, Ip = e.Ip,
                            UltimaUbicacion = e.Ubicacion, FechaBaja = e.FechaBaja
                        });
                    }
                }

                // Impresoras y computadoras registran además el motivo y la última ubicación declarada.
                var bajasImp = UltimoPor(ctx.baja.Where(x => x.ubicacion_impresora_id != null).ToList(), x => x.ubicacion_impresora_id.Value, x => x.id);
                foreach (var f in filas.Where(f => f.Tipo == TiposEquipo.Impresora && bajasImp.ContainsKey(f.Id)))
                {
                    var b = bajasImp[f.Id];
                    f.Motivo = b.motivo_baja;
                    if (!string.IsNullOrWhiteSpace(b.ultima_ubicacion)) { f.UltimaUbicacion = b.ultima_ubicacion; }
                }

                var detallesPc = ctx.detallePC.Where(x => x.pc_id != null).ToList().ToLookup(x => x.pc_id.Value, x => x.id_detalle);
                var bajasPc = UltimoPor(ctx.bajaPC.Where(x => x.detallepc_id != null).ToList(), x => x.detallepc_id.Value, x => x.id_baja);
                var motivos = ctx.motivobajaPC.ToDictionary(x => x.id_motivobaja, x => x.descripcion);
                var ubisPc = ctx.ubicacionPC.ToDictionary(x => x.id_ubicacion, x => x.nombre);
                foreach (var f in filas.Where(f => f.Tipo == TiposEquipo.Computadora))
                {
                    var b = detallesPc[f.Id].Where(bajasPc.ContainsKey).Select(d => bajasPc[d]).OrderByDescending(x => x.id_baja).FirstOrDefault();
                    if (b == null) { continue; }
                    f.Motivo = Nombre(motivos, b.motivobaja_id);
                    int idUbi;
                    if (int.TryParse(b.ultima_ubicacion, out idUbi) && ubisPc.ContainsKey(idUbi)) { f.UltimaUbicacion = ubisPc[idUbi]; }
                    else if (!string.IsNullOrWhiteSpace(b.ultima_ubicacion)) { f.UltimaUbicacion = b.ultima_ubicacion; }
                }
            }
            return filas.OrderByDescending(f => f.FechaBaja).ThenBy(f => f.Equipo).ToList();
        }

        /// <summary>Equipo dado de baja (para el formulario de reactivación), o null.</summary>
        public DTO_Equipo EquipoDeBaja(string tipo, int id)
        {
            using (var ctx = new SGEIContext())
            {
                DTO_Equipo e;
                return Equipos(ctx, tipo).TryGetValue(id, out e) && e.FechaBaja != null ? e : null;
            }
        }

        /// <summary>Ubicaciones activas de un tipo de equipo.</summary>
        public List<DTO_Opcion> Ubicaciones(string tipo)
        {
            using (var ctx = new SGEIContext())
            {
                Dictionary<int, string> u;
                switch (tipo)
                {
                    case TiposEquipo.Computadora: u = ctx.ubicacionPC.Where(x => x.fecha_baja == null).ToDictionary(x => x.id_ubicacion, x => x.nombre); break;
                    case TiposEquipo.Impresora: u = ctx.ubicacion.Where(x => x.fecha_baja == null).ToDictionary(x => x.id, x => x.nombreUbicacion); break;
                    case TiposEquipo.Telefono: u = ctx.ubicacion_tel.Where(x => x.fecha_baja == null).ToDictionary(x => x.id_ubicacion_tel, x => x.nombre_ubi); break;
                    case TiposEquipo.Camara: u = ctx.ubicacion_camara.Where(x => x.fecha_baja == null).ToDictionary(x => x.id_ubicacion_camara, x => x.nombre); break;
                    case TiposEquipo.Switch: u = ctx.ubicacion_switch.Where(x => x.fecha_baja == null).ToDictionary(x => x.id_ubicacion_switch, x => x.nombre); break;
                    case TiposEquipo.Escaner: u = ctx.ubicacion_escaner.Where(x => x.fecha_baja == null).ToDictionary(x => x.id_ubicacion_escaner, x => x.nombre); break;
                    default: u = new Dictionary<int, string>(); break;
                }
                return u.Select(kv => new DTO_Opcion { Id = kv.Key, Texto = kv.Value }).OrderBy(o => o.Texto).ToList();
            }
        }

        /// <summary>Vuelve a dar de alta un equipo: le asigna la ubicación elegida y le quita la fecha de baja. Devuelve su nombre.</summary>
        public string Reactivar(DTO_Reactivacion r, string usuario)
        {
            using (var ctx = new SGEIContext())
            {
                DTO_Equipo e;
                if (!Equipos(ctx, r.Tipo).TryGetValue(r.Id, out e) || e.FechaBaja == null) { return null; }
                var fecha = r.Fecha ?? DateTime.Now;
                switch (r.Tipo)
                {
                    case TiposEquipo.Impresora:
                    {
                        var x = ctx.ubicacion_impresora.Find(r.Id);
                        x.ubicacion_id = r.UbicacionId; x.fecha_ubicacion = fecha; x.fecha_baja = null;
                        break;
                    }
                    case TiposEquipo.Computadora:
                    {
                        ctx.pc.Find(r.Id).fecha_baja = null;
                        var rel = ctx.pc_ubicacionpc.Where(x => x.pc_id == r.Id).OrderByDescending(x => x.id_upc).FirstOrDefault()
                                  ?? ctx.pc_ubicacionpc.Add(new pc_ubicacionpc { pc_id = r.Id });
                        rel.ubicacionpc_id = r.UbicacionId; rel.fecha_ubicacion = fecha;
                        break;
                    }
                    case TiposEquipo.Telefono:
                    {
                        ctx.telefono.Find(e.EquipoId).fecha_baja = null;
                        var rel = ctx.ubicacion_tel_tel.Where(x => x.detalle_id == r.Id).OrderByDescending(x => x.id_ubicacion_tel_tel).FirstOrDefault()
                                  ?? ctx.ubicacion_tel_tel.Add(new ubicacion_tel_tel { detalle_id = r.Id });
                        rel.ubicacion_tel_id = r.UbicacionId; rel.fecha_ubicacion = fecha;
                        break;
                    }
                    case TiposEquipo.Camara:
                    {
                        ctx.camara.Find(r.Id).fecha_baja = null;
                        if (e.DetalleId != null)
                        {
                            var rel = ctx.ubicacion_camara_camara.Where(x => x.detalle_camara_id == e.DetalleId).OrderByDescending(x => x.id_ubicacion_camara_camara).FirstOrDefault()
                                      ?? ctx.ubicacion_camara_camara.Add(new ubicacion_camara_camara { detalle_camara_id = e.DetalleId });
                            rel.ubicacion_camara_id = r.UbicacionId; rel.fehca_ubicacion = fecha;
                        }
                        break;
                    }
                    case TiposEquipo.Switch:
                    {
                        ctx.swicth.Find(r.Id).fecha_baja = null;
                        if (e.DetalleId != null)
                        {
                            var rel = ctx.switch_ubicacion_switch.Where(x => x.detalle_swich_id == e.DetalleId).OrderByDescending(x => x.id_switch_ubicacion_switch).FirstOrDefault()
                                      ?? ctx.switch_ubicacion_switch.Add(new switch_ubicacion_switch { detalle_swich_id = e.DetalleId });
                            rel.ubicacion_switch_id = r.UbicacionId; rel.fecha_ubicacion = fecha;
                        }
                        break;
                    }
                    case TiposEquipo.Escaner:
                    {
                        ctx.escaner.Find(r.Id).fecha_baja = null;
                        if (e.DetalleId != null)
                        {
                            var rel = ctx.escaner_ubicacion_escaner.Where(x => x.detalle_escaner_id == e.DetalleId).OrderByDescending(x => x.id_escaner_ubicacion_escaner).FirstOrDefault()
                                      ?? ctx.escaner_ubicacion_escaner.Add(new escaner_ubicacion_escaner { detalle_escaner_id = e.DetalleId });
                            rel.ubicacion_escaner_id = r.UbicacionId; rel.fecha_ubicacion = fecha;
                        }
                        break;
                    }
                }
                ctx.SaveChanges();
                Auditar(ctx, r.Tipo, e.EquipoId, "ALTA", usuario);
                return e.Nombre ?? "";
            }
        }

        // ------------------------------------------------------------------ Utilidades

        /// <summary>El registro de mayor id por cada clave (la relación vigente cuando hay historial).</summary>
        private static Dictionary<int, T> UltimoPor<T>(IEnumerable<T> filas, Func<T, int> clave, Func<T, int> orden)
        {
            return filas.GroupBy(clave).ToDictionary(g => g.Key, g => g.OrderByDescending(orden).First());
        }

        private static string Nombre(Dictionary<int, string> nombres, int? id)
        {
            string n;
            return id != null && nombres.TryGetValue(id.Value, out n) ? n : null;
        }

        private static void Auditar(SGEIContext ctx, string tipo, int? idEquipo, string accion, string usuario)
        {
            ctx.Auditoria.Add(new Auditoria
            {
                fecha_hora = DateTime.Now, tipo_equipo = TiposEquipo.Auditoria(tipo), id_equipo = idEquipo, accion = accion, usuario = usuario
            });
            ctx.SaveChanges();
        }
    }
}
