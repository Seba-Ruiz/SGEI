namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("pc")]
    public partial class pc
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public pc()
        {
            detallePC = new HashSet<detallePC>();
            pc_incidentepc = new HashSet<pc_incidentepc>();
            pc_ubicacionpc = new HashSet<pc_ubicacionpc>();
        }

        [Key]
        public int id_pc { get; set; }

        [StringLength(20)]
        public string ip { get; set; }

        [StringLength(20)]
        public string nombre { get; set; }

        [StringLength(50)]
        public string descripcion { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha_baja { get; set; }

        public int? tipo_id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<detallePC> detallePC { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<pc_incidentepc> pc_incidentepc { get; set; }

        public virtual tipoPC tipoPC { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<pc_ubicacionpc> pc_ubicacionpc { get; set; }
    }
}
