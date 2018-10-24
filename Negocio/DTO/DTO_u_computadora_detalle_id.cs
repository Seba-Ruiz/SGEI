using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.DTO
{
    public class DTO_u_computadora_detalle_id
    {
        
        public int id_pc { get; set; }
        public string nombre_pc { get; set; }
        public string ip { get; set; }
        public string descripcion_pc { get; set; }
        public string responsablepc { get; set; }
        public string observacion { get; set; }
        public int id_ubicacion { get; set; }
        public int id_tipo { get; set; }
        public int id_ram { get; set; }
        public int id_disco { get; set; }
        public int id_so { get; set; }
        public int id_procesador { get; set; }
        public int id_mother { get; set; }
        public int id_detalle { get; set; }

    }
}
