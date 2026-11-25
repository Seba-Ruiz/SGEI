using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using Negocio.DTO;

namespace login_v6.Servicio
{
    /// <summary>
    /// Arma el mail de una notificación: asunto, HTML (con estilos en línea y tablas, para que se vea bien en
    /// Outlook, Gmail y celulares) y la versión en texto plano.
    /// </summary>
    public class MailNotificacion
    {
        private const string Primario = "#1f5fa8";
        private const string Texto = "#1f2a37";
        private const string TextoSuave = "#5b6573";
        private const string Borde = "#e3e7ed";
        private const string Fondo = "#f3f5f8";
        private const string Fuente = "'Segoe UI', Roboto, Helvetica, Arial, sans-serif";

        private static readonly CultureInfo Cultura = new CultureInfo("es-AR");

        public string Asunto { get; private set; }
        public string Html { get; private set; }
        public string TextoPlano { get; private set; }

        /// <param name="n">Datos de la notificación (título, mensaje, detalle, texto del botón).</param>
        /// <param name="guardada">La notificación ya guardada (fecha y quién la generó).</param>
        /// <param name="destinatario">Nombre de quien la recibe.</param>
        /// <param name="urlBoton">URL absoluta a la que lleva el botón (abre la notificación y la marca como leída).</param>
        /// <param name="urlTodas">URL absoluta de la pantalla Notificaciones.</param>
        public MailNotificacion(NotificacionNueva n, DTO_Notificacion guardada, string destinatario, string urlBoton, string urlTodas)
        {
            var fecha = guardada.Fecha.ToString("dddd d 'de' MMMM 'de' yyyy, HH:mm 'h'", Cultura);
            fecha = char.ToUpper(fecha[0], Cultura) + fecha.Substring(1);
            var detalle = new List<KeyValuePair<string, string>>(n.Detalle ?? new List<KeyValuePair<string, string>>());
            if (!string.IsNullOrEmpty(guardada.Origen)) { detalle.Add(new KeyValuePair<string, string>("Generada por", guardada.Origen)); }
            detalle.Add(new KeyValuePair<string, string>("Fecha y hora", fecha));
            var textoBoton = string.IsNullOrWhiteSpace(n.TextoBoton) ? "Ver en SGEI" : n.TextoBoton;

            Asunto = "SGEI · " + guardada.Titulo;
            Html = ArmarHtml(guardada, destinatario, detalle, textoBoton, urlBoton, urlTodas);
            TextoPlano = ArmarTexto(guardada, destinatario, detalle, textoBoton, urlBoton, urlTodas);
        }

        private static string ArmarHtml(DTO_Notificacion g, string destinatario, List<KeyValuePair<string, string>> detalle,
            string textoBoton, string urlBoton, string urlTodas)
        {
            Func<string, string> e = HttpUtility.HtmlEncode;
            var filas = new StringBuilder();
            for (var i = 0; i < detalle.Count; i++)
            {
                var borde = i < detalle.Count - 1 ? "border-bottom:1px solid " + Borde + ";" : "";
                filas.Append("<tr>")
                    .Append("<td style=\"padding:10px 14px;width:38%;font-size:13px;color:" + TextoSuave + ";vertical-align:top;" + borde + "\">")
                    .Append(e(detalle[i].Key)).Append("</td>")
                    .Append("<td style=\"padding:10px 14px;font-size:14px;color:" + Texto + ";font-weight:600;vertical-align:top;" + borde + "\">")
                    .Append(e(detalle[i].Value)).Append("</td>")
                    .Append("</tr>");
            }

            var sb = new StringBuilder();
            sb.Append("<!DOCTYPE html><html lang=\"es\"><head><meta charset=\"utf-8\">")
              .Append("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">")
              .Append("<title>").Append(e(g.Titulo)).Append("</title></head>")
              .Append("<body style=\"margin:0;padding:0;background:" + Fondo + ";\">")
              // Texto de vista previa (lo muestran los clientes de correo junto al asunto).
              .Append("<div style=\"display:none;max-height:0;overflow:hidden;opacity:0;\">").Append(e(g.Mensaje ?? g.Titulo)).Append("</div>")
              .Append("<table role=\"presentation\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" style=\"background:" + Fondo + ";\"><tr><td align=\"center\" style=\"padding:28px 12px;\">")
              .Append("<table role=\"presentation\" width=\"600\" cellpadding=\"0\" cellspacing=\"0\" style=\"width:100%;max-width:600px;background:#ffffff;border:1px solid " + Borde + ";border-radius:10px;overflow:hidden;font-family:" + Fuente + ";\">")

              // Encabezado
              .Append("<tr><td style=\"background:" + Primario + ";padding:20px 28px;\">")
              .Append("<div style=\"font-size:20px;font-weight:700;color:#ffffff;letter-spacing:.5px;\">SGEI</div>")
              .Append("<div style=\"font-size:12px;color:#d6e4f5;margin-top:2px;\">Sistema de Gestión de Equipos Informáticos · Hospital Alfredo Italo Perrupato</div>")
              .Append("</td></tr>")

              // Cuerpo
              .Append("<tr><td style=\"padding:28px 28px 8px 28px;\">")
              .Append("<div style=\"font-size:12px;font-weight:600;text-transform:uppercase;letter-spacing:.8px;color:" + Primario + ";\">Nueva notificación</div>")
              .Append("<h1 style=\"margin:6px 0 16px 0;font-size:22px;line-height:1.3;color:" + Texto + ";font-weight:700;\">").Append(e(g.Titulo)).Append("</h1>")
              .Append("<p style=\"margin:0 0 12px 0;font-size:15px;line-height:1.55;color:" + Texto + ";\">Hola ").Append(e(destinatario)).Append(",</p>");
            if (!string.IsNullOrWhiteSpace(g.Mensaje))
            {
                sb.Append("<p style=\"margin:0 0 20px 0;font-size:15px;line-height:1.55;color:" + Texto + ";\">").Append(e(g.Mensaje)).Append("</p>");
            }
            sb.Append("</td></tr>")

              // Detalle
              .Append("<tr><td style=\"padding:0 28px;\">")
              .Append("<div style=\"font-size:13px;font-weight:700;color:" + Texto + ";margin:0 0 8px 0;\">Detalle</div>")
              .Append("<table role=\"presentation\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" style=\"border:1px solid " + Borde + ";border-radius:8px;border-collapse:separate;\">")
              .Append(filas)
              .Append("</table></td></tr>");

            // Botón
            if (!string.IsNullOrEmpty(urlBoton))
            {
                sb.Append("<tr><td style=\"padding:26px 28px 6px 28px;\">")
                  .Append("<table role=\"presentation\" cellpadding=\"0\" cellspacing=\"0\"><tr><td style=\"border-radius:6px;background:" + Primario + ";\">")
                  .Append("<a href=\"").Append(e(urlBoton)).Append("\" style=\"display:inline-block;padding:12px 26px;font-size:15px;font-weight:600;color:#ffffff;text-decoration:none;border-radius:6px;\">")
                  .Append(e(textoBoton)).Append("</a></td></tr></table>")
                  .Append("<p style=\"margin:14px 0 0 0;font-size:12px;line-height:1.5;color:" + TextoSuave + ";\">Si el botón no funciona, copiá este enlace en el navegador:<br>")
                  .Append("<a href=\"").Append(e(urlBoton)).Append("\" style=\"color:" + Primario + ";word-break:break-all;\">").Append(e(urlBoton)).Append("</a></p>")
                  .Append("</td></tr>");
            }

            // Pie
            sb.Append("<tr><td style=\"padding:24px 28px 26px 28px;\">")
              .Append("<div style=\"border-top:1px solid " + Borde + ";padding-top:16px;font-size:12px;line-height:1.55;color:" + TextoSuave + ";\">")
              .Append("Este es un mensaje automático de SGEI, no hace falta responderlo.");
            if (!string.IsNullOrEmpty(urlTodas))
            {
                sb.Append(" Podés ver todas tus notificaciones en <a href=\"").Append(e(urlTodas)).Append("\" style=\"color:" + Primario + ";\">SGEI · Notificaciones</a>.");
            }
            sb.Append("</div></td></tr>")
              .Append("</table>")
              .Append("<div style=\"font-family:" + Fuente + ";font-size:11px;color:#8a94a3;margin-top:14px;\">© ").Append(DateTime.Now.Year)
              .Append(" SGEI · Hospital Alfredo Italo Perrupato</div>")
              .Append("</td></tr></table></body></html>");
            return sb.ToString();
        }

        private static string ArmarTexto(DTO_Notificacion g, string destinatario, List<KeyValuePair<string, string>> detalle,
            string textoBoton, string urlBoton, string urlTodas)
        {
            var sb = new StringBuilder();
            sb.AppendLine("SGEI - Nueva notificación").AppendLine()
              .AppendLine(g.Titulo).AppendLine()
              .AppendLine("Hola " + destinatario + ",");
            if (!string.IsNullOrWhiteSpace(g.Mensaje)) { sb.AppendLine(g.Mensaje); }
            sb.AppendLine().AppendLine("Detalle:");
            foreach (var d in detalle) { sb.AppendLine("  " + d.Key + ": " + d.Value); }
            if (!string.IsNullOrEmpty(urlBoton)) { sb.AppendLine().AppendLine(textoBoton + ": " + urlBoton); }
            sb.AppendLine().AppendLine("Este es un mensaje automático de SGEI, no hace falta responderlo.");
            if (!string.IsNullOrEmpty(urlTodas)) { sb.AppendLine("Todas tus notificaciones: " + urlTodas); }
            return sb.ToString();
        }
    }
}
