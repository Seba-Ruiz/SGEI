/* Computadora/Preventivo: tabla de preventivos y modal con la descripción. */
(function ($) {
    "use strict";

    $(document).on("click", ".js-ver-descripcion", function (evento) {
        evento.preventDefault();
        // .text() evita que el contenido de la descripción se interprete como HTML.
        $("#descri").text($(this).data("descripcion"));
        $("#descripcion").modal("show");
    });

    $(function () {
        $("#datatable").DataTable();
    });
})(jQuery);
