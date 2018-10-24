using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.DTO
{
    public class DTO_u_computadora_detalle
    {
        public string nombre { get; set; }
        public int id_pc { get; set; }
        public string ip { get; set; }
        public string descripcion { get; set; }
       
        public string ubicacion { get; set; }
        public DateTime? fecha_ubicacion { get; set; }
        public string responsablepc { get; set; }
        public string observacion { get; set; }
        public string nombre_tipo { get; set; }
        public string ram { get; set; }
        public string disco { get; set; }
        public string so { get; set; }
        public string procesador { get; set; }
        public string mother { get; set; }
        public int id_detalle { get; set; }

    }
}
