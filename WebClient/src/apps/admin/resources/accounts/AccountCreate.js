import React from 'react';
import { AutocompleteArrayInput, PasswordInput, ReferenceInput, Create, SelectInput, DateTimeInput, email, FormDataConsumer, NumberInput, ReferenceArrayInput, required, SimpleForm, TextInput } from 'react-admin';
import { func } from 'prop-types';

const AccountCreate = (props) => (
    <Create {...props}>
        <SimpleForm redirect="list">
            <TextInput source="firstName" validate={required()} />
            <TextInput source="middleName" validate={required()} />
            <TextInput source="lastName" validate={required()} />

            <ReferenceInput source="roleId" reference="roles" validate={required()}>
                <SelectInput optionText="name" />
            </ReferenceInput>

            <FormDataConsumer >
                {({formData}) => (
                    <ReferenceInput source="shopId" reference="shops" /*disabled={!employeeSelected(formData)} validate={employeeSelected(formData) ? required() : null}*/>
                        <SelectInput optionText="name"/>
                    </ReferenceInput>
                )}
            </FormDataConsumer>

            <TextInput source="userName" validate={email()} type="email" />
            <PasswordInput source="password" initiallyVisible validate={required()} />

        </SimpleForm>
    </Create>
);

function employeeSelected(formData) {
    return formData.roleId === "employee"
}

export default AccountCreate;
