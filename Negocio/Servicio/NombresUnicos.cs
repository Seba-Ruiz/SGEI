using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Model;

namespace Negocio.Servicio
{
    /// <summary>
    /// Evita nombres repetidos en los catálogos de cada equipo (ubicaciones, modelos, incidentes, ...) y en los nombres
    /// de los equipos. Dos nombres son iguales sin importar mayúsculas, acentos ni espacios de más
    /// ("Sistemas" = "SISTEMAS" = " sistemas "). Los registros dados de baja (fecha_baja cargada) no cuentan.
    /// </summary>
    public static class NombresUnicos
    {
        /// <summary>"  Esterilización  Central " → "ESTERILIZACION CENTRAL"</summary>
        public static string Normalizar(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto)) { return ""; }
            var sb = new StringBuilder();
            foreach (var c in texto.Trim().Normalize(NormalizationForm.FormD))
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark) { sb.Append(c); }
            }
            var sinAcentos = sb.ToString().Normalize(NormalizationForm.FormC).ToUpperInvariant();
            return string.Join(" ", sinAcentos.Split(new[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries));
        }

        /// <summary>
        /// Devuelve el nombre ya cargado que coincide con <paramref name="valor"/> (tal como está guardado), o null si no
        /// hay otro activo con ese nombre. <paramref name="idPropio"/> es el del registro que se está editando (0 en un
        /// alta), para que no choque consigo mismo. <paramref name="filtro"/> limita la búsqueda (por ejemplo, solo los
        /// equipos activos).
        /// </summary>
        public static string Repetido<T>(Expression<Func<T, int>> id, Expression<Func<T, string>> nombre, int idPropio, string valor,
            Expression<Func<T, bool>> filtro = null) where T : class
        {
            var buscado = Normalizar(valor);
            if (buscado.Length == 0) { return null; }

            using (var ctx = new SGEIContext())
            {
                IQueryable<T> consulta = ctx.Set<T>().AsNoTracking();
                var otros = Expression.Lambda<Func<T, bool>>(Expression.NotEqual(id.Body, Expression.Constant(idPropio)), id.Parameters);
                consulta = consulta.Where(otros);
                var activos = SinBaja<T>();
                if (activos != null) { consulta = consulta.Where(activos); }
                if (filtro != null) { consulta = consulta.Where(filtro); }

                // Los catálogos son chicos: se comparan en memoria, normalizados.
                return consulta.Select(nombre).ToList().FirstOrDefault(n => Normalizar(n) == buscado);
            }
        }

        /// <summary>Tipos de equipo con nombre único (el valor de data-nombre-unico en los formularios).</summary>
        public static readonly string[] TiposEquipo = { "computadora", "camara", "switch", "escaner", "telefono" };

        /// <summary>
        /// Nombre de equipo repetido: computadora (nombre de la PC) y cámara, switch, escáner y teléfono (su
        /// descripción). Devuelve el nombre ya cargado que choca, o null. Las impresoras no tienen nombre propio
        /// (su descripción es la del lugar), así que no se validan.
        /// </summary>
        public static string Equipo(string tipo, string nombre, int idPropio)
        {
            switch (tipo)
            {
                case "computadora": return Repetido<pc>(x => x.id_pc, x => x.nombre, idPropio, nombre);
                case "camara": return Repetido<camara>(x => x.id_camara, x => x.descripcion, idPropio, nombre);
                case "switch": return Repetido<swicth>(x => x.id_switch, x => x.descripcion, idPropio, nombre);
                case "escaner": return Repetido<escaner>(x => x.id_escaner, x => x.descripcion, idPropio, nombre);
                case "telefono": return Repetido<telefono>(x => x.id_telefono, x => x.descripcion, idPropio, nombre);
                default: return null;
            }
        }

        /// <summary>"Ya existe una computadora con el nombre «HPSISTEMAS05»."</summary>
        public static string MensajeEquipo(string tipo, string existente)
        {
            string que;
            switch (tipo)
            {
                case "computadora": que = "una computadora"; break;
                case "camara": que = "una cámara"; break;
                case "switch": que = "un switch"; break;
                case "escaner": que = "un escáner"; break;
                case "telefono": que = "un teléfono"; break;
                default: que = "un equipo"; break;
            }
            return "Ya existe " + que + " con el nombre «" + existente.Trim() + "». Usá otro nombre.";
        }

        /// <summary>x => x.fecha_baja == null, si la tabla tiene esa columna.</summary>
        private static Expression<Func<T, bool>> SinBaja<T>()
        {
            var propiedad = typeof(T).GetProperty("fecha_baja");
            if (propiedad == null || propiedad.PropertyType != typeof(DateTime?)) { return null; }
            var x = Expression.Parameter(typeof(T), "x");
            return Expression.Lambda<Func<T, bool>>(
                Expression.Equal(Expression.Property(x, propiedad), Expression.Constant(null, typeof(DateTime?))), x);
        }
    }
}
