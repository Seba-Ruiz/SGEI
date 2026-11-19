/*
 * SGEI - comportamiento común a todas las pantallas que usan el layout.
 * Las vistas no llevan <script> inline: lo propio de cada una va en
 * Scripts/views/<Controlador>/<vista>.js y se carga después de este archivo.
 *
 * Atributos declarativos que maneja este archivo:
 *   data-confirmar="mensaje"   en <a> o <form>: pide confirmación antes de seguir.
 *   data-spinner               en <a>: muestra el spinner "Espere..." al navegar.
 *   data-abrir-al-cargar       en un .modal: lo abre cuando carga la página.
 *   data-cerrar-sesion         en <a>: envía el formulario #logoutForm.
 *   data-wizard                en <form>: arma el formulario por pasos (h3 + section).
 *   data-grilla                en <table>: búsqueda, orden y paginación en el navegador.
 */
(function ($, window, document) {
    "use strict";

    var SGEI = window.SGEI = window.SGEI || {};

    /** Token antiforgery renderizado por el layout en #antiforgery-ajax. */
    SGEI.tokenAntiforgery = function () {
        return $("#antiforgery-ajax input[name='__RequestVerificationToken']").val();
    };

    SGEI.mostrarSpinner = function () {
        $("#spiner").modal("show");
    };

    SGEI.ocultarSpinner = function () {
        $("#spiner").modal("hide");
    };

    /**
     * Wizard por pasos (jquery.steps) sobre un formulario: cada paso es un <h3> + <section>
     * dentro del primer <div> del form. Al finalizar se envía el formulario.
     * Reemplaza a assets/pages/jquery.wizard-init.js, que corría en todas las páginas.
     */
    SGEI.crearWizard = function (selectorFormulario) {
        var $form = $(selectorFormulario);
        $form.children("div").steps({
            headerTag: "h3",
            bodyTag: "section",
            transitionEffect: "slideLeft",
            labels: {
                current: "paso actual:",
                pagination: "Paginación",
                finish: "Finalizar",
                next: "Siguiente",
                previous: "Anterior",
                loading: "Cargando..."
            },
            onFinishing: function () {
                return true;
            },
            onFinished: function () {
                $form.trigger("submit");
            }
        });
        return $form;
    };

    // Todo POST por AJAX lleva el token antiforgery en el header (lo valida el filtro global del servidor).
    $(document).ajaxSend(function (evento, xhr, opciones) {
        if ((opciones.type || "").toUpperCase() !== "POST") {
            return;
        }
        var token = SGEI.tokenAntiforgery();
        if (token) {
            xhr.setRequestHeader("RequestVerificationToken", token);
        }
    });

    // Confirmación antes de seguir un link o enviar un formulario.
    $(document).on("click", "a[data-confirmar]", function (evento) {
        if (!window.confirm($(this).data("confirmar"))) {
            evento.preventDefault();
            evento.stopImmediatePropagation();
        }
    });

    $(document).on("submit", "form[data-confirmar]", function (evento) {
        if (!window.confirm($(this).data("confirmar"))) {
            evento.preventDefault();
            evento.stopImmediatePropagation();
        }
    });

    // Spinner al navegar desde el menú (la navegación es el href normal del link).
    $(document).on("click", "a[data-spinner]", function (evento) {
        // Ctrl/Shift/clic medio abren otra pestaña: no hace falta spinner.
        if (evento.ctrlKey || evento.shiftKey || evento.metaKey || evento.which === 2) {
            return;
        }
        SGEI.mostrarSpinner();
    });

    // Al volver con "Atrás" el navegador puede restaurar la página con el spinner abierto.
    $(window).on("pageshow", function () {
        SGEI.ocultarSpinner();
    });

    $(document).on("click", "a[data-cerrar-sesion]", function (evento) {
        evento.preventDefault();
        $("#logoutForm").trigger("submit");
    });

    // DataTables: textos en castellano, 10 filas por página y disposición común a todas las grillas.
    if ($.fn.dataTable) {
        var DataTable = $.fn.dataTable;

        // Un problema de armado de una grilla (por ejemplo, columnas que no coinciden) va a la consola,
        // no a un alert() para el usuario.
        DataTable.ext.errMode = function (settings, codigo, mensaje) {
            if (window.console) {
                window.console.error(mensaje);
            }
        };

        // Fechas como las imprime el servidor (dd/MM/yyyy [HH:mm[:ss]]): sin esto se ordenan como texto.
        var reFecha = /^(\d{1,2})\/(\d{1,2})\/(\d{4})(?:\s+(\d{1,2}):(\d{2})(?::(\d{2}))?)?$/;
        var esVacio = function (d) {
            return d === null || d === undefined || /^\s*-*\s*$/.test(String(d));
        };
        // Direcciones IP: con el separador de miles "." DataTables las tomaría como números.
        var reIp = /^(\d{1,3})\.(\d{1,3})\.(\d{1,3})\.(\d{1,3})$/;
        DataTable.type("ip", {
            detect: {
                allOf: function (d) {
                    return esVacio(d) || reIp.test(String(d).trim());
                },
                oneOf: function (d) {
                    return reIp.test(String(d).trim());
                }
            },
            order: {
                pre: function (d) {
                    var m = reIp.exec(String(d || "").trim());
                    return m ? ((+m[1] * 256 + +m[2]) * 256 + +m[3]) * 256 + +m[4] : -1;
                }
            }
        });

        DataTable.type("fecha-ar", {
            detect: {
                allOf: function (d) {
                    return esVacio(d) || reFecha.test(String(d).trim());
                },
                oneOf: function (d) {
                    return reFecha.test(String(d).trim());
                }
            },
            order: {
                pre: function (d) {
                    var m = reFecha.exec(String(d || "").trim());
                    if (!m) {
                        return -Infinity;
                    }
                    return new Date(+m[3], m[2] - 1, +m[1], +(m[4] || 0), +(m[5] || 0), +(m[6] || 0)).getTime();
                }
            }
        });

        $.extend(true, DataTable.defaults, {
            pageLength: 10,
            lengthMenu: [10, 25, 50, 100],
            // Respeta el orden en que llegan los datos del servidor hasta que el usuario ordene una columna.
            order: [],
            autoWidth: false,
            layout: {
                topStart: "search",
                topEnd: "pageLength",
                bottomStart: "info",
                bottomEnd: "paging"
            },
            language: {
                decimal: ",",
                thousands: ".",
                emptyTable: "No hay datos para mostrar",
                info: "_START_–_END_ de _TOTAL_ registros",
                infoEmpty: "Sin registros",
                infoFiltered: "(de _MAX_ en total)",
                lengthMenu: "_MENU_ por página",
                loadingRecords: "Cargando...",
                processing: "Procesando...",
                search: "",
                searchPlaceholder: "Buscar en la grilla...",
                zeroRecords: "No se encontraron resultados",
                aria: {
                    orderable: "Ordenar por esta columna",
                    orderableReverse: "Invertir el orden de esta columna",
                    orderableRemove: "Quitar el orden de esta columna",
                    paginate: {
                        first: "Primera página",
                        last: "Última página",
                        next: "Página siguiente",
                        previous: "Página anterior"
                    }
                },
                paginate: {
                    first: "«",
                    last: "»",
                    next: "›",
                    previous: "‹"
                }
            }
        });
    }

    /**
     * Grilla con búsqueda, orden y paginación en el navegador (DataTables) sobre una tabla ya
     * renderizada por el servidor: <table class="table sgei-grilla" data-grilla>.
     * La columna de botones lleva <th class="sgei-col-acciones">: no se ordena ni se busca por ella.
     * Con pocas filas (una sola página) se ocultan el paginador y el selector de cantidad.
     */
    SGEI.crearGrilla = function (tabla) {
        var $tabla = $(tabla);
        if (!$.fn.dataTable || $.fn.dataTable.isDataTable(tabla)) {
            return $tabla.DataTable();
        }

        var api = $tabla.DataTable({
            columnDefs: [
                { targets: "sgei-col-acciones", orderable: false, searchable: false, className: "text-end text-nowrap" }
            ]
        });

        var $contenedor = $(api.table().container()).addClass("sgei-grilla-contenedor");
        var minimo = api.settings()[0].aLengthMenu[0];
        var actualizar = function () {
            $contenedor.toggleClass("sgei-grilla-corta", api.page.info().recordsTotal <= minimo);
        };
        api.on("draw", actualizar);
        actualizar();
        return api;
    };

    $(function () {
        $(".modal[data-abrir-al-cargar]").modal("show");

        // Formularios de alta/edición por pasos: <form data-wizard> (BeginForm con data_wizard = "true").
        $("form[data-wizard]").each(function () {
            SGEI.crearWizard(this);
        });

        // Grillas: <table data-grilla>.
        $("table[data-grilla]").each(function () {
            SGEI.crearGrilla(this);
        });

        // Tooltips de los botones de acción (también los de filas que DataTables dibuja después).
        if (window.bootstrap && window.bootstrap.Tooltip) {
            new window.bootstrap.Tooltip(document.body, {
                selector: ".sgei-accion[title]",
                trigger: "hover",
                container: "body"
            });
        }
    });

    // Un tooltip abierto no debe quedar colgado al navegar o abrir un modal desde el botón.
    $(document).on("click", ".sgei-accion", function () {
        $(".tooltip").remove();
    });

})(jQuery, window, document);
