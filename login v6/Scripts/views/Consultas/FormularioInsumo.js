/*
 * Consultas/FormularioInsumo: según el tipo de equipo carga sus equipos y sus insumos (con el stock entre paréntesis);
 * el n° de serie solo se pide para computadoras. Datos en <script id="datos-formulario">.
 */
(function ($) {
    "use strict";

    $(function () {
        var datos = JSON.parse($("#datos-formulario").text() || "{}");
        var $tipo = $("#Tipo");
        var $equipo = $("#EquipoId");
        var $insumo = $("#InsumoId");

        var llenar = function ($combo, opciones, textoVacio) {
            var valor = String($combo.val() || $combo.attr("data-valor") || "");
            $combo.empty().append($("<option>").val("").text(textoVacio));
            $.each(opciones || [], function (i, o) {
                var texto = o.stock !== undefined ? o.texto + " (" + o.stock + ")" : o.texto;
                $combo.append($("<option>").val(o.id).text(texto));
            });
            if ($combo.find("option[value='" + valor + "']").length) {
                $combo.val(valor);
            }
            $combo.removeAttr("data-valor").trigger("change.select2");
        };

        var aplicarTipo = function (cambioDeTipo) {
            var tipo = $tipo.val();
            if (cambioDeTipo) {
                $equipo.val("");
                $insumo.val("");
            }
            llenar($equipo, datos.equipos[tipo], "Elegí el equipo…");
            llenar($insumo, datos.catalogos[tipo], "Elegí el insumo…");
            $("[data-solo-tipo]").each(function () {
                $(this).toggle($(this).attr("data-solo-tipo") === tipo);
            });
            $("#NroSerie").prop("required", tipo === "computadora");
        };

        $tipo.on("change", function () { aplicarTipo(true); });
        aplicarTipo(false);
    });
})(jQuery);
