using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Model;

namespace AccessData.Dominio
{
    public class ram_dominio
    {
        public void Eliminar(int id)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    ramPC ram = ctx.ramPC.Find(id);
                    ram.fecha_baja = DateTime.Now;

                    ctx.SaveChanges();
                }
            }
            catch (Exception E)
            {
                throw;
            }
        }

        public List<ramPC> ListarSinCantidad()
        {
            var ram = new List<ramPC>();
            try
            {
                using (var ctx = new SGEIContext())

                    ram = ctx.ramPC
                        .Where(x => x.fecha_baja == null)
                        .ToList();
            }
            catch (Exception E)
            {

                throw;
            }
            return ram;
        }
        public List<ramPC> Listar()
        {
            var ram = new List<ramPC>();
            try
            {
                using (var ctx = new SGEIContext())

                    ram = ctx.ramPC
                        .Where(x => x.fecha_baja == null)
                        .OrderBy(x => x.descripcion)
                        .ToList();
            }
            catch (Exception E)
            {

                throw;
            }
            return ram;
        }

        public ramPC Obtener(int id)
        {
            var ram = new ramPC();
            try
            {
                using (var ctx = new SGEIContext())

                    ram = ctx.ramPC.Where(x => x.id_ram == id)
                                                .SingleOrDefault();

            }
            catch (Exception E)
            {
                throw;
            }
            return ram;
        }

        public void Guardar(ramPC ram)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    if (ram.id_ram > 0) //Registro que ya existe
                    {
                        ctx.Entry(ram).State = EntityState.Modified;
                    }
                    else // Registro que es nuevo
                    {
                        ctx.Entry(ram).State = EntityState.Added;
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
