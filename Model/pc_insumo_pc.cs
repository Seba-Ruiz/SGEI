namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class pc_insumo_pc
    {
        [Key]
        public int id_ipc { get; set; }

        public int? detalle_pc_id { get; set; }

        public int? insumo_id { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha_insumo { get; set; }

        [StringLength(50)]
        public string nro_serie { get; set; }

        public virtual detallePC detallePC { get; set; }

        public virtual insumo_pc insumo_pc { get; set; }
    }
}
