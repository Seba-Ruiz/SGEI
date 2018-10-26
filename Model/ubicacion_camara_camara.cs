namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ubicacion_camara_camara
    {
        [Key]
        public int id_ubicacion_camara_camara { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fehca_ubicacion { get; set; }

        public int? detalle_camara_id { get; set; }

        public int? ubicacion_impresora_id { get; set; }

        public virtual detalle_camara detalle_camara { get; set; }

        public virtual ubicacion_camara ubicacion_camara { get; set; }
    }
}
