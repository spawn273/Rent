import React from 'react';
import { Admin, Resource } from 'react-admin';
import dataProvider from './dataProvider/dataProvider';
import i18n from './i18n/ru';
import equipment from './apps/guest/resources/equipment';

const App = () => (
    <Admin dataProvider={dataProvider} i18nProvider={i18n}>
        <Resource name="guest/equipment" {...equipment} />
        <Resource name="equipmentTypes" />
        <Resource name="shops" />
    </Admin>
);

export default App;
