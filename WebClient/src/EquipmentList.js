// in src/users.js
import React from 'react';
import { Create, Edit, ReferenceInput, SelectInput, SimpleForm, TextInput, List, Datagrid, TextField, ReferenceField } from 'react-admin';
import { useSelector, useDispatch } from 'react-redux';

export const EquipmentList = props => {
    const shop = useSelector((state) => state.shop);
    return (
        <List {...props} filter={{ shopId: shop }}>
        <Datagrid rowClick="edit">
                <TextField source="id" />
                <TextField source="name" />
                <ReferenceField source="equipmentTypeId" reference="equipmentTypes">
                    <TextField source="name" />
                </ReferenceField>
            </Datagrid>
        </List>
    );
}

export const EquipmentEdit = props => (
    <Edit {...props}>
        <SimpleForm>
            <ReferenceInput source="equipmentTypeId" reference="equipmentTypes">
                <SelectInput optionText="name" />
            </ReferenceInput>
            <TextInput source="name" />
        </SimpleForm>
    </Edit>
);

export const EquipmentCreate = props => (
    <Create {...props}>
        <SimpleForm>
            <ReferenceInput source="equipmentTypeId" reference="equipmentTypes">
                <SelectInput optionText="name" />
            </ReferenceInput>
            <TextInput source="name" />
        </SimpleForm>
    </Create>
);
