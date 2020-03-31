import React from 'react';
import { ReferenceField, ReferenceArrayField, SingleFieldList, ChipField, useGetMany, ArrayInput, CheckboxGroupInput, ReferenceInput, AutocompleteInput, SelectInput, FormDataConsumer, AutocompleteArrayInput, ReferenceArrayInput, SelectArrayInput, SimpleFormIterator, required, List, Show, Edit, SimpleForm, TextInput, DateTimeInput, ReferenceManyField, EditButton, SimpleShowLayout, Datagrid, TextField, DateField } from 'react-admin';

import { Form, Field } from 'react-final-form'
import arrayMutators from 'final-form-arrays'
import { FieldArray } from 'react-final-form-arrays'
import MUIDataTable from "mui-datatables";


import { makeStyles } from '@material-ui/core/styles';
import Table from '@material-ui/core/Table';
import TableBody from '@material-ui/core/TableBody';
import TableCell from '@material-ui/core/TableCell';
import TableContainer from '@material-ui/core/TableContainer';
import TableHead from '@material-ui/core/TableHead';
import TableRow from '@material-ui/core/TableRow';
import Paper from '@material-ui/core/Paper';


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

const OrderOrigin = ({ formData, ...rest }) => {
    console.log(formData);


    let asd = useGetMany('equipments', formData.equipmentIds);
    console.log(asd);

    if (!asd.loaded) {
        return null;
    }

    const columns = ["id", "name", "Company"];

    return (
        <TableContainer component={Paper}>
            <Table size="small" aria-label="a dense table">
                <TableHead>
                    <TableRow>
                        <TableCell>Id</TableCell>
                        <TableCell>Name</TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {asd.data.map((row) => (
                        <TableRow key={row.id}>
                            <TableCell component="th" scope="row">{row.id}</TableCell>
                            <TableCell>{row.name}</TableCell>
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
