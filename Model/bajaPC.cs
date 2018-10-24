namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("bajaPC")]
    public partial class bajaPC
    {
        [Key]
        public int id_baja { get; set; }

        [StringLength(50)]
        public string ultima_ubicacion { get; set; }

        public int? motivobaja_id { get; set; }

        public int? detallepc_id { get; set; }

        public virtual detallePC detallePC { get; set; }

        public virtual motivobajaPC motivobajaPC { get; set; }
    }
}
