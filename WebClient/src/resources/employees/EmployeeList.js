import React from 'react';
import { List, Datagrid, TextField, ReferenceField } from 'react-admin';
import { useSelector } from 'react-redux';

const EmployeeList = props => {
    const shop = useSelector((state) => state.shop);
    return (
        <List {...props} filter={{ shopId: shop }}>
            <Datagrid>
                <TextField source="name" />
                <TextField source="phone" />
            </Datagrid>
        </List>
    );
}

export default EmployeeList;
