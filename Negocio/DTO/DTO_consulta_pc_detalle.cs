using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.DTO
{
    public class DTO_consulta_pc_detalle
    {
        public DateTime? fecha_incidente { get; set; }
        public DateTime? fecha_reparacion { get; set; }
        public string reparacion { get; set; }
        public string nombre { get; set; }

    }
}
