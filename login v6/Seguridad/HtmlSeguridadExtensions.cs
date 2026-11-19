using System.Web;
using System.Web.Mvc;

namespace login_v6.Seguridad
{
    public static class HtmlSeguridadExtensions
    {
        /// <summary>
        /// Botón de borrado seguro: genera un formulario POST con token antiforgery
        /// (en lugar de un link GET) y pide confirmación antes de enviarlo.
        /// Se dibuja como botón de acción de grilla (.sgei-accion), en rojo.
        /// </summary>
        /// <param name="url">URL de la acción, por ejemplo Url.Content("~/ABM/EliminarRam/5").</param>
        /// <param name="mensajeConfirmacion">Texto del cuadro de confirmación.</param>
        /// <param name="claseIcono">Clases CSS del ícono del botón.</param>
        public static IHtmlString BotonEliminar(this HtmlHelper html, string url, string mensajeConfirmacion, string claseIcono = "bi bi-trash")
        {
            var form = new TagBuilder("form");
            form.MergeAttribute("action", url);
            form.MergeAttribute("method", "post");
            // La confirmación la maneja Scripts/app/sgei.js (form[data-confirmar]).
            form.MergeAttribute("data-confirmar", mensajeConfirmacion);
            form.AddCssClass("d-inline-flex");

            var icono = new TagBuilder("i");
            icono.AddCssClass(claseIcono);
            icono.MergeAttribute("aria-hidden", "true");

            var boton = new TagBuilder("button");
            boton.MergeAttribute("type", "submit");
            boton.MergeAttribute("title", "Eliminar");
            boton.MergeAttribute("aria-label", "Eliminar");
            boton.AddCssClass("sgei-accion sgei-accion-peligro");
            boton.InnerHtml = icono.ToString(TagRenderMode.Normal);

            form.InnerHtml = html.AntiForgeryToken().ToHtmlString() + boton.ToString(TagRenderMode.Normal);
            return new HtmlString(form.ToString(TagRenderMode.Normal));
        }
    }
}
