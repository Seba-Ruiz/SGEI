using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace GIMP.Servicio
{
    public class Byte_A_Imagen
    {

        public Image Bytes_A_Ima(Byte[] ImgBytes)
        {
            Bitmap imagen = null;
            Byte[] bytes = (Byte[])(ImgBytes);
            MemoryStream ms = new MemoryStream(bytes);
            imagen = new Bitmap(ms);
            return imagen;
        }
    }
}