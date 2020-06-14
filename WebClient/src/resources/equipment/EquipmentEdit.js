import React from 'react';
import { Edit, ReferenceInput, SelectInput, SimpleForm, TextInput, required } from 'react-admin';

const EquipmentEdit = ({ permissions, ...props }) => (
    <Edit {...props}>
        <SimpleForm>
            {
                permissions && permissions.role === 'admin' &&
                    <ReferenceInput source="shopId" reference="shops" >
                        <SelectInput optionText="name" validate={required()}/>
                    </ReferenceInput>
            }
            <ReferenceInput source="equipmentTypeId" reference="equipmentTypes">
                <SelectInput optionText="name" validate={required()}/>
            </ReferenceInput>
            <TextInput source="name" validate={required()}/>
        </SimpleForm>
    </Edit>
);

export default EquipmentEdit;
