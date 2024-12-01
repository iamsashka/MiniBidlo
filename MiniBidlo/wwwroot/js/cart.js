// Функция для обновления количества товара в корзине
function updateItemQuantity(itemId, newQuantity) {
    fetch(`/Cart/UpdateQuantity?itemId=${itemId}&quantity=${newQuantity}`, {
        method: 'POST'
    }).then(response => response.json())
        .then(data => {
            if (data.success) {
                location.reload(); // Обновление страницы после успешного изменения
            } else {
                alert('Ошибка при обновлении количества');
            }
        });
}
