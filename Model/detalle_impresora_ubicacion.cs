namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class detalle_impresora_ubicacion
    {
        [StringLength(20)]
        public string ip { get; set; }

        [StringLength(30)]
        public string pc_dondeseconecta { get; set; }

        public int? ubicacion_impresora_id { get; set; }

        public int id { get; set; }

        public virtual ubicacion_impresora ubicacion_impresora { get; set; }
    }
}
