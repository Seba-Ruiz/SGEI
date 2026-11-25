using System;

namespace login_v6.Models
{
    /// <summary>Datos de la modal "Mi cuenta" (Views/Manage/_MiCuenta.cshtml).</summary>
    public class MiCuentaViewModel
    {
        public const int LargoNombre = 50;
        public const int LargoTelefono = 30;

        public string Id { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        /// <summary>"Seba Ruiz", o el usuario si no tiene nombre cargado.</summary>
        public string NombreCompleto { get; set; }
        public string Iniciales { get; set; }
        public string Rol { get; set; }
        public DateTime? UltimoIngreso { get; set; }
    }
}
