import React from 'react';
import { AutocompleteArrayInput, PasswordInput, ReferenceInput, Create, SelectInput, DateTimeInput, email, FormDataConsumer, NumberInput, ReferenceArrayInput, required, SimpleForm, TextInput } from 'react-admin';

const ShopCreate = (props) => (
    <Create {...props}>
        <SimpleForm redirect="list">
            <TextInput source="name" validate={required()} />
            <TextInput source="address" validate={required()} />
            <TextInput source="phone" validate={required()} />
        </SimpleForm>
    </Create>
);

export default ShopCreate;
