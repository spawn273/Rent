import React from 'react';
import { AutocompleteArrayInput, ReferenceInput, SelectInput, DateTimeInput, Edit, FormDataConsumer, NumberInput, email, ReferenceArrayInput, required, SimpleForm, TextInput, usePermissions } from 'react-admin';

const EquipmentTypeEdit = ({ permissions, ...props }) => {
    return (
        <Edit {...props} undoable={false} >
            <SimpleForm>
                <TextInput source="name" validate={required()} />
                <NumberInput source="pricePerHour" validate={required()} />
                <NumberInput source="pricePerDay" validate={required()} />
            </SimpleForm>
        </Edit>
    )
};

export default EquipmentTypeEdit;
