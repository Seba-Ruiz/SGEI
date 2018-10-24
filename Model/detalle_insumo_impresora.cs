namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class detalle_insumo_impresora
    {
        [Column(TypeName = "date")]
        public DateTime? fechacambioinsumo { get; set; }

        public int? insumo_id { get; set; }

        public int? ubicacion_impresora_id { get; set; }

        public int id { get; set; }

        public virtual ubicacion_impresora ubicacion_impresora { get; set; }

        public virtual insumo insumo { get; set; }
    }
}
