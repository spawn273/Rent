import polyglotI18nProvider from 'ra-i18n-polyglot';
import russianMessages from 'ra-language-russian';

const resources = {
    rents: {
        name: 'Аренда |||| Аренды',
        fields: {
            id: 'Номер договора',
            customer: 'Клиент',
            from: "Дата выдачи",
            to: "Дата возврата (договор)",
            closed: "Дата возврата (факт)",
            shopId: "Арендная точка",
            equipmentIds: "Оборудование",
            payment: "Оплата",
            comment: "Комментарий"
        },
    },
    equipment: {
        name: 'Оборудование |||| Оборудование',
        fields: {
            id: 'Инвентарный номер',
            name: 'Наименование',
            pricePerDay: 'Цена за день',
            pricePerHour: 'Цена за час',
            shopId: 'Арендная точка',
            equipmentTypeId: 'Тип оборудования',
            rentIds: 'Текущая аренда',
        },
    },
    equipmentTypes: {
        name: 'Цены',
        fields: {
            name: 'Наименование',
            pricePerDay: 'Цена за день',
            pricePerHour: 'Цена за час',
        },
    },

    employees: {
        name: 'Сотрудники |||| Сотрудники',
        fields: {
            name: 'Имя',
            shopId: 'Арендная точка',
            phone: 'Телефон',
        },
    },

    shops: {
        name: 'Арендные точки |||| Арендные точки',
        fields: {
            name: 'Наименование',
            address: 'Адрес',
            phone: 'Телефон',
        },
    },

    'guest/equipment': {
        name: 'Список оборудования |||| Список оборудования',
        fields: {
            available: 'Доступен для аренды',
            name: 'Наименование',
            type: 'Тип оборудования',
            shopId: 'Арендная точка',
            shop: 'Точка',
            equipmentType: 'Тип',
        }
    },

    'admin/accounts': {
        name: 'Аккаунты',
        fields: {
            roleId: 'Роль',
            name: 'Имя',
            userName: 'Логин',
            shopId: 'Арендная точка',
            phone: 'Телефон',
            password: 'Пароль',
            firstName: 'Имя',
            middleName: 'Отчество',
            lastName: 'Фамилия',
        }
    },
}

const custom = {
    shopSelector: "Арендная точка",
    rents: {
        filters: {
            opened: "Открытые",
            today: "Возврат сегодня",
        },
        show: {
            close: "Завершить"
        },
        table: {
            type: "Тип",
            total: 'Итого',
        }
    }
}

const messages = {
    ra: { ...russianMessages.ra},
    resources: resources,
    custom: custom
};

const i18nProvider = polyglotI18nProvider(() => messages, 'ru');

export default i18nProvider;
