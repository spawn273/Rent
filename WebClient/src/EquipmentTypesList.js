import React from 'react';
import { List, Datagrid, TextField, EmailField } from 'react-admin';

export const EquipmentTypesList = props => (
    <List {...props}>
        <Datagrid rowClick="edit">
            <TextField source="id" />
            <TextField source="name" />
            <TextField source="price" />
        </Datagrid>
    </List>
);
