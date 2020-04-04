(function ($) {

    $(document).ready(function () {
        $("td [data-trigger='copy']").on('click', copyCell);
        $("th [data-trigger='copy']").on('click', copyColumn);
        $("[data-copy][data-trigger='copy']").on('click', copyId);
        $("[data-trigger='copy-current-url']").on('click', copyUrl);
    });

    function copyCell(event) {
        let $td = $(event.currentTarget).closest("td");
        copyToClipboard($td.text().trim());
    }

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

    function copyUrl() {
        copyToClipboard(window.location.href);
        alert("De link is gekopieerd.");
    }

    function copyId(event) {
        copyToClipboard($("[data-id='" + $(event.currentTarget).data("copy") + "']").text());
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