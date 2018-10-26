
function show(input) {
    var fileList = input.files;
    var anyWindow = window.URL || window.webkitURL;
    for (var i = 0; i < fileList.length; i++) {
        var objectUrl = anyWindow.createObjectURL(fileList[i]);
        $('#picHolder > div').append('<img src="' + objectUrl + '" height="100" width="90" style="border:solid; margin-right: 10px;" />');
        window.URL.revokeObjectURL(fileList[i]);
    }
}

function deleteProduct(elem) {
    debugger;
    var ctrl = $(elem);
    if (confirm('Do You Really Want To Delete This Item?')) {
        $.ajax({
            url: '/Admin/Product/DeleteProduct',
            data: { "id": ctrl.data('id') },
            type: 'POST',
            dataType: 'json'
        }).done(function (res) {
            if (res.Result === "OK") {
                ctrl.closest('tr').remove();
            } else if (res.Result.Message) {
                alert(ctrl.Result.Message);
            }
        }).fail(function () {
            alert("There is something Wrong. Please Try Again");
        });
    }
}

$(document).ready(function () {

    var inputLocalFont = document.getElementById("file");
    inputLocalFont.addEventListener("change", show, false);

    $('#price, #percentage').on('change keyup paste', function (e) {
        var val = $(this).val();
        var val2 = val.replace(/[^0-9\.]/g, '');
        $(this).val(val2);
        var price = $('#price').val();
        var percentage = $('#percentage').val();
        var discountamount = (price / 100) * percentage;
        var fprice = price - discountamount;
        $('#fprice').val(fprice.toFixed(2));
        if ((e.which !== 46 || $(this).val().indexOf('.') !== -1) && (e.which < 48 || e.which > 57)) {
            e.preventDefault();
        }
    });

    $('input[name="Product.Featured"][type="hidden"]').remove();
    $('input[name="Scheduled"][type="hidden"]').remove();
    $('#datepicker').css('display', 'none');
    $('#scheduled').on('change',
        function () {
            var val = this.checked ? 1 : 0;
            if (val === 1) {
                $('#datepicker').css('display', 'block');
                $('#datepicker').datepicker({ dateFormat: "MM dd,yy" });
            } else {
                $('#datepicker').css('display', 'none');
            }
        });

    $('#categoryTypeId').change(function () {
        var id = $(this).val();
        $('#categoryId').find('option').remove().end()
            .append('<option value="">Choose Category </option>');
        $.ajax({
            url: '/Admin/Product/PopulateCategoryById',
            type: "POST",
            data: { "id": id }
        }).done(function (json) {
            json = json || {};
            $.each(json, function (key, value) {
                $('#categoryId')
                    .append($("<option></option>")
                        .attr("value", value.CategoryId)
                        .text(value.CategoryName));
            });
        }).fail(function (xhr, textStatus) {
            alert("Request Failed:" + textStatus);
        });
    });
});