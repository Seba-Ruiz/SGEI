using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.DTO
{
    public class DTO_buscar_insumo_serie
    {
        public string nombre_insumo { get; set; }
        public string nro_serie { get; set; }
        public string ubicado_en { get; set; }
        public string ip_equipo { get; set; }
        public string nombre_equipo { get; set; }
        public string observacion_pc { get; set; }
        public DateTime? fecha_colocacion { get; set; }
        public string responsable { get; set; }
    }
}
