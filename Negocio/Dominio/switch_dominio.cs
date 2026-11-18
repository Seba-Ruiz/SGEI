using Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Dominio
{
    public class switch_dominio
    {
        public void Eliminar(int id)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    swicth sw = ctx.swicth.Find(id);
                    sw.fecha_baja = DateTime.Now;

                    ctx.SaveChanges();
                }
            }
            catch (Exception E)
            {
                throw E;
            }
        }
        public int ObtenerUltimo()
        {
            var sw = new swicth();
            try
            {
                using (var ctx = new SGEIContext())

                    sw = ctx.swicth.OrderByDescending(x => x.id_switch)
                                                .FirstOrDefault();

            }
            catch (Exception E)
            {
                throw E;
            }
            return sw.id_switch;
        }

        public List<swicth> Listar()
        {
            var sw = new List<swicth>();
            try
            {
                using (var ctx = new SGEIContext())

                    sw = ctx.swicth
                        .Where(x => x.fecha_baja == null)
                        .ToList();
            }
            catch (Exception E)
            {

                throw E;
            }
            return sw;
        }

        public swicth Obtener(int id)
        {
            var sw = new swicth();
            try
            {
                using (var ctx = new SGEIContext())

                    sw = ctx.swicth.Where(x => x.id_switch == id)
                                                .SingleOrDefault();

            }
            catch (Exception E)
            {
                throw E;
            }
            return sw;
        }

        public void Guardar(swicth sw)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    if (sw.id_switch > 0) //Registro que ya existe
                    {
                        ctx.Entry(sw).State = EntityState.Modified;
                    }
                    else // Registro que es nuevo
                    {
                        ctx.Entry(sw).State = EntityState.Added;
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
