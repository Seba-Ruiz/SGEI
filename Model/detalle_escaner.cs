namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class detalle_escaner
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public detalle_escaner()
        {
            escaner_ubicacion_escaner = new HashSet<escaner_ubicacion_escaner>();
        }

        [Key]
        public int id_detalle_escaner { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha_detalle { get; set; }

        public int? marca_modelo_id { get; set; }

        public int? escaner_id { get; set; }

        [StringLength(50)]
        public string nroip { get; set; }

        public virtual escaner escaner { get; set; }

        public virtual marca_modelo_escaner marca_modelo_escaner { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<escaner_ubicacion_escaner> escaner_ubicacion_escaner { get; set; }
    }
}
