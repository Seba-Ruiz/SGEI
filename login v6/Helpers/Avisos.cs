using System.Web.Mvc;

namespace login_v6.Helpers
{
    /// <summary>
    /// Aviso de éxito ("toast" verde, centrado arriba) que se muestra en la próxima pantalla: la acción guarda,
    /// deja el mensaje con estos métodos y redirige (por ejemplo, a la grilla). Lo dibuja Views/Shared/_Avisos.cshtml.
    /// </summary>
    public static class Avisos
    {
        public const string Clave = "sgei.aviso.exito";

        public const string ClaveError = "sgei.aviso.error";

        public static void AvisoExito(this ControllerBase controlador, string mensaje)
        {
            controlador.TempData[Clave] = mensaje;
        }

        /// <summary>Aviso rojo en la próxima pantalla (por ejemplo, cuando no se pudo guardar y se vuelve al formulario).</summary>
        public static void AvisoError(this ControllerBase controlador, string mensaje)
        {
            controlador.TempData[ClaveError] = mensaje;
        }

        /// <summary>
        /// Nombre de equipo repetido al guardar (si el navegador no lo frenó antes, ver Scripts/app/sgei.js
        /// data-nombre-unico): deja el aviso rojo y vuelve a la pantalla de la que vino. Devuelve null si el nombre está libre.
        /// </summary>
        public static ActionResult VolverSiNombreRepetido(this Controller controlador, string tipo, string nombre, int idPropio, string volverA)
        {
            var existente = Negocio.Servicio.NombresUnicos.Equipo(tipo, nombre, idPropio);
            if (existente == null) { return null; }
            controlador.AvisoError(Negocio.Servicio.NombresUnicos.MensajeEquipo(tipo, existente));
            var anterior = controlador.Request.UrlReferrer;
            var url = anterior != null && controlador.Url.IsLocalUrl(anterior.PathAndQuery) ? anterior.PathAndQuery : volverA;
            return new RedirectResult(url);
        }

        /// <summary>"Se agregó el insumo «MOUSE»." o "Se guardaron los cambios del insumo «MOUSE»."</summary>
        /// <param name="que">Con artículo: "el insumo", "la memoria".</param>
        public static void AvisoGuardado(this ControllerBase controlador, bool nuevo, string que, string nombre)
        {
            controlador.AvisoExito(nuevo
                ? "Se agregó " + que + Nombre(nombre) + "."
                : "Se guardaron los cambios " + De(que) + Nombre(nombre) + ".");
        }

        /// <summary>"Se eliminó la memoria «8 GB»."</summary>
        public static void AvisoEliminado(this ControllerBase controlador, string que, string nombre)
        {
            controlador.AvisoExito("Se eliminó " + que + Nombre(nombre) + ".");
        }

        /// <summary>« nombre » entre comillas, o nada si no hay nombre.</summary>
        public static string Nombre(string nombre)
        {
            return string.IsNullOrWhiteSpace(nombre) ? "" : " «" + nombre.Trim() + "»";
        }

        /// <summary>
        /// Respuesta de error para un pedido AJAX: { ok: false, mensaje } con el código HTTP indicado. El script la
        /// muestra con SGEI.mostrarAviso(mensaje, "error").
        /// </summary>
        public static JsonResult ErrorJson(this Controller controlador, int estado, string mensaje)
        {
            controlador.Response.StatusCode = estado;
            controlador.Response.TrySkipIisCustomErrors = true;
            return new JsonResult { Data = new { ok = false, mensaje } };
        }

        /// <summary>"el insumo" -> "del insumo"; "la memoria" -> "de la memoria".</summary>
        public static string De(string que)
        {
            return que.StartsWith("el ") ? "del " + que.Substring(3) : "de " + que;
        }
    }
}
