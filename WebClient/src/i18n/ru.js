import polyglotI18nProvider from 'ra-i18n-polyglot';
import russianMessages from 'ra-language-russian';

const resources = {
    rents: {
        name: 'Аренда |||| Аренды',
        fields: {
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
            name: 'Наименование',
            pricePerDay: 'Цена за день',
            pricePerHour: 'Цена за час',
            shopId: 'Арендная точка',
            equipmentTypeId: 'Тип оборудования',
        },
    },
    equipmentTypes: {
        name: 'Тип оборудования |||| Тип оборудования',
        fields: {
            name: 'Наименование',
            pricePerDay: 'Цена за день',
            pricePerHour: 'Цена за час',
        },
    },

    employees: {
        name: 'Сотрудники |||| Сотрудники',
        fields: {
            name: 'Наименование',
            shopId: 'Арендная точка',
        },
    },

    shops: {
        name: 'Арендные точки |||| Арендные точки',
        fields: {
            name: 'Наименование',
        },
    },

    'guest/equipment': {
        name: 'Список оборудования |||| Список оборудования',
        fields: {
            available: 'Доступен для аренды',
            name: 'Наименование',
            type: 'Тип оборудования',
            shopId: 'Арендная точка',
            shop: 'Арендная точка',
            equipmentType: 'Тип оборудования',
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
