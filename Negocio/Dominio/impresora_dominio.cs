
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Negocio.Dominio
{
    public class impresora_dominio
    {

        public void Eliminar(int id)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    impresora imp = ctx.impresora.Find(id);
                    imp.fecha_baja = DateTime.Now;

                    ctx.SaveChanges();
                }
            }
            catch (Exception E)
            {
                throw E;
            }
        }

        public impresora Obtener(int id)
        {
            var impresoras = new impresora();
            try
            {
                using (var ctx = new SGEIContext())

                    impresoras = ctx.impresora.Where(x => x.id == id)
                                                .SingleOrDefault();

            }
            catch (Exception E)
            {
                throw E;
            }
            return impresoras;
        }

        public void Guardar(impresora imp)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    if (imp.id > 0) //Registro que ya existe
                    {
                        ctx.Entry(imp).State = EntityState.Modified;
                    }
                    else // Registro que es nuevo
                    {
                        ctx.Entry(imp).State = EntityState.Added;
                    }
                    ctx.SaveChanges();
                }
            }
            catch (Exception E)
            {
                throw E;
            }
        }

        public List<impresora> Listar()
        {
            var impresoras = new List<impresora>();
            try
            {
                using (var ctx = new SGEIContext())

                    impresoras = ctx.impresora.Where(x => x.fecha_baja == null)
                        .ToList();
            }
            catch (Exception E)
            {

                throw E;
            }
            return impresoras;
        }
    }
}
