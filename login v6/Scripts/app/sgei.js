/*
 * SGEI - comportamiento común a todas las pantallas que usan el layout.
 * Las vistas no llevan <script> inline: lo propio de cada una va en
 * Scripts/views/<Controlador>/<vista>.js y se carga después de este archivo.
 *
 * Atributos declarativos que maneja este archivo:
 *   data-confirmar="mensaje"   en <a> o <form>: pide confirmación antes de seguir.
 *   data-spinner               en <a>: muestra el spinner "Espere..." al navegar.
 *   data-abrir-al-cargar       en un .modal: lo abre cuando carga la página.
 *   data-cerrar-sesion         en <a>: envía el formulario #logoutForm.
 *   data-wizard="alta|edicion" en <form>: arma el formulario por pasos (h3 + section); el último paso
 *                              muestra el resumen de lo cargado o de los cambios (data-wizard-resumen).
 *   data-grilla                en <table>: búsqueda, orden y paginación en el navegador.
 *   data-filtro-grilla="#tabla" en <select>: filtra esa grilla por la columna data-filtro-columna
 *                              (selector del <th>) con el texto de la opción elegida. Con data-filtro-lista
 *                              la celda puede tener varios valores separados por coma.
 *   data-buscable              en <select>: combo con buscador (Select2).
 *   .sgei-grilla-fechas[data-filtro-grilla] con inputs [data-desde]/[data-hasta]: filtra la grilla por un
 *                              rango de fechas de la columna data-filtro-columna.
 */
(function ($, window, document) {
    "use strict";

    var SGEI = window.SGEI = window.SGEI || {};

    /** Token antiforgery renderizado por el layout en #antiforgery-ajax. */
    SGEI.tokenAntiforgery = function () {
        return $("#antiforgery-ajax input[name='__RequestVerificationToken']").val();
    };

    SGEI.mostrarSpinner = function () {
        $("#spiner").modal("show");
    };

    SGEI.ocultarSpinner = function () {
        $("#spiner").modal("hide");
    };

    var escapar = function (texto) {
        return $("<div>").text(texto).html();
    };

    /** "Ingrese nombre de la PC" -> "Nombre de la PC"; "Seleccione la ubicación" -> "Ubicación". */
    var nombreDeCampo = function (etiqueta) {
        var texto = String(etiqueta).replace(/\s+/g, " ").trim().replace(/:$/, "");
        texto = texto.replace(/^(ingrese|seleccione|elija|indique)\s+((el|la|los|las)\s+)?/i, "");
        return texto.charAt(0).toUpperCase() + texto.slice(1);
    };

    /** Valor que se ve en un campo: el actual o el que traía la página al cargarse (inicial = true). */
    var valorDeCampo = function (campo, inicial) {
        if (campo.tagName === "SELECT") {
            var elegida = null;
            for (var i = 0; i < campo.options.length; i++) {
                if (inicial ? campo.options[i].defaultSelected : campo.options[i].selected) {
                    elegida = campo.options[i];
                    break;
                }
            }
            // Sin "selected" en el HTML, el navegador muestra la primera opción.
            if (!elegida && inicial && campo.options.length) {
                elegida = campo.options[0];
            }
            return elegida ? elegida.text.trim() : "";
        }
        return String(inicial ? campo.defaultValue : campo.value).trim();
    };

    /**
     * Resumen del último paso del wizard: los datos de todos los pasos anteriores, agrupados por paso.
     * En un alta muestra lo que se va a guardar; en una edición, el valor anterior y el nuevo de cada campo,
     * con los cambios resaltados.
     */
    SGEI.armarResumenWizard = function ($pasos, titulos, $destino, modo) {
        var edicion = modo === "edicion";
        var cambios = 0;
        var faltantes = 0;
        var filas = [];

        $pasos.each(function (indice) {
            if ($(this).find("[data-wizard-resumen]").length) {
                return;
            }
            var campos = $(this).find("input, select, textarea").filter(function () {
                return this.type !== "hidden" && this.type !== "checkbox" && this.type !== "radio" && this.name;
            });
            if (!campos.length) {
                return;
            }
            filas.push('<tr class="sgei-resumen-paso"><th colspan="' + (edicion ? 3 : 2) + '">' + escapar(titulos[indice] || "") + "</th></tr>");

            campos.each(function () {
                var etiqueta = $(this).closest(".mb-3").find("label").first().text() || this.name;
                var ahora = valorDeCampo(this, false);
                var antes = valorDeCampo(this, true);
                var cambio = edicion && ahora !== antes;
                var falta = this.required && !ahora;
                var celda = function (valor, clase) {
                    return '<td class="' + clase + '">' + (valor ? escapar(valor) : '<span class="text-muted">—</span>') + "</td>";
                };
                cambios += cambio ? 1 : 0;
                faltantes += falta ? 1 : 0;

                filas.push('<tr class="' + (cambio ? "sgei-resumen-cambio" : "") + (falta ? " sgei-resumen-falta" : "") + '">'
                    + "<th scope=\"row\">" + escapar(nombreDeCampo(etiqueta))
                    + (cambio ? ' <span class="badge text-bg-warning">Modificado</span>' : "")
                    + (falta ? ' <span class="badge text-bg-danger">Sin completar</span>' : "")
                    + "</th>"
                    + (edicion ? celda(antes, "sgei-resumen-antes") : "")
                    + celda(ahora, "sgei-resumen-ahora")
                    + "</tr>");
            });
        });

        var aviso;
        if (edicion) {
            aviso = cambios
                ? '<div class="alert alert-warning py-2"><i class="bi bi-pencil-square me-2"></i><strong>' + cambios + (cambios === 1 ? " cambio" : " cambios")
                    + "</strong> respecto de lo que estaba cargado. Revisalos y presioná <strong>Finalizar</strong> para guardarlos.</div>"
                : '<div class="alert alert-secondary py-2"><i class="bi bi-info-circle me-2"></i>No modificaste ningún dato. '
                    + "Si presionás <strong>Finalizar</strong> se guarda igual que estaba.</div>";
        } else {
            aviso = '<div class="alert alert-info py-2"><i class="bi bi-clipboard-check me-2"></i>Revisá los datos que se van a guardar. '
                + "Si algo no está bien, volvé con <strong>Anterior</strong>; si está todo bien, presioná <strong>Finalizar</strong>.</div>";
        }
        if (faltantes) {
            aviso += '<div class="alert alert-danger py-2"><i class="bi bi-exclamation-triangle me-2"></i>'
                + (faltantes === 1 ? "Hay 1 campo obligatorio sin completar." : "Hay " + faltantes + " campos obligatorios sin completar.") + "</div>";
        }

        var encabezado = edicion
            ? '<tr><th scope="col">Campo</th><th scope="col">Antes</th><th scope="col">Ahora</th></tr>'
            : '<tr><th scope="col">Campo</th><th scope="col">Valor</th></tr>';

        $destino.html(aviso
            + '<div class="table-responsive"><table class="table table-sm sgei-resumen' + (edicion ? " sgei-resumen-edicion" : "") + '">'
            + "<thead>" + encabezado + "</thead><tbody>" + filas.join("") + "</tbody></table></div>");
    };

    /**
     * Wizard por pasos (jquery.steps) sobre un formulario: cada paso es un <h3> + <section>
     * dentro del primer <div> del form. El formulario indica si es un alta o una edición
     * (data-wizard="alta" | "edicion"); el último paso lleva <div data-wizard-resumen> y ahí se arma
     * el resumen al llegar. "Finalizar" envía el formulario (una sola vez).
     * Reemplaza a assets/pages/jquery.wizard-init.js, que corría en todas las páginas.
     */
    SGEI.crearWizard = function (selectorFormulario) {
        var $form = $(selectorFormulario);
        var $wizard = $form.children("div").first();
        var modo = $form.attr("data-wizard");
        var titulos = $wizard.children("h3").map(function () {
            return $(this).text().trim();
        }).get();
        var enviando = false;

        $wizard.steps({
            headerTag: "h3",
            bodyTag: "section",
            // Con "fade" un paso se oculta antes de mostrar el siguiente: cada paso puede tener su propio alto.
            transitionEffect: "fade",
            transitionEffectSpeed: 150,
            labels: {
                current: "paso actual:",
                pagination: "Paginación",
                finish: "Finalizar",
                next: "Siguiente",
                previous: "Anterior",
                loading: "Cargando..."
            },
            onStepChanged: function (evento, actual) {
                var $pasos = $wizard.find("section");
                var $resumen = $pasos.eq(actual).find("[data-wizard-resumen]");
                if ($resumen.length) {
                    SGEI.armarResumenWizard($pasos, titulos, $resumen, modo);
                }
            },
            // No se avanza (ni se termina) con un nombre de equipo repetido en el paso (data-nombre-unico).
            onStepChanging: function (evento, actual, siguiente) {
                return siguiente < actual || SGEI.nombresDisponibles($wizard.find("section").eq(actual));
            },
            onFinishing: function () {
                return !enviando && SGEI.nombresDisponibles($form);
            },
            onFinished: function () {
                enviando = true;
                SGEI.mostrarSpinner();
                $form.trigger("submit");
            }
        });
        return $form;
    };

    /*
     * Nombre de equipo único: <input data-nombre-unico="computadora|camara|switch|escaner|telefono"
     * data-nombre-id="id del equipo (0 en un alta)" data-nombre-url="Validaciones/NombreEquipo">.
     * Mientras se escribe consulta al servidor; si ya existe otro equipo con ese nombre (sin importar mayúsculas,
     * acentos ni espacios) marca el campo en rojo con el mensaje y no deja avanzar el wizard ni enviar el formulario.
     * El servidor lo vuelve a controlar al guardar.
     */
    var verificarNombre = function ($input) {
        var valor = $.trim($input.val());
        var $error = $input.nextAll("[data-nombre-error]").first();
        if (!$error.length) {
            $error = $('<div class="invalid-feedback d-block" data-nombre-error hidden></div>').insertAfter($input);
        }
        var marcar = function (mensaje) {
            $input.data("nombreEstado", mensaje ? "repetido" : "ok").toggleClass("is-invalid", !!mensaje);
            $input[0].setCustomValidity(mensaje || "");
            $error.text(mensaje || "").prop("hidden", !mensaje);
        };
        if (!valor) { marcar(null); return; }
        if ($input.data("nombreConsultado") === valor) { return; }

        var anterior = $input.data("nombrePedido");
        if (anterior) { anterior.abort(); }
        $input.data("nombreEstado", "consultando");
        var pedido = $.ajax({
            url: $input.attr("data-nombre-url"),
            data: { tipo: $input.attr("data-nombre-unico"), nombre: valor, id: $input.attr("data-nombre-id") || 0 },
            cache: false
        })
            .done(function (r) {
                $input.data("nombreConsultado", valor);
                marcar(r && r.ok === false ? r.mensaje : null);
            })
            .fail(function (xhr, estado) {
                // Si no se pudo consultar, no se frena al usuario: el servidor lo controla al guardar.
                if (estado !== "abort") { marcar(null); }
            })
            .always(function () { $input.removeData("nombrePedido"); });
        $input.data("nombrePedido", pedido);
    };

    var esperas = {};
    $(document).on("input", "input[data-nombre-unico]", function () {
        var $input = $(this);
        var clave = this.id || this.name;
        $input.data("nombreEstado", "consultando");
        window.clearTimeout(esperas[clave]);
        esperas[clave] = window.setTimeout(function () { verificarNombre($input); }, 350);
    });
    $(document).on("change blur", "input[data-nombre-unico]", function () {
        window.clearTimeout(esperas[this.id || this.name]);
        verificarNombre($(this));
    });

    /** true si ningún nombre del contenedor está repetido (o todavía consultándose); si no, marca el primero. */
    SGEI.nombresDisponibles = function (contenedor) {
        var ok = true;
        $(contenedor).find("input[data-nombre-unico]").each(function () {
            var $input = $(this);
            var estado = $input.data("nombreEstado");
            if (estado === "consultando") {
                verificarNombre($input);
                ok = false;
            } else if (estado === "repetido") {
                ok = false;
            }
            if (!ok) {
                $input.trigger("focus");
                return false;
            }
        });
        return ok;
    };

    $(document).on("submit", "form", function (evento) {
        if (!SGEI.nombresDisponibles(this)) {
            evento.preventDefault();
            evento.stopImmediatePropagation();
            SGEI.ocultarSpinner();
        }
    });

    // Todo POST por AJAX lleva el token antiforgery en el header (lo valida el filtro global del servidor).
    $(document).ajaxSend(function (evento, xhr, opciones) {
        if ((opciones.type || "").toUpperCase() !== "POST") {
            return;
        }
        var token = SGEI.tokenAntiforgery();
        if (token) {
            xhr.setRequestHeader("RequestVerificationToken", token);
        }
    });

    // Confirmación antes de seguir un link o enviar un formulario.
    $(document).on("click", "a[data-confirmar]", function (evento) {
        if (!window.confirm($(this).data("confirmar"))) {
            evento.preventDefault();
            evento.stopImmediatePropagation();
        }
    });

    $(document).on("submit", "form[data-confirmar]", function (evento) {
        if (!window.confirm($(this).data("confirmar"))) {
            evento.preventDefault();
            evento.stopImmediatePropagation();
        }
    });

    // Spinner al navegar desde el menú (la navegación es el href normal del link).
    $(document).on("click", "a[data-spinner]", function (evento) {
        // Ctrl/Shift/clic medio abren otra pestaña: no hace falta spinner.
        if (evento.ctrlKey || evento.shiftKey || evento.metaKey || evento.which === 2) {
            return;
        }
        SGEI.mostrarSpinner();
    });

    // Al volver con "Atrás" el navegador puede restaurar la página con el spinner abierto.
    $(window).on("pageshow", function () {
        SGEI.ocultarSpinner();
    });

    $(document).on("click", "a[data-cerrar-sesion]", function (evento) {
        evento.preventDefault();
        $("#logoutForm").trigger("submit");
    });

    // DataTables: textos en castellano, 10 filas por página y disposición común a todas las grillas.
    if ($.fn.dataTable) {
        var DataTable = $.fn.dataTable;

        // Un problema de armado de una grilla (por ejemplo, columnas que no coinciden) va a la consola,
        // no a un alert() para el usuario.
        DataTable.ext.errMode = function (settings, codigo, mensaje) {
            if (window.console) {
                window.console.error(mensaje);
            }
        };

        // Fechas como las imprime el servidor (dd/MM/yyyy [HH:mm[:ss]]): sin esto se ordenan como texto.
        var reFecha = /^(\d{1,2})\/(\d{1,2})\/(\d{4})(?:\s+(\d{1,2}):(\d{2})(?::(\d{2}))?)?$/;
        var esVacio = function (d) {
            return d === null || d === undefined || /^\s*-*\s*$/.test(String(d));
        };
        // Direcciones IP: con el separador de miles "." DataTables las tomaría como números.
        var reIp = /^(\d{1,3})\.(\d{1,3})\.(\d{1,3})\.(\d{1,3})$/;
        DataTable.type("ip", {
            detect: {
                allOf: function (d) {
                    return esVacio(d) || reIp.test(String(d).trim());
                },
                oneOf: function (d) {
                    return reIp.test(String(d).trim());
                }
            },
            order: {
                pre: function (d) {
                    var m = reIp.exec(String(d || "").trim());
                    return m ? ((+m[1] * 256 + +m[2]) * 256 + +m[3]) * 256 + +m[4] : -1;
                }
            }
        });

        DataTable.type("fecha-ar", {
            detect: {
                allOf: function (d) {
                    return esVacio(d) || reFecha.test(String(d).trim());
                },
                oneOf: function (d) {
                    return reFecha.test(String(d).trim());
                }
            },
            order: {
                pre: function (d) {
                    var m = reFecha.exec(String(d || "").trim());
                    if (!m) {
                        return -Infinity;
                    }
                    return new Date(+m[3], m[2] - 1, +m[1], +(m[4] || 0), +(m[5] || 0), +(m[6] || 0)).getTime();
                }
            }
        });

        $.extend(true, DataTable.defaults, {
            pageLength: 10,
            lengthMenu: [10, 25, 50, 100],
            // Respeta el orden en que llegan los datos del servidor hasta que el usuario ordene una columna.
            order: [],
            autoWidth: false,
            layout: {
                topStart: "search",
                topEnd: "pageLength",
                bottomStart: "info",
                bottomEnd: "paging"
            },
            language: {
                decimal: ",",
                thousands: ".",
                emptyTable: "No hay datos para mostrar",
                info: "_START_–_END_ de _TOTAL_ registros",
                infoEmpty: "Sin registros",
                infoFiltered: "(de _MAX_ en total)",
                lengthMenu: "_MENU_ por página",
                loadingRecords: "Cargando...",
                processing: "Procesando...",
                search: "",
                searchPlaceholder: "Buscar en la grilla...",
                zeroRecords: "No se encontraron resultados",
                aria: {
                    orderable: "Ordenar por esta columna",
                    orderableReverse: "Invertir el orden de esta columna",
                    orderableRemove: "Quitar el orden de esta columna",
                    paginate: {
                        first: "Primera página",
                        last: "Última página",
                        next: "Página siguiente",
                        previous: "Página anterior"
                    }
                },
                paginate: {
                    first: "«",
                    last: "»",
                    next: "›",
                    previous: "‹"
                }
            }
        });
    }

    /**
     * Grilla con búsqueda, orden y paginación en el navegador (DataTables) sobre una tabla ya
     * renderizada por el servidor: <table class="table sgei-grilla" data-grilla>.
     * La columna de botones lleva <th class="sgei-col-acciones">: no se ordena ni se busca por ella.
     * Con pocas filas (una sola página) se ocultan el paginador y el selector de cantidad.
     */
    SGEI.crearGrilla = function (tabla) {
        var $tabla = $(tabla);
        if (!$.fn.dataTable || $.fn.dataTable.isDataTable(tabla)) {
            return $tabla.DataTable();
        }

        var api = $tabla.DataTable({
            columnDefs: [
                { targets: "sgei-col-acciones", orderable: false, searchable: false, className: "text-end text-nowrap" }
            ]
        });

        var $contenedor = $(api.table().container()).addClass("sgei-grilla-contenedor");
        var minimo = api.settings()[0].aLengthMenu[0];
        var actualizar = function () {
            $contenedor.toggleClass("sgei-grilla-corta", api.page.info().recordsTotal <= minimo);
        };
        api.on("draw", actualizar);
        actualizar();
        return api;
    };

    /**
     * Combo que filtra una grilla por una columna, en el navegador (sin recargar la página):
     *   <div class="sgei-grilla-filtro">
     *     <select data-filtro-grilla="#grilla" data-filtro-columna=".sgei-col-ubicacion">...</select>
     *   </div>
     * La opción con valor vacío ("Todas...") quita el filtro. El contenedor .sgei-grilla-filtro se ubica
     * junto al buscador de la grilla. Si el combo llega con una opción elegida (?ubicacion=N), se aplica.
     */
    SGEI.crearFiltroGrilla = function (combo) {
        var $combo = $(combo);
        var $tabla = $($combo.data("filtroGrilla"));
        if (!$tabla.length || !$.fn.dataTable || !$.fn.dataTable.isDataTable($tabla[0])) {
            return;
        }

        var api = $tabla.DataTable();
        var columna = api.column($combo.data("filtroColumna"));
        var $contenedor = $combo.closest(".sgei-grilla-filtro");
        ubicarJuntoAlBuscador(api, $contenedor.length ? $contenedor : $combo);

        // DataTables le quita los acentos a los datos de búsqueda ("ESTERILIZACIÓN" -> "ESTERILIZACION"):
        // se comparan los dos textos sin acentos.
        var sinAcentos = function (texto) {
            return String(texto).normalize("NFD").replace(/[̀-ͯ]/g, "").trim();
        };

        // data-filtro-lista: la celda tiene varios valores separados por coma ("Franco Ríos, María Pérez");
        // la fila pasa si alguno coincide.
        var esLista = $combo.is("[data-filtro-lista]");

        var filtrar = function () {
            var texto = $combo.val() ? sinAcentos($combo.find("option:selected").text()) : "";
            // Coincidencia exacta: "ADMISION" no trae las filas de "ADMISION NUEVA".
            columna.search(texto ? function (valor) {
                if (esLista) {
                    return String(valor).split(",").some(function (v) { return sinAcentos(v) === texto; });
                }
                return sinAcentos(valor) === texto;
            } : "").draw();
        };

        $combo.on("change", filtrar);
        if ($combo.val()) {
            filtrar();
        }
    };

    /** Pone un filtro en la barra de la grilla: después del buscador y de los filtros que ya estén ahí, en orden. */
    var ubicarJuntoAlBuscador = function (api, $elemento) {
        var $barra = $(api.table().container()).find(".dt-search").first().parent();
        var $ancla = $barra.children(".sgei-grilla-filtro, .sgei-grilla-fechas").last();
        ($ancla.length ? $ancla : $barra.children(".dt-search").first()).after($elemento);
    };

    /**
     * Filtro por rango de fechas sobre una columna con fechas dd/MM/yyyy:
     *   <div class="sgei-grilla-fechas" data-filtro-grilla="#grilla" data-filtro-columna=".col-fecha">
     *     <input type="date" data-desde> <input type="date" data-hasta>
     *   </div>
     * Cualquiera de las dos puntas puede quedar vacía.
     */
    var reFechaCelda = /(\d{1,2})\/(\d{1,2})\/(\d{4})/;
    SGEI.crearFiltroFechas = function (contenedor) {
        var $contenedor = $(contenedor);
        var $tabla = $($contenedor.data("filtroGrilla"));
        if (!$tabla.length || !$.fn.dataTable || !$.fn.dataTable.isDataTable($tabla[0])) {
            return;
        }

        var api = $tabla.DataTable();
        var columna = api.column($contenedor.data("filtroColumna"));
        var $desde = $contenedor.find("[data-desde]");
        var $hasta = $contenedor.find("[data-hasta]");
        ubicarJuntoAlBuscador(api, $contenedor);

        // "2026-11-20" (input date) o "20/11/2026" (celda) -> 20261120, para comparar como número.
        var desdeInput = function (v) { return v ? +v.replace(/-/g, "") : null; };
        var desdeCelda = function (texto) {
            var m = reFechaCelda.exec(String(texto));
            return m ? +(m[3] + ("0" + m[2]).slice(-2) + ("0" + m[1]).slice(-2)) : null;
        };

        var filtrar = function () {
            var desde = desdeInput($desde.val());
            var hasta = desdeInput($hasta.val());
            $hasta.attr("min", $desde.val() || null);
            $desde.attr("max", $hasta.val() || null);
            columna.search(desde || hasta ? function (valor) {
                var fecha = desdeCelda(valor);
                if (fecha === null) {
                    return false;
                }
                return (desde === null || fecha >= desde) && (hasta === null || fecha <= hasta);
            } : "").draw();
        };

        $desde.add($hasta).on("change", filtrar);
        if ($desde.val() || $hasta.val()) {
            filtrar();
        }
    };

    /** Combo con buscador (Select2, tema Bootstrap 5): <select data-buscable>. */
    SGEI.crearComboBuscable = function (combo) {
        var $combo = $(combo);
        if (!$.fn.select2 || $combo.hasClass("select2-hidden-accessible")) {
            return;
        }
        var $modal = $combo.closest(".modal");
        $combo.select2({
            theme: "bootstrap-5",
            width: "100%",
            // Dentro de un modal la lista tiene que colgar del modal para poder escribir en el buscador.
            dropdownParent: $modal.length ? $modal : $(document.body),
            language: {
                noResults: function () { return "No se encontraron resultados"; },
                searching: function () { return "Buscando..."; }
            }
        });
    };

    // Select2 4.1 con jQuery 3.6+ no pone el cursor en el buscador al abrir la lista.
    // Se espera un ciclo: el evento llega antes de que Select2 termine de armar la lista.
    $(document).on("select2:open", function () {
        window.setTimeout(function () {
            var buscador = document.querySelector(".select2-container--open .select2-search__field");
            if (buscador) {
                buscador.focus();
            }
        }, 0);
    });

    /**
     * Aviso de éxito (toast verde centrado arriba, Views/Shared/_Avisos.cshtml). El servidor lo deja después de
     * guardar; desde JavaScript se puede mostrar uno con SGEI.mostrarAviso("Se guardó ...").
     */
    var mostrarToast = function (toast) {
        if (window.bootstrap && window.bootstrap.Toast) {
            window.bootstrap.Toast.getOrCreateInstance(toast, { delay: 5000 }).show();
        }
    };

    /**
     * Editor Summernote sobre un <textarea data-editor> (requiere el bundle ~/bundles/summernote en la vista).
     * data-editor-alto: alto en px · data-editor-requerido: no deja enviar el formulario vacío.
     * Las imágenes se pegan dentro del texto (hasta 1 MB cada una); el servidor sanea el HTML al guardar.
     */
    SGEI.MAXIMO_IMAGEN_EDITOR = 1024 * 1024;

    SGEI.crearEditor = function (textarea) {
        var $t = $(textarea);
        if (!$.fn.summernote || $t.data("summernote")) { return $t; }

        $t.summernote({
            lang: "es-ES",
            height: Number($t.attr("data-editor-alto")) || 200,
            placeholder: $t.attr("placeholder") || "",
            dialogsInBody: true,
            disableDragAndDrop: false,
            maximumImageFileSize: SGEI.MAXIMO_IMAGEN_EDITOR,
            toolbar: [
                ["estilo", ["style"]],
                ["fuente", ["bold", "italic", "underline", "strikethrough", "clear"]],
                ["color", ["color"]],
                ["parrafo", ["ul", "ol", "paragraph"]],
                ["insertar", ["table", "link", "picture", "hr"]],
                ["ver", ["fullscreen"]]
            ],
            callbacks: {
                onImageUploadError: function () {
                    SGEI.mostrarAviso("La imagen supera 1 MB. Achicala o recortala y volvé a pegarla.", "error");
                }
            }
        });

        if ($t.is("[data-editor-requerido]")) {
            $t.closest("form").on("submit", function (e) {
                var vacio = $t.summernote("isEmpty") && $t.summernote("code").indexOf("<img") < 0;
                $t.siblings(".note-editor").toggleClass("is-invalid", vacio);
                if (vacio) {
                    e.preventDefault();
                    e.stopImmediatePropagation();
                    $t.summernote("focus");
                }
            });
        }
        return $t;
    };

    /** tipo: "exito" (por defecto, verde) o "error" (rojo). */
    SGEI.mostrarAviso = function (mensaje, tipo) {
        var error = tipo === "error";
        var $toast = $('<div class="toast align-items-center ' + (error ? "text-bg-danger" : "text-bg-success") + ' border-0 sgei-aviso" role="'
            + (error ? "alert" : "status") + '" aria-live="' + (error ? "assertive" : "polite") + '" aria-atomic="true">'
            + '<div class="d-flex"><div class="toast-body"><i class="bi ' + (error ? "bi-exclamation-triangle-fill" : "bi-check-circle-fill") + ' me-2" aria-hidden="true"></i></div>'
            + '<button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast" aria-label="Cerrar"></button></div></div>');
        $toast.find(".toast-body").append(document.createTextNode(mensaje));
        $toast.on("hidden.bs.toast", function () { $toast.remove(); });
        $("#sgei-avisos").append($toast);
        mostrarToast($toast[0]);
    };

    $(function () {
        $("[data-sgei-aviso]").each(function () {
            mostrarToast(this);
        });

        $(".modal[data-abrir-al-cargar]").modal("show");

        // Formularios de alta/edición por pasos: <form data-wizard> (BeginForm con data_wizard = "true").
        $("form[data-wizard]").each(function () {
            SGEI.crearWizard(this);
        });

        // Grillas: <table data-grilla>.
        $("table[data-grilla]").each(function () {
            SGEI.crearGrilla(this);
        });

        // Después de las grillas: el filtro se mueve a la barra de la grilla y recién ahí se arma el Select2.
        $("select[data-filtro-grilla]").each(function () {
            SGEI.crearFiltroGrilla(this);
        });

        $(".sgei-grilla-fechas[data-filtro-grilla]").each(function () {
            SGEI.crearFiltroFechas(this);
        });

        $("select[data-buscable]").each(function () {
            SGEI.crearComboBuscable(this);
        });

        $("textarea[data-editor]").each(function () {
            SGEI.crearEditor(this);
        });
    });

    // Tooltips de los botones de acción de las grillas. Se manejan a mano (trigger "manual") para que
    // se cierren siempre al salir el mouse o el foco del botón, al hacer clic (confirm, modal, navegación)
    // y cuando DataTables vuelve a dibujar la grilla (buscar, paginar, ordenar): en ese caso el botón ya
    // no está en la página, por eso se guarda el tooltip abierto y se cierra ese.
    var Tooltip = window.bootstrap && window.bootstrap.Tooltip;
    var tooltipAbierto = null;

    SGEI.ocultarTooltips = function () {
        if (tooltipAbierto) {
            tooltipAbierto.hide();
            tooltipAbierto = null;
        }
        // Por si quedó alguno a medio animar.
        $("body > .tooltip").remove();
    };

    var esFocoDeTeclado = function (elemento) {
        try {
            return elemento.matches(":focus-visible");
        } catch (e) {
            return false;
        }
    };

    if (Tooltip) {
        $(document).on("mouseenter focusin", ".sgei-accion[title], .sgei-accion[data-bs-original-title]", function (evento) {
            // El foco por teclado muestra el tooltip; el que deja el clic del mouse, no.
            if (evento.type === "focusin" && !esFocoDeTeclado(this)) {
                return;
            }
            SGEI.ocultarTooltips();
            tooltipAbierto = Tooltip.getOrCreateInstance(this, { trigger: "manual", container: "body", animation: false });
            tooltipAbierto.show();
        });

        $(document).on("mouseleave focusout mousedown click", ".sgei-accion", SGEI.ocultarTooltips);
        $(document).on("draw.dt", SGEI.ocultarTooltips);
        $(window).on("scroll pagehide", SGEI.ocultarTooltips);
    }

})(jQuery, window, document);
