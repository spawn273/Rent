import React from 'react';
import { AutocompleteArrayInput, ReferenceInput, SelectInput, DateTimeInput, Edit, FormDataConsumer, NumberInput, email, ReferenceArrayInput, required, SimpleForm, TextInput, usePermissions } from 'react-admin';

const AccountEdit = ({ permissions, ...props }) => {
    return (
        <Edit {...props} undoable={false} >
            <SimpleForm >
                <TextInput source="firstName" validate={required()} />
                <TextInput source="middleName" validate={required()} />
                <TextInput source="lastName" validate={required()} />

                <ReferenceInput source="roleId" reference="roles" validate={required()}>
                    <SelectInput optionText="name" />
                </ReferenceInput>

                <ReferenceInput source="shopId" reference="shops">
                    <SelectInput optionText="name"/>
                </ReferenceInput>

                <TextInput source="userName" validate={email()} type="email" />

            </SimpleForm>
        </Edit>
    )
};

export default AccountEdit;
