namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class camara_incidente_camara
    {
        [Key]
        public int id_camara_incidente_camara { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha_incidente { get; set; }

        [StringLength(150)]
        public string reparacion { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha_reparacion { get; set; }

        public int? incidente_id { get; set; }

        public int? camara_id { get; set; }

        public virtual camara camara { get; set; }

        public virtual incidente_camara incidente_camara { get; set; }
    }
}
