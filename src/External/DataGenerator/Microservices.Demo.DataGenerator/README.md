## Описание сервиса
Сервис операций генерирует нагрузку на [customer-service] и [order-service].
Запускает в background-задаче генерацию заказов и заказчиков в соответствии с настройками.

(!) Важно (!): заказчики, указанные в заказах должны быть созданы в **customer_service** в рамках генерации.

## Настройки сервиса

Настройки можно указать через переменные окружения.

### Адреса брокеров кафки
Название переменной окружения: `DEMO_KAFKA_BROKERS`
Допустимые значения (строка)
Список адресов хостов брокеров кафки, разделенных через запятую

### Кол-во заказов в секунду 
Название переменной окружения: `DEMO_ORDERS_PER_SECOND`
Допустимые значения 1-100
Сколько сообщений за секунду должно быть поставлено в топик **orders_input**

### Кол-во заказчиков в секунду
Название переменной окружения: `DEMO_CUSTOMERS_PER_SECOND`
Допустимые значения 1-10
Сколько раз в секунду будет вызван [метод V1CreateCustomer].

### Создавать каждый N-ый заказ с ошибкой валидации
Название переменной окружения: `DEMO_INVALID_ORDER_COUNTER_NUMBER` ((?) не смог придумать лучше - пишите какие будут идеи)
Допустимые значения 0-10000
С момента запуска сервиса ведется счетчик созданных заказов и, если он достиг указанного значения, то отправляется сообщение заказа, который не должен пройти валидацию ([Заказ проходит валидацию перед тем как будет сохранен в БД].

### URL-адрес customer-service
Название переменной окружения: `DEMO_CUSTOMER_SERVICE_URL`
URL, по которому расположен customer-service

