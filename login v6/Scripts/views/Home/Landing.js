/* Home/Landing: carrusel de la portada. */
(function ($) {
    "use strict";

    $(function () {
        $(".owl-carousel").owlCarousel({
            loop: true,
            margin: 10,
            nav: false,
            autoplay: true,
            autoplayTimeout: 4000,
            responsive: { 0: { items: 1 } }
        });
    });
})(jQuery);
