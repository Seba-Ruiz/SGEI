namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ubicacion_tel_tel
    {
        [Key]
        public int id_ubicacion_tel_tel { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha_ubicacion { get; set; }

        public int? ubicacion_tel_id { get; set; }

        public int? detalle_id { get; set; }

        public virtual detalle_tel detalle_tel { get; set; }

        public virtual ubicacion_tel ubicacion_tel { get; set; }
    }
}
