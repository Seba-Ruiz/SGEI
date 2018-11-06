using Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Dominio
{
    public class mmarca_sw_dominio
    {
        public void Eliminar(int id)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    marca_modelo_switch mm = ctx.marca_modelo_switch.Find(id);
                    mm.fecha_baja = DateTime.Now;

                    ctx.SaveChanges();
                }
            }
            catch (Exception E)
            {
                throw;
            }
        }


        public List<marca_modelo_switch> Listar()
        {
            var mm = new List<marca_modelo_switch>();
            try
            {
                using (var ctx = new SGEIContext())

                    mm = ctx.marca_modelo_switch
                        .Where(x => x.fecha_baja == null)
                        .ToList();
            }
            catch (Exception E)
            {

                throw;
            }
            return mm;
        }

        public marca_modelo_switch Obtener(int id)
        {
            var mm = new marca_modelo_switch();
            try
            {
                using (var ctx = new SGEIContext())

                    mm = ctx.marca_modelo_switch.Where(x => x.id_mmarca == id)
                                                .SingleOrDefault();

            }
            catch (Exception E)
            {
                throw;
            }
            return mm;
        }

        public void Guardar(marca_modelo_switch mm)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    if (mm.id_mmarca > 0) //Registro que ya existe
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
                throw;
            }
        }
    }
}
