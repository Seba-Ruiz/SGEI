namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class insumo_camara
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public insumo_camara()
        {
            detalle_insumo_camara = new HashSet<detalle_insumo_camara>();
        }

        [Key]
        public int id_insumo { get; set; }

        [StringLength(50)]
        public string nombre { get; set; }

        [StringLength(10)]
        public string fecha_baja { get; set; }

        public int? cantidad { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<detalle_insumo_camara> detalle_insumo_camara { get; set; }
    }
}
