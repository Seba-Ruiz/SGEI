namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("detallePC")]
    public partial class detallePC
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public detallePC()
        {
            bajaPC = new HashSet<bajaPC>();
            pc_insumo_pc = new HashSet<pc_insumo_pc>();
        }

        [Key]
        public int id_detalle { get; set; }

        public int? pc_id { get; set; }

        public int? ram_id { get; set; }

        public int? so_id { get; set; }

        public int? disco_id { get; set; }

        public int? mother_id { get; set; }

        [StringLength(50)]
        public string observacion { get; set; }

        [StringLength(50)]
        public string responsablepc { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha_detalle { get; set; }

        public int? procesador_id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<bajaPC> bajaPC { get; set; }

        public virtual discoPC discoPC { get; set; }

        public virtual motherPC motherPC { get; set; }

        public virtual pc pc { get; set; }

        public virtual procesadorPC procesadorPC { get; set; }

        public virtual ramPC ramPC { get; set; }

        public virtual soPC soPC { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<pc_insumo_pc> pc_insumo_pc { get; set; }
    }
}
