namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class switch_ubicacion_switch
    {
        [Key]
        public int id_switch_ubicacion_switch { get; set; }

        [StringLength(10)]
        public string fecha_ubicacion { get; set; }

        public int? ubicacion_switch_id { get; set; }

        public int? detalle_swich_id { get; set; }

        public virtual switch_detalle switch_detalle { get; set; }

        public virtual ubicacion_switch ubicacion_switch { get; set; }
    }
}
