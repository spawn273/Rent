import React from 'react';
import { Create, ReferenceInput, SelectInput, SimpleForm, TextInput, required } from 'react-admin';

const EquipmentCreate = ({ permissions, ...props }) => (
    <Create {...props}>
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
    </Create>
);


export default EquipmentCreate;
