namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("imagen")]
    public partial class imagen
    {
        public int id { get; set; }

        [Column("imagen")]
        public byte[] imagen1 { get; set; }

        public int? incidente_id { get; set; }

        public virtual incidente incidente { get; set; }
    }
}
