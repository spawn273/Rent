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
// const Wrapper1 = (props) => {
//     const shop = useSelector((state) => state.shop);
//     return props.children(props, shop);
// };

// const Wrapper = (props) => { 
//     const shop = useSelector((state) => state.shop);
//     return ({children})
// };

const App = () => (
    <Admin customReducers={{ shop: shopReducer }} customRoutes={customRoutes} layout={MyLayout} 
        dataProvider={dataProvider} authProvider={authProvider} i18nProvider={i18n}>

        <Resource name="shops" list={shops.list} />
        <Resource name="rents" {...rents} />
        <Resource name="employees" {...employees} />
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
