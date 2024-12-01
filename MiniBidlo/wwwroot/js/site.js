let cart = []; // Массив для хранения товаров в корзине

// Функция для добавления товара в корзину
function addToCart(productId, productName, productPrice) {
    const existingProductIndex = cart.findIndex(item => item.id === productId);
    if (existingProductIndex === -1) {
        cart.push({ id: productId, name: productName, price: productPrice, quantity: 1 });
    } else {
        cart[existingProductIndex].quantity++;
    }
    updateCartSummary();
}

// Функция для обновления отображения корзины
function updateCartSummary() {
    const cartItems = document.getElementById('cart-items');
    const cartTotal = document.getElementById('cart-total');

    if (cartItems) {
        cartItems.innerHTML = '';
        let totalSum = 0;
        cart.forEach(item => {
            totalSum += item.price * item.quantity;
            const listItem = document.createElement('li');
            listItem.textContent = `${item.name} - ${item.quantity} x ${item.price} ₽`;
            cartItems.appendChild(listItem);
        });
        cartTotal.textContent = `Итого: ${totalSum} ₽`;
    }
}

// Функция для оформления заказа
function checkout() {
    alert('Переход к оформлению заказа.');
    // Здесь можно добавить логику для перехода на страницу оформления заказа или отправки данных на сервер.
}

// Функция фильтрации по категории
$('#category-filter').change(function () {
    const filterValue = $(this).val().toLowerCase();
    $('.product').each(function () {
        const productCategory = $(this).data('category').toLowerCase();
        if (filterValue === '' || productCategory === filterValue) {
            $(this).show();
        } else {
            $(this).hide();
        }
    });
});

// Функция поиска по названию
$('#search-input').on('input', function () {
    const searchValue = $(this).val().toLowerCase();
    $('.product').each(function () {
        const productName = $(this).data('name').toLowerCase();
        if (productName.includes(searchValue)) {
            $(this).show();
        } else {
            $(this).hide();
        }
    });
});

// Функция сортировки
$('#sort-select').change(function () {
    const sortValue = $(this).val();
    let sortedProducts = $('.product').toArray();

    if (sortValue === 'price-asc') {
        sortedProducts.sort((a, b) => {
            return parseFloat($(a).find('.price').text()) - parseFloat($(b).find('.price').text());
        });
    } else if (sortValue === 'price-desc') {
        sortedProducts.sort((a, b) => {
            return parseFloat($(b).find('.price').text()) - parseFloat($(a).find('.price').text());
        });
    } else if (sortValue === 'name-asc') {
        sortedProducts.sort((a, b) => {
            return $(a).find('h3').text().localeCompare($(b).find('h3').text());
        });
    } else if (sortValue === 'name-desc') {
        sortedProducts.sort((a, b) => {
            return $(b).find('h3').text().localeCompare($(a).find('h3').text());
        });
    }

    $('.products').html(sortedProducts);
});
