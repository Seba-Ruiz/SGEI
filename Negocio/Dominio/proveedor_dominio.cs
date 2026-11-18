using Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Dominio
{
    public class proveedor_dominio
    {
        public void Eliminar(int id)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    Proveedor pro = ctx.Proveedor.Find(id);
                    pro.fecha_baja = DateTime.Now;

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
            var pro = new Proveedor();
            try
            {
                using (var ctx = new SGEIContext())

                    pro = ctx.Proveedor.OrderByDescending(x => x.id)
                                                .FirstOrDefault();

            }
            catch (Exception E)
            {
                throw E;
            }
            return pro.id;
        }

        public List<Proveedor> Listar()
        {
            var pro = new List<Proveedor>();
            try
            {
                using (var ctx = new SGEIContext())

                    pro = ctx.Proveedor
                        .Where(x => x.fecha_baja == null)
                        .ToList();
            }
            catch (Exception E)
            {

                throw E;
            }
            return pro;
        }

        public Proveedor Obtener(int id)
        {
            var pro = new Proveedor();
            try
            {
                using (var ctx = new SGEIContext())

                    pro = ctx.Proveedor.Where(x => x.id == id)
                                                .SingleOrDefault();

            }
            catch (Exception E)
            {
                throw E;
            }
            return pro;
        }

        public void Guardar(Proveedor pro)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    if (pro.id > 0) //Registro que ya existe
                    {
                        ctx.Entry(pro).State = EntityState.Modified;
                    }
                    else // Registro que es nuevo
                    {
                        ctx.Entry(pro).State = EntityState.Added;
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
