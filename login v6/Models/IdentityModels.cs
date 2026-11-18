using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using login_v6.Seguridad;
using Negocio.Dominio;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace login_v6.Models
{
    // Para agregar datos de perfil del usuario, agregue más propiedades a su clase ApplicationUser. Visite https://go.microsoft.com/fwlink/?LinkID=317594 para obtener más información.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Tenga en cuenta que el valor de authenticationType debe coincidir con el definido en CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            // AspNetUserRoles guarda el historial de roles; solo vale el más reciente.
            // Se reemplazan los claims de rol que agrega Identity por el rol vigente.
            foreach (var claim in userIdentity.FindAll(ClaimTypes.Role).ToList())
            {
                userIdentity.RemoveClaim(claim);
            }

            var rolVigente = new usuario_dominio().obtenerTipoRol(Id);
            if (rolVigente != null)
            {
                userIdentity.AddClaim(new Claim(RolesSgei.ClaimRolId, rolVigente.Id));
                userIdentity.AddClaim(new Claim(ClaimTypes.Role, rolVigente.Name));
            }

            return userIdentity;
        }
    }

    //public class ApplicationRole: IdentityRole
    //{
    //    public ApplicationRole(): base(){}
    //    public ApplicationRole(string nombreRol): base(nombreRol) { }

    //}
   



    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
       
        public ApplicationDbContext()

            : base("SGEIContext", throwIfV1Schema: false)
        {
            
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<login_v6.Models.RoleViewModel> RoleViewModels { get; set; }
    }
}