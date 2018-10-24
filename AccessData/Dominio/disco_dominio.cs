using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace AccessData.Dominio
{
    public class disco_dominio
    {
        public void Eliminar(int id)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    discoPC disco = ctx.discoPC.Find(id);
                    disco.fecha_baja = DateTime.Now;

                    ctx.SaveChanges();
                }
            }
            catch (Exception E)
            {
                throw;
            }
        }

        
        public List<discoPC> Listar()
        {
            var disco = new List<discoPC>();
            try
            {
                using (var ctx = new SGEIContext())

                    disco = ctx.discoPC
                        .Where(x => x.fecha_baja == null)
                        .OrderBy(x => x.descripcion)
                        .ToList();
            }
            catch (Exception E)
            {

                throw;
            }
            return disco;
        }

        public discoPC Obtener(int id)
        {
            var disco = new discoPC();
            try
            {
                using (var ctx = new SGEIContext())

                    disco = ctx.discoPC.Where(x => x.id_disco == id)
                                                .SingleOrDefault();

            }
            catch (Exception E)
            {
                throw;
            }
            return disco;
        }

        public void Guardar(discoPC disco)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    if (disco.id_disco > 0) //Registro que ya existe
                    {
                        ctx.Entry(disco).State = EntityState.Modified;
                    }
                    else // Registro que es nuevo
                    {
                        ctx.Entry(disco).State = EntityState.Added;
                    }
                    ctx.SaveChanges();
                }
            }
            catch (Exception E)
            {
                throw;
            }
        }
    }
}
