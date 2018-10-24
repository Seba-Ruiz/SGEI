using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.DTO
{
    public class DTO_u_impresora_04
    {

        public string marca_modelo { get; set; }
        public string tipo { get; set; }
        public string descripcion { get; set; }
        public string responsable { get; set; }
        public string pc_dondeseconecta { get; set; }
        public string ip { get; set; }
        public DateTime? fecha_baja{ get; set; }
       
        public string motivo_baja { get; set; }
        public string ultima_ubicacion { get; set; }
        public int id { get; set; }

    }
}
