namespace Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>Aviso para un usuario (campana + mail). Tabla: BaseDeDatos/2026-11-25_notificaciones.sql.</summary>
    [Table("notificacion")]
    public partial class notificacion
    {
        [Key]
        public int id_notificacion { get; set; }

        /// <summary>Destinatario.</summary>
        [Required]
        [StringLength(128)]
        public string usuario_id { get; set; }

        /// <summary>Quién generó el aviso (null si lo generó el sistema).</summary>
        [StringLength(128)]
        public string origen_usuario_id { get; set; }

        [Required]
        [StringLength(40)]
        public string tipo { get; set; }

        [Required]
        [StringLength(200)]
        public string titulo { get; set; }

        [StringLength(1000)]
        public string mensaje { get; set; }

        [StringLength(400)]
        public string url { get; set; }

        [StringLength(20)]
        public string referencia_tipo { get; set; }

        public int? referencia_id { get; set; }

        public DateTime fecha { get; set; }

        public DateTime? fecha_lectura { get; set; }

        public DateTime? fecha_mail { get; set; }

        [StringLength(500)]
        public string error_mail { get; set; }
    }
}
