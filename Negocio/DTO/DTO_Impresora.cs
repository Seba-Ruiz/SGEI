using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.DTO
{
    public class DTO_Impresora
    {
        public int? mmarca { get; set; }
        public int? ubicacion { get; set; }
        public DateTime? fecha_ubicacion { get; set; }
        public string descripcion { get; set; }
        public DateTime? fecha_baja { get; set; }
        public string ip { get; set; }
        public string pc_donde_se_conecta { get; set; }

    }
}
