namespace Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>Cambio en un incidente: quién, cuándo, qué campo y sus valores (como texto).</summary>
    [Table("incidente_historial")]
    public partial class incidente_historial
    {
        [Key]
        public int id_historial { get; set; }

        [Required]
        [StringLength(20)]
        public string tipo_equipo { get; set; }

        public int id_incidente { get; set; }

        [Required]
        [StringLength(128)]
        public string usuario_id { get; set; }

        public DateTime fecha { get; set; }

        /// <summary>ALTA, TICKET, EQUIPO, INCIDENTE, FECHA_INCIDENTE, ACCION, FECHA_ACCION, FOTO.</summary>
        [Required]
        [StringLength(20)]
        public string campo { get; set; }

        [StringLength(200)]
        public string valor_anterior { get; set; }

        [StringLength(200)]
        public string valor_nuevo { get; set; }
    }
}
