namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class pc_ubicacionpc
    {
        [Key]
        public int id_upc { get; set; }

        public int? pc_id { get; set; }

        public int? ubicacionpc_id { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha_ubicacion { get; set; }

        public virtual pc pc { get; set; }

        public virtual ubicacionPC ubicacionPC { get; set; }
    }
}
