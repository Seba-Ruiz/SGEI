namespace Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>Incidente generado automáticamente al cargar un ticket con equipo.</summary>
    [Table("ticket_incidente")]
    public partial class ticket_incidente
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id_ticket { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string tipo_equipo { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id_incidente { get; set; }

        public DateTime fecha { get; set; }
    }
}
