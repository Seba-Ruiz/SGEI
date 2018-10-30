namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class marca_modelo_switch
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public marca_modelo_switch()
        {
            switch_detalle = new HashSet<switch_detalle>();
        }

        [Key]
        public int id_mmarca { get; set; }

        [StringLength(50)]
        public string marca_modelo { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha_baja { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<switch_detalle> switch_detalle { get; set; }
    }
}
