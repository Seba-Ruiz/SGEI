namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class incidente_switch
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public incidente_switch()
        {
            switch_incidente_switch = new HashSet<switch_incidente_switch>();
        }

        [Key]
        public int id_incidente_switch { get; set; }

        [StringLength(100)]
        public string descripcion { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha_baja { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<switch_incidente_switch> switch_incidente_switch { get; set; }
    }
}
