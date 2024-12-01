// addcart.js

function addToCart(productId, quantity) {
    fetch('/Cart/AddToCart', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ productId: productId, quantity: quantity })
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                alert('Товар успешно добавлен в корзину!');
                // Обновить модальное окно корзины или выполнить другие действия
                updateCartModal();
                updateCartSummary();
            } else {
                alert('Ошибка при добавлении товара в корзину: ' + data.message);
            }
        })
        .catch(error => {
            console.error('Ошибка:', error);
            alert('Произошла ошибка при добавлении товара в корзину.');
        });
}
