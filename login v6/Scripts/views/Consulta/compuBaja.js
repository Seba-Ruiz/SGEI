/* Consulta/compuBaja: carga en el modal el motivo de baja de la computadora elegida. */
(function ($) {
    "use strict";

    $(document).on("click", ".js-ver-motivo", function (evento) {
        evento.preventDefault();
        // El contenido llega por AJAX; el modal se abre cuando ya está cargado.
        $("#resultado").load($(this).data("url"), function () {
            $("#motivo").modal("show");
        });
    });
})(jQuery);
