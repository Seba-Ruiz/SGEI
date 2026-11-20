namespace Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>Comentario de un ticket (HTML de Summernote, ya saneado).</summary>
    [Table("ticket_comentario")]
    public partial class ticket_comentario
    {
        [Key]
        public int id_comentario { get; set; }

        public int id_ticket { get; set; }

        [Required]
        [StringLength(128)]
        public string usuario_id { get; set; }

        [Required]
        public string texto { get; set; }

        public DateTime fecha { get; set; }

        public DateTime? fecha_edicion { get; set; }

        public virtual ticket ticket { get; set; }
    }
}
