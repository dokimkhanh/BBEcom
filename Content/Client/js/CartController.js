var cart = {
    init: function () {
        cart.regEvents();
    },
    regEvents: function () {
        $('#btnUpdateCart').off('click').on('click', function () {
            var listProduct = $('.productQuantity');
            var cartList = [];
            $.each(listProduct, function (i, item) {
                cartList.push({
                    Quantity: $(item).val(),
                    product: {
                        Id: $(item).data('id')
                    }
                });
            });
            //console.log(cartList);

            $.ajax({
                url: '/Cart/Update',
                data: { cartModel: JSON.stringify(cartList) },
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    console.log(res)
                    if (res.status) {
                        window.location.href = '/gio-hang'
                    } else {
                        //alert('Kho hàng không đủ, vui lòng giảm số lượng sản phẩm')
                        showToast("Kho hàng không đủ, vui lòng giảm số lượng sản phẩm");
                    }
                }
            })
        });

        $('#btnDeleteCart').off('click').on('click', function () {
            $.ajax({
                url: '/Cart/DeleteAllCart',
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    if (res.status) {
                        window.location.href = '/gio-hang'
                    }
                }
            });
        });

        $('#btnDelete').off('click').on('click', function (e) {
            e.preventDefault();
            $.ajax({
                url: '/Cart/Delete',
                data: {
                    id: $(this).data('id')
                },
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    if (res.status) {
                        window.location.href = '/gio-hang'
                    }
                }
            });
        });
    }
}
cart.init();

function showToast(message, backgroundColor) {
    Toastify({
        text: message,
        duration: 3000,
        backgroundColor: backgroundColor || "#333",  // Nếu không có màu được truyền vào, sử dụng màu xám đậm mặc định
        close: true,
        gravity: "top",
        position: "right",
        stopOnFocus: true,
    }).showToast();
}