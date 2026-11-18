using System.Collections.Generic;

namespace login_v6.Models
{
    public enum TipoResultado
    {
        Exito,
        Error,
        Aviso
    }

    /// <summary>Botón de una pantalla de resultado. El primero de la lista es el principal.</summary>
    public sealed class BotonResultado
    {
        public BotonResultado(string texto, string url)
        {
            Texto = texto;
            Url = url;
        }

        public string Texto { get; private set; }
        public string Url { get; private set; }
    }

    /// <summary>
    /// Pantalla de resultado de una operación (guardado, baja, error de validación).
    /// Se muestra con @Html.Partial("_Resultado", new ResultadoViewModel { ... }).
    /// </summary>
    public sealed class ResultadoViewModel
    {
        public ResultadoViewModel()
        {
            Botones = new List<BotonResultado>();
        }

        public TipoResultado Tipo { get; set; }
        public string Titulo { get; set; }
        public string Mensaje { get; set; }
        public IList<BotonResultado> Botones { get; set; }
    }
}
