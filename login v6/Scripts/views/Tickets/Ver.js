/*
 * Tickets/Ver: cambio de estado con el combo (se guarda al elegir) y edición en el lugar de los comentarios propios
 * (el editor Summernote se crea recién al tocar "Editar").
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

        $(document).on("click", "[data-editar-comentario]", function () {
            var id = $(this).attr("data-editar-comentario");
            var $mensaje = $("#comentario-" + id);
            var $form = $mensaje.find("[data-edicion-comentario]");
            $mensaje.find("[data-texto-comentario], .sgei-mensaje-acciones").attr("hidden", true);
            $form.removeAttr("hidden");
            var $editor = SGEI.crearEditor($form.find("textarea"));
            $editor.data("original", $editor.data("original") || $editor.summernote("code"));
            $editor.summernote("focus");
        });

        $(document).on("click", "[data-cancelar-edicion]", function () {
            var $mensaje = $(this).closest(".sgei-mensaje");
            var $form = $mensaje.find("[data-edicion-comentario]");
            var $editor = $form.find("textarea");
            if ($editor.data("original") !== undefined) { $editor.summernote("code", $editor.data("original")); }
            $form.attr("hidden", true);
            $mensaje.find("[data-texto-comentario], .sgei-mensaje-acciones").removeAttr("hidden");
        });
    });
})(jQuery);
