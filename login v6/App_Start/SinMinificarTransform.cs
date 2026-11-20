using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Optimization;

namespace login_v6
{
    /// <summary>
    /// Concatena los archivos del bundle sin minificar y fija el Content-Type.
    ///
    /// El minificador de System.Web.Optimization (WebGrease, 2014) no entiende JavaScript ni CSS
    /// modernos (falla con Bootstrap 5, DataTables 2 y las variables CSS). Las librerías ya traen
    /// su versión .min, que el bundle elige solo en producción. El Content-Type es obligatorio:
    /// el sitio envía "X-Content-Type-Options: nosniff" y sin él el navegador descarta el archivo.
    ///
    /// En los CSS, además, las rutas relativas de url(...) se pasan a absolutas según la carpeta de cada
    /// archivo: el bundle se sirve desde ~/Content/css y, por ejemplo, "fonts/bootstrap-icons.woff2" de
    /// Content/lib/bootstrap-icons/ apuntaría a Content/fonts/ (404). CssRewriteUrlTransform no sirve
    /// para esto: no se aplica cuando el bundle usa la versión .min.css del archivo (producción).
    /// Las rutas incluyen la carpeta de la aplicación en IIS (por ejemplo /SGEI/Content/...).
    /// </summary>
    public sealed class SinMinificarTransform : IBundleTransform
    {
        public static readonly SinMinificarTransform Js = new SinMinificarTransform("text/javascript", false);
        public static readonly SinMinificarTransform Css = new SinMinificarTransform("text/css", true);

        // url(...), url("...") o url('...')
        private static readonly Regex Url = new Regex(@"url\(\s*(?<comilla>['""]?)(?<url>[^'"")]+)\k<comilla>\s*\)",
                                                      RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private readonly string contentType;
        private readonly bool reescribirUrls;

        private SinMinificarTransform(string contentType, bool reescribirUrls)
        {
            this.contentType = contentType;
            this.reescribirUrls = reescribirUrls;
        }

        public void Process(BundleContext context, BundleResponse response)
        {
            response.ContentType = contentType;

            if (!reescribirUrls)
            {
                return;
            }

            var css = new StringBuilder();
            foreach (var archivo in response.Files)
            {
                var ruta = archivo.VirtualFile != null
                    ? archivo.VirtualFile.VirtualPath
                    : VirtualPathUtility.ToAbsolute(archivo.IncludedVirtualPath);
                var carpeta = VirtualPathUtility.GetDirectory(ruta);

                string contenido;
                using (var flujo = archivo.VirtualFile.Open())
                using (var lector = new StreamReader(flujo, Encoding.UTF8, true))
                {
                    contenido = lector.ReadToEnd();
                }

                css.AppendLine(Url.Replace(contenido, m => ReescribirUrl(m, carpeta)));
            }
            response.Content = css.ToString();
        }

        private static string ReescribirUrl(Match m, string carpeta)
        {
            var url = m.Groups["url"].Value.Trim();
            if (url.Length == 0 || url.StartsWith("/") || url.StartsWith("#")
                || url.StartsWith("data:", StringComparison.OrdinalIgnoreCase)
                || url.Contains("://") || url.StartsWith("//"))
            {
                return m.Value;
            }

            // "fonts/x.woff2?v=1#y": se combina la ruta y se conserva lo que va desde "?" o "#".
            var corte = url.IndexOfAny(new[] { '?', '#' });
            var camino = corte >= 0 ? url.Substring(0, corte) : url;
            var resto = corte >= 0 ? url.Substring(corte) : "";

            var absoluta = VirtualPathUtility.Combine(carpeta, camino);
            return "url(\"" + absoluta + resto + "\")";
        }
    }
}
