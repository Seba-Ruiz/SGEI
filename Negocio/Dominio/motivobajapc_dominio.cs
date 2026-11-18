using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Negocio.Dominio
{
    public class motivobajapc_dominio
    {
        public void Eliminar(int id)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    motivobajaPC motivo = ctx.motivobajaPC.Find(id);
                    motivo.fecha_baja = DateTime.Now;

                    ctx.SaveChanges();
                }
            }
            catch (Exception E)
            {
                throw E;
            }
        }


        public List<motivobajaPC> Listar()
        {
            var motivo = new List<motivobajaPC>();
            try
            {
                using (var ctx = new SGEIContext())

                    motivo = ctx.motivobajaPC
                        .Where(x => x.fecha_baja == null)
                        .ToList();
            }
            catch (Exception E)
            {

                throw E;
            }
            return motivo;
        }

        public motivobajaPC Obtener(int id)
        {
            var motivo = new motivobajaPC();
            try
            {
                using (var ctx = new SGEIContext())

                    motivo = ctx.motivobajaPC.Where(x => x.id_motivobaja == id)
                                                .SingleOrDefault();

            }
            catch (Exception E)
            {
                throw E;
            }
            return motivo;
        }

        public void Guardar(motivobajaPC motivo)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    if (motivo.id_motivobaja > 0) //Registro que ya existe
                    {
                        ctx.Entry(motivo).State = EntityState.Modified;
                    }
                    else // Registro que es nuevo
                    {
                        ctx.Entry(motivo).State = EntityState.Added;
                    }
                    ctx.SaveChanges();
                }
            }
            catch (Exception E)
            {
                throw E;
            }
        }
    }
}
