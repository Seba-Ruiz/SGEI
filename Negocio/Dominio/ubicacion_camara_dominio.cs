using Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Dominio
{
    public class ubicacion_camara_dominio
    {
        public void Eliminar(int id)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    ubicacion_camara ubi = ctx.ubicacion_camara.Find(id);
                    ubi.fecha_baja = DateTime.Now;

                    ctx.SaveChanges();
                }
            }
            catch (Exception E)
            {
                throw E;
            }
        }


        public List<ubicacion_camara> Listar()
        {
            var ubi = new List<ubicacion_camara>();
            try
            {
                using (var ctx = new SGEIContext())

                    ubi = ctx.ubicacion_camara
                        .Where(x => x.fecha_baja == null)
                        .ToList();
            }
            catch (Exception E)
            {

                throw E;
            }
            return ubi;
        }

        public ubicacion_camara Obtener(int id)
        {
            var ubi = new ubicacion_camara();
            try
            {
                using (var ctx = new SGEIContext())

                    ubi = ctx.ubicacion_camara.Where(x => x.id_ubicacion_camara == id)
                                                .SingleOrDefault();

            }
            catch (Exception E)
            {
                throw E;
            }
            return ubi;
        }

        public void Guardar(ubicacion_camara ubi)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    if (ubi.id_ubicacion_camara > 0) //Registro que ya existe
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
