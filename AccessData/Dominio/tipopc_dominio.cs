using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace AccessData.Dominio
{
    public class tipopc_dominio
    {
        public void Eliminar(int id)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    tipoPC tipo = ctx.tipoPC.Find(id);
                    tipo.fecha_baja = DateTime.Now;

                    ctx.SaveChanges();
                }
            }
            catch (Exception E)
            {
                throw;
            }
        }


        public List<tipoPC> Listar()
        {
            var tipo = new List<tipoPC>();
            try
            {
                using (var ctx = new SGEIContext())

                    tipo = ctx.tipoPC
                        .Where(x => x.fecha_baja == null)
                        .OrderBy(x => x.nombre_tipo)
                        .ToList();
            }
            catch (Exception E)
            {

                throw;
            }
            return tipo;
        }

        public tipoPC Obtener(int id)
        {
            var tipo = new tipoPC();
            try
            {
                using (var ctx = new SGEIContext())

                    tipo = ctx.tipoPC.Where(x => x.id_tipo == id)
                                                .SingleOrDefault();

            }
            catch (Exception E)
            {
                throw;
            }
            return tipo;
        }

        public void Guardar(tipoPC tipo)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    if (tipo.id_tipo > 0) //Registro que ya existe
                    {
                        ctx.Entry(tipo).State = EntityState.Modified;
                    }
                    else // Registro que es nuevo
                    {
                        ctx.Entry(tipo).State = EntityState.Added;
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
