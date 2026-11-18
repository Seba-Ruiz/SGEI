/* Consulta/compuBaja: carga en el modal el motivo de baja de la computadora elegida. */
(function ($) {
    "use strict";

    $(document).on("click", ".js-ver-motivo", function (evento) {
        evento.preventDefault();
        $("#resultado").load($(this).data("url"));
        $("#motivo").modal("show");
    });
})(jQuery);
