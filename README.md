# shop1c ( GASTRONOM v1 )

Now this is not an actual project. Its new and supported version: https://github.com/kuchersd/gastronom 

1С Shop - это проект, созданный для контроля товарооборота в небольшом магазине.
Создан на C# WinForms с локальной БД.
Главные задачи для проекта: 
- контроль товарооборота;
- выявление недостач;
- создание отчёта по продажам за некоторое время + копирование в Google Drive;
- контроль сотрудников(права доступа к отчётам, принятый/проданный товар и т.д.);

Если коротко: данный проект позволяет избегать и находить "дыры", а точнее недостачи во время работы магазина, увидеть детальную картинку по прибыли или же потери.
Программа рассчитанна на интеграцию со сканером для штрих-кодов, что позволяет в разы быстрее проводить операции на кассе.

Ниже будет составлен план, описывающий каждую вкладку/форму и все элементы там.
- ГЛАВНАЯ СТРАНИЦА
  
  ![Mainpage](https://github.com/kuchersd/shop1c/blob/master/screenshots/mainpage.png)
  
  Представляет собой самую главную и связующую форму. Именно здесь находится меню и главные элементы для прохождения операции на кассе.
  Изначально все элементы управления неактивны, кроме поля пароля и соответствующей кнопки.
  После авторизации, в зависимости от вашего ранга доступа, некоторые элементы становятся активными. (Если залогиниться как root - то будут активны все элем.)
  
  Ниже: меню (Товары/Меню пересменки/Просмотр отчётов/Просмотр настроек), состояние сканнера штрих-кодов (подключён ли).
  
  ![Panel](https://github.com/kuchersd/shop1c/blob/master/screenshots/toppanel.jpg)
  
  Всю остальную, правую часть формы занимает так званая "панель управления кассой". Принцип работы: в небольшой текст-бокс посередине вписываться (с помощью сканнера)
  штрих-код товара, далее по данному коду находится товар, его описание, цена и это всё добавляется в корзину. Программа просчитывает итоговую ценну по чеку и ожидает 
  чтобы в текст-бокс вписали сколько денег было дано продавцу и далее будет видна сдача. Если покупатель оплачивает картой - тогда ничего не нужно вписывать.
  Справа внизу присутствует список последних операций: Время/Сколько дали/Сдача и т.д.
  
  ![Cashien](https://github.com/kuchersd/shop1c/blob/master/screenshots/cashien2.png)
  
 - ТОВАРЫ
  
    Представляет собой форму, в которой есть возможность просмотреть все товары из БД в виде списка(Название/Штрих-код/Подробнее/Изначальная цена/Цена продажи/Количество).
  Так же, можно сохранить отчёт об остатке товара в файл Excel; добавить новый товар, заполнив все поля с данными; отредактировать/удалить существующий товар(в доработке).
  В этой форме реализован поиск товара по названию или же штрих-коду.
  
  ![Products](https://github.com/kuchersd/shop1c/blob/master/screenshots/prodlist.png)
  
  - ПРОСМОТР ОТЧЁТОВ
  
    Представляет собой форму, в которой пользователь может просмотреть продажи товаров за сегодня или любой другой промежуток времени. 
  При выборе просмотра отчёта за эту неделю, будут показаны операции конкретно с понедельника по воскресенье, так изначально в программе день пересменки(перерасчёта) - это воскресенье. Вся история реализована в виде списка(Время операции/Код,товар,описание/Итог/Клиент дал/Сдача/Способ оплаты/Продавец).
  Отчёт так же можно сохранить в файл Excel или же настроить автоматическую выгрузку на Google Drive.
  Ниже, в форме доступны графики, которые представляют статистику по выбраному промежутку времени: кол-во продаж по сравнению с другими днями.
  
  ![SalesList](https://github.com/kuchersd/shop1c/blob/master/screenshots/saleslist.png)
  
  - ПЕРЕСМЕНКА, ПОДСЧЁТ ОСТАТКА

  ![Peresmenka](https://github.com/kuchersd/shop1c/blob/master/screenshots/peresmenka.png)
  ![Ostatok](https://github.com/kuchersd/shop1c/blob/master/screenshots/proverkaoststka.png)
