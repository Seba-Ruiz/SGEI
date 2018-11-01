namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class detalle_insumo_camara
    {
        [Key]
        public int id_detalle_insumo_camara { get; set; }

        [StringLength(10)]
        public string fecha_insumo { get; set; }

        public int? insumo_id { get; set; }

        public int? detalle_id { get; set; }

        public virtual detalle_camara detalle_camara { get; set; }

        public virtual insumo_camara insumo_camara { get; set; }
    }
}
