
$(document).ready(function () {
    $('#total').text($('.grandtotal span').text());


    $('#cartBtn').unbind('click').click(function () {
        var q = $('#quantity').text();
        var productId = $('#cartBtn').data('id');
        $('#items').empty();
        $.ajax({
            url: '/Cart/AddToCart/',
            type: 'POST',
            data: { id: productId, quantity: q },
            dataType: 'json',
            cache: false,
            success: function (result) {
                var json = JSON.parse(result);
                var total = json.Sum.toFixed(2);
                $('#total').text(total);
                $('#cartBtn').text('Cart Added');
                $.each(json.Items,
                    function (i, e) {
                        if (i === 3) {
                            return false;
                        }
                        $('#items').append('<div class= "cart-entry" >' +
                            '<a class="image"><img style="height:50px; width=100%; display:block;" src="/Uploads/' +
                            e.ImageUrl +
                            '" alt="" /></a>' +
                            '<div class="content">' +
                            '<a class="title" href="/Product/Detail/' + e.Product.ProductId + '">' +
                            e.Product.Name + '</a>' +
                            '<div class="quantity">Quantity: ' + e.Count + '</div>' +
                            '<div class="price">$' + e.Product.FinalPrice * e.Count + '</div>' +
                            '</div>' +
                            '<div class="button-x" data-id="' + e.Product.ProductId + '"><i ' +
                            'class="fa fa-close"></i></div></div>');
                    });
                $('#items').append('<div class="summary"><div class="subtotal">' +
                    'Product Items: ' +
                    json.Items.length +
                    '</div><div class="grandtotal">' +
                    'Grand Total <span>$' +
                    total +
                    '</span></div></div><div class="cart-buttons"><div class="column">' +
                    '<a href="/Cart/Index" class="button style-3">View Cart</a><div class="clear"></div>' +
                    '</div><div class="column"><a href="/Cart/Checkout" class="button style-4">Checkout</a>' +
                    '<div class="clear"></div></div><div class="clear"></div></div>');
            },
            failure: function () {
                alert("Try Again, There is something wrong.");
            }
        });
    });


    $('.cartTag').unbind('click').click(function () {
        debugger;
        $(this).text('Cart Added');
        var id = $(this).data('id');
        var q = 1;
        $('#items').empty();
        $.ajax({
            url: '/Cart/AddToCart/',
            type: 'POST',
            data: { id: id, quantity: q },
            dataType: 'json',
            cache: false,
            success: function (result) {
                var json = JSON.parse(result);
                $('#total').text(json.Sum.toFixed(2));
                var total = json.Sum.toFixed(2);
                addNotification(json, total);
            },
            failure: function () {
                alert("Try Again, There is something wrong.");
            }
        });
    });

    $('#wishlistBtn').click(function() {
        var productId = $(this).data('id');
        $.ajax({
            url: '/Cart/AddToWishList',
            type: 'POST',
            data: { id: productId },
            success: function () {
                $('#wishlistBtn').text('Added In WishList')
                                 .css('color', '#FF0000');
            }
        });
    });

    //$('#search').autocomplete({
    //    source: function (request, response) {
    //        $.ajax({
    //            url: '/Product/ProductItems',
    //            data: { name: $('#search').val() },
    //            type: 'POST',
    //            dataType: 'json',
    //            success: function (data) {
    //                response(data);
    //            }
    //        });
    //    },
    //    minLength: 2
    //});

    $('#search').keyup(function () {
        var html = '';
        $.ajax({
            url: '/Product/ProductItems',
            data: { name: $('#search').val() },
            type: 'POST',
            dataType: 'json',
            success: function(data) {
                $.each(data,
                    function(i, e) {
                        var i = 0;
                        html += '<div class="col-md-3 col-sm-4 shop-grid-item">' +
                            '<div class="product-slide-entry shift-image">' +
                            '<div class="product-image">';

                    });
            }
        });
    });
});

function AddWishItemInCart(elem) {
    debugger;
    var productId = $(elem).data('pid');
    var id = $(elem).data('id');
    var quantity = $('#quantity_' + id).text();
    var html = '<h3 class="cart-column-title size-1">Products</h3>';
    $('.wishItems').empty();
    $.ajax({
        url: '/Cart/AddWishItemsInCart',
        type: 'POST',
        dataType: 'json',
        cache: false,
        data: { productId: productId, quantity: quantity },
        success: function (data) {
            var json = JSON.parse(data);
            var total = json.Sum.toFixed(2);
            $.each(json.WishLists,
                function (i, e) {
                    i = i + 1;
                    html += '<div class="traditional-cart-entry style-1">' +
                        '<a class="image" href="#"><img alt="" src="/Uploads/' +
                        e.ImageUrl +
                        '"></a><div class="content"><div class="cell-view">' +
                        '<a class="title" href="#">' +
                        e.Product.Name +
                        '</a>';

                    if (e.Product.Details.length > 50) {
                        html += '<div class="inline-description">' + e.Product.Details.substr(0, 50) + '</div>';
                    } else {
                        html += '<div class="inline-description">' + e.Product.Details + '</div>';
                    }

                    html += '<div class="price"><div data-price="' +
                        e.Product.Price +
                        '" class="prev">$' +
                        e.Product.Price +
                        '</div><div data-fprice="' +
                        e.Product.FinalPrice +
                        '" class="current">$' +
                        e.Product.FinalPrice +
                        '</div></div><div class="quantity-selector detail-info-entry">' +
                        '<div class="detail-info-entry-title">Quantity</div><div class="entry number-minus">&nbsp;</div>' +
                        '<div id="quantity_' +
                        i +
                        '" class="entry number">10</div><div class="entry number-plus">&nbsp;</div>' +
                        '<a data-pid="' +
                        e.ProductId +
                        '" data-id="' +
                        i +
                        '" class="button style-17" onclick="AddWishItemInCart(this)">Add To Cart</a>' +
                        '<a data-pid="' +
                        e.ProductId
                        + '" data-id="' + i + '" class="button style-17" onclick="RemoveWishItem(this)">Remove WishList</a>' +
                        '</div></div></div></div>';
                });

            $('.wishItems').html(html);

            $('#items').empty();
            $('#total').text(total);
            addNotification(json, total);
        }
    });
}

function RemoveWishItem(elem) {
    var productId = $(elem).data('pid');
    var html = '<h3 class="cart-column-title size-1">Products</h3>';
    $('.wishItems').empty();
    $.ajax({
        url: '/Cart/RemoveWishList',
        type: 'POST',
        dataType: 'json',
        cache: false,
        data: { id: productId },
        success: function (data) {
            $.each(data,
                function (i, e) {
                    i = i + 1;
                    html += '<div class="traditional-cart-entry style-1">' +
                        '<a class="image" href="#"><img alt="" src="/Uploads/' +
                        e.ImageUrl +
                        '"></a><div class="content"><div class="cell-view">' +
                        '<a class="title" href="#">' +
                        e.Product.Name +
                        '</a>';

                    if (e.Product.Details.length > 50) {
                        html += '<div class="inline-description">' + e.Product.Details.substr(0, 50) + '</div>';
                    } else {
                        html += '<div class="inline-description">' + e.Product.Details + '</div>';
                    }

                    html += '<div class="price"><div data-price="' +
                        e.Product.Price +
                        '" class="prev">$' +
                        e.Product.Price +
                        '</div><div data-fprice="' +
                        e.Product.FinalPrice +
                        '" class="current">$' +
                        e.Product.FinalPrice +
                        '</div></div><div class="quantity-selector detail-info-entry">' +
                        '<div class="detail-info-entry-title">Quantity</div><div class="entry number-minus">&nbsp;</div>' +
                        '<div id="quantity_' +
                        i +
                        '" class="entry number">10</div><div class="entry number-plus">&nbsp;</div>' +
                        '<a data-pid="' +
                        e.ProductId +
                        '" data-id="' +
                        i +
                        '" class="button style-17" onclick="AddWishItemInCart(this)">Add To Cart</a>' +
                        '<a data-pid="' +
                        e.ProductId
                        + '" data-id="' + i + '" class="button style-17" onclick="RemoveWishItem(this)">Remove WishList</a>' +
                        '</div></div></div></div>';
                });

            $('.wishItems').html(html);
        }
    });
}

function RemoveCart(elem) {
    var id = $(elem).data('id');
    $.ajax({
        url: '/Cart/RemoveFromCart/',
        type: 'POST',
        data: { productId: id },
        dataType: 'json',
        cache: false,
        success: function (result) {
            var json = JSON.parse(result);
            var total = json.Sum.toFixed(2);
            $('#items').empty();
            $('.cartItems').empty();
            var html = '<h3 class="cart-column-title size-1">Products</h3>';
            $.each(json.Items,
                function (i, e) {
                    if (i === 3) {
                        return false;
                    }
                    html += '<div class="traditional-cart-entry style-1">' +
                        '<a class="image" href="#"><img alt=""' +
                        'src="/Uploads/' +
                        e.ImageUrl +
                        '"></a>' +
                        '<div class="content"><div class="cell-view">' +
                        '<a class="tag" href="#">' +
                        e.Product.Category.CategoryName +
                        '</a>' +
                        '<a class="title" href="#">' +
                        e.Product.Name +
                        '</a>';

                    if (e.Product.Details.length > 50) {
                        html += '<div class="inline-description">' + e.Product.Details.substr(0, 50) + '</div>';
                    } else {
                        html += '<div class="inline-description">' + e.Product.Details + '</div>';
                    }

                    html += '<div class="price"><div class="prev">$' +
                        (e.Product.Price * e.Count).toFixed(2) +
                        '</div><div ' +
                        'class="current">$' +
                        (e.Product.FinalPrice * e.Count).toFixed(2) +
                        '</div></div><div class="quantity-selector detail-info-entry">' +
                        '<div class="detail-info-entry-title">Quantity</div>' +
                        '<div class="entry number-minus">&nbsp;</div>' +
                        '<div class="entry number">' +
                        e.Count +
                        '</div>' +
                        '<div class="entry number-plus">&nbsp;</div>' +
                        '<a id="update" data-id="' +
                        e.ProductId +
                        '" class="button style-15" onclick="UpdateCart(this)">Update Cart</a>' +
                        '<a id="remove" data-id="' +
                        e.ProductId +
                        '" class="button style-17" onclick="RemoveCart(this)">remove</a>' +
                        '</div></div></div></div>';
                });
            $('.cartItems').html(html);
            $('#cartTotal').text('$' + total);
            $('#total').text(total);
            addNotification(json, total);
        }
    });
}

function UpdateCart(elem) {
    var id = $(elem).data('id');
    var countId = $(elem).data('countid');
    var count = $('#count_' + countId).text();
    $.ajax({
        url: '/Cart/UpdateCart/',
        type: 'POST',
        data: { id: id, count: count },
        success: function (result) {
            $('#update_' + countId).text('Cart Updated');
            var json = JSON.parse(result);
            var total = json.Sum.toFixed(2);
            var price = $('#prev_' + countId).data('price');
            var fprice = $('#current_' + countId).data('fprice');
            $('#prev_' + countId).text('$' + (price * count).toFixed(2));
            $('#current_' + countId).text('$' + (fprice * count).toFixed(2));
            $('#items').empty();
            $('#total').text(total);
            $('#cartTotal').text('$' + total);
            addNotification(json, total);
        }
    });
}

function RemoveNotif(elem) {
    var id = $(elem).data('id');
    $.ajax({
        url: '/Cart/RemoveFromCart/',
        type: 'POST',
        data: { productId: id },
        dataType: 'json',
        cache: false,
        success: function (result) {
            var json = JSON.parse(result);
            var total = json.Sum.toFixed(2);
            $('#items').empty();
            $('#total').text(total);
            addNotification(json, total);
        }
    });
    RemoveCart(elem);
}

function addNotification(json, total) {
    $.each(json.Items,
        function (i, e) {
            if (i === 3) {
                return false;
            }
            $('#items').append('<div class= "cart-entry" >' +
                '<a class="image"><img style="height:50px; width=100%; display:block;" src="/Uploads/' +
                e.ImageUrl +
                '" alt="" /></a>' +
                '<div class="content">' +
                '<a class="title" href="#">' + e.Product.Name + '</a>' +
                '<div class="quantity">Quantity: ' + e.Count + '</div>' +
                '<div class="price">$' + e.Product.FinalPrice * e.Count + '</div>' +
                '</div><div class="button-x" data-id="' + e.Product.ProductId + '" onclick="RemoveNotif(this)">' +
                '<i class="fa fa-close"></i></div>' +
                '</div>');
        });
    $('#items').append('<div class="summary"><div class="subtotal">' +
        'Product Items: ' +
        json.Items.length +
        '</div><div class="grandtotal">' +
        'Grand Total <span>$' +
        total +
        '</span></div></div><div class="cart-buttons"><div class="column">' +
        '<a href="/Cart/Index" class="button style-3">View Cart</a><div class="clear"></div>' +
        '</div><div class="column"><a href="/Cart/Checkout" class="button style-4">Checkout</a>' +
        '<div class="clear"></div></div><div class="clear"></div></div>');
}