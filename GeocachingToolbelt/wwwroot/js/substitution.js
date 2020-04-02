(function ($) {
    var encoded = '',
        decoded = '';

    $(document).ready(function () {
        $("[data-id='encoded-text']").keyup(updateText);
    });

    var updateText = function () {
        encoded = $("[data-id='encoded-text']").val();

        decodeText();
        displayResult();    
    }

    var decodeText = function () {
        console.log(encoded)
        decoded = encoded;
    };

    var displayResult = function () {
        $("[data-id='decoded-text']").html(decoded);
    };

})(jQuery);