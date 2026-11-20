using System;
using System.Linq;

namespace Negocio.DTO
{
    /// <summary>Opción de un combo: id, texto y un dato extra (ubicación del equipo, stock del insumo).</summary>
    public class DTO_Opcion
    {
        public int Id { get; set; }
        public string Texto { get; set; }
        public string Extra { get; set; }
    }

    /// <summary>Un equipo de cualquier tipo, con los datos para mostrarlo y elegirlo.</summary>
    public class DTO_Equipo
    {
        /// <summary>Clave que usan los incidentes/insumos/bajas de ese tipo (ver ConsultasUnificadas).</summary>
        public int Id { get; set; }
        /// <summary>Id de la tabla principal del equipo (pc, telefono, camara, ...): el que se audita y se reactiva.</summary>
        public int EquipoId { get; set; }
        /// <summary>Id del detalle del equipo, cuando su ubicación cuelga del detalle (teléfono, cámara, switch, escáner).</summary>
        public int? DetalleId { get; set; }
        public string Nombre { get; set; }
        public string Ip { get; set; }
        public int? UbicacionId { get; set; }
        public string Ubicacion { get; set; }
        public DateTime? FechaBaja { get; set; }

        /// <summary>"HPSISTEMAS05 · 192.168.10.5 · SISTEMAS"</summary>
        public string Texto
        {
            get { return string.Join(" · ", new[] { Nombre, Ip, Ubicacion }.Where(s => !string.IsNullOrWhiteSpace(s))); }
        }
    }

    public class DTO_IncidenteFila
    {
        public string Tipo { get; set; }
        public string TipoNombre { get; set; }
        public int Id { get; set; }
        public string Equipo { get; set; }
        public string Ip { get; set; }
        public string Ubicacion { get; set; }
        public string Incidente { get; set; }
        public DateTime? FechaIncidente { get; set; }
        public string Accion { get; set; }
        public DateTime? FechaAccion { get; set; }
        public int Fotos { get; set; }
    }

    /// <summary>Alta/edición de un incidente de cualquier equipo.</summary>
    public class DTO_IncidenteFormulario
    {
        public string Tipo { get; set; }
        /// <summary>0 = nuevo.</summary>
        public int Id { get; set; }
        public int? EquipoId { get; set; }
        /// <summary>Incidente del catálogo (todos los equipos menos impresoras).</summary>
        public int? IncidenteId { get; set; }
        /// <summary>Descripción libre (impresoras).</summary>
        public string IncidenteTexto { get; set; }
        public DateTime? FechaIncidente { get; set; }
        public string Accion { get; set; }
        public DateTime? FechaAccion { get; set; }
    }

    public class DTO_InsumoFila
    {
        public string Tipo { get; set; }
        public string TipoNombre { get; set; }
        public int Id { get; set; }
        public string Equipo { get; set; }
        public string Ip { get; set; }
        public string Ubicacion { get; set; }
        public string Insumo { get; set; }
        public string NroSerie { get; set; }
        public DateTime? Fecha { get; set; }
    }

    /// <summary>Alta/edición de un insumo colocado en un equipo (impresoras y computadoras).</summary>
    public class DTO_InsumoFormulario
    {
        public string Tipo { get; set; }
        public int Id { get; set; }
        public int? EquipoId { get; set; }
        public int? InsumoId { get; set; }
        /// <summary>Solo computadoras.</summary>
        public string NroSerie { get; set; }
        public DateTime? Fecha { get; set; }
    }

    public class DTO_BajaFila
    {
        public string Tipo { get; set; }
        public string TipoNombre { get; set; }
        public int Id { get; set; }
        public string Equipo { get; set; }
        public string Ip { get; set; }
        public string UltimaUbicacion { get; set; }
        public string Motivo { get; set; }
        public DateTime? FechaBaja { get; set; }
    }

    /// <summary>Volver a dar de alta un equipo dado de baja: nueva ubicación y fecha.</summary>
    public class DTO_Reactivacion
    {
        public string Tipo { get; set; }
        public int Id { get; set; }
        public int? UbicacionId { get; set; }
        public DateTime? Fecha { get; set; }
    }
}
