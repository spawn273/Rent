import React from 'react';
import { AutocompleteArrayInput, ReferenceInput, SelectInput, DateTimeInput, Edit, FormDataConsumer, NumberInput, email, ReferenceArrayInput, required, SimpleForm, TextInput, usePermissions } from 'react-admin';

const ShopEdit = ({ permissions, ...props }) => {
    return (
        <Edit {...props} undoable={false} >
            <SimpleForm>
                <TextInput source="name" validate={required()} />
                <TextInput source="address" validate={required()} />
                <TextInput source="phone" validate={required()} />
            </SimpleForm>
        </Edit>
    )
};

export default ShopEdit;
