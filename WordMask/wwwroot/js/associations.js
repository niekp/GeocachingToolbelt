(function ($) {
    $(document).ready(function () {
        $("[data-trigger='updateAssociations']").click(updateAssociations);
    });

    var updateAssociations = function (event) {
        let mask = $("[data-id='mask']").val();
        let word = event.currentTarget.innerHTML;

        
        for (var i = 0; i < word.length; i++) {
            addAssociation(mask.charAt(i), word.charAt(i))
        }
    };

    var addAssociation = function (a, b) {
        let associations = $("[data-id='associations']").val().toUpperCase().split(",");
        a = a.toUpperCase();
        b = b.toUpperCase();

        let newAssociations = "";
        associations.forEach(function (item) {
            let association = item.split('=');
            if (association.length == 2) {
                if (newAssociations != "") {
                    newAssociations += ", ";
                }
                if (association[0].trim() == a) {
                    association[1] = b;
                }

                newAssociations += association[0].trim() + "=" + association[1].trim();
            }
        });

        if (newAssociations.indexOf(a + "=") < 0) {
            if (newAssociations != "") {
                newAssociations += ", ";
            }

            newAssociations += a + "=" + b;
        }
        $("[data-id='associations']").val(newAssociations);
    };

})(jQuery);