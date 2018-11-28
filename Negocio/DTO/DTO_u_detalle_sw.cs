using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.DTO
{
    public class DTO_u_detalle_sw
    {
        public string nro_ip { get; set; }
        public string descripcion { get; set; }
        public int id_sw { get; set; }
        public int id_mm { get; set; }
        public DateTime? fecha_ubicacion { get; set; }
        public int id_ubicacion_sw { get; set; }
        public int id_detalle { get; set; }
        public int? interfaces { get; set; }
    }
}
