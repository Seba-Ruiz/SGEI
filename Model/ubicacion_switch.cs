namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ubicacion_switch
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ubicacion_switch()
        {
            switch_ubicacion_switch = new HashSet<switch_ubicacion_switch>();
        }

        [Key]
        public int id_ubicacion_switch { get; set; }

        [StringLength(50)]
        public string nombre { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha_baja { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<switch_ubicacion_switch> switch_ubicacion_switch { get; set; }
    }
}
