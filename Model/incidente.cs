namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("incidente")]
    public partial class incidente
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public incidente()
        {
            imagen = new HashSet<imagen>();
        }

        public int id { get; set; }

        [Column("incidente")]
        [StringLength(50)]
        public string incidente1 { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha_incidente { get; set; }

        [StringLength(50)]
        public string accion { get; set; }

        public int? ubicacion_impresora_id { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha_accion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<imagen> imagen { get; set; }

        public virtual ubicacion_impresora ubicacion_impresora { get; set; }
    }
}
