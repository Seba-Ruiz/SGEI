using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Model;


namespace Negocio.Dominio
{
    public class insumo_cam_dominio
    {

        public void Eliminar(int id)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    insumo_camara insu = ctx.insumo_camara.Find(id);
                    insu.fecha_baja = DateTime.Now;

                    ctx.SaveChanges();
                }
            }
            catch (Exception E)
            {
                throw;
            }
        }

        public List<insumo_camara> ListarSinCantidad()
        {
            var insumos = new List<insumo_camara>();
            try
            {
                using (var ctx = new SGEIContext())

                    insumos = ctx.insumo_camara
                        .Where(x => x.fecha_baja == null & x.cantidad > 0)
                        .ToList();
            }
            catch (Exception E)
            {

                throw;
            }
            return insumos;
        }
        public List<insumo_camara> Listar()
        {
            var insumos = new List<insumo_camara>();
            try
            {
                using (var ctx = new SGEIContext())

                    insumos = ctx.insumo_camara
                        .Where(x => x.fecha_baja == null)
                        .ToList();
            }
            catch (Exception E)
            {

                throw;
            }
            return insumos;
        }

        public insumo_camara Obtener(int id)
        {
            var insumos = new insumo_camara();
            try
            {
                using (var ctx = new SGEIContext())

                    insumos = ctx.insumo_camara.Where(x => x.id_insumo == id)
                                                .SingleOrDefault();

            }
            catch (Exception E)
            {
                throw;
            }
            return insumos;
        }

        public void Guardar(insumo_camara insu)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    if (insu.id_insumo > 0) //Registro que ya existe
                    {
                        ctx.Entry(insu).State = EntityState.Modified;
                    }
                    else // Registro que es nuevo
                    {
                        ctx.Entry(insu).State = EntityState.Added;
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
