import React from 'react';
import { Create, ReferenceField, ReferenceArrayField, SingleFieldList, ChipField, useGetMany, ArrayInput, CheckboxGroupInput, ReferenceInput, AutocompleteInput, SelectInput, FormDataConsumer, AutocompleteArrayInput, ReferenceArrayInput, SelectArrayInput, SimpleFormIterator, required, List, Show, Edit, SimpleForm, TextInput, DateTimeInput, ReferenceManyField, EditButton, SimpleShowLayout, Datagrid, TextField, DateField } from 'react-admin';

import { Form, Field } from 'react-final-form'
import arrayMutators from 'final-form-arrays'
import { FieldArray } from 'react-final-form-arrays'
import MUIDataTable from "mui-datatables";
import { useSelector, useDispatch } from 'react-redux';


import { makeStyles } from '@material-ui/core/styles';
import Table from '@material-ui/core/Table';
import TableBody from '@material-ui/core/TableBody';
import TableCell from '@material-ui/core/TableCell';
import TableContainer from '@material-ui/core/TableContainer';
import TableHead from '@material-ui/core/TableHead';
import TableRow from '@material-ui/core/TableRow';
import Paper from '@material-ui/core/Paper';


export const RentsList = props => {
    const shop = useSelector((state) => state.shop);
    return <List {...props} filter={{ shopId: shop }}>
        <Datagrid rowClick="edit">
            <TextField source="id" />
            <DateField source="from" />
            <DateField source="to" />
        </Datagrid>
    </List>
};

export const RentsShow = (props) => (
    <Show {...props}>
        <SimpleShowLayout>
            <TextField source="id" />
            <DateField source="from" />
            <DateField source="to" />
        </SimpleShowLayout>
    </Show>
);

const OrderOrigin = ({ formData, ...rest }) => {
    console.log(formData);

    if (!formData.equipmentIds) {
        formData.equipmentIds = [];
    }

    let response = useGetMany('equipments', formData.equipmentIds);

    if (!response.loaded) {
        return null;
    }

    const data = response.data
    console.log(data);

    return (
        <TableContainer component={Paper}>
            <Table size="small" aria-label="a dense table">
                <TableHead>
                    <TableRow>
                        <TableCell>Id</TableCell>
                        <TableCell>Name</TableCell>
                        <TableCell>Price</TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {data.map((row) => (
                        <TableRow key={row.id}>
                            <TableCell component="th" scope="row">{row.id}</TableCell>
                            <TableCell>{row.name}</TableCell>
                            <TableCell>{row.price}</TableCell>
                        </TableRow>
                    ))}
                </TableBody>
            </Table>
        </TableContainer>
    );
};

export const RentsEdit = (props) => (
    <Edit {...props}>
        <SimpleForm >
            <TextInput disabled source="id" />
            <DateTimeInput source="from" validate={required()} />
            <DateTimeInput source="to" validate={required()} />

            <ReferenceArrayInput source="equipmentIds" reference="equipments">
                <AutocompleteArrayInput />
            </ReferenceArrayInput>

            <FormDataConsumer >
                {formDataProps => (
                    <OrderOrigin {...formDataProps} />
                )}
            </FormDataConsumer>

        </SimpleForm>
    </Edit>
);

export const RentsCreate = (props) => (
    <Create {...props}>
        <SimpleForm redirect="list">
            <DateTimeInput source="from" validate={required()} initialValue={new Date()} />
            <DateTimeInput source="to" validate={required()} initialValue={new Date()} />

            <ReferenceArrayInput source="equipmentIds" reference="equipments">
                <AutocompleteArrayInput />
            </ReferenceArrayInput>

            <FormDataConsumer >
                {formDataProps => (
                    <OrderOrigin {...formDataProps} />
                )}
            </FormDataConsumer>

        </SimpleForm>
    </Create>
);
