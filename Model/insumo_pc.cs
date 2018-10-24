namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class insumo_pc
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public insumo_pc()
        {
            pc_insumo_pc = new HashSet<pc_insumo_pc>();
        }

        [Key]
        public int id_insumo { get; set; }

        [StringLength(50)]
        public string nombre { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha_baja { get; set; }

        public int? cantidad { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<pc_insumo_pc> pc_insumo_pc { get; set; }
    }
}
