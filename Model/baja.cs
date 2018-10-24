namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("baja")]
    public partial class baja
    {
        public int id { get; set; }

        [StringLength(100)]
        public string ultima_ubicacion { get; set; }

        [StringLength(150)]
        public string motivo_baja { get; set; }

        public int? ubicacion_impresora_id { get; set; }

        public virtual ubicacion_impresora ubicacion_impresora { get; set; }
    }
}
