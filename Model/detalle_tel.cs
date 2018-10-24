namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class detalle_tel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public detalle_tel()
        {
            incidente_tel_tel = new HashSet<incidente_tel_tel>();
            ubicacion_tel_tel = new HashSet<ubicacion_tel_tel>();
        }

        [Key]
        public int id_detalle { get; set; }

        [StringLength(50)]
        public string nro_interno { get; set; }

        public int? marca_modelo_id { get; set; }

        public int? telefono_id { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha_detalle { get; set; }

        public int? tipo_tel_id { get; set; }

        public virtual marca_modelo_cel marca_modelo_cel { get; set; }

        public virtual telefono telefono { get; set; }

        public virtual tipo_tel tipo_tel { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<incidente_tel_tel> incidente_tel_tel { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ubicacion_tel_tel> ubicacion_tel_tel { get; set; }
    }
}
