// in src/App.js
import React from 'react';
import { Admin, Resource, ListGuesser } from 'react-admin';
import jsonServerProvider from 'ra-data-json-server';
import { EquipmentList, EquipmentEdit, EquipmentCreate } from './EquipmentList';
import { EquipmentTypesList } from './EquipmentTypesList';
import { ShopsList } from './ShopsCrud';
import { RentsList, RentsShow, RentsEdit } from './RentsCrud';
import { EmployeesList } from './EmployeesCrud';
import MyLayout from './MyLayout';
import customRoutes from './customRoutes';
import shopReducer from './shopReducer';
import myDataProvider from './DataProvider';


const App = () => (
    <Admin customReducers={{ shop: shopReducer }}  customRoutes={customRoutes} layout={MyLayout} dataProvider={myDataProvider}>
        <Resource name="shops" list={ShopsList} />
        <Resource name="rents" list={RentsList} edit={RentsEdit} />
        <Resource name="employees" list={EmployeesList} />
        <Resource name="equipments" list={EquipmentList} edit={EquipmentEdit} create={EquipmentCreate}  />
        <Resource name="equipmentTypes" list={EquipmentTypesList} />
    </Admin>
);

export default App;
