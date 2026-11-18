using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace login_v6.Helpers
{
    /// <summary>
    /// Scripts propios con versión para invalidar la caché del navegador.
    ///
    /// Cada vista puede tener su archivo en Scripts/views/&lt;Controlador&gt;/&lt;vista&gt;.js;
    /// el layout lo incluye automáticamente si existe. A todas las URLs se les agrega
    /// "?v=&lt;VersionJs&gt;" tomado de Web.config: al publicar en producción alcanza con
    /// cambiar ese valor para que todos los navegadores descarguen los .js nuevos.
    /// </summary>
    public static class ScriptsHelper
    {
        public const string ClaveVersion = "VersionJs";
        private const string CarpetaVistas = "~/Views/";
        private const string CarpetaScripts = "~/Scripts/views/";

        // Cache de existencia de archivos: la app se reinicia al publicar, así que no se invalida.
        private static readonly ConcurrentDictionary<string, bool> existe =
            new ConcurrentDictionary<string, bool>(StringComparer.OrdinalIgnoreCase);

        public static string Version
        {
            get { return ConfigurationManager.AppSettings[ClaveVersion] ?? "1"; }
        }

        /// <summary>URL de un script propio con el parámetro de versión, por ejemplo ~/Scripts/app/sgei.js?v=3.</summary>
        public static string ScriptVersionado(this UrlHelper url, string rutaVirtual)
        {
            return url.Content(rutaVirtual) + "?v=" + Uri.EscapeDataString(Version);
        }

        /// <summary>Etiqueta &lt;script&gt; de un script propio con versión.</summary>
        public static IHtmlString Script(this HtmlHelper html, string rutaVirtual)
        {
            var url = new UrlHelper(html.ViewContext.RequestContext);
            var tag = new TagBuilder("script");
            tag.MergeAttribute("src", url.ScriptVersionado(rutaVirtual));
            return new HtmlString(tag.ToString(TagRenderMode.Normal));
        }

        /// <summary>
        /// Incluye Scripts/views/&lt;Controlador&gt;/&lt;vista&gt;.js de la vista que se está
        /// renderizando, si el archivo existe. Lo usa el layout y las vistas sin layout.
        /// </summary>
        public static IHtmlString ScriptDeVista(this HtmlHelper html)
        {
            var vista = html.ViewContext.View as RazorView;
            var ruta = vista == null ? null : RutaScriptDeVista(vista.ViewPath);
            if (ruta == null || !existe.GetOrAdd(ruta, r => HostingEnvironment.VirtualPathProvider.FileExists(r)))
            {
                return MvcHtmlString.Empty;
            }
            return html.Script(ruta);
        }

        /// <summary>~/Views/ABM/rams.cshtml -> ~/Scripts/views/ABM/rams.js</summary>
        public static string RutaScriptDeVista(string rutaVista)
        {
            if (string.IsNullOrEmpty(rutaVista) || !rutaVista.StartsWith(CarpetaVistas, StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }
            var relativa = rutaVista.Substring(CarpetaVistas.Length);
            return CarpetaScripts + Path.ChangeExtension(relativa, ".js").Replace('\\', '/');
        }
    }
}
