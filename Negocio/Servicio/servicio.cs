using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Negocio.Servicio
{
    public class servicio
    {

        public int sumaImpresora()
        {
            int impresoras;
        
        using (var ctx = new SGEIContext())
                {
                    impresoras = Convert.ToInt32(ctx.ubicacion_impresora.Where(x=>x.fecha_baja==null).Count());
                
                }
            return impresoras;
        }
        public int sumaComputadora()
        {
            int compu;

            using (var ctx = new SGEIContext())
            {
                compu = Convert.ToInt32(ctx.pc.Where(x => x.fecha_baja == null).Count());

            }
            return compu;
        }

        public int sumaTelefono()
        {
            int telefonos;

            using (var ctx = new SGEIContext())
            {
                telefonos = Convert.ToInt32(ctx.telefono.Where(x => x.fecha_baja == null).Count());

            }
            return telefonos;
        }

        public int sumaCamara()
        {
            int camaras;

            using (var ctx = new SGEIContext())
            {
                camaras = Convert.ToInt32(ctx.camara.Where(x => x.fecha_baja == null).Count());

            }
            return camaras;
        }

        public int sumaSW()
        {
            int sw;

            using (var ctx = new SGEIContext())
            {
                sw = Convert.ToInt32(ctx.swicth.Where(x => x.fecha_baja == null).Count());

            }
            return sw;
        }
        public int sumaES()
        {
            int sw;

            using (var ctx = new SGEIContext())
            {
                sw = Convert.ToInt32(ctx.escaner.Where(x => x.fecha_baja == null).Count());

            }
            return sw;
        }

        public List<pc> computadoras_de_baja()
        {
            using (var ctx = new SGEIContext())
            {
                var pcsdebaja = ctx.pc.Where(x => x.fecha_baja != null).ToList();

                return pcsdebaja;
            }
            
        }




    }
}
