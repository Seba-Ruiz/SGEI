namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class switch_detalle
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public switch_detalle()
        {
            switch_ubicacion_switch = new HashSet<switch_ubicacion_switch>();
        }

        [Key]
        public int id_detalle_switch { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha_detalle { get; set; }

        public int? marca_modelo_id { get; set; }

        public int? switch_id { get; set; }

        public int? interfaces { get; set; }

        public int? nroip { get; set; }

        public virtual marca_modelo_switch marca_modelo_switch { get; set; }

        public virtual swicth swicth { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<switch_ubicacion_switch> switch_ubicacion_switch { get; set; }
    }
}
