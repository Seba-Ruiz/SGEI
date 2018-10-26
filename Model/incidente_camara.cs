namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class incidente_camara
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public incidente_camara()
        {
            camara_incidente_camara = new HashSet<camara_incidente_camara>();
        }

        [Key]
        public int id_incidente { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha_baja { get; set; }

        [StringLength(100)]
        public string descripcion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<camara_incidente_camara> camara_incidente_camara { get; set; }
    }
}
