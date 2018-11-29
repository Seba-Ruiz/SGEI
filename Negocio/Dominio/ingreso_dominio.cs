using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Negocio.Dominio
{
    public class ingreso_dominio
    {
        public void Guardar(IngresoInsumo a)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    if (a.id > 0) //Registro que ya existe
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
                throw;
            }
        }

        public List<IngresoInsumo> ListarIngresoImpresoras()
        {
            var ingreso = new List<IngresoInsumo>();
            try
            {
                using (var ctx = new SGEIContext())

                    ingreso = ctx.IngresoInsumo
                        .Where(x => x.tipo_equipo == "IMPRESORA")
                        .ToList();
            }
            catch (Exception E)
            {

                throw;
            }
            return ingreso;
        }
        public List<IngresoInsumo> ListarIngresoPC()
        {
            var ingreso = new List<IngresoInsumo>();
            try
            {
                using (var ctx = new SGEIContext())

                    ingreso = ctx.IngresoInsumo
                        .Where(x => x.tipo_equipo == "COMPUTADORA")
                        .ToList();
            }
            catch (Exception E)
            {

                throw;
            }
            return ingreso;
        }
    }
}
