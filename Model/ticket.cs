namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>Ticket de trabajo (menú Tickets). Script: BaseDeDatos/2026-11-20_tickets.sql.</summary>
    [Table("ticket")]
    public partial class ticket
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ticket()
        {
            ticket_comentario = new HashSet<ticket_comentario>();
            ticket_historial = new HashSet<ticket_historial>();
            ticket_asignado = new HashSet<ticket_asignado>();
        }

        [Key]
        public int id_ticket { get; set; }

        [Required]
        [StringLength(150)]
        public string titulo { get; set; }

        public string descripcion { get; set; }

        /// <summary>1 Baja · 2 Media · 3 Alta · 4 Urgente</summary>
        public byte prioridad { get; set; }

        /// <summary>1 Pendiente · 2 En curso · 3 Bloqueado · 4 Terminado</summary>
        public byte estado { get; set; }

        [Required]
        [StringLength(128)]
        public string creado_por_id { get; set; }

        public DateTime fecha_alta { get; set; }

        [Column(TypeName = "date")]
        public DateTime fecha_prevista { get; set; }

        public DateTime fecha_estado { get; set; }

        public DateTime? fecha_fin { get; set; }

        [StringLength(20)]
        public string tipo_equipo { get; set; }

        public int? id_equipo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ticket_comentario> ticket_comentario { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ticket_historial> ticket_historial { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ticket_asignado> ticket_asignado { get; set; }
    }
}
