//Выведите платёжные ссылки для трёх разных систем платежа: 
// Output будет следующий:
// pay.system1.ru/order?amount=13000RUB&hash={MD5 хеш ID заказа}
// order.system2.ru/pay?hash={MD5 хеш ID заказа + сумма заказа}
// system3.com/pay?amount=13000&curency=RUB&hash={SHA-1 хеш сумма заказа + ID заказа + секретный ключ от системы}