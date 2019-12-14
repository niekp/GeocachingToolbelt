(function ($) {

    $(document).ready(function () {
        $("[data-trigger='copy']").on('click', copyColumn);
    });

    var copyColumn = function (event) {
        let $th = $(event.currentTarget).closest("th"),
            index = $th.index(),
            $result = $("[data-id='result']");

        var data = "";
        $result.find("tr").each(function () {
            let $tds = $(this).find("td");
            data += $tds.eq(0).html() + "\t\t" + $tds.eq(index).html() + "\n";
        });

        copyToClipboard(data);
    }

    const copyToClipboard = str => {
        const el = document.createElement('textarea');
        el.value = str;
        document.body.appendChild(el);
        el.select();
        document.execCommand('copy');
        document.body.removeChild(el);
    };

})(jQuery);