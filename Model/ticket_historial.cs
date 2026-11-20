namespace Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>Cambio en un ticket: quién, cuándo, qué campo y sus valores (como texto).</summary>
    [Table("ticket_historial")]
    public partial class ticket_historial
    {
        [Key]
        public int id_historial { get; set; }

        public int id_ticket { get; set; }

        [Required]
        [StringLength(128)]
        public string usuario_id { get; set; }

        public DateTime fecha { get; set; }

        [Required]
        [StringLength(20)]
        public string campo { get; set; }

        [StringLength(200)]
        public string valor_anterior { get; set; }

        [StringLength(200)]
        public string valor_nuevo { get; set; }

        public virtual ticket ticket { get; set; }
    }
}
