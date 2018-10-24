using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;

namespace AccessData.Dominio
{
    public class insumo_dominio
    {
        public void Eliminar(int id)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    insumo insu = ctx.insumo.Find(id);
                    insu.fecha_baja = DateTime.Now;

                    ctx.SaveChanges();
                }
            }
            catch (Exception E)
            {
                throw;
            }
        }

        public List<insumo> ListarSinCantidad()
        {
            var insumos = new List<insumo>();
            try
            {
                using (var ctx = new SGEIContext())

                    insumos = ctx.insumo
                        .Where(x=>x.fecha_baja == null & x.cantidad>0)
                        .ToList();
            }
            catch (Exception E)
            {

                throw;
            }
            return insumos;
        }
        public List<insumo> Listar()
        {
            var insumos = new List<insumo>();
            try
            {
                using (var ctx = new SGEIContext())

                    insumos = ctx.insumo
                        .Where(x => x.fecha_baja == null)
                        .ToList();
            }
            catch (Exception E)
            {

                throw;
            }
            return insumos;
        }

        public insumo Obtener(int id)
        {
            var insumos = new insumo();
            try
            {
                using (var ctx = new SGEIContext())

                    insumos = ctx.insumo.Where(x => x.id == id)
                                                .SingleOrDefault();

            }
            catch (Exception E)
            {
                throw;
            }
            return insumos;
        }

        public void Guardar(insumo insu)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    if (insu.id > 0) //Registro que ya existe
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
