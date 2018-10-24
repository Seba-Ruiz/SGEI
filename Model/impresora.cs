namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("impresora")]
    public partial class impresora
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public impresora()
        {
            ubicacion_impresora = new HashSet<ubicacion_impresora>();
        }

        public int id { get; set; }

        [StringLength(10)]
        public string tipo { get; set; }

        [StringLength(50)]
        public string marca_modelo { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha_baja { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ubicacion_impresora> ubicacion_impresora { get; set; }
    }
}
