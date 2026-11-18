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
    /// </summary>
    public sealed class SinMinificarTransform : IBundleTransform
    {
        public static readonly SinMinificarTransform Js = new SinMinificarTransform("text/javascript");
        public static readonly SinMinificarTransform Css = new SinMinificarTransform("text/css");

        private readonly string contentType;

        private SinMinificarTransform(string contentType)
        {
            this.contentType = contentType;
        }

        public void Process(BundleContext context, BundleResponse response)
        {
            response.ContentType = contentType;
        }
    }
}
