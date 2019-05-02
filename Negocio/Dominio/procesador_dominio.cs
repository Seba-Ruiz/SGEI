using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Negocio.Dominio
{
    public class procesador_dominio
    {
        public void Eliminar(int id)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    procesadorPC procesador = ctx.procesadorPC.Find(id);
                    procesador.fecha_baja = DateTime.Now;

                    ctx.SaveChanges();
                }
            }
            catch (Exception E)
            {
                throw E;
            }
        }


        public List<procesadorPC> Listar()
        {
            var procesador = new List<procesadorPC>();
            try
            {
                using (var ctx = new SGEIContext())

                    procesador = ctx.procesadorPC
                        .Where(x => x.fecha_baja == null)
                        .OrderBy(x => x.descripcion)
                        .ToList();
            }
            catch (Exception E)
            {

                throw E;
            }
            return procesador;
        }

        public procesadorPC Obtener(int id)
        {
            var procesador = new procesadorPC();
            try
            {
                using (var ctx = new SGEIContext())

                    procesador = ctx.procesadorPC.Where(x => x.id_procesador == id)
                                                .SingleOrDefault();

            }
            catch (Exception E)
            {
                throw E;
            }
            return procesador;
        }

        public void Guardar(procesadorPC procesador)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    if (procesador.id_procesador > 0) //Registro que ya existe
                    {
                        ctx.Entry(procesador).State = EntityState.Modified;
                    }
                    else // Registro que es nuevo
                    {
                        ctx.Entry(procesador).State = EntityState.Added;
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
