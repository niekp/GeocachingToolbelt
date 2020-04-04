(function ($) {
    class requester {
        constructor() {
            this.controller = "formule";
        }
        GetLetters(formula, callback) {
            $.post(this.controller + "/GetLetters", { Formula: formula }, callback, 'json');
        }
        SolveFormula(formula, letters, callback) {
            if (Object.keys(letters).length)
                $.post(this.controller + "/SolveFormula", { Formula: formula, Letters: letters }, callback);
        }
    }

    var r = new requester();

    $(document).ready(function () {
        loadFormula();
        $("[data-id='formula']").on('keyup', setup);
    });

    var setup = function () {
        let formula = $("[data-id='formula']").val(),
            $result = $("[data-id='result']");

        $result.children().remove();
        r.GetLetters(formula, function (variables) {
            variables.forEach(function (variable) {
                if (!variable.length) {
                    return;
                }

                $("<tr>" +
                    "<td>" +
                    variable +
                    "</td>" +
                    "<td><input type='text' data-variable='" + variable + "' class='form-input' /></td>" +
                    "</tr > ").appendTo($result);
            });

            $("[data-id='result-container']").toggle(variables.length > 0);
            $("[data-variable]").on('keyup', calculate);
            loadLetters();
            calculate();
        });

    };

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
        let formula = $("[data-id='formula']").val(),
            letters = GetLetterValues();

        save();

        r.SolveFormula(formula, letters, function (coord) {
            console.log(coord)
            $('[data-id="coordinate-result"]').val(coord);
        });

    };

    var save = function () {
        var rawData = localStorage.getItem("formula");
        var data = {
            formula: "",
            letters: {}
        };

        if (rawData) {
            var data = JSON.parse(rawData);
        }

        data.formula = $("[data-id='formula']").val();
        data.letters = { ...data.letters, ...GetLetterValues()}

        localStorage.setItem("formula", JSON.stringify(data));
    }

    var loadFormula = function () {
        var rawData = localStorage.getItem("formula");
        if (!rawData) {
            return;
        }

        var data = JSON.parse(rawData);
        $("[data-id='formula']").val(data.formula).change();
    }

    var loadLetters = function() {
        var rawData = localStorage.getItem("formula");
        if (!rawData) {
            return;
        }
        var data = JSON.parse(rawData);
        try {
            Object.keys(data.letters).forEach(function (letter) {
                $("[data-variable='" + letter + "']").val(data.letters[letter]);
            });
        } catch (e) { };
    }

})(jQuery);