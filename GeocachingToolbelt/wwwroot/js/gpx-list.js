(function ($) {

    $(document).ready(function () {
        $("[data-draw-radius]").on('click', drawRadius);
    });

    function drawRadius(event) {
        var radius = parseInt($(event.currentTarget).data("draw-radius"));
        
        $("[data-container='coordinates'] coord").attr("data-radius", radius);
        $("[data-container='coordinates'] coord").attr("data-forcemarker", true);
        addCoordsToMap();

        if (radius == 161) {
            $("[data-draw-radius]").data("draw-radius", 0);
        } else {
            $("[data-draw-radius]").data("draw-radius", 161);
        }
    }

})(jQuery);