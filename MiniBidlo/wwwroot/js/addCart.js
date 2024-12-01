function addToCart(productId, productName, productPrice) {
    // Создание данных для запроса
    const data = {
        productId: productId,
        quantity: 1 // или получаем количество с интерфейса, если требуется
    };

    // Отправка AJAX-запроса на сервер
    fetch('/Cart/AddToCart', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value // Для защиты от CSRF
        },
        body: JSON.stringify(data)
    })
        .then(response => {
            if (response.ok) {
                alert('Товар добавлен в корзину!');
                // Обновляем корзину на клиенте или показываем модальное окно
            } else {
                alert('Ошибка при добавлении товара в корзину');
            }
        })
        .catch(error => {
            console.error('Ошибка:', error);
            alert('Ошибка при добавлении товара в корзину');
        });
}
