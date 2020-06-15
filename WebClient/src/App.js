import React from 'react';
import { Admin, Resource } from 'react-admin';
import authProvider from './authProvider/authProvider';
import customRoutes from './customRoutes/customRoutes';
import dataProvider from './dataProvider/dataProvider';
import i18n from './i18n/ru';
import MyLayout from './layout/MyLayout';
import employees from './resources/employees';
import equipment from './resources/equipment';
import equipmentTypes from './resources/equipmentTypes';
import rents from './resources/rents';
import shops from './resources/shops';
import shopReducer from './shopSelector/shopReducer';

const App = () => (
    <Admin customReducers={{ shop: shopReducer }} customRoutes={customRoutes} layout={MyLayout} 
        dataProvider={dataProvider} authProvider={authProvider} i18nProvider={i18n}>

        <Resource name="shops" list={shops.list} />
        <Resource name="rents" {...rents} />
        <Resource name="employees" {...employees} />
        <Resource name="customers" />
        <Resource name="equipment" {...equipment}/>
        <Resource name="equipmentTypes" list={equipmentTypes.list} />


    </Admin>
);
 
export default App;
