function addToCart(productId, quantity) {
    quantity = parseInt(quantity);  // Преобразуем количество в число

    if (quantity <= 0) {
        alert('Количество товара должно быть больше нуля!');
        return; // Прерываем выполнение, если количество некорректно
    }

    // Получаем количество товара на складе из страницы
    var stockQuantityElement = Array.from(document.querySelectorAll('.specifications li'))
        .find(li => li.textContent.includes('Количество:'));

    if (stockQuantityElement) {
        var stockQuantity = parseInt(stockQuantityElement.textContent.split(':')[1].trim());
    } else {
        alert('Не удалось найти количество товара на складе.');
        return;
    }

    if (quantity > stockQuantity) {
        alert('На складе недостаточно товара!');
        return; // Прерываем выполнение, если товара на складе меньше, чем указано
    }

    // Отправляем запрос на сервер для добавления товара в корзину
    fetch('/Cart/AddToCart', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ productId: productId, quantity: quantity })
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                alert('Товар успешно добавлен в корзину!');
            } else {
                alert(data.message);  // Показать ошибку, если товар не добавлен
            }
        })
        .catch(error => {
            alert('Произошла ошибка при добавлении товара в корзину.');
        });
}
