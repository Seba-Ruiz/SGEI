using System.Security.Claims;
using System.Security.Principal;

namespace login_v6.Seguridad
{
    /// <summary>
    /// Roles del sistema (Id de AspNetRoles). El rol vigente de un usuario es el registro
    /// más reciente de AspNetUserRoles (columna fecha_rol).
    /// </summary>
    public static class RolesSgei
    {
        public const string AdministradorId = "1";
        public const string OperarioId = "2";

        /// <summary>Claim con el Id del rol vigente, agregado al iniciar sesión.</summary>
        public const string ClaimRolId = "sgei:rol_id";

        public static bool EsAdministrador(this IPrincipal usuario)
        {
            var claims = usuario as ClaimsPrincipal;
            return claims != null
                && claims.Identity != null
                && claims.Identity.IsAuthenticated
                && claims.HasClaim(ClaimRolId, AdministradorId);
        }
    }
}
