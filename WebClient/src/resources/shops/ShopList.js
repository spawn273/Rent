import React from 'react';
import { List, Datagrid, TextField, NumberField } from 'react-admin';

const ShopList = props => (
    <List {...props} /*filters={<ListFilter/>}*/>
        <Datagrid optimized rowClick="edit">
            <TextField source="name" />
            <TextField source="address" />
            <TextField source="phone" />
        </Datagrid>
    </List>
);

export default ShopList;
