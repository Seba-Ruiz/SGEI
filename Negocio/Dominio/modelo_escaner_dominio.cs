using Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Dominio
{
    public class modelo_escaner_dominio
    {
        public void Eliminar(int id)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    marca_modelo_escaner mm = ctx.marca_modelo_escaner.Find(id);
                    mm.fecha_baja = DateTime.Now;

                    ctx.SaveChanges();
                }
            }
            catch (Exception E)
            {
                throw E;
            }
        }


        public List<marca_modelo_escaner> Listar()
        {
            var mm = new List<marca_modelo_escaner>();
            try
            {
                using (var ctx = new SGEIContext())

                    mm = ctx.marca_modelo_escaner
                        .Where(x => x.fecha_baja == null)
                        .ToList();
            }
            catch (Exception E)
            {

                throw E;
            }
            return mm;
        }

        public marca_modelo_escaner Obtener(int id)
        {
            var mm = new marca_modelo_escaner();
            try
            {
                using (var ctx = new SGEIContext())

                    mm = ctx.marca_modelo_escaner.Where(x => x.id_marca_modelo_escaner == id)
                                                .SingleOrDefault();

            }
            catch (Exception E)
            {
                throw E;
            }
            return mm;
        }

        public void Guardar(marca_modelo_escaner mm)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    if (mm.id_marca_modelo_escaner > 0) //Registro que ya existe
                    {
                        ctx.Entry(mm).State = EntityState.Modified;
                    }
                    else // Registro que es nuevo
                    {
                        ctx.Entry(mm).State = EntityState.Added;
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
