import React from 'react';
import { AutocompleteArrayInput, Create, DateTimeInput, FormDataConsumer, NumberInput, ReferenceArrayInput, required, SimpleForm, TextInput } from 'react-admin';
import EquipmentTable from './EquipmentTable';

const RentCreate = (props) => (
    <Create {...props}>
        <SimpleForm redirect="list">
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


export default RentCreate;
