import React from 'react';
import { Create, ReferenceInput, SelectInput, SimpleForm, TextInput } from 'react-admin';

const EquipmentCreate = props => (
    <Create {...props}>
        <SimpleForm>
            <ReferenceInput source="equipmentTypeId" reference="equipmentTypes">
                <SelectInput optionText="name" />
            </ReferenceInput>
            <TextInput source="name" />
        </SimpleForm>
    </Create>
);


export default EquipmentCreate;
