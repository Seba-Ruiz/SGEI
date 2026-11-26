using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using login_v6.Helpers;
using Negocio.DTO;
using Negocio.Servicio;

namespace login_v6.Controllers
{
    /// <summary>
    /// Menú Consultas: incidentes, insumos colocados y bajas de todos los equipos en una grilla cada uno,
    /// con su alta, detalle, edición y eliminación (Negocio/Servicio/ConsultasUnificadas.cs).
    /// </summary>
    [Authorize]
    public class ConsultasController : Controller
    {
        private const int MaximoFotoBytes = 5 * 1024 * 1024;
        private readonly ConsultasUnificadas servicio = new ConsultasUnificadas();

        // ================================================================== Incidentes

        public ActionResult Incidentes()
        {
            var filas = servicio.ListarIncidentes();
            ViewBag.Ubicaciones = UbicacionesDe(filas.Select(f => f.Ubicacion));
            return View(filas);
        }

        public ActionResult NuevoIncidente(string tipo)
        {
            return FormularioIncidente(new DTO_IncidenteFormulario
            {
                Tipo = TiposEquipo.EsValido(tipo) ? tipo : TiposEquipo.Computadora,
                FechaIncidente = DateTime.Today,
                FechaAccion = DateTime.Today
            });
        }

        public ActionResult EditarIncidente(string tipo, int id)
        {
            var f = TiposEquipo.EsValido(tipo) ? servicio.ObtenerIncidente(tipo, id) : null;
            return f == null ? (ActionResult)HttpNotFound() : FormularioIncidente(f);
        }

        [HttpPost]
        public ActionResult GuardarIncidente(DTO_IncidenteFormulario f, HttpPostedFileBase foto)
        {
            if (!TiposEquipo.EsValido(f.Tipo)) { return HttpNotFound(); }
            var esImpresora = f.Tipo == TiposEquipo.Impresora;

            if (f.EquipoId == null) { ModelState.AddModelError("EquipoId", "Elegí el equipo."); }
            if (esImpresora)
            {
                f.IncidenteId = null;
                if (string.IsNullOrWhiteSpace(f.IncidenteTexto)) { ModelState.AddModelError("IncidenteTexto", "Describí el incidente."); }
                else if (f.IncidenteTexto.Trim().Length > TiposEquipo.LargoTexto) { ModelState.AddModelError("IncidenteTexto", "Máximo " + TiposEquipo.LargoTexto + " caracteres."); }
            }
            else
            {
                f.IncidenteTexto = null;
                if (f.IncidenteId == null) { ModelState.AddModelError("IncidenteId", "Elegí el incidente."); }
            }
            ValidarFechas(f.FechaIncidente, f.FechaAccion, "FechaIncidente", "FechaAccion");
            if (string.IsNullOrWhiteSpace(f.Accion)) { ModelState.AddModelError("Accion", "Indicá la acción realizada."); }
            else if (f.Accion.Trim().Length > TiposEquipo.LargoAccion(f.Tipo)) { ModelState.AddModelError("Accion", "Máximo " + TiposEquipo.LargoAccion(f.Tipo) + " caracteres."); }

            byte[] contenidoFoto = null;
            if (esImpresora && foto != null && foto.ContentLength > 0)
            {
                if (!(foto.ContentType ?? "").StartsWith("image/", StringComparison.OrdinalIgnoreCase)) { ModelState.AddModelError("foto", "La foto tiene que ser una imagen (JPG, PNG, ...)."); }
                else if (foto.ContentLength > MaximoFotoBytes) { ModelState.AddModelError("foto", "La foto no puede pesar más de 5 MB."); }
                else
                {
                    using (var ms = new MemoryStream())
                    {
                        foto.InputStream.CopyTo(ms);
                        contenidoFoto = ms.ToArray();
                    }
                }
            }

            if (!ModelState.IsValid) { return FormularioIncidente(f); }

            f.IncidenteTexto = f.IncidenteTexto != null ? f.IncidenteTexto.Trim() : null;
            f.Accion = f.Accion.Trim();
            var nuevo = f.Id == 0;
            string equipo;
            var id = servicio.GuardarIncidente(f, User.Identity.Name, UsuarioId, out equipo);
            if (contenidoFoto != null)
            {
                servicio.AgregarFoto(id, contenidoFoto);
                servicio.RegistrarFoto(id, UsuarioId, true);
            }

            var de = TiposEquipo.ConArticulo(f.Tipo);
            this.AvisoExito(nuevo
                ? "Se cargó el incidente en " + de + Avisos.Nombre(equipo) + (contenidoFoto != null ? ", con su foto." : ".")
                : "Se guardaron los cambios del incidente de " + de + Avisos.Nombre(equipo) + (contenidoFoto != null ? " y se agregó la foto." : "."));
            // Al editar se vuelve al detalle, donde queda el cambio en el historial.
            return nuevo ? RedirectToAction("Incidentes") : RedirectToAction("VerIncidente", new { tipo = f.Tipo, id });
        }

        /// <summary>Detalle del incidente con sus observaciones (cada uno edita o elimina las suyas) y su historial.</summary>
        public ActionResult VerIncidente(string tipo, int id)
        {
            var detalle = TiposEquipo.EsValido(tipo) ? servicio.DetalleIncidente(tipo, id) : null;
            if (detalle == null) { return HttpNotFound(); }
            ViewBag.Fotos = tipo == TiposEquipo.Impresora ? servicio.FotosDeIncidente(id) : new List<int>();
            ViewBag.UsuarioId = UsuarioId;
            return View(detalle);
        }

        // ------------------------------------------------------------------ Observaciones del incidente

        [HttpPost]
        [ValidateInput(false)] // HTML de Summernote: se sanea en el servicio (HtmlSeguro).
        public ActionResult Observar(string tipo, int id, string texto)
        {
            if (!TiposEquipo.EsValido(tipo)) { return HttpNotFound(); }
            var error = ValidarObservacion(texto);
            if (error != null)
            {
                TempData["incidente.error"] = error;
                TempData["incidente.borrador"] = texto != null && texto.Length <= LargoMaximoHtml ? texto : null;
                return RedirectToAction("VerIncidente", new { tipo, id });
            }
            var idObservacion = servicio.Observar(tipo, id, UsuarioId, texto);
            if (idObservacion == null) { return HttpNotFound(); }
            this.AvisoExito("Se agregó tu observación al incidente.");
            return Redirect(Url.Action("VerIncidente", new { tipo, id }) + "#observacion-" + idObservacion);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditarObservacion(int id, string texto)
        {
            var incidente = servicio.IncidenteDeObservacion(id);
            if (incidente == null) { return RedirectToAction("Incidentes"); }
            var error = ValidarObservacion(texto) ?? servicio.EditarObservacion(id, UsuarioId, texto);
            if (error != null) { TempData["incidente.error"] = error; }
            else { this.AvisoExito("Se guardaron los cambios de tu observación."); }
            return Redirect(Url.Action("VerIncidente", new { tipo = incidente.Item1, id = incidente.Item2 }) + "#observacion-" + id);
        }

        [HttpPost]
        public ActionResult EliminarObservacion(int id)
        {
            var incidente = servicio.IncidenteDeObservacion(id);
            if (incidente == null) { return RedirectToAction("Incidentes"); }
            var error = servicio.EliminarObservacion(id, UsuarioId);
            if (error != null) { TempData["incidente.error"] = error; }
            else { this.AvisoExito("Se eliminó tu observación."); }
            return Redirect(Url.Action("VerIncidente", new { tipo = incidente.Item1, id = incidente.Item2 }) + "#actividad");
        }

        /// <summary>Largo máximo del HTML de una observación (incluye las imágenes pegadas).</summary>
        private const int LargoMaximoHtml = 12 * 1024 * 1024;

        private static string ValidarObservacion(string texto)
        {
            if (HtmlSeguro.EstaVacio(texto)) { return "Escribí la observación."; }
            if (texto.Length > LargoMaximoHtml) { return "La observación es demasiado grande (revisá el tamaño de las imágenes)."; }
            return null;
        }

        private string UsuarioId { get { return Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(User.Identity); } }

        [HttpPost]
        public ActionResult EliminarIncidente(string tipo, int id)
        {
            if (!TiposEquipo.EsValido(tipo)) { return HttpNotFound(); }
            var equipo = servicio.EliminarIncidente(tipo, id, User.Identity.Name);
            if (equipo != null)
            {
                this.AvisoExito("Se eliminó el incidente de " + TiposEquipo.ConArticulo(tipo) + Avisos.Nombre(equipo) + ".");
            }
            return RedirectToAction("Incidentes");
        }

        /// <summary>Foto de un incidente de impresora.</summary>
        public ActionResult Foto(int id)
        {
            var contenido = servicio.Foto(id);
            return contenido == null ? (ActionResult)HttpNotFound() : File(contenido, TipoDeImagen(contenido));
        }

        [HttpPost]
        public ActionResult EliminarFoto(int id)
        {
            var incidente = servicio.EliminarFoto(id);
            if (incidente == null) { return HttpNotFound(); }
            servicio.RegistrarFoto(incidente.Value, UsuarioId, false);
            this.AvisoExito("Se eliminó la foto del incidente.");
            return RedirectToAction("VerIncidente", new { tipo = TiposEquipo.Impresora, id = incidente });
        }

        private ActionResult FormularioIncidente(DTO_IncidenteFormulario f)
        {
            var equipos = new Dictionary<string, object>();
            var catalogos = new Dictionary<string, object>();
            foreach (var tipo in TiposEquipo.Todos)
            {
                var incluir = tipo == f.Tipo ? f.EquipoId : null;
                equipos[tipo] = servicio.EquiposParaElegir(tipo, false, incluir).Select(e => new { id = e.Id, texto = e.Texto }).ToList();
                catalogos[tipo] = servicio.CatalogoIncidentes(tipo).Select(o => new { id = o.Id, texto = o.Texto }).ToList();
            }
            ViewBag.Datos = new
            {
                equipos,
                catalogos,
                largoAccion = TiposEquipo.Todos.ToDictionary(t => t, t => TiposEquipo.LargoAccion(t))
            };
            ViewBag.Fotos = f.Id != 0 && f.Tipo == TiposEquipo.Impresora ? servicio.FotosDeIncidente(f.Id).Count : 0;
            return View("FormularioIncidente", f);
        }

        // ================================================================== Insumos colocados

        public ActionResult Insumos()
        {
            var filas = servicio.ListarInsumos();
            ViewBag.Ubicaciones = UbicacionesDe(filas.Select(f => f.Ubicacion));
            return View(filas);
        }

        public ActionResult NuevoInsumo(string tipo)
        {
            return FormularioInsumo(new DTO_InsumoFormulario
            {
                Tipo = TiposEquipo.ConInsumos.Contains(tipo) ? tipo : TiposEquipo.Computadora,
                Fecha = DateTime.Today
            });
        }

        public ActionResult EditarInsumo(string tipo, int id)
        {
            var f = TiposEquipo.ConInsumos.Contains(tipo) ? servicio.ObtenerInsumo(tipo, id) : null;
            return f == null ? (ActionResult)HttpNotFound() : FormularioInsumo(f);
        }

        [HttpPost]
        public ActionResult GuardarInsumo(DTO_InsumoFormulario f)
        {
            if (!TiposEquipo.ConInsumos.Contains(f.Tipo)) { return HttpNotFound(); }
            if (f.EquipoId == null) { ModelState.AddModelError("EquipoId", "Elegí el equipo."); }
            if (f.InsumoId == null) { ModelState.AddModelError("InsumoId", "Elegí el insumo."); }
            if (f.Fecha == null) { ModelState.AddModelError("Fecha", "Indicá la fecha de colocación."); }
            else if (f.Fecha.Value.Date > DateTime.Today) { ModelState.AddModelError("Fecha", "La fecha no puede ser posterior a hoy."); }
            if (f.Tipo == TiposEquipo.Computadora)
            {
                if (string.IsNullOrWhiteSpace(f.NroSerie)) { ModelState.AddModelError("NroSerie", "Indicá el número de serie."); }
                else if (f.NroSerie.Trim().Length > TiposEquipo.LargoTexto) { ModelState.AddModelError("NroSerie", "Máximo " + TiposEquipo.LargoTexto + " caracteres."); }
            }
            else
            {
                f.NroSerie = null;
            }
            if (!ModelState.IsValid) { return FormularioInsumo(f); }

            if (f.NroSerie != null) { f.NroSerie = f.NroSerie.Trim(); }
            var nuevo = f.Id == 0;
            string equipo, insumo;
            var error = servicio.GuardarInsumo(f, User.Identity.Name, out equipo, out insumo);
            if (error != null)
            {
                ModelState.AddModelError("InsumoId", error);
                return FormularioInsumo(f);
            }

            var de = TiposEquipo.ConArticulo(f.Tipo);
            this.AvisoExito(nuevo
                ? "Se registró el insumo" + Avisos.Nombre(insumo) + " en " + de + Avisos.Nombre(equipo) + "."
                : "Se guardaron los cambios del insumo" + Avisos.Nombre(insumo) + " de " + de + Avisos.Nombre(equipo) + ".");
            return RedirectToAction("Insumos");
        }

        public ActionResult VerInsumo(string tipo, int id)
        {
            var fila = TiposEquipo.ConInsumos.Contains(tipo) ? servicio.ObtenerInsumoFila(tipo, id) : null;
            return fila == null ? (ActionResult)HttpNotFound() : View(fila);
        }

        [HttpPost]
        public ActionResult EliminarInsumo(string tipo, int id)
        {
            if (!TiposEquipo.ConInsumos.Contains(tipo)) { return HttpNotFound(); }
            var insumo = servicio.EliminarInsumo(tipo, id, User.Identity.Name);
            if (insumo != null)
            {
                this.AvisoExito("Se eliminó el insumo" + Avisos.Nombre(insumo) + " y se devolvió 1 unidad al stock.");
            }
            return RedirectToAction("Insumos");
        }

        private ActionResult FormularioInsumo(DTO_InsumoFormulario f)
        {
            var equipos = new Dictionary<string, object>();
            var catalogos = new Dictionary<string, object>();
            foreach (var tipo in TiposEquipo.ConInsumos)
            {
                var incluir = tipo == f.Tipo ? f.EquipoId : null;
                equipos[tipo] = servicio.EquiposParaElegir(tipo, true, incluir).Select(e => new { id = e.Id, texto = e.Texto }).ToList();
                catalogos[tipo] = servicio.CatalogoInsumos(tipo).Select(o => new { id = o.Id, texto = o.Texto, stock = o.Extra }).ToList();
            }
            ViewBag.Datos = new { equipos, catalogos };
            return View("FormularioInsumo", f);
        }

        // ================================================================== Bajas

        public ActionResult Bajas()
        {
            var filas = servicio.ListarBajas();
            ViewBag.Ubicaciones = UbicacionesDe(filas.Select(f => f.UltimaUbicacion));
            return View(filas);
        }

        [HttpGet]
        public ActionResult Reactivar(string tipo, int id)
        {
            var equipo = TiposEquipo.EsValido(tipo) ? servicio.EquipoDeBaja(tipo, id) : null;
            if (equipo == null) { return HttpNotFound(); }
            return FormularioReactivacion(new DTO_Reactivacion { Tipo = tipo, Id = id, UbicacionId = equipo.UbicacionId, Fecha = DateTime.Today }, equipo);
        }

        [HttpPost]
        public ActionResult Reactivar(DTO_Reactivacion r)
        {
            var equipo = TiposEquipo.EsValido(r.Tipo) ? servicio.EquipoDeBaja(r.Tipo, r.Id) : null;
            if (equipo == null) { return HttpNotFound(); }
            if (r.UbicacionId == null) { ModelState.AddModelError("UbicacionId", "Elegí la ubicación."); }
            if (r.Fecha == null) { ModelState.AddModelError("Fecha", "Indicá la fecha."); }
            if (!ModelState.IsValid) { return FormularioReactivacion(r, equipo); }

            var nombre = servicio.Reactivar(r, User.Identity.Name);
            var ubicacion = servicio.Ubicaciones(r.Tipo).Where(u => u.Id == r.UbicacionId).Select(u => u.Texto).FirstOrDefault();
            this.AvisoExito("Se volvió a dar de alta " + TiposEquipo.ConArticulo(r.Tipo) + Avisos.Nombre(nombre)
                            + (ubicacion != null ? " en " + ubicacion : "") + ".");
            return RedirectToAction("Bajas");
        }

        private ActionResult FormularioReactivacion(DTO_Reactivacion r, DTO_Equipo equipo)
        {
            ViewBag.Equipo = equipo;
            ViewBag.Ubicaciones = new SelectList(servicio.Ubicaciones(r.Tipo), "Id", "Texto", r.UbicacionId);
            return View("Reactivar", r);
        }

        // ================================================================== Utilidades

        private void ValidarFechas(DateTime? desde, DateTime? hasta, string campoDesde, string campoHasta)
        {
            if (desde == null) { ModelState.AddModelError(campoDesde, "Indicá la fecha del incidente."); }
            else if (desde.Value.Date > DateTime.Today) { ModelState.AddModelError(campoDesde, "La fecha no puede ser posterior a hoy."); }
            if (hasta == null) { ModelState.AddModelError(campoHasta, "Indicá la fecha de la acción."); }
            else if (desde != null && hasta.Value.Date < desde.Value.Date) { ModelState.AddModelError(campoHasta, "No puede ser anterior a la fecha del incidente."); }
        }

        private static List<string> UbicacionesDe(IEnumerable<string> ubicaciones)
        {
            return ubicaciones.Where(u => !string.IsNullOrWhiteSpace(u)).Select(u => u.Trim()).Distinct().OrderBy(u => u).ToList();
        }

        /// <summary>Content-Type de una imagen según sus primeros bytes (las fotos se guardan sin tipo).</summary>
        private static string TipoDeImagen(byte[] b)
        {
            if (b.Length > 3 && b[0] == 0xFF && b[1] == 0xD8) { return "image/jpeg"; }
            if (b.Length > 8 && b[0] == 0x89 && b[1] == 0x50 && b[2] == 0x4E && b[3] == 0x47) { return "image/png"; }
            if (b.Length > 6 && b[0] == 0x47 && b[1] == 0x49 && b[2] == 0x46) { return "image/gif"; }
            if (b.Length > 12 && b[0] == 0x52 && b[1] == 0x49 && b[2] == 0x46 && b[3] == 0x46 && b[8] == 0x57 && b[9] == 0x45 && b[10] == 0x42 && b[11] == 0x50) { return "image/webp"; }
            if (b.Length > 2 && b[0] == 0x42 && b[1] == 0x4D) { return "image/bmp"; }
            return "application/octet-stream";
        }
    }
}
