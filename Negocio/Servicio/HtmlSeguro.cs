using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Negocio.Servicio
{
    /// <summary>
    /// Limpia el HTML que llega de Summernote (descripción y comentarios de tickets) antes de guardarlo, para que se
    /// pueda mostrar tal cual: deja solo etiquetas y atributos de formato conocidos, links http/https/mailto, imágenes
    /// http/https o pegadas (data:image/...;base64) y estilos de formato simples. Lo demás se descarta (scripts, eventos
    /// onclick, iframes, javascript:, ...). Además cierra las etiquetas que quedan abiertas, para no romper la página.
    /// </summary>
    public static class HtmlSeguro
    {
        private static readonly HashSet<string> Etiquetas = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "p", "br", "b", "strong", "i", "em", "u", "s", "strike", "del", "sub", "sup", "span", "div", "font",
            "h1", "h2", "h3", "h4", "h5", "h6", "blockquote", "pre", "code", "ul", "ol", "li", "a", "img", "hr",
            "table", "thead", "tbody", "tfoot", "tr", "th", "td", "colgroup", "col", "caption"
        };

        /// <summary>Se descartan con todo su contenido.</summary>
        private static readonly HashSet<string> ConContenido = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "script", "style", "iframe", "frame", "frameset", "object", "embed", "applet", "noscript", "template",
            "textarea", "select", "svg", "math", "title", "head", "xml"
        };

        private static readonly HashSet<string> Vacias = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "br", "hr", "img", "col" };

        private static readonly Dictionary<string, string[]> AtributosPorEtiqueta = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
        {
            { "a", new[] { "href", "target" } },
            { "img", new[] { "src", "alt", "width", "height", "data-filename" } },
            { "td", new[] { "colspan", "rowspan" } },
            { "th", new[] { "colspan", "rowspan" } },
            { "font", new[] { "color", "face", "size" } },
            { "ol", new[] { "start" } },
            { "col", new[] { "span" } }
        };

        private static readonly string[] AtributosComunes = { "style", "class", "title" };

        private static readonly HashSet<string> Estilos = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "color", "background-color", "text-align", "font-weight", "font-style", "font-size", "font-family",
            "text-decoration", "text-decoration-line", "line-height", "margin-left", "padding-left", "width", "height",
            "float", "vertical-align", "border", "border-collapse", "white-space", "list-style-type"
        };

        private static readonly Regex Token = new Regex(
            @"<!--.*?(?:-->|$)|<(/?)([a-zA-Z][a-zA-Z0-9]*)((?:\s+[^\s>""'=/]+(?:\s*=\s*(?:""[^""]*""|'[^']*'|[^\s>""']+))?)*)\s*/?>",
            RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private static readonly Regex Atributo = new Regex(
            @"([^\s>""'=/]+)(?:\s*=\s*(?:""([^""]*)""|'([^']*)'|([^\s>""']+)))?",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private static readonly Regex ImagenPegada = new Regex(@"^data:image/(png|jpe?g|gif|webp|bmp);base64,[A-Za-z0-9+/=\s]+$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private static readonly Regex Clase = new Regex(@"^[A-Za-z0-9_-]+$", RegexOptions.Compiled | RegexOptions.CultureInvariant);
        private static readonly Regex ValorSimple = new Regex(@"^[#\w\s,.'""%-]*$", RegexOptions.Compiled | RegexOptions.CultureInvariant);
        private static readonly Regex Numero = new Regex(@"^\d{1,5}(%|px)?$", RegexOptions.Compiled | RegexOptions.CultureInvariant);
        private static readonly Regex Etiqueta = new Regex(@"<[^>]*>", RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public static string Limpiar(string html)
        {
            if (string.IsNullOrEmpty(html)) { return ""; }

            var salida = new StringBuilder(html.Length);
            var abiertas = new List<string>();
            var pos = 0;

            while (pos < html.Length)
            {
                var m = Token.Match(html, pos);
                if (!m.Success)
                {
                    salida.Append(Texto(html.Substring(pos)));
                    break;
                }
                salida.Append(Texto(html.Substring(pos, m.Index - pos)));
                pos = m.Index + m.Length;

                if (m.Value.StartsWith("<!--", StringComparison.Ordinal)) { continue; }

                var cierre = m.Groups[1].Value == "/";
                var nombre = m.Groups[2].Value.ToLowerInvariant();

                if (ConContenido.Contains(nombre))
                {
                    if (!cierre)
                    {
                        // Saltea todo hasta su cierre (o hasta el final si no lo tiene).
                        var fin = Regex.Match(html.Substring(pos), "</" + nombre + @"\s*>", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
                        pos = fin.Success ? pos + fin.Index + fin.Length : html.Length;
                    }
                    continue;
                }
                if (!Etiquetas.Contains(nombre)) { continue; }

                if (cierre)
                {
                    var i = abiertas.LastIndexOf(nombre);
                    if (i < 0) { continue; }
                    for (var j = abiertas.Count - 1; j >= i; j--) { salida.Append("</").Append(abiertas[j]).Append('>'); }
                    abiertas.RemoveRange(i, abiertas.Count - i);
                    continue;
                }

                salida.Append('<').Append(nombre).Append(Atributos(nombre, m.Groups[3].Value)).Append('>');
                if (!Vacias.Contains(nombre)) { abiertas.Add(nombre); }
            }

            for (var j = abiertas.Count - 1; j >= 0; j--) { salida.Append("</").Append(abiertas[j]).Append('>'); }
            return salida.ToString();
        }

        /// <summary>¿El HTML no tiene texto ni imágenes? (Summernote vacío deja "&lt;p&gt;&lt;br&gt;&lt;/p&gt;").</summary>
        public static bool EstaVacio(string html)
        {
            if (string.IsNullOrWhiteSpace(html)) { return true; }
            if (html.IndexOf("<img", StringComparison.OrdinalIgnoreCase) >= 0) { return false; }
            return string.IsNullOrWhiteSpace(TextoPlano(html));
        }

        /// <summary>Texto sin etiquetas, para resúmenes.</summary>
        public static string TextoPlano(string html)
        {
            if (string.IsNullOrEmpty(html)) { return ""; }
            var texto = WebUtility.HtmlDecode(Etiqueta.Replace(html.Replace("</p>", " ").Replace("<br>", " "), " "));
            return Regex.Replace(texto.Replace(' ', ' '), @"\s+", " ").Trim();
        }

        private static string Texto(string texto)
        {
            return texto.Replace("<", "&lt;").Replace(">", "&gt;");
        }

        private static string Atributos(string etiqueta, string texto)
        {
            string[] propios;
            AtributosPorEtiqueta.TryGetValue(etiqueta, out propios);
            var permitidos = AtributosComunes.Concat(propios ?? new string[0]);
            var vistos = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var salida = new StringBuilder();

            foreach (Match m in Atributo.Matches(texto))
            {
                var nombre = m.Groups[1].Value.ToLowerInvariant();
                if (!permitidos.Contains(nombre) || !vistos.Add(nombre)) { continue; }
                var crudo = m.Groups[2].Success ? m.Groups[2].Value : m.Groups[3].Success ? m.Groups[3].Value : m.Groups[4].Value;
                var valor = Valor(etiqueta, nombre, WebUtility.HtmlDecode(crudo ?? ""));
                if (valor == null) { continue; }
                salida.Append(' ').Append(nombre).Append("=\"").Append(WebUtility.HtmlEncode(valor)).Append('"');
            }

            if (etiqueta == "a")
            {
                // Los links siempre abren aparte y sin acceso a esta ventana.
                if (!vistos.Contains("target")) { salida.Append(" target=\"_blank\""); }
                salida.Append(" rel=\"noopener noreferrer\"");
            }
            return salida.ToString();
        }

        /// <summary>Valor permitido del atributo, o null para descartarlo.</summary>
        private static string Valor(string etiqueta, string nombre, string valor)
        {
            valor = valor.Trim();
            switch (nombre)
            {
                case "href":
                    return EsUrlSegura(valor, false) ? valor : null;
                case "src":
                    return EsUrlSegura(valor, true) ? valor : null;
                case "target":
                    return "_blank";
                case "style":
                    var estilo = Estilo(valor);
                    return estilo.Length > 0 ? estilo : null;
                case "class":
                    var clases = valor.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Where(c => Clase.IsMatch(c)).ToArray();
                    return clases.Length > 0 ? string.Join(" ", clases) : null;
                case "width":
                case "height":
                case "colspan":
                case "rowspan":
                case "start":
                case "span":
                case "size":
                    return Numero.IsMatch(valor) ? valor : null;
                default:
                    // alt, title, data-filename, color, face
                    return valor.Length <= 300 && (nombre == "alt" || nombre == "title" || nombre == "data-filename" || ValorSimple.IsMatch(valor)) ? valor : null;
            }
        }

        private static bool EsUrlSegura(string url, bool esImagen)
        {
            if (esImagen && url.StartsWith("data:", StringComparison.OrdinalIgnoreCase)) { return ImagenPegada.IsMatch(url); }

            // Sin espacios ni caracteres de control que puedan esconder el esquema ("java\tscript:").
            var compacta = new string(url.Where(c => !char.IsWhiteSpace(c) && !char.IsControl(c)).ToArray()).ToLowerInvariant();
            if (compacta.StartsWith("http://") || compacta.StartsWith("https://")) { return true; }
            if (esImagen) { return false; }
            if (compacta.StartsWith("mailto:") || compacta.StartsWith("tel:")) { return true; }

            // Relativa: sin esquema antes del primer / ? #.
            var dosPuntos = compacta.IndexOf(':');
            var corte = compacta.IndexOfAny(new[] { '/', '?', '#' });
            return dosPuntos < 0 || (corte >= 0 && corte < dosPuntos);
        }

        private static string Estilo(string estilo)
        {
            var partes = new List<string>();
            foreach (var declaracion in estilo.Split(';'))
            {
                var i = declaracion.IndexOf(':');
                if (i <= 0) { continue; }
                var propiedad = declaracion.Substring(0, i).Trim().ToLowerInvariant();
                var valor = declaracion.Substring(i + 1).Trim();
                var v = valor.ToLowerInvariant();
                if (!Estilos.Contains(propiedad) || valor.Length == 0 || valor.Length > 100) { continue; }
                if (v.Contains("url(") || v.Contains("expression") || v.Contains("javascript") || v.Contains("\\")
                    || v.Contains("/*") || v.Contains("<") || v.Contains(">") || v.Contains("@") || v.Contains("behavior")) { continue; }
                partes.Add(propiedad + ": " + valor);
            }
            return string.Join("; ", partes);
        }
    }
}
