(function ($) {
    class requester {
        constructor() {
            this.controller = "/formule";
            this.guid = $("[data-id='guid']").val();
        }
        SolveFormula(formula, letters, callback){ 
            if (Object.keys(letters).length)
                $.post(this.controller + "/SolveFormula", { Guid: this.guid, WP: formula, Letters: letters }, callback, 'json');
        }
    }

    var r = new requester();

    $(document).ready(function () {
        $("[data-id='waypoint']").on('change', calculate);
        $("[data-variable]").on('keyup', calculate);
        calculate();
    });

    var GetLetterValues = function() {
        var letters = {};
        $("[data-variable]").each(function () {
            let letter = $(this).data('variable'),
                value = $(this).val();
            letters[letter] = value;
        });
        return letters;
    }

    var calculate = function () {
        let formula = $("[data-id='waypoint'] option:selected").val(),
            letters = GetLetterValues();

        r.SolveFormula(formula, letters, function (coords) {
            $('[data-id="coordinate-result"]').text("");
            if (!coords) {
                return;
            }

            $("[data-container='coordinates']").children().remove();
            var current = coords[coords.length - 1];
            var previous = coords.slice(0, coords.length - 1);
            var counter = 1;
            previous.forEach(function (coord) {
                if (coord.latitude) {
                    $("[data-container='coordinates']").append("<coord data-lat='" + coord.latitude + "' data-title='WP " + counter + "' data-long='" + coord.longitude + "'></coord>");
                    counter++;
                }
            });

            $("[data-container='coordinates']").append("<coord data-lat='" + current.latitude + "' data-color='red' data-long='" + current.longitude + "'></coord>");
            $('[data-id="coordinate-result"]').append("<span class='" + (current.latitude ? "valid" : "invalid") + "'>" + current.result + "</span>");
            addCoordsToMap();
            
        });

    };

})(jQuery);