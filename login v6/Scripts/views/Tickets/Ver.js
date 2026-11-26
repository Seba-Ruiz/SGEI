/*
 * Tickets/Ver: cambio de estado con el combo (se guarda al elegir).
 */
(function ($) {
    "use strict";

    $(function () {
        $("#estado-ticket").on("change", function () {
            var $combo = $(this);
            if ($combo.val() === $combo.attr("data-estado-actual")) { return; }
            SGEI.mostrarSpinner();
            $("#formulario-estado").trigger("submit");
        });

        // La edición en el lugar de los comentarios propios está en Scripts/app/sgei.js (la comparten las observaciones de incidentes).
    });
})(jQuery);
