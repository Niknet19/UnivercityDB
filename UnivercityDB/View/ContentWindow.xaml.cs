﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UnivercityDB.View
{
    /// <summary>
    /// Логика взаимодействия для ContentWindow.xaml
    /// </summary>
    public partial class ContentWindow : UserControl
    {
        public ContentWindow()
        {
            InitializeComponent();
            UserManualTextBox.Text = @"
### Руководство пользователя ###

#### Введение ####
Это руководство пользователя предназначено для работы с программой управления базой данных отдела кадров.
Это руководство пользователя предназначено для работы с программой управления базой данных отдела кадров. Оно включает описание функциональности приложения, объясняет, как выполнять основные операции, и предоставляет инструкции для каждого элемента интерфейса.

    Содержание

    1 Авторизация

    2 Приветственный экран

    3 Строка меню

        3.1 Настройки

        3.2 Сотрудники

        3.3 Приказы

        3.4 Документы

        3.5 Справочники

        3.6 Справка

#### Описание ####

    1. Авторизация

        При запуске программы открывается окно авторизации. Для входа в систему пользователь вводит логин и пароль. При успешной авторизации отображается приветственный экран.

    2. Приветственный экран

        После авторизации пользователь попадает на приветственный экран, где в верхней части окна расположена строка меню с шестью основными пунктами.

    3. Строка меню

        В строке меню расположены основные пункты, каждый из которых выполняет определённые функции:

    3.1 Настройки

        В этом пункте находятся следующие подменю:

        1.1 Настройки шрифта: открывается окно настройки шрифтов для приложения.

        1.2 Сменить пароль: позволяет пользователю сменить текущий пароль.

        1.3 Выйти: завершает текущую сессию и возвращает пользователя к окну авторизации.

    3.2 Сотрудники

        Открывает экран управления сотрудниками. На этом экране доступны:

        Таблица сотрудников: отображает данные о всех сотрудниках.

        Кнопки управления:

        “Новый сотрудник” — добавление нового сотрудника в базу.

        “Обновить” — обновление данных в таблице.

        “Удалить” — удаление выбранного сотрудника.

        Поиск: поле для ввода имени сотрудника и кнопка “Поиск” для фильтрации данных.

        Кнопки дополнительной информации:

        “Работа” — список прошлых мест работы сотрудника.

        “Должности” — список должностей сотрудника.

        “Подразделения” — список подразделений, где работает сотрудник.

    3.3 Приказы

        Содержит три подменю для управления приказами:

        - Приказы о перемещении.

        - Приказы о поощрении.

        - Приказы о взысканиях.

        На любом из экранов приказов доступны:

        Поле поиска для фильтрации данных по сотрудникам.

        Список приказов, соответствующих поисковому запросу.

        Кнопки:

        “Новый приказ” — создание нового приказа.

        “Обновить” — обновление данных.

        “Удалить” — удаление выбранного приказа.

    3.4 Документы

        Содержит подменю “Создать документ”, которое открывает экран для работы с SQL-запросами:

        Поле для ввода SQL-запроса: позволяет формулировать запросы к базе данных.

        Поле для вывода результата: отображает результаты выполненного запроса в виде таблицы.

        Элементы управления:

        “Выполнить” — выполняет введённый SQL-запрос.

        “Сохранить” — открывает меню сохранения результата в файл.

        Выбор шаблонов запросов: содержит список заранее подготовленных запросов.

    3.5 Справочники

        Этот пункт предоставляет доступ к различным справочным данным, таким как города, улицы и другие. Экран включает:

        Список справочников в виде подменю.

        Поле поиска и кнопку “Поиск” для фильтрации справочных данных.

        Кнопки управления:

        “Добавить” — добавление новой записи в справочник.

        “Удалить” — удаление выбранной записи.

        “Редактировать” — изменение данных выбранной записи.

    3.6 Справка

        Содержит два подменю:

        “О программе”: отображает общую информацию о приложении.

        “Содержание”: открывает это руководство пользователя для ознакомления.

        Данное руководство охватывает основные элементы интерфейса и функции приложения. Если у вас возникли вопросы или потребуется дополнительная информация, обратитесь к разделу “Справка” в строке меню.";
        }
    }
}
