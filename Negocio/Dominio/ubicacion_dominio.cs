using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Negocio.Dominio
{
    public class ubicacion_dominio
    {
        public List<ubicacion> Listar()
        {
            var ubicaciones = new List<ubicacion>();
            try
            {
                using (var ctx = new SGEIContext())

                    ubicaciones = ctx.ubicacion
                        .Where(x => x.fecha_baja == null)
                        .OrderBy(x => x.nombreUbicacion)
                        .ToList();
            }
            catch (Exception E)
            {

                throw;
            }
            return ubicaciones;
        }


        public ubicacion Obtener(int? id)
        {
            var ubicaciones = new ubicacion();
            try
            {
                using (var ctx = new SGEIContext())

                    ubicaciones = ctx.ubicacion.Where(x => x.id == id)
                                                .SingleOrDefault();

            }
            catch (Exception E)
            {
                throw;
            }
            return ubicaciones;
        }

        public void Guardar(ubicacion ubi)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    if (ubi.id > 0) //Registro que ya existe
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
                throw;
            }
        }

        public void Eliminar(int id)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    ubicacion ubi = ctx.ubicacion.Find(id);
                    ubi.fecha_baja = DateTime.Now;

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