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

        public static void AvisoExito(this ControllerBase controlador, string mensaje)
        {
            controlador.TempData[Clave] = mensaje;
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

        /// <summary>"el insumo" -> "del insumo"; "la memoria" -> "de la memoria".</summary>
        public static string De(string que)
        {
            return que.StartsWith("el ") ? "del " + que.Substring(3) : "de " + que;
        }
    }
}
