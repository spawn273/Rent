import React from 'react';
import { List, Datagrid, TextField, EmailField } from 'react-admin';

const EquipmentTypeList = props => (
    <List {...props}>
        <Datagrid rowClick="edit">
            <TextField source="id" />
            <TextField source="name" />
            <TextField source="pricePerHour" />
            <TextField source="pricePerDay" />
        </Datagrid>
    </List>
);

export default EquipmentTypeList;
