namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ubicacion_escaner
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ubicacion_escaner()
        {
            escaner_ubicacion_escaner = new HashSet<escaner_ubicacion_escaner>();
        }

        [Key]
        public int id_ubicacion_escaner { get; set; }

        [StringLength(50)]
        public string nombre { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha_baja { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<escaner_ubicacion_escaner> escaner_ubicacion_escaner { get; set; }
    }
}
