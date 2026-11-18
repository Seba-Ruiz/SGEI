using Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Dominio
{
    public class ubicacion_escaner_dominio
    {

        public void Eliminar(int id)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    ubicacion_escaner ubi = ctx.ubicacion_escaner.Find(id);
                    ubi.fecha_baja = DateTime.Now;

                    ctx.SaveChanges();
                }
            }
            catch (Exception E)
            {
                throw E;
            }
        }


        public List<ubicacion_escaner> Listar()
        {
            var ubi = new List<ubicacion_escaner>();
            try
            {
                using (var ctx = new SGEIContext())

                    ubi = ctx.ubicacion_escaner
                        .Where(x => x.fecha_baja == null)
                        .ToList();
            }
            catch (Exception E)
            {

                throw E;
            }
            return ubi;
        }

        public ubicacion_escaner Obtener(int id)
        {
            var ubi = new ubicacion_escaner();
            try
            {
                using (var ctx = new SGEIContext())

                    ubi = ctx.ubicacion_escaner.Where(x => x.id_ubicacion_escaner == id)
                                                .SingleOrDefault();

            }
            catch (Exception E)
            {
                throw E;
            }
            return ubi;
        }

        public void Guardar(ubicacion_escaner ubi)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    if (ubi.id_ubicacion_escaner > 0) //Registro que ya existe
                    {
                        ctx.Entry(ubi).State = EntityState.Modified;
                    }
                    else // Registro que es nuevo
                    {
                        ctx.Entry(ubi).State = EntityState.Added;
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
