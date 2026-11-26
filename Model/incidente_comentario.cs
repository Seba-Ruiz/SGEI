namespace Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>Observación de un usuario en un incidente de cualquier equipo. Script: BaseDeDatos/2026-11-26_incidentes_observaciones.sql.</summary>
    [Table("incidente_comentario")]
    public partial class incidente_comentario
    {
        [Key]
        public int id_comentario { get; set; }

        /// <summary>Tipo de equipo (TiposEquipo): computadora, impresora, telefono, camara, switch, escaner.</summary>
        [Required]
        [StringLength(20)]
        public string tipo_equipo { get; set; }

        /// <summary>Id en la tabla de incidentes de ese tipo.</summary>
        public int id_incidente { get; set; }

        [Required]
        [StringLength(128)]
        public string usuario_id { get; set; }

        /// <summary>HTML saneado (HtmlSeguro).</summary>
        [Required]
        public string texto { get; set; }

        public DateTime fecha { get; set; }

        public DateTime? fecha_edicion { get; set; }
    }
}
