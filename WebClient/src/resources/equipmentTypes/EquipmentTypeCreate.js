import React from 'react';
import { AutocompleteArrayInput, PasswordInput, ReferenceInput, Create, SelectInput, DateTimeInput, email, FormDataConsumer, NumberInput, ReferenceArrayInput, required, SimpleForm, TextInput } from 'react-admin';

const EquipmentTypeCreate = (props) => (
    <Create {...props}>
        <SimpleForm redirect="list">
            <TextInput source="name" validate={required()} />
            <NumberInput source="pricePerHour" validate={required()} />
            <NumberInput source="pricePerDay" validate={required()} />
        </SimpleForm>
    </Create>
);

export default EquipmentTypeCreate;
