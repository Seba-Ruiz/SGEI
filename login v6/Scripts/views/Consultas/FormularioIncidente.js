/*
 * Consultas/FormularioIncidente: según el tipo de equipo elegido carga sus equipos y su catálogo de incidentes,
 * y muestra los campos que corresponden (las impresoras describen el incidente y pueden llevar foto).
 * Los datos vienen en <script id="datos-formulario"> (ConsultasController.FormularioIncidente).
 */
(function ($) {
    "use strict";

    $(function () {
        var datos = JSON.parse($("#datos-formulario").text() || "{}");
        var $tipo = $("#Tipo");
        var $equipo = $("#EquipoId");
        var $incidente = $("#IncidenteId");
        var $accion = $("#Accion");

        var llenarCombo = function ($combo, opciones, textoVacio) {
            var valor = String($combo.val() || $combo.attr("data-valor") || "");
            $combo.empty().append($("<option>").val("").text(textoVacio));
            $.each(opciones || [], function (i, o) {
                $combo.append($("<option>").val(o.id).text(o.texto));
            });
            if ($combo.find("option[value='" + valor + "']").length) {
                $combo.val(valor);
            }
            $combo.removeAttr("data-valor").trigger("change.select2");
        };

        var aplicarTipo = function (cambioDeTipo) {
            var tipo = $tipo.val();
            var esImpresora = tipo === "impresora";
            if (cambioDeTipo) {
                $equipo.val("");
                $incidente.val("");
            }
            llenarCombo($equipo, datos.equipos[tipo], "Elegí el equipo…");
            llenarCombo($incidente, datos.catalogos[tipo], "Elegí el incidente…");

            $("[data-solo-tipo]").each(function () {
                $(this).toggle($(this).attr("data-solo-tipo") === tipo);
            });
            $("[data-salvo-tipo]").each(function () {
                $(this).toggle($(this).attr("data-salvo-tipo") !== tipo);
            });
            $("#IncidenteTexto").prop("required", esImpresora);
            $incidente.prop("required", !esImpresora);
            if (datos.largoAccion && datos.largoAccion[tipo]) {
                $accion.attr("maxlength", datos.largoAccion[tipo]);
            }
        };

        $tipo.on("change", function () { aplicarTipo(true); });
        aplicarTipo(false);

        // La fecha de la acción no puede ser anterior a la del incidente.
        $("#FechaIncidente").on("change", function () {
            $("#FechaAccion").attr("min", this.value || null);
        }).trigger("change");
    });
})(jQuery);
