﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccessData.DTO
{
    public class DTO_Telefono
    {
        public int mmarca { get; set; }
        public int ubicacion { get; set; }
        public int interno { get; set; }
        public string descripcion { get; set; }
        public int tipo_tel { get; set; }

    }
}