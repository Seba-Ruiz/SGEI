using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace AccessData.Dominio
{
    public class usuario_dominio
    {
        private SGEIContext db = new SGEIContext();
        
        public usuario Obtener(string nombre)
        {
            var user = new usuario();
            try
            {
                using (var ctx = new SGEIContext())

                    user = ctx.usuario
                                       .Where(x => x.nombre == nombre)
                                       .SingleOrDefault();

            }
            catch (Exception E)
            {
                throw;
            }
            return user;
        }

    }
}
