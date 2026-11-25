/*
 * Notificaciones/Index: si mientras se ve la lista llega una notificación nueva o se marcan como leídas desde otra
 * pestaña (SignalR, ver Scripts/app/notificaciones.js), ofrece actualizarla. Si se marcan todas como leídas desde la
 * campana de esta misma pantalla, la recarga.
 */
(function ($) {
    "use strict";

    $(function () {
        var $contenedor = $("[data-notificaciones]");
        if (!$contenedor.length) { return; }

        var ofrecerActualizar = function (texto) {
            var $aviso = $contenedor.find("[data-hay-cambios]");
            if (!$aviso.length) {
                $aviso = $('<div class="alert alert-info d-flex align-items-center justify-content-between gap-2 py-2" role="status" data-hay-cambios>'
                    + '<span><i class="bi bi-bell-fill me-2" aria-hidden="true"></i><span data-texto></span></span>'
                    + '<a class="btn btn-sm btn-primary" data-spinner>Actualizar</a></div>');
                $aviso.find("a").attr("href", window.location.href);
                $contenedor.find(".sgei-notificaciones-filtro").after($aviso);
            }
            $aviso.find("[data-texto]").text(texto);
        };

        $(document).on("sgei:notificacion", function () {
            ofrecerActualizar("Llegaron notificaciones nuevas.");
        });

        $(document).on("sgei:notificaciones-cambiaron", function () {
            if (!$contenedor.find("[data-hay-cambios]").length) {
                ofrecerActualizar("Hubo cambios en tus notificaciones.");
            }
        });

        $(document).on("sgei:notificaciones-leidas", function () {
            window.location.reload();
        });
    });
})(jQuery);
