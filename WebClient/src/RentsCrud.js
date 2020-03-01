import React from 'react';
import { ArrayInput, AutocompleteArrayInput, ReferenceArrayInput, SelectArrayInput, SimpleFormIterator, required, List, Show, Edit, SimpleForm, TextInput, DateTimeInput, ReferenceManyField, EditButton, SimpleShowLayout, Datagrid, TextField, DateField } from 'react-admin';

export const RentsList = props => (
    <List {...props}>
        <Datagrid rowClick="edit">
            <TextField source="id" />
            <DateField source="from" />
            <DateField source="to" />
        </Datagrid>
    </List>
);

export const RentsShow = (props) => (
    <Show {...props}>
        <SimpleShowLayout>
            <TextField source="id" />
            <DateField source="from" />
            <DateField source="to" />
        </SimpleShowLayout>
    </Show>
);

export const RentsEdit = (props) => (
    <Edit {...props}>
        <SimpleForm>
            <TextInput disabled label="Id" source="id" />
            <DateTimeInput source="from" validate={required()} />
            <DateTimeInput source="to" validate={required()} />
            <ReferenceArrayInput source="equipment_ids" reference="equipments">
                <AutocompleteArrayInput />
                {/* <SelectArrayInput optionText="name" /> */}
            </ReferenceArrayInput>
            {/* <ReferenceManyField label="Equipment" reference="equipments" target="rentId">
                <Datagrid>
                    <TextField source="id" />
                    <TextField source="name" />
                </Datagrid>
            </ReferenceManyField> */}
        </SimpleForm>
    </Edit>
);
