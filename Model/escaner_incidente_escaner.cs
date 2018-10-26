namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class escaner_incidente_escaner
    {
        [Key]
        public int id_escaner_incidente_escaner { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha_incidente { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha_reparacion { get; set; }

        [StringLength(150)]
        public string reparacion { get; set; }

        public int? escaner_id { get; set; }

        public int? incidente_escaner_id { get; set; }

        public virtual escaner escaner { get; set; }

        public virtual incidente_escaner incidente_escaner { get; set; }
    }
}
