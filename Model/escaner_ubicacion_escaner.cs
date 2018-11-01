namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class escaner_ubicacion_escaner
    {
        [Key]
        public int id_escaner_ubicacion_escaner { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha_ubicacion { get; set; }

        public int? detalle_escaner_id { get; set; }

        public int? ubicacion_escaner_id { get; set; }

        public virtual detalle_escaner detalle_escaner { get; set; }

        public virtual ubicacion_escaner ubicacion_escaner { get; set; }
    }
}
