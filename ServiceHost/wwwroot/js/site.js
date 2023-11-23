var SinglePage = {};

SinglePage.LoadModal = function () {
    var url = window.location.hash.toLowerCase();
    if (url.startsWith("#showmodal")) {
        url = url.split("showmodal=")[1];
        $.get(url,
            null,
            function (htmlPage) {
                $("#ModalContent").html(htmlPage);
                const container = document.getElementById("ModalContent");
                const forms = container.getElementsByTagName("form");
                const newForm = forms[forms.length - 1];
                $.validator.unobtrusive.parse(newForm);
                showModal();
            }).fail(function (error) {
                alert("An error has been occured.");
            });
    }
};

function showModal() {
    $("#MainModal").modal("show");
}

function hideModal() {
    $("#MainModal").modal("hide");

}

$(document).ready(function () {
    window.onhashchange = function () {
        SinglePage.LoadModal();
    };
    $("#MainModal").on("shown.bs.modal",
        function () {
            window.location.hash = "##";
        }
    );
});

jQuery.validator.addMethod("decimalPlacesNumber",
    function (value, element, params) {
        const numString = value.toString();
        var decimal = 0;
        if (numString.indexOf('.') !== -1) {
            decimal = numString.length - numString.indexOf('.') - 1;
        }
        var maximum = element.attributes["data-val-decimalplacesnumber-decimal"].value;
        if (decimal > 2 && decimal != maximum)
            return false;
        else {
            return true;
        }
    });
jQuery.validator.unobtrusive.adapters.addBool("decimalPlacesNumber");
