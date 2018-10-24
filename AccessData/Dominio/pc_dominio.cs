using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace AccessData.Dominio
{
    public class pc_dominio
    {
        public void Eliminar(int id)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    pc compu = ctx.pc.Find(id);
                    compu.fecha_baja = DateTime.Now;

                    ctx.SaveChanges();
                }
            }
            catch (Exception E)
            {
                throw;
            }
        }


        public List<pc> Listar()
        {
            var compu = new List<pc>();
            try
            {
                using (var ctx = new SGEIContext())

                    compu = ctx.pc
                        .Where(x => x.fecha_baja == null)
                        .ToList();
            }
            catch (Exception E)
            {

                throw;
            }
            return compu;
        }

        public pc Obtener(int id)
        {
            var compu = new pc();
            try
            {
                using (var ctx = new SGEIContext())

                    compu = ctx.pc.Where(x => x.id_pc == id)
                                                .SingleOrDefault();

            }
            catch (Exception E)
            {
                throw;
            }
            return compu;
        }

        public int ObtenerUltimo()
        {
            var compu = new pc();
            try
            {
                using (var ctx = new SGEIContext())

                    compu = ctx.pc.OrderByDescending(x=>x.id_pc)
                                                .FirstOrDefault();

            }
            catch (Exception E)
            {
                throw;
            }
            return compu.id_pc;
        }

        public void Guardar(pc compu)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    if (compu.id_pc > 0) //Registro que ya existe
                    {
                        ctx.Entry(compu).State = EntityState.Modified;
                    }
                    else // Registro que es nuevo
                    {
                        ctx.Entry(compu).State = EntityState.Added;
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
