using System;
using System.Collections.Generic;
using System.Linq;

namespace login_v6.Helpers
{
    /// <summary>Un link del menú lateral.</summary>
    public sealed class ItemMenu
    {
        public ItemMenu(string texto, string controlador, string accion, bool soloAdministrador = false)
        {
            Texto = texto;
            Controlador = controlador;
            Accion = accion;
            SoloAdministrador = soloAdministrador;
        }

        public string Texto { get; private set; }
        public string Controlador { get; private set; }
        public string Accion { get; private set; }
        public bool SoloAdministrador { get; private set; }

        public bool EsActual(string controlador, string accion)
        {
            return string.Equals(Controlador, controlador, StringComparison.OrdinalIgnoreCase)
                && string.Equals(Accion, accion, StringComparison.OrdinalIgnoreCase);
        }
    }

    /// <summary>Grupo de links dentro de una sección (por ejemplo "Computadoras" dentro de "Consultas").</summary>
    public sealed class GrupoMenu
    {
        public GrupoMenu(string titulo, params ItemMenu[] items)
        {
            Titulo = titulo;
            Items = items;
        }

        /// <summary>Puede ser null: los links se muestran sin subtítulo.</summary>
        public string Titulo { get; private set; }
        public IList<ItemMenu> Items { get; private set; }
    }

    /// <summary>Sección plegable del menú lateral, o link directo si tiene un único item sin grupos.</summary>
    public sealed class SeccionMenu
    {
        public SeccionMenu(string id, string texto, string icono, bool soloAdministrador, params GrupoMenu[] grupos)
        {
            Id = id;
            Texto = texto;
            Icono = icono;
            SoloAdministrador = soloAdministrador;
            Grupos = grupos;
        }

        public string Id { get; private set; }
        public string Texto { get; private set; }
        public string Icono { get; private set; }
        public bool SoloAdministrador { get; private set; }
        public IList<GrupoMenu> Grupos { get; private set; }

        public IEnumerable<ItemMenu> Items { get { return Grupos.SelectMany(g => g.Items); } }

        public bool ContieneActual(string controlador, string accion)
        {
            return Items.Any(i => i.EsActual(controlador, accion));
        }
    }

    /// <summary>
    /// Estructura del menú lateral. Para agregar una pantalla al menú alcanza con sumarla acá.
    /// </summary>
    public static class MenuSgei
    {
        private static GrupoMenu G(string titulo, params ItemMenu[] items) { return new GrupoMenu(titulo, items); }
        private static ItemMenu I(string texto, string controlador, string accion, bool soloAdministrador = false)
        {
            return new ItemMenu(texto, controlador, accion, soloAdministrador);
        }

        public static readonly ItemMenu Inicio = I("Inicio", "Inicio", "Index");
        public static readonly ItemMenu Dashboard = I("Dashboard", "Inicio", "Dashboard");

        public static readonly IList<SeccionMenu> Secciones = new[]
        {
            new SeccionMenu("altas", "Altas", "bi-plus-square", false,
                G(null,
                    I("Impresora", "Home", "AltaImpr"),
                    I("Computadora", "Home", "AltaCompu"),
                    I("Teléfono", "Home", "Altatel"),
                    I("Cámara", "Home", "Altacam"),
                    I("Switch", "Home", "AltaSW"),
                    I("Escáner", "Home", "AltaES"))),

            new SeccionMenu("consultas", "Consultas", "bi-clipboard-data", false,
                G("Impresoras",
                    I("Informes", "Consulta", "Index"),
                    I("Bajas", "Home", "impresoras_baja"),
                    I("Preventivos", "Impresora", "Preventivo")),
                G("Computadoras",
                    I("Informes", "Consulta", "computadoras"),
                    I("Bajas", "Consulta", "compuBaja"),
                    I("Preventivos", "Computadora", "Preventivo")),
                G("Otros equipos",
                    I("Teléfonos", "Consulta", "telefonos"),
                    I("Cámaras", "Consulta", "camara"),
                    I("Switches", "Consulta", "switchs"),
                    I("Escáneres", "Consulta", "escaners"))),

            new SeccionMenu("ingresos", "Ingresos", "bi-box-seam", false,
                G("Impresoras",
                    I("Cargar insumos", "Ingresos", "Index"),
                    I("Ver ingresos", "Ingresos", "VerIngresoImpresora")),
                G("Computadoras",
                    I("Cargar insumos", "Ingresos", "IndexCompu"),
                    I("Ver ingresos", "Ingresos", "VerIngresoPC")),
                G("Cámaras",
                    I("Cargar insumos", "Ingresos", "IndexCam"),
                    I("Ver ingresos", "Ingresos", "VerIngresoCam")),
                G(null,
                    I("Proveedores", "ABM", "proveedores", soloAdministrador: true))),

            new SeccionMenu("buscar", "Buscar", "bi-search", false,
                G(null,
                    I("PC por número", "Buscar", "Buscar_PC"),
                    I("PC por IP", "Buscar", "Buscar_PC_IP"),
                    I("Insumo por n° de serie", "Buscar", "Buscar_Insu"))),

            new SeccionMenu("gestion", "Gestión", "bi-gear", true,
                G("Impresoras",
                    I("Insumos", "ABM", "insumos"),
                    I("Modelos", "ABM", "impresoras"),
                    I("Ubicaciones", "ABM", "ubicacion")),
                G("Computadoras",
                    I("Insumos", "ABM", "insumospc"),
                    I("Memorias", "ABM", "rams"),
                    I("Discos", "ABM", "discos"),
                    I("Sistemas operativos", "ABM", "sos"),
                    I("Placas madre", "ABM", "mothers"),
                    I("Procesadores", "ABM", "procesadorespc"),
                    I("Tipos", "ABM", "tipospc"),
                    I("Ubicaciones", "ABM", "ubicacionespc"),
                    I("Incidentes", "ABM", "incidentespc"),
                    I("Motivos de baja", "ABM", "motivospc")),
                G("Teléfonos",
                    I("Tipos", "ABM", "tipo_telefonos"),
                    I("Modelos", "ABM", "mmarcas"),
                    I("Ubicaciones", "ABM", "ubicacionestel"),
                    I("Incidentes", "ABM", "incidentestel")),
                G("Cámaras",
                    I("Insumos", "ABM", "insumoscam"),
                    I("Modelos", "ABM", "modelocam"),
                    I("Ubicaciones", "ABM", "ubicam"),
                    I("Incidentes", "ABM", "incidentecam")),
                G("Switches",
                    I("Modelos", "ABM", "modelosw"),
                    I("Ubicaciones", "ABM", "ubicacionSW"),
                    I("Incidentes", "ABM", "incidenteSW")),
                G("Escáneres",
                    I("Modelos", "ABM", "modeloES"),
                    I("Ubicaciones", "ABM", "UbicacionES"),
                    I("Incidentes", "ABM", "IncidenteES"))),

            new SeccionMenu("usuarios", "Usuarios", "bi-people", true,
                G(null,
                    I("Gestionar", "Usuario", "Index"),
                    I("Roles", "Role", "Index"),
                    I("Nuevo usuario", "Account", "Register")))
        };
    }
}
