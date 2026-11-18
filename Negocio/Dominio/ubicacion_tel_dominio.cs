using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Negocio.Dominio
{
    public class ubicacion_tel_dominio
    {
        public void Eliminar(int id)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    ubicacion_tel ubi = ctx.ubicacion_tel.Find(id);
                    ubi.fecha_baja = DateTime.Now;

                    ctx.SaveChanges();
                }
            }
            catch (Exception E)
            {
                throw E;
            }
        }


        public List<ubicacion_tel> Listar()
        {
            var ubi = new List<ubicacion_tel>();
            try
            {
                using (var ctx = new SGEIContext())

                    ubi = ctx.ubicacion_tel
                        .Where(x => x.fecha_baja == null)
                        .ToList();
            }
            catch (Exception E)
            {

                throw E;
            }
            return ubi;
        }

        public ubicacion_tel Obtener(int id)
        {
            var ubi = new ubicacion_tel();
            try
            {
                using (var ctx = new SGEIContext())

                    ubi = ctx.ubicacion_tel.Where(x => x.id_ubicacion_tel == id)
                                                .SingleOrDefault();

            }
            catch (Exception E)
            {
                throw E;
            }
            return ubi;
        }

        public void Guardar(ubicacion_tel ubi)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    if (ubi.id_ubicacion_tel > 0) //Registro que ya existe
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
