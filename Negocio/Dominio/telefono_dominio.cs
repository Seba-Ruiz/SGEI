using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Negocio.Dominio
{
    public class telefono_dominio
    {
        public void Eliminar(int id)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    telefono tel = ctx.telefono.Find(id);
                    tel.fecha_baja = DateTime.Now;

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
            var tel = new telefono();
            try
            {
                using (var ctx = new SGEIContext())

                    tel = ctx.telefono.OrderByDescending(x => x.id_telefono)
                                                .FirstOrDefault();

            }
            catch (Exception E)
            {
                throw;
            }
            return tel.id_telefono;
        }

        public List<telefono> Listar()
        {
            var tel = new List<telefono>();
            try
            {
                using (var ctx = new SGEIContext())

                    tel = ctx.telefono
                        .Where(x => x.fecha_baja == null)
                        .ToList();
            }
            catch (Exception E)
            {

                throw;
            }
            return tel;
        }

        public telefono Obtener(int id)
        {
            var tel = new telefono();
            try
            {
                using (var ctx = new SGEIContext())

                    tel = ctx.telefono.Where(x => x.id_telefono == id)
                                                .SingleOrDefault();

            }
            catch (Exception E)
            {
                throw;
            }
            return tel;
        }

        public void Guardar(telefono tel)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    if (tel.id_telefono > 0) //Registro que ya existe
                    {
                        ctx.Entry(tel).State = EntityState.Modified;
                    }
                    else // Registro que es nuevo
                    {
                        ctx.Entry(tel).State = EntityState.Added;
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
