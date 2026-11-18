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

    // DataTables: textos en castellano y 25 filas por página en todas las tablas.
    if ($.fn.dataTable) {
        $.extend(true, $.fn.dataTable.defaults, {
            pageLength: 25,
            language: {
                decimal: ",",
                thousands: ".",
                emptyTable: "No hay datos para mostrar",
                info: "Mostrando _START_ a _END_ de _TOTAL_ registros",
                infoEmpty: "Sin registros",
                infoFiltered: "(filtrado de _MAX_ registros)",
                lengthMenu: "Mostrar _MENU_ registros",
                loadingRecords: "Cargando...",
                processing: "Procesando...",
                search: "Buscar:",
                zeroRecords: "No se encontraron resultados",
                aria: {
                    orderable: "Ordenar por esta columna",
                    orderableReverse: "Invertir el orden de esta columna"
                },
                paginate: {
                    first: "Primera",
                    last: "Última",
                    next: "Siguiente",
                    previous: "Anterior"
                }
            }
        });
    }

    $(function () {
        $(".modal[data-abrir-al-cargar]").modal("show");

        // Formularios de alta/edición por pasos: <form data-wizard> (BeginForm con data_wizard = "true").
        $("form[data-wizard]").each(function () {
            SGEI.crearWizard(this);
        });
    });

})(jQuery, window, document);
