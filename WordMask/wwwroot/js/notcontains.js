(function ($) {
    $(document).ready(function () {
        $("[data-id='mask']").change(updateNotContains);
    });

    var updateNotContains = function () {
        console.log($("[data-id='notcontain-automode']").prop("checked"))
        if (!$("[data-id='notcontain-automode']").prop("checked")) {
            return;
        }

        let mask = $("[data-id='mask']").val().toUpperCase();
        let associations = $("[data-id='associations']").val().toUpperCase().split(",");
        let notcontains = "";

        associations.forEach(function (item) {
            let association = item.split('=');
            if (association.length == 2) {
                if (mask.indexOf(association[0].trim()) < 0) {
                    if (notcontains) {
                        notcontains += ", ";
                    }
                    notcontains += association[1].trim();
                }
            }
        });

        console.log('not', notcontains)

        $("[data-id='notcontain']").val(notcontains);
    };

})(jQuery);