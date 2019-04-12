namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("mantenimiento")]
    public partial class mantenimiento
    {
        [Key]
        public int id_mantenimiento { get; set; }

        public DateTime? fecha_mantenimiento { get; set; }

        [StringLength(250)]
        public string descripcion { get; set; }

        public DateTime? proximo_mantenimiento { get; set; }

        public int? pc_id { get; set; }

        public virtual pc pc { get; set; }
    }
}
