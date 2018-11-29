namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("IngresoInsumo")]
    public partial class IngresoInsumo
    {
        public int id { get; set; }

        public int? cantidad { get; set; }

        [StringLength(50)]
        public string tipo_equipo { get; set; }

        [StringLength(50)]
        public string nombre_insumo { get; set; }

        public DateTime? fecha_ingreso { get; set; }

        public int? nro_comprobante { get; set; }

        public int? id_insumo { get; set; }

        public int? id_proveedor { get; set; }

        [StringLength(50)]
        public string proveedor { get; set; }

        public virtual Proveedor Proveedor1 { get; set; }
    }
}
