using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Negocio.Dominio
{
    public class u_impresora_dominio
    {
        
        public void mover(int u_impresora, int u_nueva_impresora)
        {
            using (var ctx = new SGEIContext())
            {
                var resultado = ctx.ubicacion_impresora.Find(u_impresora);

                resultado.fecha_ubicacion = DateTime.Now;
                resultado.ubicacion_id = u_nueva_impresora;

                ctx.SaveChanges();
            }
        }
    }
}
