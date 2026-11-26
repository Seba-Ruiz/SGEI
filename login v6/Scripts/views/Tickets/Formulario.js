/*
 * Tickets/Formulario: el combo de equipos se llena según el tipo elegido (datos en <script id="datos-formulario">);
 * con "Sin equipo" se oculta. Con un equipo elegido aparece el incidente que se carga automáticamente en ese equipo al
 * guardar: salvo en impresoras (usan el título del ticket) se elige del catálogo de incidentes de ese tipo. Si el
 * ticket ya generó el incidente de ese equipo, solo se avisa. La descripción usa Summernote (SGEI.crearEditor, en sgei.js).
 */
(function ($) {
    "use strict";

    $(function () {
        var datos = JSON.parse($("#datos-formulario").text() || "{}");
        var $tipo = $("#TipoEquipo");
        var $equipo = $("#EquipoId");
        var $bloque = $("[data-con-equipo]");
        var $incidente = $("[data-con-incidente]");
        var $catalogo = $("#IncidenteCatalogoId");
        var $bloqueCatalogo = $("[data-incidente-catalogo]");
        var $ayuda = $("[data-incidente-ayuda]");
        var generados = datos.generados || [];

        var llenar = function ($combo, opciones, valor, vacio) {
            $combo.empty().append($("<option>").val("").text(vacio));
            $.each(opciones || [], function (i, o) {
                $combo.append($("<option>").val(o.id).text(o.texto));
            });
            $combo.val($combo.find("option[value='" + valor + "']").length ? valor : "").trigger("change.select2");
        };

        var aplicarIncidente = function () {
            var tipo = $tipo.val();
            var equipo = $equipo.val();
            if (!tipo || !equipo) {
                $incidente.prop("hidden", true);
                $catalogo.prop("required", false);
                return;
            }
            var yaGenerado = generados.indexOf(tipo + ":" + equipo) >= 0;
            var esImpresora = tipo === "impresora";
            var pideCatalogo = !yaGenerado && !esImpresora;
            $incidente.prop("hidden", false);
            $bloqueCatalogo.prop("hidden", !pideCatalogo);
            $catalogo.prop("required", pideCatalogo);
            $ayuda.text(yaGenerado
                ? "Este ticket ya cargó un incidente en este equipo: se puede ver en el detalle del ticket."
                : esImpresora
                    ? "Al guardar se carga un incidente en la impresora con el título del ticket, la fecha de hoy y la acción «Pendiente»."
                    : "Al guardar se carga este incidente en el equipo, con la fecha de hoy y la acción «Pendiente». Después se sigue desde Consultas › Incidentes.");
        };

        var aplicarTipo = function (cambioDeTipo) {
            var tipo = $tipo.val();
            var valor = cambioDeTipo ? "" : String($equipo.attr("data-valor") || "");
            llenar($equipo, (datos.equipos || {})[tipo], valor, "Elegí el equipo…");
            $equipo.removeAttr("data-valor");
            var valorCatalogo = cambioDeTipo ? "" : String($catalogo.attr("data-valor") || "");
            llenar($catalogo, (datos.catalogos || {})[tipo], valorCatalogo, "");
            $catalogo.removeAttr("data-valor");
            $bloque.toggle(!!tipo);
            $equipo.prop("required", !!tipo);
            aplicarIncidente();
        };

        $tipo.on("change", function () { aplicarTipo(true); });
        $equipo.on("change", aplicarIncidente);
        aplicarTipo(false);
    });
})(jQuery);
