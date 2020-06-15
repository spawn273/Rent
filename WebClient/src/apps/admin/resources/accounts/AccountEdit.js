import React from 'react';
import { AutocompleteArrayInput, ReferenceInput, SelectInput, DateTimeInput, Edit, FormDataConsumer, NumberInput, email, ReferenceArrayInput, required, SimpleForm, TextInput, usePermissions } from 'react-admin';

const AccountEdit = ({ permissions, ...props }) => {
    return (
        <Edit {...props} undoable={false} >
            <SimpleForm >
                <TextInput source="userName" validate={[required(), email()]} type="email" />
                <ReferenceInput source="roleId" reference="roles">
                    <SelectInput optionText="name"  validate={required()} />
                </ReferenceInput>

                <ReferenceInput source="shopId" reference="shops">
                    <SelectInput optionText="name" validate={required()} />
                </ReferenceInput>
                
                <TextInput source="firstName" validate={required()} />
                <TextInput source="lastName" validate={required()} />
                <TextInput source="middleName" validate={required()} />
                <TextInput source="phone"/>

            </SimpleForm>
        </Edit>
    )
};

export default AccountEdit;
