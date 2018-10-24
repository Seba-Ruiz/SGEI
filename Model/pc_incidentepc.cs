namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class pc_incidentepc
    {
        [Key]
        public int id_pc_incidente { get; set; }

        public int? incidentepc_id { get; set; }

        public int? pc_id { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha_incidente { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha_reparacion { get; set; }

        [StringLength(50)]
        public string reparacion { get; set; }

        public virtual incidentePC incidentePC { get; set; }

        public virtual pc pc { get; set; }
    }
}
