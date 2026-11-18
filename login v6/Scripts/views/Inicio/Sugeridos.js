/* Inicio/Sugeridos: mantenimientos sugeridos de PCs e impresoras (realizar o posponer). */
(function ($) {
    "use strict";

    // data-accion del link -> modal a abrir, input oculto con el id y label con el nombre.
    var MODALES = {
        "posponer-pc":             { modal: "#coso",       id: "#id",       nombre: "#nombre_p" },
        "posponer-impresora":      { modal: "#coso1",      id: "#id1",      nombre: "#nombre_i" },
        "mantenimiento-pc":        { modal: "#manteCompu", id: "#id_pc",    nombre: "#nombre_pc" },
        "mantenimiento-impresora": { modal: "#manteImpre", id: "#id_impre", nombre: "#nombre_impre" }
    };

    $(document).on("click", ".js-sugerido", function (evento) {
        evento.preventDefault();
        var $link = $(this);
        var destino = MODALES[$link.data("accion")];
        if (!destino) {
            return;
        }
        $(destino.id).val($link.data("id"));
        $(destino.nombre).text($link.data("nombre"));
        $(destino.modal).modal("show");
    });

    $(function () {
        $("#datatable").DataTable();
    });
})(jQuery);
