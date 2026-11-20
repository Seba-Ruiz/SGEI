/*
 * Computadora/Index: grilla con todas las computadoras y combo de ubicación como filtro.
 * El combo se ubica junto al buscador de la grilla y filtra en el navegador por la columna
 * Ubicación (sin recargar la página); se combina con la búsqueda y el orden de la grilla.
 */
(function ($) {
    "use strict";

    $(function () {
        var $tabla = $("#grilla-computadoras");
        var $filtro = $("#filtro-ubicacion");
        var $combo = $filtro.find("select");
        if (!$tabla.length || !$.fn.dataTable || !$.fn.dataTable.isDataTable($tabla[0])) {
            return;
        }

        var api = $tabla.DataTable();
        var columna = api.column(".sgei-col-ubicacion");

        // Junto al buscador, en la barra superior de la grilla.
        $(api.table().container()).find(".dt-search").first().after($filtro);

        var filtrar = function () {
            var ubicacion = $combo.val() ? $combo.find("option:selected").text().trim() : "";
            columna.search(ubicacion ? function (valor) {
                return String(valor).trim() === ubicacion;
            } : "").draw();
        };

        $combo.on("change", filtrar);

        // Ubicación preseleccionada desde la URL (?ubicacion=N).
        if ($combo.val()) {
            filtrar();
        }
    });
})(jQuery);
