using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Negocio.Dominio
{
    public class tipo_telefono_dominio
    {
        public void Eliminar(int id)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    tipo_tel tel = ctx.tipo_tel.Find(id);
                    tel.fecha_baja = DateTime.Now;

                    ctx.SaveChanges();
                }
            }
            catch (Exception E)
            {
                throw;
            }
        }


        public List<tipo_tel> Listar()
        {
            var tel = new List<tipo_tel>();
            try
            {
                using (var ctx = new SGEIContext())

                    tel = ctx.tipo_tel
                        .Where(x => x.fecha_baja == null)
                        .ToList();
            }
            catch (Exception E)
            {

                throw;
            }
            return tel;
        }

        public tipo_tel Obtener(int id)
        {
            var tel = new tipo_tel();
            try
            {
                using (var ctx = new SGEIContext())

                    tel = ctx.tipo_tel.Where(x => x.id_tipo == id)
                                                .SingleOrDefault();

            }
            catch (Exception E)
            {
                throw;
            }
            return tel;
        }

        public void Guardar(tipo_tel tel)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    if (tel.id_tipo > 0) //Registro que ya existe
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
