using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Negocio.Dominio
{
    public class mantenimiento_dominio
    {
        public void Guardar(mantenimiento a)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    if (a.id_mantenimiento > 0) //Registro que ya existe
                    {
                        ctx.Entry(a).State = EntityState.Modified;
                    }
                    else // Registro que es nuevo
                    {
                        ctx.Entry(a).State = EntityState.Added;
                    }
                    ctx.SaveChanges();
                }
            }
            catch (Exception E)
            {
                throw E;
            }
        }



        public void Guardari(mantenimiento_impre a)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    if (a.id_mante > 0) //Registro que ya existe
                    {
                        ctx.Entry(a).State = EntityState.Modified;
                    }
                    else // Registro que es nuevo
                    {
                        ctx.Entry(a).State = EntityState.Added;
                    }
                    ctx.SaveChanges();
                }
            }
            catch (Exception E)
            {
                throw E;
            }
        }
        public mantenimiento ObtenerMantePC(int id)
        {
            var mante = new mantenimiento();
            try
            {
                using (var ctx = new SGEIContext())

                    mante = ctx.mantenimiento.Where(x => x.pc_id == id && x.realizado==false)
                                                .SingleOrDefault();

            }
            catch (Exception E)
            {
                throw E;
            }
            return mante;
        }
        public mantenimiento_impre ObtenerManteImpre(int id)
        {
            var mante = new mantenimiento_impre();
            try
            {
                using (var ctx = new SGEIContext())

                    mante = ctx.mantenimiento_impre.Where(x => x.id_impresora == id && x.realizado==false)
                                                .SingleOrDefault();

            }
            catch (Exception E)
            {
                throw E;
            }
            return mante;
        }

        public List<mantenimiento> Listar()
        {
            var mante = new List<mantenimiento>();
            try
            {
                using (var ctx = new SGEIContext())

                    mante = ctx.mantenimiento.Where(x => x.fecha_mantenimiento != null)
                        .ToList();
            }
            catch (Exception E)
            {

                throw E;
            }
            return mante;
        }

        public List<mantenimiento_impre> Listari()
        {
            var mante = new List<mantenimiento_impre>();
            try
            {
                using (var ctx = new SGEIContext())

                    mante = ctx.mantenimiento_impre.Where(x => x.fecha_mantenimiento != null)
                        .ToList();
            }
            catch (Exception E)
            {

                throw E;
            }
            return mante;
        }
    }
}
