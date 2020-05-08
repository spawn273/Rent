import React from 'react';
import { Edit, ReferenceInput, SelectInput, SimpleForm, TextInput } from 'react-admin';

const EquipmentEdit = props => (
    <Edit {...props}>
        <SimpleForm>
            <ReferenceInput source="equipmentTypeId" reference="equipmentTypes">
                <SelectInput optionText="name" />
            </ReferenceInput>
            <TextInput source="name" />
        </SimpleForm>
    </Edit>
);

export default EquipmentEdit;
