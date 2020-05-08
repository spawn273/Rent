import React from 'react';
import { List, Datagrid, TextField, ReferenceField } from 'react-admin';

const EmployeeList = props => (
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

export default EmployeeList;
