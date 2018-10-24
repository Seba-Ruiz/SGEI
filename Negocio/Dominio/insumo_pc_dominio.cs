using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Negocio.Dominio
{
    public class insumo_pc_dominio
    {
        public void Eliminar(int id)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    insumo_pc insu = ctx.insumo_pc.Find(id);
                    insu.fecha_baja = DateTime.Now;

                    ctx.SaveChanges();
                }
            }
            catch (Exception E)
            {
                throw;
            }
        }

        public void Restar(int insumo)
        {
            using (var ctx = new SGEIContext())
            {
                insumo_pc insu = ctx.insumo_pc.Find(insumo);
                insu.cantidad = insu.cantidad - 1;
                ctx.SaveChanges();
            }

        }

        public List<insumo_pc> ListarSinCantidad()
        {
            var insumos = new List<insumo_pc>();
            try
            {
                using (var ctx = new SGEIContext())

                    insumos = ctx.insumo_pc
                        .Where(x => x.fecha_baja == null & x.cantidad > 0)
                        .ToList();
            }
            catch (Exception E)
            {

                throw;
            }
            return insumos;
        }


        public List<insumo_pc> Listar()
        {
            var insu = new List<insumo_pc>();
            try
            {
                using (var ctx = new SGEIContext())

                    insu = ctx.insumo_pc
                        .Where(x => x.fecha_baja == null)
                        .ToList();
            }
            catch (Exception E)
            {

                throw;
            }
            return insu;
        }

        public insumo_pc Obtener(int id)
        {
            var insu = new insumo_pc();
            try
            {
                using (var ctx = new SGEIContext())

                    insu = ctx.insumo_pc.Where(x => x.id_insumo == id)
                                                .SingleOrDefault();

            }
            catch (Exception E)
            {
                throw;
            }
            return insu;
        }

        public void Guardar(insumo_pc insu)
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
