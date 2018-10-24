namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ubicacion_tel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ubicacion_tel()
        {
            ubicacion_tel_tel = new HashSet<ubicacion_tel_tel>();
        }

        [Key]
        public int id_ubicacion_tel { get; set; }

        [StringLength(50)]
        public string nombre_ubi { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha_baja { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ubicacion_tel_tel> ubicacion_tel_tel { get; set; }
    }
}
