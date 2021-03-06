import React from 'react';
import { AutocompleteArrayInput, SelectInput, ReferenceInput, Create, DateTimeInput, FormDataConsumer, NumberInput, ReferenceArrayInput, required, SimpleForm, TextInput } from 'react-admin';
import EquipmentTable from './EquipmentTable';

const RentCreate = ({ permissions, ...props }) => {

    return (
        <Create {...props}>
            <SimpleForm redirect="list">
                {
                    permissions && permissions.role === 'admin' &&
                        <ReferenceInput source="shopId" reference="shops" >
                            <SelectInput optionText="name" validate={required()}/>
                        </ReferenceInput>
                }
    
                <TextInput source="customer" validate={required()} />
    
                <DateTimeInput source="from" validate={required()} initialValue={new Date()} />
                <DateTimeInput source="to" validate={required()} initialValue={new Date()} />
                <NumberInput source="payment" />
                <TextInput multiline source="comment" />
    
                <ReferenceArrayInput source="equipmentIds" reference="equipment">
                    <AutocompleteArrayInput />
                </ReferenceArrayInput>
    
                <FormDataConsumer >
                    {formDataProps => (
                        <EquipmentTable record={formDataProps.formData} />
                    )}
                </FormDataConsumer>
    
            </SimpleForm>
        </Create>
    );
}

export default RentCreate;
