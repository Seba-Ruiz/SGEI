namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class mantenimiento_impre
    {
        [Key]
        public int id_mante { get; set; }

        public DateTime? fecha_mantenimiento { get; set; }

        [StringLength(250)]
        public string descripcion { get; set; }

        public DateTime? proximo_mantenimiento { get; set; }

        public int? id_impresora { get; set; }

        public virtual ubicacion_impresora ubicacion_impresora { get; set; }
    }
}
