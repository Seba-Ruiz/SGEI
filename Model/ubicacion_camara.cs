namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ubicacion_camara
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ubicacion_camara()
        {
            ubicacion_camara_camara = new HashSet<ubicacion_camara_camara>();
        }

        [Key]
        public int id_ubicacion_camara { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha_baja { get; set; }

        [StringLength(50)]
        public string nombre { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ubicacion_camara_camara> ubicacion_camara_camara { get; set; }
    }
}
