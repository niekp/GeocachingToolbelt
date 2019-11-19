(function ($) {

    $(document).ready(function () {
        $("[data-id='words']").on('keyup', calculate)
    });

    var calculate = function () {
        let words = $("[data-id='words']").val().split("\n"),
            $result = $("[data-id='result']");

        $("[data-id='result-container']").toggle(words.length > 0);

        $result.children().remove();
        words.forEach(function (word) {
            if (!word.length) {
                return;
            }

            var value = new wordvalue(word);

            $("<tr>" +
                "<td>" + word + "</td>" +
                "<td>" + value.stackValue() + "</td>" +
                "<td>" + value.totalValue() + "</td>" +
                "<td>" + (value.firstLetter() > 9 ? (value.firstLetter() + " / " + value.firstLetterStacked()) : value.firstLetter()) + "</td>" +
                "<td>" + value.length() + "</td>" +
                "<td class='vanity'>" + value.vanityValue() + "</td>" +
                "<td>" + (value.scrabbleValue() > 9 ? (value.scrabbleValue() + " / " + value.stack(value.scrabbleValue())) : value.scrabbleValue()) + "</td>" +
                "</tr > ").appendTo($result);
        });
    };


    class wordvalue {
        constructor(word) {
            this.word = word;
        }

        length() {
            return this.word.length;
        }

        totalValue() {
            let v = 0;
            this.word.split('').forEach(l => v+=this.letterValue(l));
            return v;
        }

        stackValue() {
            return this.stack(this.totalValue());
        }

        firstLetter() {
            if (this.word.length < 1) {
                return 0;
            }

            return this.letterValue(this.word.substr(0, 1));
        }

        firstLetterStacked() {
            return this.stack(this.firstLetter());
        }

        vanityValue() {
            let result = '';
            this.word.split('').forEach(l => result += this.vanity(l).toString());
            return result;
        }

        scrabbleValue() {
            let v = 0;
            this.word.split('').forEach(l => v += this.scrabble(l));
            return v;
        }

        vanity(letter) {
            switch (letter.toLowerCase()) {
                case 'a':
                case 'b':
                case 'c':
                    return 2;
                case 'd':
                case 'e':
                case 'f':
                    return 3;
                case 'g':
                case 'h':
                case 'i':
                    return 4;
                case 'j':
                case 'k':
                case 'l':
                    return 5;
                case 'm':
                case 'n':
                case 'o':
                    return 6;
                case 'p':
                case 'q':
                case 'r':
                case 's':
                    return 7;
                case 't':
                case 'u':
                case 'v':
                    return 8;
                case 'w':
                case 'x':
                case 'y':
                case 'z':
                    return 9;
            }

            return 0;
        }

        scrabble(letter) {
            switch (letter.toLowerCase()) {
                case 'a':
                case 'e':
                case 'i':
                case 'n':
                case 'o':
                    return 1;

                case 'd':
                case 'r':
                case 's':
                case 't':
                    return 2;

                case 'b':
                case 'g':
                case 'k':
                case 'l':
                case 'm':
                case 'p':
                    return 3;

                case 'f':
                case 'h':
                case 'j':
                case 'u':
                case 'v':
                case 'z':
                    return 4

                case 'c':
                case 'w':
                    return 5

                case 'x':
                case 'y':
                    return 8;

                case 'q':
                    return 10;
            }

            return 0;
        }

        letterValue(letter) {
            return "abcdefghijklmnopqrstuvwxyz".indexOf(letter.toLowerCase()) + 1;
        }

        stack(value) {
            while (value.toString().length > 1) {
                let x = 0;
                value.toString().split("").forEach(n => x += parseInt(n));
                value = x;
            }
            return value;
        }

    }

})(jQuery);