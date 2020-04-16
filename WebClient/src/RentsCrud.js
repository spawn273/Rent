import React from 'react';
import { ShowController, useTranslate, ShowView, usePermissions, Create, ReferenceField, ReferenceArrayField, SingleFieldList, ChipField, useGetMany, ArrayInput, CheckboxGroupInput, ReferenceInput, AutocompleteInput, SelectInput, FormDataConsumer, AutocompleteArrayInput, ReferenceArrayInput, SelectArrayInput, SimpleFormIterator, required, List, Show, Edit, SimpleForm, TextInput, DateTimeInput, ReferenceManyField, EditButton, SimpleShowLayout, Datagrid, TextField, DateField } from 'react-admin';

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

import { cloneElement, useMemo } from 'react';
import PropTypes from 'prop-types';
import {
    TopToolbar, Filter, CreateButton, ExportButton, Button, sanitizeListRestProps,
} from 'react-admin';
import IconEvent from '@material-ui/icons/Event';
import { Fragment } from 'react';
import { useFormState } from 'react-final-form'

const ListActions = ({
    create,
    currentSort,
    className,
    resource,
    filters,
    displayedFilters,
    exporter, // you can hide ExportButton if exporter = (null || false)
    filterValues,
    permanentFilter,
    hasCreate, // you can hide CreateButton if hasCreate = false
    hasShow,
    basePath,
    selectedIds,
    onUnselectItems,
    showFilter,
    maxResults,
    total,
    ...rest
}) => {
    return (
        <TopToolbar className={className} {...rest}>
            {filters && cloneElement(filters, {
                resource,
                showFilter,
                displayedFilters,
                filterValues,
                context: 'button',
            })}
            {create && <CreateButton basePath={basePath} />}
            <ExportButton
                disabled={total === 0}
                resource={resource}
                sort={currentSort}
                filter={{ ...filterValues, ...permanentFilter }}
                exporter={exporter}
                maxResults={maxResults}
            />
            {/* Add your custom actions */}

        </TopToolbar>
    )
};

ListActions.defaultProps = {
    selectedIds: [],
    onUnselectItems: () => null,
};

const PostFilter = (props) => (
    <Filter {...props}>
        <TextInput label="Search" source="q" alwaysOn />
        <TextInput label="Title" source="title" defaultValue="Hello, World!" />
    </Filter>
);

export const RentsList = ({ permissions, ...props }) => {
    const shop = useSelector((state) => state.shop);
    const isMyShop = permissions && permissions.isMyShop(shop);
    return <List {...props} filters={<PostFilter />} actions={<ListActions create={isMyShop} {...props} />} filter={{ shopId: shop }}>
        <Datagrid rowClick="show">
            <TextField source="id" />
            <TextField source="customer" />
            <DateField showTime source="from" />
            <DateField showTime source="to" />
            
            <ReferenceManyField reference="equipments" target="equipmentIds">
                <SingleFieldList>
                    <ChipField source="name" />
                </SingleFieldList>
            </ReferenceManyField>
        </Datagrid>
    </List>
};

const RentsShowActions = ({ permissions, basePath, data, record, resource }) => {
    const isMyShop = record && permissions && permissions.isMyShop(record.shopId);
    return (
    <TopToolbar>
        { isMyShop && <EditButton basePath={basePath} record={data} /> }
    </TopToolbar>
)};

export const RentsShow = ({permissions, ...props }) => (
    <ShowController  {...props} >
        {controllerProps =>
            <ShowView actions={<RentsShowActions permissions={permissions} record={controllerProps.record} />} {...props} {...controllerProps}>
                <SimpleShowLayout>
                    <TextField source="id"/>

                    <ReferenceField reference="shops" source="shopId">
                        <TextField source="name" />
                    </ReferenceField>

                    <TextField source="customer" />

                    <DateField showTime source="from" />
                    <DateField showTime source="to" />

                    <ReferenceArrayField reference="equipments" source="equipmentIds" >
                        <SingleFieldList>
                            <ChipField source="name" />
                        </SingleFieldList>
                    </ReferenceArrayField>

                    <RentTable equipmentIds={ controllerProps.record ?
                        controllerProps.record.equipmentIds :
                        null
                    } />
                </SimpleShowLayout>
            </ShowView>
        }
    </ShowController>
);

export const RentsEdit = ({permissions, ...props }) => {
    return (
    <Edit {...props}>
        <SimpleForm >
            <TextInput disabled source="id" />
            <FormDataConsumer >
                {({ formData: record, ...props }) => {
                    const isMyShop = record && permissions && permissions.isMyShop(record.shopId);
                    return isMyShop && <TextInput {...props} source="customer" />
                }}
            </FormDataConsumer>
            <FormDataConsumer >
                {({ formData: record, ...props }) => {
                    const isMyShop = record && permissions && permissions.isMyShop(record.shopId);
                    return isMyShop && <DateTimeInput {...props} source="from" validate={required()} />
                }}
            </FormDataConsumer>
            <FormDataConsumer >
                {({ formData: record, ...props }) => {
                    const isMyShop = record && permissions && permissions.isMyShop(record.shopId);
                    return isMyShop && <DateTimeInput {...props} source="to" validate={required()} />
                }}
            </FormDataConsumer>
            <FormDataConsumer >
                {({ formData: record, ...props }) => {
                    const isMyShop = record && permissions && permissions.isMyShop(record.shopId);
                    return isMyShop && <ReferenceArrayInput {...props} source="equipmentIds" reference="equipments">
                        <AutocompleteArrayInput />
                    </ReferenceArrayInput>
                }}
            </FormDataConsumer>
            <FormDataConsumer >
                {({ formData: record }) => {
                    const isMyShop = record && permissions && permissions.isMyShop(record.shopId);
                    return isMyShop && <RentTable equipmentIds={record.equipmentIds} />
                }}
            </FormDataConsumer>
        </SimpleForm>
    </Edit>
)};

export const RentsCreate = (props) => (
    <Create {...props}>
        <SimpleForm redirect="list">
            <TextInput source="customer" />

            <DateTimeInput source="from" validate={required()} initialValue={new Date()} />
            <DateTimeInput source="to" validate={required()} initialValue={new Date()} />

            <ReferenceArrayInput source="equipmentIds" reference="equipments">
                <AutocompleteArrayInput />
            </ReferenceArrayInput>

            <FormDataConsumer >
                {formDataProps => (
                    <RentTable {...formDataProps} />
                )}
            </FormDataConsumer>

        </SimpleForm>
    </Create>
);

const RentTable = ({ equipmentIds, ...rest }) => {
    if (!equipmentIds) {
        equipmentIds = [];
    }

    const translate = useTranslate();
    let response = useGetMany('equipments', equipmentIds);

    if (!response.loaded) {
        return null;
    }

    const data = response.data

    return (
        <TableContainer component={Paper}>
            <Table size="small">
                <TableHead>
                    <TableRow>
                        <TableCell>Id</TableCell>
                        <TableCell>{translate('resources.equipments.fields.name')}</TableCell>
                        <TableCell>{translate('resources.equipments.fields.pricePerHour')}</TableCell>
                        <TableCell>{translate('resources.equipments.fields.pricePerDay')}</TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {data.map((row) => (
                        <TableRow key={row.id}>
                            <TableCell component="th" scope="row">{row.id}</TableCell>
                            <TableCell>{row.name}</TableCell>
                            <TableCell>{row.pricePerHour}</TableCell>
                            <TableCell>{row.pricePerDay}</TableCell>
                        </TableRow>
                    ))}
                </TableBody>
            </Table>
        </TableContainer>
    );
};
