namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("insumo")]
    public partial class insumo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public insumo()
        {
            detalle_insumo_impresora = new HashSet<detalle_insumo_impresora>();
        }

        public int id { get; set; }

        public int? cantidad { get; set; }

        [StringLength(20)]
        public string nombre { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha_baja { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<detalle_insumo_impresora> detalle_insumo_impresora { get; set; }
    }
}
