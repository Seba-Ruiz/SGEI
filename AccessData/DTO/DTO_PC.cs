using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccessData
{
    public class DTO_PC
    {
        //PC
        public string nombrepc { get; set; }
        public string ip { get; set; }
        public string descripcion { get; set; }
        
        //tipo
        public int id_tipo { get; set; }
        
        //Detalle
        public string observacion { get; set; }
        public string responsable { get; set; }
        
        //ubicacion
        public int id_ubicacion { get; set; }
        
    }
}
