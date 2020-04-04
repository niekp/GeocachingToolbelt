(function ($) {
    class requester {
        constructor() {
            this.controller = "/formule";
            this.guid = $("[data-id='guid']").val();
        }
        SolveFormula(formula, letters, callback){ 
            if (Object.keys(letters).length)
                $.post(this.controller + "/SolveFormula", { Guid: this.guid, Formula: formula, Letters: letters }, callback);
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

        r.SolveFormula(formula, letters, function (coord) {
            console.log(coord)
            $('[data-id="coordinate-result"]').text(coord);
        });

    };

})(jQuery);