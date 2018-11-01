namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class switch_incidente_switch
    {
        [Key]
        public int id_switch_incidente_switch { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha_incidente { get; set; }

        [StringLength(150)]
        public string reparacion { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha_reparacion { get; set; }

        public int? switch_id { get; set; }

        public int? incidente_switch_id { get; set; }

        public virtual incidente_switch incidente_switch { get; set; }

        public virtual swicth swicth { get; set; }
    }
}
