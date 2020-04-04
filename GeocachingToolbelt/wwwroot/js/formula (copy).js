(function ($) {

    $(document).ready(function () {
        $("[data-id='formula']").on('keyup', setup);
    });

    var setup = function () {
        let formula = $("[data-id='formula']").val(),
            $result = $("[data-id='result']"),
            variables = getVariables(formula);

        $("[data-id='result-container']").toggle(formula.length > 0);

        $result.children().remove();

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

        $("[data-variable]").on('keyup', calculate);
    };

    var calculate = function () {
        let formula = $("[data-id='formula']").val(),
            splitted = splitCoordinate(formula, getSplitPosition(formula)),
            nord = splitted[0],
            east = splitted[1];
        nord = nord.substring(1, nord.length - 1);
        
        let coordinate = [nord, east].map(function (coord) {
            $("[data-variable]").each(function () {
                let letter = $(this).data('variable'),
                    value = $(this).val();

                if (value == "") {
                    value = 0;
                }

                coord = coord.replace(new RegExp(letter, 'g'), value);
            });


            var parts = coord.match(/\((.*?)\)/);

            if (parts) {
                let submatch = parts[0];
                try {
                    let result = eval(submatch);
                    if (!isNaN(result) && result >= 0) {
                        coord = coord.replace(submatch, result);
                    } else {
                        coord = coord.replace(submatch, "?");
                    }
                } catch { }
            }

            var evaluated = null;
            try {
                evaluated = eval(coord);
                $('[data-id="coordinate-result"]').css('color', 'auto');
            } catch (e) {
                $('[data-id="coordinate-result"]').css('color', 'red');
            }

            return evaluated || coord;
        });

        $('[data-id="coordinate-result"]').val("N" + coordinate[0] + " E" + coordinate[1])
    }

    var getSplitPosition = function (formula) {
        var bracketOpen = 0,
            sumOpen = false;

        for (i = 0; i < formula.length; i++) {
            let char = formula[i];
            if (char == '(') {
                bracketOpen++;
            }
            if (char == ')') {
                bracketOpen--;
            }

            if (bracketOpen || (char.match(/[\+\-\*\/]/))) {
                sumOpen = true;
            } else if (char == ' ' || char == ')') {
                sumOpen = false;
            }

            if (!sumOpen && char == 'E') {
                return i;
            }
        }
    }

    var splitCoordinate = function(value, index) {
        return [value.substring(0, index), value.substring(index)];
    }

    var getVariables = function (formula) {
        let variables = [],
            splitted = splitCoordinate(formula, getSplitPosition(formula)),
            nord = splitted[0],
            east = splitted[1];
        
        nord = nord.substring(1, nord.length - 1);
        east = east.substring(1, east.length - 1);

        [nord, east].forEach(function (coord) {
            coord = coord.replace(/\s/g, '');

            for (i = 0; i < coord.length; i++) {
                let char = coord[i];

                if (char.match(/^[A-Za-z]+$/) && variables.indexOf(char) < 0) {
                    variables.push(char);
                    continue;
                }

            }
        });

        return variables.sort();
    }


})(jQuery);