namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Auditoria")]
    public partial class Auditoria
    {
        public int id { get; set; }

        [StringLength(50)]
        public string accion { get; set; }

        public DateTime? fecha_hora { get; set; }

        public int? id_equipo { get; set; }

        [StringLength(50)]
        public string tipo_equipo { get; set; }

        [StringLength(150)]
        public string usuario { get; set; }
    }
}
