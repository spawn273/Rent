import React from 'react';
import { Admin, Resource } from 'react-admin';
import dataProvider from './dataProvider/dataProvider';
import i18n from './i18n/ru';
import accounts from './apps/admin/resources/accounts';
import authProvider from './apps/admin/authProvider';
import shops from './resources/shops';

const App = () => (
    <Admin dataProvider={dataProvider} i18nProvider={i18n} authProvider={authProvider}>
        <Resource name="admin/accounts" {...accounts} />
        <Resource name="shops" {...shops}/>
        <Resource name="roles" />
    </Admin>
);

export default App;
