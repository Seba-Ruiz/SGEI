using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.DTO
{
    public class DTO_preventivo_impre
    {
        public string nombre { get; set; }
        public string detalle { get; set; }
        public string descripcion { get; set; }
        public DateTime? fecha_mantenimiento { get; set; }
        public DateTime? proximo_mantenimiento { get; set; }
        public string pospuesto_por { get; set; }
        public DateTime? fecha_pospuso { get; set; }
        public string realizado_por { get; set; }
        public string ubicacion { get; set; }

    }
}
