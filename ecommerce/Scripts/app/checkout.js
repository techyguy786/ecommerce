$(document).ready(function () {

    $('#order-review').click(function () {
        $('#order').removeClass('active');
        $('#order-desc').css('display', 'none');
        $('#checkout-form').css('margin-top', '0');
        $('#shipping').addClass('active');
        $('#shipping-desc').css('display', 'block');
    });

    $('#order').click(function () {
        debugger;
        $('#checkout-form').css('margin-top', '40px');
    });

    $('#shippingbtn').click(function (e) {
        e.preventDefault();
        $('#shipping').removeClass('active');
        $('#shipping-desc').css('display', 'none');
        $('#payment').addClass('active');
        $('#payment-desc').css('display', 'block');
    });

    $('#paymentbtn').click(function (e) {
        e.preventDefault();
        $('#payment').removeClass('active');
        $('#payment-desc').css('display', 'none');
    });

});