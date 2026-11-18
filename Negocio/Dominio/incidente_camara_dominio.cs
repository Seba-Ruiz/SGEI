using Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Dominio
{
    public class incidente_camara_dominio
    {

        public List<camara_incidente_camara> verIncidente(int id)
        {
            var incidentes = new List<camara_incidente_camara>();

            try
            {
                using (var ctx = new SGEIContext())

                    incidentes = ctx.camara_incidente_camara.Where(x => x.camara_id == id)
                                              .ToList();

            }
            catch (Exception E)
            {
                throw E;
            }
            return incidentes;
        }





        public void Eliminar(int id)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    incidente_camara inci = ctx.incidente_camara.Find(id);
                    inci.fecha_baja = DateTime.Now;

                    ctx.SaveChanges();
                }
            }
            catch (Exception E)
            {
                throw E;
            }
        }


        public List<incidente_camara> Listar()
        {
            var inci = new List<incidente_camara>();
            try
            {
                using (var ctx = new SGEIContext())

                    inci = ctx.incidente_camara
                        .Where(x => x.fecha_baja == null)
                        .ToList();
            }
            catch (Exception E)
            {

                throw E;
            }
            return inci;
        }

        public incidente_camara Obtener(int id)
        {
            var inci = new incidente_camara();
            try
            {
                using (var ctx = new SGEIContext())

                    inci = ctx.incidente_camara.Where(x => x.id_incidente == id)
                                                .SingleOrDefault();

            }
            catch (Exception E)
            {
                throw E;
            }
            return inci;
        }

        public void Guardar(incidente_camara inci)
        {
            try
            {
                using (var ctx = new SGEIContext())
                {
                    if (inci.id_incidente > 0) //Registro que ya existe
                    {
                        ctx.Entry(inci).State = EntityState.Modified;
                    }
                    else // Registro que es nuevo
                    {
                        ctx.Entry(inci).State = EntityState.Added;
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
