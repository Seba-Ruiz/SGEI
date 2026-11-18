/* Ingresos/Index (impresoras): abre el modal para cargar un ingreso del insumo elegido. */
(function ($) {
    "use strict";

    $(document).on("click", ".js-agregar-ingreso", function (evento) {
        evento.preventDefault();
        $("#id").val($(this).data("id"));
        $("#coso").modal("show");
    });
})(jQuery);
