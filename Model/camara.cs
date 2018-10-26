namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("camara")]
    public partial class camara
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public camara()
        {
            camara_incidente_camara = new HashSet<camara_incidente_camara>();
            detalle_camara = new HashSet<detalle_camara>();
        }

        [Key]
        public int id_camara { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha_baja { get; set; }

        [StringLength(100)]
        public string descripcion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<camara_incidente_camara> camara_incidente_camara { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<detalle_camara> detalle_camara { get; set; }
    }
}
