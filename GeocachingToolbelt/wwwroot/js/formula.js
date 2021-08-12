(function ($) {
    String.prototype.replaceAt = function (index, replacement) {
        return this.substr(0, index) + replacement + this.substr(index + replacement.length);
    }

    class solver {
        // return list<string>
        GetSums(formula) {
            var bracketOpen = 0,
                sumStart = 0,
                sums = [];

            for (var pos = 0; pos < formula.length; pos++) {
                var character = formula[pos];

                if (character == '(') {
                    if (bracketOpen == 0) {
                        sumStart = pos;
                    }
                    bracketOpen++;
                }

                if (character == ')') {
                    bracketOpen--;
                    if (bracketOpen == 0) {
                        sums.push(formula.substr(sumStart, pos - sumStart + 1));
                    }
                }
            }

            return sums;
        }

        // return string
        ReplaceLetters(formula, letters) {
            var coordinate = formula.toUpperCase();
            var blacklist = ["N", "E", "S", "W"];

            // Replace the letters in the formula.
            Object.keys(letters).forEach(function (letter) {
                let bracketOpen = 0;
                let pos = 0;
                
                while (pos < coordinate.length) {
                    var character = coordinate[pos];
                    if (character == letter && letters[letter]) {
                        var onBlacklist = blacklist.indexOf(letter) >= 0;
                        if (onBlacklist) {
                            // An N is only bad as first character
                            if ((letter == "N" || letter == "S") && pos > 0) {
                                onBlacklist = false;
                            }
                            // Skip the E only if it's surrounded by spaces or space E 00
                            if (letter == "E" || letter == "W") {
                                // No worries if its at the end.
                                if (pos + 2 > coordinate.length || pos < 1) {
                                    onBlacklist = false;
                                }

                                if (onBlacklist &&
                                    (coordinate[pos - 1] != ' ')
                                    && (
                                        (coordinate[pos + 1] != ' ')
                                        || (coordinate[pos + 1] != '0' && coordinate[pos + 2] != '0')
                                    )
                                ) {
                                    onBlacklist = false;
                                }
                            }
                        }

                        if (bracketOpen > 0 || !onBlacklist) {
                            coordinate = coordinate.replaceAt(pos, letters[letter]);
                            pos += letters[letter].length - 1; // Move the cursor along if the value is longer then 1 position
                        }
                    }

                    if (character == '(') {
                        bracketOpen++;
                    }

                    if (character == ')') {
                        bracketOpen--;
                    }

                    pos++;
                }

            });

            return coordinate;
        }

        SolveSum(sum) {
            try {
                var result = eval(sum);
                return result;
            } catch (e) {
                return sum;
            }
        }

        // return char[] (this still gets done serverside. for future use.)
        GetLetters(formula) {
            if (!formula) {
                return {};
            }

            formula = formula.toUpperCase();

            var characters = "";

            var blacklist = ["N", "E", "S", "W"];
            var bracketOpen = 0;
            var pos = 0;

            while (pos < formula.length) {
                var character = formula[pos];

                var onBlacklist = blacklist.indexOf(character) >= 0;
                if (onBlacklist) {
                    // An N is only bad as first character
                    if ((character == 'N' || character == 'S') && pos > 0) {
                        onBlacklist = false;
                    }
                    // Skip the E only if it's surrounded by spaces or space E 00
                    if (character == 'E' || character == 'W') {
                        // No worries if its at the end.

                        if (pos + 2 > formula.length || pos < 1) {
                            onBlacklist = false;
                        }

                        if (onBlacklist &&
                            (formula[pos - 1] != ' ')
                            && (
                                (formula[pos + 1] != ' ')
                                || (formula[pos + 1] != '0' && formula[pos + 2] != '0')
                            )
                        ) {
                            onBlacklist = false;
                        }
                    }
                }

                if (bracketOpen > 0 || !onBlacklist) {
                    characters += character;
                }

                if (character == '(') {
                    bracketOpen++;
                }

                if (character == ')') {
                    bracketOpen--;
                }

                pos++;
            }

            var unique = [...new Set(characters)].map(c => c.toUpperCase());

            return unique.filter((letter) => {
                return !letter.match(new RegExp("[^A-Z]", 'g'));
            });
        }

        SolveFormula(formula, letters) {
            formula = formula.toUpperCase();
            formula = this.ReplaceLetters(formula, letters);
            var sums = this.GetSums(formula);
            var coordinate = formula;

            sums.forEach(function (sum) {
                coordinate = coordinate.replace(new RegExp(sum, 'g'), this.SolveSum(sum));
            }, this);

            return coordinate;
        }
    }

    class requester {
        constructor() {
            this.controller = "/formule";
            this.guid = $("[data-id='guid']").val();
            this.s = new solver();
        }

        SolveFormula(formula, letters, callback) {
            $.post(this.controller + "/SolveFormula", { Guid: this.guid, WP: formula, Letters: letters }, callback, 'json');
        }
    }

    var r = new requester();
    var s = new solver();

    $(document).ready(function () {
        $("[data-id='waypoint']").on('change', calculate);
        $("[data-variable]").on('keyup', calculate);
        calculate();
    });

    var GetLetterValues = function() {
        var letters = {};
        if (!$("[data-variable]").length) {
            return null;
        }

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

        var result = s.SolveFormula(formula, letters);
        $('[data-id="coordinate-result"]').html("<span>" + result + "</span>");

        /*
        r.SolveFormula(formula, letters, function (coords) {
            $('[data-id="coordinate-result"]').text("");
            if (!coords) {
                return;
            }
            $('[data-id="coordinate-result"]').append("<span>" + coords + "</span>");

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
    */

    };

})(jQuery);