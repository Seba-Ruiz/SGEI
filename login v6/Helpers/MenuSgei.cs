using System;
using System.Collections.Generic;
using System.Linq;

namespace login_v6.Helpers
{
    /// <summary>Un link del menú lateral o de la barra de un equipo.</summary>
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

    /// <summary>Grupo de links dentro de una sección del menú lateral.</summary>
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

    /// <summary>Sección plegable del menú lateral.</summary>
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

    /// <summary>Desplegable de la barra de un equipo: Consultas, Ingresos, Buscar o Gestión.</summary>
    public sealed class MenuEquipo
    {
        public MenuEquipo(string texto, string icono, bool soloAdministrador, params ItemMenu[] items)
        {
            Texto = texto;
            Icono = icono;
            SoloAdministrador = soloAdministrador;
            Items = items;
        }

        public string Texto { get; private set; }
        public string Icono { get; private set; }
        public bool SoloAdministrador { get; private set; }
        public IList<ItemMenu> Items { get; private set; }

        public bool ContieneActual(string controlador, string accion)
        {
            return Items.Any(i => i.EsActual(controlador, accion));
        }
    }

    /// <summary>
    /// Un tipo de equipo (Computadoras, Impresoras, ...): su grilla, su alta y los desplegables de su barra.
    /// La barra se muestra en todas las pantallas del equipo: las de sus menús y las de <see cref="Paginas"/>.
    /// </summary>
    public sealed class Equipo
    {
        private readonly HashSet<string> controladores;
        private readonly HashSet<string> paginas;

        public Equipo(string id, string nombre, string icono, ItemMenu grilla, ItemMenu alta, MenuEquipo[] menus,
                      string[] controladoresPropios, params string[] paginas)
        {
            Id = id;
            Nombre = nombre;
            Icono = icono;
            Grilla = grilla;
            Alta = alta;
            Menus = menus;
            controladores = new HashSet<string>(controladoresPropios, StringComparer.OrdinalIgnoreCase);
            this.paginas = new HashSet<string>(paginas, StringComparer.OrdinalIgnoreCase);
        }

        public string Id { get; private set; }
        public string Nombre { get; private set; }
        public string Icono { get; private set; }
        public ItemMenu Grilla { get; private set; }
        public ItemMenu Alta { get; private set; }
        public IList<MenuEquipo> Menus { get; private set; }

        /// <summary>¿La pantalla actual es de este equipo?</summary>
        public bool Contiene(string controlador, string accion)
        {
            return controladores.Contains(controlador ?? "")
                || paginas.Contains(controlador + "/" + accion)
                || Grilla.EsActual(controlador, accion)
                || Alta.EsActual(controlador, accion)
                || Menus.Any(m => m.ContieneActual(controlador, accion));
        }
    }

    /// <summary>
    /// Navegación de SGEI. Menú lateral: Inicio, Dashboard, Equipos, Consultas (incidentes, insumos y bajas de todos
    /// los equipos) y Administración (Usuarios, Proveedores).
    /// Lo propio de cada equipo (alta, preventivos, ingresos, búsquedas y gestión) está en la barra de ese equipo,
    /// arriba de sus pantallas (Views/Shared/_BarraEquipo.cshtml). Para sumar una pantalla alcanza con agregarla acá.
    /// </summary>
    public static class MenuSgei
    {
        private static ItemMenu I(string texto, string controlador, string accion, bool soloAdministrador = false)
        {
            return new ItemMenu(texto, controlador, accion, soloAdministrador);
        }

        private static MenuEquipo Preventivos(string controlador) { return new MenuEquipo("Preventivos", "bi-tools", false, I("Preventivos", controlador, "Preventivo")); }
        private static MenuEquipo Ingresos(params ItemMenu[] items) { return new MenuEquipo("Ingresos", "bi-box-seam", false, items); }
        private static MenuEquipo Buscar(params ItemMenu[] items) { return new MenuEquipo("Buscar", "bi-search", false, items); }
        private static MenuEquipo Gestion(params ItemMenu[] items) { return new MenuEquipo("Gestión", "bi-gear", true, items); }
        private static string[] Controladores(params string[] nombres) { return nombres; }

        /// <summary>Dashboard (unificado con el antiguo Inicio): queda en /Inicio/Index.</summary>
        public static readonly ItemMenu Dashboard = I("Dashboard", "Inicio", "Index");
        /// <summary>Board de tickets; queda marcado en todas las pantallas de TicketsController.</summary>
        public static readonly ItemMenu Tickets = I("Tickets", "Tickets", "Index");
        public static readonly ItemMenu Proveedores = I("Proveedores", "ABM", "proveedores", soloAdministrador: true);

        public static readonly IList<Equipo> Equipos = new[]
        {
            new Equipo("computadoras", "Computadoras", "bi-pc-display",
                I("Computadoras", "Computadora", "Index"),
                I("Nueva computadora", "Home", "AltaCompu"),
                new[]
                {
                    Preventivos("Computadora"),
                    Ingresos(
                        I("Cargar insumos", "Ingresos", "IndexCompu"),
                        I("Ver ingresos", "Ingresos", "VerIngresoPC")),
                    Buscar(
                        I("PC por número", "Buscar", "Buscar_PC"),
                        I("PC por IP", "Buscar", "Buscar_PC_IP"),
                        I("Insumo por n° de serie", "Buscar", "Buscar_Insu")),
                    Gestion(
                        I("Insumos", "ABM", "insumospc"),
                        I("Memorias", "ABM", "rams"),
                        I("Discos", "ABM", "discos"),
                        I("Sistemas operativos", "ABM", "sos"),
                        I("Placas madre", "ABM", "mothers"),
                        I("Procesadores", "ABM", "procesadorespc"),
                        I("Tipos", "ABM", "tipospc"),
                        I("Ubicaciones", "ABM", "ubicacionespc"),
                        I("Incidentes", "ABM", "incidentespc"),
                        I("Motivos de baja", "ABM", "motivospc"))
                },
                Controladores("Computadora", "Buscar"),
                "ABM/Crudinsumopc", "ABM/Editarinsumopc", "ABM/CrudRam", "ABM/EditarRam", "ABM/CrudDisco", "ABM/EditarDisco",
                "ABM/Crudso", "ABM/Editarso", "ABM/Crudmoth", "ABM/Editarmoth", "ABM/Crudprocesadorpc", "ABM/Editarprocesadorpc",
                "ABM/Crudtipopc", "ABM/Editartipopc", "ABM/Crudubipc", "ABM/Editarubipc", "ABM/Crudincidentepc", "ABM/Editarincidentepc",
                "ABM/Crudmotivopc", "ABM/Editarmotivopc"),

            new Equipo("impresoras", "Impresoras", "bi-printer",
                I("Impresoras", "Home", "Index"),
                I("Nueva impresora", "Home", "AltaImpr"),
                new[]
                {
                    Preventivos("Impresora"),
                    Ingresos(
                        I("Cargar insumos", "Ingresos", "Index"),
                        I("Ver ingresos", "Ingresos", "VerIngresoImpresora")),
                    Gestion(
                        I("Insumos", "ABM", "insumos"),
                        I("Modelos", "ABM", "impresoras"),
                        I("Ubicaciones", "ABM", "ubicacion"))
                },
                Controladores("Impresora"),
                "Home/Baja", "Home/Baja_aceptada", "Home/BuscarParaMover", "Home/Mover",
                "Home/Crud", "Home/CrudDetalle_Impresora_Ubicacion", "Home/CrudUbicacion_impresora",
                "ABM/Crud", "ABM/Editar", "ABM/Insumos", "ABM/CrudImpresora", "ABM/EditarImpresora", "ABM/Impresoras",
                "ABM/CrudUbicacion", "ABM/EditarUbicacion", "ABM/Ubicacion"),

            new Equipo("telefonos", "Teléfonos", "bi-telephone",
                I("Teléfonos", "Telefonos", "Index"),
                I("Nuevo teléfono", "Home", "AltaTel"),
                new[]
                {
                    Gestion(
                        I("Tipos", "ABM", "tipo_telefonos"),
                        I("Modelos", "ABM", "mmarcas"),
                        I("Ubicaciones", "ABM", "ubicacionestel"),
                        I("Incidentes", "ABM", "incidentestel"))
                },
                Controladores("Telefonos"),
                "ABM/Crudtipo_tel", "ABM/Editartipo_tel", "ABM/Crudmmarca", "ABM/Editarmmarca",
                "ABM/Crudubicaciontel", "ABM/Editarubicaciontel", "ABM/Crudincidentetel", "ABM/Editarincidentetel"),

            new Equipo("camaras", "Cámaras", "bi-camera-video",
                I("Cámaras", "Camara", "Index"),
                I("Nueva cámara", "Home", "AltaCam"),
                new[]
                {
                    Ingresos(
                        I("Cargar insumos", "Ingresos", "IndexCam"),
                        I("Ver ingresos", "Ingresos", "VerIngresoCam")),
                    Gestion(
                        I("Insumos", "ABM", "insumoscam"),
                        I("Modelos", "ABM", "modelocam"),
                        I("Ubicaciones", "ABM", "ubicam"),
                        I("Incidentes", "ABM", "incidentecam"))
                },
                Controladores("Camara"),
                "ABM/Crudinsumocam", "ABM/EditarinsumoCam", "ABM/Crudmmaarcacam", "ABM/EditarmodeloCam",
                "ABM/Crudubicam", "ABM/EditarubiCam", "ABM/Crudincidentecam", "ABM/EditarincidenteCam"),

            new Equipo("switches", "Switches", "bi-diagram-3",
                I("Switches", "Swichs", "Index"),
                I("Nuevo switch", "Home", "Altasw"),
                new[]
                {
                    Gestion(
                        I("Modelos", "ABM", "modelosw"),
                        I("Ubicaciones", "ABM", "ubicacionSW"),
                        I("Incidentes", "ABM", "incidenteSW"))
                },
                Controladores("Swichs"),
                "ABM/Crudmodelosw", "ABM/Editarmodelosw", "ABM/Crudubicacionsw", "ABM/Editarubicacionsw",
                "ABM/Crudincidentesw", "ABM/Editarincidentesw"),

            new Equipo("escaneres", "Escáneres", "bi-upc-scan",
                I("Escáneres", "Escaner", "Index"),
                I("Nuevo escáner", "Home", "Altaes"),
                new[]
                {
                    Gestion(
                        I("Modelos", "ABM", "modeloES"),
                        I("Ubicaciones", "ABM", "UbicacionES"),
                        I("Incidentes", "ABM", "IncidenteES"))
                },
                Controladores("Escaner"),
                "ABM/CrudmodeloES", "ABM/EditarmodeloES", "ABM/CrudubicacionES", "ABM/EditarubicacionES",
                "ABM/CrudincidenteES", "ABM/EditarincidenteES")
        };

        /// <summary>
        /// Consultas de todos los equipos juntos (ConsultasController). Cada item queda marcado también en sus
        /// pantallas de alta, detalle y edición: las acciones cuyo nombre contiene la palabra clave (ver _MenuLateral).
        /// </summary>
        public static readonly SeccionMenu Consultas = new SeccionMenu("consultas", "Consultas", "bi-clipboard-data", false,
            new GrupoMenu(null,
                I("Incidentes", "Consultas", "Incidentes"),
                I("Insumos", "Consultas", "Insumos"),
                I("Bajas", "Consultas", "Bajas")));

        /// <summary>Usuarios (solo administradores), en la parte de administración del menú lateral.</summary>
        public static readonly SeccionMenu Usuarios = new SeccionMenu("usuarios", "Usuarios", "bi-people", true,
            new GrupoMenu(null,
                I("Gestionar", "Usuario", "Index"),
                I("Roles", "Role", "Index"),
                I("Nuevo usuario", "Account", "Register")));

        /// <summary>El equipo al que pertenece la pantalla actual, o null (Inicio, Usuarios, cuenta, ...).</summary>
        public static Equipo EquipoActual(string controlador, string accion)
        {
            return Equipos.FirstOrDefault(e => e.Contiene(controlador, accion));
        }
    }
}
