namespace Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>Persona asignada a un ticket (un ticket puede tener varias). Script: BaseDeDatos/2026-11-26_tickets_varios_asignados.sql.</summary>
    [Table("ticket_asignado")]
    public partial class ticket_asignado
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id_ticket { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(128)]
        public string usuario_id { get; set; }

        public DateTime fecha_asignacion { get; set; }

        public virtual ticket ticket { get; set; }
    }
}
