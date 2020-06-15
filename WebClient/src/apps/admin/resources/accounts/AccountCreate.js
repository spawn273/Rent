import React from 'react';
import { AutocompleteArrayInput, PasswordInput, ReferenceInput, Create, SelectInput, DateTimeInput, email, FormDataConsumer, NumberInput, ReferenceArrayInput, required, SimpleForm, TextInput } from 'react-admin';
import { func } from 'prop-types';

const AccountCreate = (props) => (
    <Create {...props}>
        <SimpleForm redirect="list">
            <TextInput source="userName" validate={[required(), email()]} type="email" />
            <PasswordInput source="password" initiallyVisible validate={required()} />

            <ReferenceInput source="roleId" reference="roles">
                <SelectInput optionText="name" validate={required()} />
            </ReferenceInput>

            <ReferenceInput source="shopId" reference="shops">
                <SelectInput optionText="name" validate={required()} />
            </ReferenceInput>

            <TextInput source="firstName" validate={required()} />
            <TextInput source="lastName" validate={required()} />
            <TextInput source="middleName" validate={required()} />
            <TextInput source="phone"/>

        </SimpleForm>
    </Create>
);

function employeeSelected(formData) {
    return formData.roleId === "employee"
}

export default AccountCreate;
