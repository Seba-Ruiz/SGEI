﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.DTO
{
    public class DTO_u_telefono_01
    {

        public string nro_interno { get; set; }
        public string descripcion { get; set; }

        public int id_telefono { get; set; }
        public string mmarca { get; set; }
        public DateTime? fecha_ubicacion { get; set; }
        public string nombre_ubi { get; set; }
        public string nombre_tipo { get; set; }
        public int id_detalle { get; set; }


    }
}
