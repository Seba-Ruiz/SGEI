namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class incidente_tel_tel
    {
        [Key]
        public int id_incidente_tel_tel { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha_incidente { get; set; }

        public int? incidentetel_id { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha_reparacion { get; set; }

        [StringLength(50)]
        public string reparacion { get; set; }

        public int? detalle_id { get; set; }

        public virtual detalle_tel detalle_tel { get; set; }

        public virtual incidente_tel incidente_tel { get; set; }
    }
}
