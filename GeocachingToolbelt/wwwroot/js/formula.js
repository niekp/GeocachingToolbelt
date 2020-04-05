(function ($) {
    class requester {
        constructor() {
            this.controller = "/formule";
            this.guid = $("[data-id='guid']").val();
        }
        SolveFormula(formula, letters, callback){ 
            if (Object.keys(letters).length)
                $.post(this.controller + "/SolveFormula", { Guid: this.guid, Formula: formula, Letters: letters }, callback, 'json');
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
        console.log(formula);

        r.SolveFormula(formula, letters, function (coords) {
            $('[data-id="coordinate-result"]').text("");
            if (!coords) {
                return;
            }

            $("[data-container='coordinates']").children().remove();
            coords.forEach(function (coord) {
                if (coord.latitude) {
                    $("[data-container='coordinates']").append("<coord data-lat='" + coord.latitude + "' data-long='" + coord.longitude + "'></coord>");
                }

                $('[data-id="coordinate-result"]').append("<span class='" + (coord.latitude ? "valid" : "invalid") + "'>" + coord.result + "</span>");
            });
            addCoordsToMap();
            
        });

    };

})(jQuery);