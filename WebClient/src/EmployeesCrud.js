import React from 'react';
import { List, Datagrid, TextField, ReferenceField } from 'react-admin';

export const EmployeesList = props => (
    <List {...props}>
        <Datagrid rowClick="edit">
            <TextField source="id" />
            <TextField source="name" />
            <ReferenceField source="shopId" reference="shops">
                <TextField source="name" />
            </ReferenceField>
        </Datagrid>
    </List>
);
