namespace Model
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class SGEIContext : DbContext
    {
        public SGEIContext()
            : base("name=SGEIContext")
        {
        }

        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<baja> baja { get; set; }
        public virtual DbSet<bajaPC> bajaPC { get; set; }
        public virtual DbSet<detalle_impresora_ubicacion> detalle_impresora_ubicacion { get; set; }
        public virtual DbSet<detalle_insumo_impresora> detalle_insumo_impresora { get; set; }
        public virtual DbSet<detalle_tel> detalle_tel { get; set; }
        public virtual DbSet<detallePC> detallePC { get; set; }
        public virtual DbSet<discoPC> discoPC { get; set; }
        public virtual DbSet<imagen> imagen { get; set; }
        public virtual DbSet<impresora> impresora { get; set; }
        public virtual DbSet<incidente> incidente { get; set; }
        public virtual DbSet<incidente_tel> incidente_tel { get; set; }
        public virtual DbSet<incidente_tel_tel> incidente_tel_tel { get; set; }
        public virtual DbSet<incidentePC> incidentePC { get; set; }
        public virtual DbSet<insumo> insumo { get; set; }
        public virtual DbSet<insumo_pc> insumo_pc { get; set; }
        public virtual DbSet<marca_modelo_cel> marca_modelo_cel { get; set; }
        public virtual DbSet<motherPC> motherPC { get; set; }
        public virtual DbSet<motivobajaPC> motivobajaPC { get; set; }
        public virtual DbSet<pc> pc { get; set; }
        public virtual DbSet<pc_incidentepc> pc_incidentepc { get; set; }
        public virtual DbSet<pc_insumo_pc> pc_insumo_pc { get; set; }
        public virtual DbSet<pc_ubicacionpc> pc_ubicacionpc { get; set; }
        public virtual DbSet<procesadorPC> procesadorPC { get; set; }
        public virtual DbSet<ramPC> ramPC { get; set; }
        public virtual DbSet<soPC> soPC { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<telefono> telefono { get; set; }
        public virtual DbSet<tipo_tel> tipo_tel { get; set; }
        public virtual DbSet<tipoPC> tipoPC { get; set; }
        public virtual DbSet<ubicacion> ubicacion { get; set; }
        public virtual DbSet<ubicacion_impresora> ubicacion_impresora { get; set; }
        public virtual DbSet<ubicacion_tel> ubicacion_tel { get; set; }
        public virtual DbSet<ubicacion_tel_tel> ubicacion_tel_tel { get; set; }
        public virtual DbSet<ubicacionPC> ubicacionPC { get; set; }
        public virtual DbSet<usuario> usuario { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRoles>()
                .HasMany(e => e.AspNetUserRoles)
                .WithRequired(e => e.AspNetRoles)
                .HasForeignKey(e => e.RoleId);

            modelBuilder.Entity<AspNetUsers>()
                .HasMany(e => e.AspNetUserClaims)
                .WithRequired(e => e.AspNetUsers)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUsers>()
                .HasMany(e => e.AspNetUserLogins)
                .WithRequired(e => e.AspNetUsers)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUsers>()
                .HasMany(e => e.AspNetUserRoles)
                .WithRequired(e => e.AspNetUsers)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<baja>()
                .Property(e => e.ultima_ubicacion)
                .IsUnicode(false);

            modelBuilder.Entity<baja>()
                .Property(e => e.motivo_baja)
                .IsUnicode(false);

            modelBuilder.Entity<detalle_impresora_ubicacion>()
                .Property(e => e.ip)
                .IsUnicode(false);

            modelBuilder.Entity<detalle_impresora_ubicacion>()
                .Property(e => e.pc_dondeseconecta)
                .IsUnicode(false);

            modelBuilder.Entity<detalle_tel>()
                .HasMany(e => e.incidente_tel_tel)
                .WithOptional(e => e.detalle_tel)
                .HasForeignKey(e => e.detalle_id);

            modelBuilder.Entity<detalle_tel>()
                .HasMany(e => e.ubicacion_tel_tel)
                .WithOptional(e => e.detalle_tel)
                .HasForeignKey(e => e.detalle_id);

            modelBuilder.Entity<detallePC>()
                .HasMany(e => e.bajaPC)
                .WithOptional(e => e.detallePC)
                .HasForeignKey(e => e.detallepc_id);

            modelBuilder.Entity<detallePC>()
                .HasMany(e => e.pc_insumo_pc)
                .WithOptional(e => e.detallePC)
                .HasForeignKey(e => e.detalle_pc_id);

            modelBuilder.Entity<discoPC>()
                .HasMany(e => e.detallePC)
                .WithOptional(e => e.discoPC)
                .HasForeignKey(e => e.disco_id);

            modelBuilder.Entity<impresora>()
                .Property(e => e.tipo)
                .IsUnicode(false);

            modelBuilder.Entity<impresora>()
                .Property(e => e.marca_modelo)
                .IsUnicode(false);

            modelBuilder.Entity<impresora>()
                .HasMany(e => e.ubicacion_impresora)
                .WithOptional(e => e.impresora)
                .HasForeignKey(e => e.impresora_id)
                .WillCascadeOnDelete();

            modelBuilder.Entity<incidente>()
                .Property(e => e.incidente1)
                .IsUnicode(false);

            modelBuilder.Entity<incidente>()
                .Property(e => e.accion)
                .IsUnicode(false);

            modelBuilder.Entity<incidente>()
                .HasMany(e => e.imagen)
                .WithOptional(e => e.incidente)
                .HasForeignKey(e => e.incidente_id)
                .WillCascadeOnDelete();

            modelBuilder.Entity<incidente_tel>()
                .HasMany(e => e.incidente_tel_tel)
                .WithOptional(e => e.incidente_tel)
                .HasForeignKey(e => e.incidentetel_id);

            modelBuilder.Entity<incidentePC>()
                .HasMany(e => e.pc_incidentepc)
                .WithOptional(e => e.incidentePC)
                .HasForeignKey(e => e.incidentepc_id);

            modelBuilder.Entity<insumo>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<insumo>()
                .HasMany(e => e.detalle_insumo_impresora)
                .WithOptional(e => e.insumo)
                .HasForeignKey(e => e.insumo_id)
                .WillCascadeOnDelete();

            modelBuilder.Entity<insumo_pc>()
                .HasMany(e => e.pc_insumo_pc)
                .WithOptional(e => e.insumo_pc)
                .HasForeignKey(e => e.insumo_id);

            modelBuilder.Entity<marca_modelo_cel>()
                .HasMany(e => e.detalle_tel)
                .WithOptional(e => e.marca_modelo_cel)
                .HasForeignKey(e => e.marca_modelo_id);

            modelBuilder.Entity<motherPC>()
                .HasMany(e => e.detallePC)
                .WithOptional(e => e.motherPC)
                .HasForeignKey(e => e.mother_id);

            modelBuilder.Entity<motivobajaPC>()
                .HasMany(e => e.bajaPC)
                .WithOptional(e => e.motivobajaPC)
                .HasForeignKey(e => e.motivobaja_id);

            modelBuilder.Entity<pc>()
                .HasMany(e => e.detallePC)
                .WithOptional(e => e.pc)
                .HasForeignKey(e => e.pc_id);

            modelBuilder.Entity<pc>()
                .HasMany(e => e.pc_incidentepc)
                .WithOptional(e => e.pc)
                .HasForeignKey(e => e.pc_id);

            modelBuilder.Entity<pc>()
                .HasMany(e => e.pc_ubicacionpc)
                .WithOptional(e => e.pc)
                .HasForeignKey(e => e.pc_id);

            modelBuilder.Entity<procesadorPC>()
                .HasMany(e => e.detallePC)
                .WithOptional(e => e.procesadorPC)
                .HasForeignKey(e => e.procesador_id);

            modelBuilder.Entity<ramPC>()
                .HasMany(e => e.detallePC)
                .WithOptional(e => e.ramPC)
                .HasForeignKey(e => e.ram_id);

            modelBuilder.Entity<soPC>()
                .HasMany(e => e.detallePC)
                .WithOptional(e => e.soPC)
                .HasForeignKey(e => e.so_id);

            modelBuilder.Entity<telefono>()
                .HasMany(e => e.detalle_tel)
                .WithOptional(e => e.telefono)
                .HasForeignKey(e => e.telefono_id);

            modelBuilder.Entity<tipo_tel>()
                .HasMany(e => e.detalle_tel)
                .WithOptional(e => e.tipo_tel)
                .HasForeignKey(e => e.tipo_tel_id);

            modelBuilder.Entity<tipoPC>()
                .HasMany(e => e.pc)
                .WithOptional(e => e.tipoPC)
                .HasForeignKey(e => e.tipo_id);

            modelBuilder.Entity<ubicacion>()
                .Property(e => e.nombreUbicacion)
                .IsUnicode(false);

            modelBuilder.Entity<ubicacion>()
                .Property(e => e.responsable)
                .IsUnicode(false);

            modelBuilder.Entity<ubicacion>()
                .HasMany(e => e.ubicacion_impresora)
                .WithOptional(e => e.ubicacion)
                .HasForeignKey(e => e.ubicacion_id)
                .WillCascadeOnDelete();

            modelBuilder.Entity<ubicacion_impresora>()
                .HasMany(e => e.baja)
                .WithOptional(e => e.ubicacion_impresora)
                .HasForeignKey(e => e.ubicacion_impresora_id);

            modelBuilder.Entity<ubicacion_impresora>()
                .HasMany(e => e.detalle_impresora_ubicacion)
                .WithOptional(e => e.ubicacion_impresora)
                .HasForeignKey(e => e.ubicacion_impresora_id)
                .WillCascadeOnDelete();

            modelBuilder.Entity<ubicacion_impresora>()
                .HasMany(e => e.detalle_insumo_impresora)
                .WithOptional(e => e.ubicacion_impresora)
                .HasForeignKey(e => e.ubicacion_impresora_id)
                .WillCascadeOnDelete();

            modelBuilder.Entity<ubicacion_impresora>()
                .HasMany(e => e.incidente)
                .WithOptional(e => e.ubicacion_impresora)
                .HasForeignKey(e => e.ubicacion_impresora_id)
                .WillCascadeOnDelete();

            modelBuilder.Entity<ubicacion_tel>()
                .HasMany(e => e.ubicacion_tel_tel)
                .WithOptional(e => e.ubicacion_tel)
                .HasForeignKey(e => e.ubicacion_tel_id);

            modelBuilder.Entity<ubicacionPC>()
                .HasMany(e => e.pc_ubicacionpc)
                .WithOptional(e => e.ubicacionPC)
                .HasForeignKey(e => e.ubicacionpc_id);

            modelBuilder.Entity<usuario>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<usuario>()
                .Property(e => e.pass)
                .IsUnicode(false);
        }
    }
}
