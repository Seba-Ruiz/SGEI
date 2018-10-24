using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.DTO
{
    public class DTO_u_detalle_telefono
    {
        public string nro_interno { get; set; }
        public string descripcion { get; set; }
        public int id_telefono { get; set; }
        public int id_mm { get; set; }
        public DateTime? fecha_ubicacion { get; set; }
        public int id_ubicacion_tel { get; set; }
        public int id_tipo { get; set; }
        public int id_detalle { get; set; }

    }
}
