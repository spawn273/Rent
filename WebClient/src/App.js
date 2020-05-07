// in src/App.js
import React from 'react';
import { Admin, Resource, ListGuesser } from 'react-admin';
import jsonServerProvider from 'ra-data-json-server';
import { EquipmentTypesList } from './EquipmentTypesList';
import { ShopsList } from './ShopsCrud';
import { EmployeesList } from './EmployeesCrud';
import MyLayout from './MyLayout';
import customRoutes from './customRoutes';
import shopReducer from './shopReducer';
import myDataProvider from './DataProvider';
import authProvider from './authProvider';
import polyglotI18nProvider from 'ra-i18n-polyglot';
import russianMessages from 'ra-language-russian';
import EmployeeInfo from './EmployeeInfo';
import rents from './rents';
import equipment from './equipment';
import equipmentTypes from './equipmentTypes';


import { useSelector, useDispatch } from 'react-redux';

const Wrapper1 = (props) => {
    const shop = useSelector((state) => state.shop);
    return props.children(props, shop);
};

// const Wrapper = (props) => { 
//     const shop = useSelector((state) => state.shop);


//     return ({children})
// };

const resources = {
    rents: {
        name: 'Аренда |||| Аренды',
        fields: {
            customer: 'Клиент',
            from: "Дата выдачи",
            to: "Дата возврата (договор)",
            closed: "Дата возврата (факт)",
            shopId: "Магазин",
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
            shopId: 'Магазин',
            equipmentTypeId: 'Тип оборудования',
        },
    },
    equipmentTypes: {
        name: 'Тип оборудования |||| Тип оборудования',
        fields: {
            name: 'Наименование',
        },
    },
}

const custom = {
    shopSelector: "Магазин",
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

// const messages = {
//     ra: { ...russianMessages.ra, ...rum },
// };

const messages = {
    ra: { ...russianMessages.ra},
    resources: resources,
    custom: custom
};

const i18nProvider = polyglotI18nProvider(() => messages, 'ru');

const App = () => (
    <Admin customReducers={{ shop: shopReducer }} customRoutes={customRoutes} layout={MyLayout} 
        dataProvider={myDataProvider} authProvider={authProvider} i18nProvider={i18nProvider}>

        <Resource name="shops" list={ShopsList} />
        <Resource name="rents" {...rents} />
        <Resource name="employees" list={EmployeesList} />
        <Resource name="customers" />
        <Resource name="equipment" {...equipment}/>
        <Resource name="equipmentTypes" {...equipmentTypes} />


    </Admin>
);

        // {permissions => {
        //     return [
        //         <Resource name="shops" list={ShopsList} />,
        //         // <Wrapper1>
        //         //     {
        //         //         (props, shop) => <Resource {...props} name="rents" list={RentsList} 
        //         //                             edit={RentsEdit} 
        //         //                             create={RentsCreate}/>
        //         //     }
        //         // </Wrapper1>,

        //         <Resource name="rents" list={RentsList}
        //             edit={RentsEdit}
        //             create={RentsCreate} />,

        //         <Resource name="employees" list={EmployeesList} />,
        //         <Resource name="equipment" list={EquipmentList} edit={EquipmentEdit} create={EquipmentCreate} />,
        //         <Resource name="equipmentTypes" list={EquipmentTypesList} />,
        //     ]
        // }}

export default App;
