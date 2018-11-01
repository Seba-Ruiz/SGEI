using Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Dominio
{
    public class camara_dominio
    {
        public void Eliminar(int id)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    camara cam = ctx.camara.Find(id);
                    cam.fecha_baja = DateTime.Now;

                    ctx.SaveChanges();
                }
            }
            catch (Exception E)
            {
                throw;
            }
        }
        public int ObtenerUltimo()
        {
            var cam = new camara();
            try
            {
                using (var ctx = new SGEIContext())

                    cam = ctx.camara.OrderByDescending(x => x.id_camara)
                                                .FirstOrDefault();

            }
            catch (Exception E)
            {
                throw;
            }
            return cam.id_camara;
        }

        public List<camara> Listar()
        {
            var cam = new List<camara>();
            try
            {
                using (var ctx = new SGEIContext())

                    cam = ctx.camara
                        .Where(x => x.fecha_baja == null)
                        .ToList();
            }
            catch (Exception E)
            {

                throw;
            }
            return cam;
        }

        public camara Obtener(int id)
        {
            var cam = new camara();
            try
            {
                using (var ctx = new SGEIContext())

                    cam = ctx.camara.Where(x => x.id_camara == id)
                                                .SingleOrDefault();

            }
            catch (Exception E)
            {
                throw;
            }
            return cam;
        }

        public void Guardar(camara cam)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    if (cam.id_camara > 0) //Registro que ya existe
                    {
                        ctx.Entry(cam).State = EntityState.Modified;
                    }
                    else // Registro que es nuevo
                    {
                        ctx.Entry(cam).State = EntityState.Added;
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
