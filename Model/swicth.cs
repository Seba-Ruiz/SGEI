namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("swicth")]
    public partial class swicth
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public swicth()
        {
            switch_detalle = new HashSet<switch_detalle>();
            switch_incidente_switch = new HashSet<switch_incidente_switch>();
        }

        [Key]
        public int id_switch { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha_baja { get; set; }

        [StringLength(150)]
        public string descripcion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<switch_detalle> switch_detalle { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<switch_incidente_switch> switch_incidente_switch { get; set; }
    }
}
