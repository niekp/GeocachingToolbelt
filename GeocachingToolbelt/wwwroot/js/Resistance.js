(function ($) {

    $(document).ready(function () {
        $("[data-id='colors']").on('keyup', calculate);
        createColorPicker();
    });

    var calculate = function () {
        let colors = $("[data-id='colors']").val().split("\n"),
            $result = $("[data-id='result']");

        $("[data-id='result-container']").toggle(colors.length > 0);

        $result.children().remove();
        colors.forEach(function (color) {
            if (!color.length) {
                return;
            }

            var value = new resistance(color);

            $("<tr>" +
                "<td " + (!value.color ? " style='color:red;'" : "") + ">" +
                    (value.color ? value.color.color.replace(/^\w/, c => c.toUpperCase()) : color) +
                "</td>" +
                "<td>" + (value.color ? value.color.resistanceNumber : "-") + "</td>" +
                "<td style='background:" + (value.color ? value.color.colorCode : "none") + "'>&nbsp;</td>" +
                "</tr > ").appendTo($result);
        });
    };

    var createColorPicker = function () {
        $colorpicker = $("[data-id='color-picker']");

        colors.forEach(color => {
            if (color.color == 'paars') // is dubbel met violet
                return;

            $colorpicker.append("<div class='color' data-color='" + color.color + "' style='background:" + color.colorCode + "' title='" + color.color + "'>&nbsp;</div>");
        });

        $("[data-color]").on('click', clickColor);
    }

    var clickColor = function (event) {
        var r = new resistance($(event.currentTarget).data('color'));
        if (r.color) {
            var colors = $("[data-id='colors']").val().trim()
            if (colors.length > 0) {
                colors += "\n";
            }
            colors += r.color.color;
            $("[data-id='colors']").val(colors);
        }
        calculate();
    }

    class color {
        constructor(color) {
            this.color = color;
            this.resistanceNumber = null;
            this.colorCode = null;

            switch (color) {
                case 'zwart':
                    this.resistanceNumber = 0;
                    this.colorCode = 'black';
                    break;
                case 'bruin':
                    this.resistanceNumber = 1;
                    this.colorCode = 'brown';
                    break;
                case 'rood':
                    this.resistanceNumber = 2;
                    this.colorCode = 'red';
                    break;
                case 'oranje':
                    this.resistanceNumber = 3;
                    this.colorCode = 'orange';
                    break;
                case 'geel':
                    this.resistanceNumber = 4;
                    this.colorCode = 'yellow';
                    break;
                case 'groen':
                    this.resistanceNumber = 5;
                    this.colorCode = 'green';
                    break;
                case 'blauw':
                    this.resistanceNumber = 6;
                    this.colorCode = 'blue';
                    break;
                case 'violet':
                    this.resistanceNumber = 7;
                    this.colorCode = 'purple';
                    break;
                case 'paars':
                    this.resistanceNumber = 7;
                    this.colorCode = 'purple';
                    break;
                case 'grijs':
                    this.resistanceNumber = 8;
                    this.colorCode = 'grey';
                    break;
                case 'wit':
                    this.resistanceNumber = 9;
                    this.colorCode = 'white';
                    break;
            }
        }
    }

    var colors = [
        new color('zwart'),
        new color('bruin'),
        new color('rood'),
        new color('oranje'),
        new color('geel'),
        new color('groen'),
        new color('blauw'),
        new color('violet'),
        new color('paars'),
        new color('grijs'),
        new color('wit')
    ]

    class resistance {
        constructor(input) {
            this.input = input;
            this.color = this.guessColor(input);
        }

        guessColor(input) {
            var resultColor = '';
            var resultCount = 0;

            colors.forEach(color => {
                if (color.color.startsWith(input.toLowerCase())) {
                    resultColor = color;
                    resultCount++;
                }
            });

            if (resultCount == 1) {
                return resultColor;
            }

            return;
        }
    }


})(jQuery);