namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class marca_modelo_escaner
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public marca_modelo_escaner()
        {
            detalle_escaner = new HashSet<detalle_escaner>();
        }

        [Key]
        public int id_marca_modelo_escaner { get; set; }

        [StringLength(150)]
        public string descripcion { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha_baja { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<detalle_escaner> detalle_escaner { get; set; }
    }
}
