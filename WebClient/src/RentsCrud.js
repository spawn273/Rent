import React from 'react';
import { SearchInput, useUpdate, NumberInput, NumberField, ShowController, BooleanInput, useTranslate, ShowView, usePermissions, Create, ReferenceField, ReferenceArrayField, SingleFieldList, ChipField, useGetMany, ArrayInput, CheckboxGroupInput, ReferenceInput, AutocompleteInput, SelectInput, FormDataConsumer, AutocompleteArrayInput, ReferenceArrayInput, SelectArrayInput, SimpleFormIterator, required, List, Show, Edit, SimpleForm, TextInput, DateTimeInput, ReferenceManyField, EditButton, SimpleShowLayout, Datagrid, TextField, DateField } from 'react-admin';

import { Form, Field } from 'react-final-form'
import arrayMutators from 'final-form-arrays'
import { FieldArray } from 'react-final-form-arrays'
import MUIDataTable from "mui-datatables";
import { useSelector, useDispatch } from 'react-redux';


import { makeStyles, Chip } from '@material-ui/core';
import Table from '@material-ui/core/Table';
import TableBody from '@material-ui/core/TableBody';
import TableCell from '@material-ui/core/TableCell';
import TableContainer from '@material-ui/core/TableContainer';
import TableHead from '@material-ui/core/TableHead';
import TableRow from '@material-ui/core/TableRow';
import Paper from '@material-ui/core/Paper';

import { Link } from 'react-router-dom';
import { stringify } from 'query-string';

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

const useQuickFilterStyles = makeStyles(theme => ({
    chip: {
        marginBottom: theme.spacing(1),
    },
}));
const QuickFilter = ({ label }) => {
    const translate = useTranslate();
    const classes = useQuickFilterStyles();
    return <Chip className={classes.chip} label={translate(label)} />;
};

const PostFilter = (props) => (
    <Filter {...props}>
        <SearchInput source="q" alwaysOn />
        <BooleanInput label="custom.rents.filters.closed" source="closed" alwaysOn/>
        <BooleanInput label="custom.rents.filters.today" source="endLte" alwaysOn 
            parse={v => v ? new Date().toISOString() : null} format={v => v ? true : false}/>
    </Filter>
);

export const RentsList = ({ permissions, ...props }) => {
    const shop = useSelector((state) => state.shop);
    const isMyShop = permissions && permissions.isMyShop(shop);
    return <List {...props} filters={<PostFilter />} filterDefaultValues={{ endLte: Date.UTC() }}
        actions={<ListActions create={isMyShop} {...props} />}
        filter={{ shopId: shop }}>
        <Datagrid rowClick="show">
            <TextField source="id" />
            <TextField source="customer" />
            <DateField showTime source="from" />
            <DateField showTime source="to" />
            <DateField showTime source="closed" />
            <NumberField source="payment" />
            
            <ReferenceArrayField reference="equipments" source="equipmentIds">
                <SingleFieldList>
                    <ChipField source="name" />
                </SingleFieldList>
            </ReferenceArrayField>
        </Datagrid>
    </List>
};

const ShowActions = ({ permissions, basePath, data, record, resource }) => {
    const loaded = data != null;
    const isMyShop = loaded && permissions && permissions.isMyShop(data.shopId);
    const open = loaded && data.closed == null
    return (
    <TopToolbar>
        { isMyShop && open && <ApproveButton record = {data}/>}
        { isMyShop && <EditButton basePath={basePath} record={data} /> }
    </TopToolbar>
)};

const ApproveButton = ({ record }) => {
    const [approve, { loading }] = useUpdate('rents', record.id, { ...record, closed: new Date() }, record);
    return <Button label="custom.rents.show.close" onClick={approve} disabled={loading} />;
};

export const RentsShow = ({permissions, ...props }) => (
    <ShowController  {...props} >
        {controllerProps =>
            <ShowView actions={<ShowActions permissions={permissions} />} {...props} {...controllerProps}>
                <SimpleShowLayout>
                    <TextField source="id"/>

                    <ReferenceField reference="shops" source="shopId">
                        <TextField source="name" />
                    </ReferenceField>

                    <TextField source="customer" />

                    <DateField showTime source="from" />
                    <DateField showTime source="to" />
                    <DateField showTime source="closed" />
                    <NumberField source="payment" />

                    <ReferenceArrayField reference="equipments" source="equipmentIds" >
                        <SingleFieldList>
                            <ChipField source="name" />
                        </SingleFieldList>
                    </ReferenceArrayField>

                    <RentTable record={ controllerProps.record } />
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
                    return isMyShop && <TextInput {...props} source="customer" validate={required()}/>
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
                    return isMyShop && <DateTimeInput {...props} source="closed" />
                }}
            </FormDataConsumer>
            <FormDataConsumer >
                {({ formData: record, ...props }) => {
                    const isMyShop = record && permissions && permissions.isMyShop(record.shopId);
                    return isMyShop && <NumberInput {...props} source="payment" />
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
                    return isMyShop && <RentTable record={record} />
                }}
            </FormDataConsumer>
        </SimpleForm>
    </Edit>
)};

export const RentsCreate = (props) => (
    <Create {...props}>
        <SimpleForm redirect="list">
            <TextInput source="customer" validate={required()}/>

            <DateTimeInput source="from" validate={required()} initialValue={new Date()} />
            <DateTimeInput source="to" validate={required()} initialValue={new Date()} />
            <NumberInput source="payment" />

            <ReferenceArrayInput source="equipmentIds" reference="equipments">
                <AutocompleteArrayInput />
            </ReferenceArrayInput>

            <FormDataConsumer >
                {formDataProps => (
                    <RentTable record = {formDataProps.formData} />
                )}
            </FormDataConsumer>

        </SimpleForm>
    </Create>
);



const RentTable = ({ record }) => {

    let equipmentIds = record.equipmentIds;
    if (!equipmentIds) {
        equipmentIds = [];
    }

    const translate = useTranslate();

    let eqRequest = useGetMany('equipments', equipmentIds);

    let typesIds = [];
    if (eqRequest.loaded) {
        typesIds = eqRequest.data.map((e) => e.equipmentTypeId);
    }

    let typesRequest = useGetMany('equipmentTypes', typesIds);

    let allLoaded = (arr) => {
        if (!arr) {
            return false;
        }
        for (let i = 0; i < arr.length; i++) {
            if (!arr[i]) {
                return false;
            }
        }
        return true;
    }

    const loaded = equipmentIds.length == 0 || eqRequest.loaded && typesRequest.loaded && allLoaded(eqRequest.data) && allLoaded(typesRequest.data);
    if (!loaded) {
        return null;
    }

    let types = typesRequest.data.reduce(function(acc, cur, i) {
        acc[cur.id] = cur;
        return acc;
    }, {});

    return (
        <TableContainer component={Paper}>
            <Table size="small">
                <TableHead>
                    <TableRow>
                        <TableCell>Id</TableCell>
                        <TableCell>{translate('custom.rents.table.type')}</TableCell>
                        <TableCell>{translate('resources.equipments.fields.name')}</TableCell>
                        <TableCell>{translate('resources.equipments.fields.pricePerHour')}</TableCell>
                        <TableCell>{translate('resources.equipments.fields.pricePerDay')}</TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {eqRequest.data.map((row) => (
                        <TableRow key={row.id}>
                            <TableCell component="th" scope="row">{row.id}</TableCell>
                            <TableCell>{types[row.equipmentTypeId] ? types[row.equipmentTypeId].name : ""}</TableCell>
                            <TableCell>{row.name}</TableCell>
                            <TableCell>{types[row.equipmentTypeId] ? types[row.equipmentTypeId].pricePerHour : ""}</TableCell>
                            <TableCell>{types[row.equipmentTypeId] ? types[row.equipmentTypeId].pricePerDay : ""}</TableCell>
                        </TableRow>
                    ))}
                </TableBody>
            </Table>
        </TableContainer>
    );
};
