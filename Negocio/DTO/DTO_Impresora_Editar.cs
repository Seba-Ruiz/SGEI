using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.DTO
{
    public class DTO_Impresora_Editar
    {
        public int id_ubicacion_impresora { set; get; }
        public int id_detalle_ubicacion_impresora { set; get; }
        public int? mmarca { get; set; }
        public int? ubicacion { get; set; }
        public DateTime? fecha_ubicacion { get; set; }
        public string descripcion { get; set; }
        public DateTime? fecha_baja { get; set; }
        public string ip { get; set; }
        public string pc_donde_se_conecta { get; set; }

    }
}
