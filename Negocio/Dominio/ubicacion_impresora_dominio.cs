using Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Dominio
{
    public class ubicacion_impresora_dominio
    {
        public ubicacion_impresora Obtener(int id)
        {
            var detalles = new ubicacion_impresora();
            try
            {
                using (var ctx = new SGEIContext())

                    detalles = ctx.ubicacion_impresora.Where(x => x.id == id)
                                                .SingleOrDefault();

            }
            catch (Exception E)
            {
                throw;
            }
            return detalles;
        }
        public int ObtenerUltimo()
        {
            var imp = new ubicacion_impresora();
            try
            {
                using (var ctx = new SGEIContext())

                    imp = ctx.ubicacion_impresora.OrderByDescending(x => x.id)
                                                .FirstOrDefault();

            }
            catch (Exception E)
            {
                throw;
            }
            return imp.id;
        }
        public void Guardar(ubicacion_impresora ubi_det)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    if (ubi_det.id > 0) //Registro que ya existe
                    {
                        ctx.Entry(ubi_det).State = EntityState.Modified;
                    }
                    else // Registro que es nuevo
                    {
                        ctx.Entry(ubi_det).State = EntityState.Added;
                    }
                    ctx.SaveChanges();
                }
            }
            catch (Exception E)
            {
                throw;
            }
        }
        public List<ubicacion_impresora> Listar()
        {
            var cam = new List<ubicacion_impresora>();
            try
            {
                using (var ctx = new SGEIContext())

                    cam = ctx.ubicacion_impresora
                        .Where(x => x.fecha_baja == null)
                        .ToList();
            }
            catch (Exception E)
            {

                throw;
            }
            return cam;
        }
    }
}
