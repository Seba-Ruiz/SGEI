namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class detalle_camara
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public detalle_camara()
        {
            detalle_insumo_camara = new HashSet<detalle_insumo_camara>();
            ubicacion_camara_camara = new HashSet<ubicacion_camara_camara>();
        }

        [Key]
        public int id_detalle { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha_detalle { get; set; }

        public int? marca_model_id { get; set; }

        public int? camara_id { get; set; }

        [StringLength(20)]
        public string nroip { get; set; }

        public virtual camara camara { get; set; }

        public virtual marca_modelo_camara marca_modelo_camara { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<detalle_insumo_camara> detalle_insumo_camara { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ubicacion_camara_camara> ubicacion_camara_camara { get; set; }
    }
}
