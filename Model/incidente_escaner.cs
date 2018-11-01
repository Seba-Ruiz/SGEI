namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class incidente_escaner
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public incidente_escaner()
        {
            escaner_incidente_escaner = new HashSet<escaner_incidente_escaner>();
        }

        [Key]
        public int id_incidente_escaner { get; set; }

        [StringLength(100)]
        public string descripcion { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha_baja { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<escaner_incidente_escaner> escaner_incidente_escaner { get; set; }
    }
}
