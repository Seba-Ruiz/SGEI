namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AspNetUserRoles
    {
        [Required]
        [StringLength(128)]
        public string UserId { get; set; }

        [Required]
        [StringLength(128)]
        public string RoleId { get; set; }

        public DateTime? fecha_rol { get; set; }

        public int id { get; set; }

        public virtual AspNetRoles AspNetRoles { get; set; }

        public virtual AspNetUsers AspNetUsers { get; set; }
    }
}
