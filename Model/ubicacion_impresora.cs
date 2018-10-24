namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ubicacion_impresora
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ubicacion_impresora()
        {
            baja = new HashSet<baja>();
            detalle_impresora_ubicacion = new HashSet<detalle_impresora_ubicacion>();
            detalle_insumo_impresora = new HashSet<detalle_insumo_impresora>();
            incidente = new HashSet<incidente>();
        }

        public int id { get; set; }

        public int? impresora_id { get; set; }

        public int? ubicacion_id { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha_ubicacion { get; set; }

        [StringLength(50)]
        public string descripcion { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha_baja { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<baja> baja { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<detalle_impresora_ubicacion> detalle_impresora_ubicacion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<detalle_insumo_impresora> detalle_insumo_impresora { get; set; }

        public virtual impresora impresora { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<incidente> incidente { get; set; }

        public virtual ubicacion ubicacion { get; set; }
    }
}
