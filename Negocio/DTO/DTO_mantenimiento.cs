using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.DTO
{
    public class DTO_mantenimiento
    {
        public string nombre { get; set; }
        public DateTime? ultimo_mantenimiento { get; set; }
        public string ubicacion { get; set; }
        public string descripcion { get; set; }
        public string tipo { get; set; }
    }
}
