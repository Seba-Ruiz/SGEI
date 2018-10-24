namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ubicacion")]
    public partial class ubicacion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ubicacion()
        {
            ubicacion_impresora = new HashSet<ubicacion_impresora>();
        }

        public int id { get; set; }

        [StringLength(20)]
        public string nombreUbicacion { get; set; }

        [StringLength(20)]
        public string responsable { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha_baja { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ubicacion_impresora> ubicacion_impresora { get; set; }
    }
}
