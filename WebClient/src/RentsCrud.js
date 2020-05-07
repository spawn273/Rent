import React from 'react';
import { SearchInput, translate, RichTextField, SaveButton, Loading, Toolbar,useUpdate, NumberInput, NumberField, ShowController, BooleanInput, useTranslate, ShowView, usePermissions, Create, ReferenceField, ReferenceArrayField, SingleFieldList, ChipField, useGetMany, ArrayInput, CheckboxGroupInput, ReferenceInput, AutocompleteInput, SelectInput, FormDataConsumer, AutocompleteArrayInput, ReferenceArrayInput, SelectArrayInput, SimpleFormIterator, required, List, Show, Edit, SimpleForm, TextInput, DateTimeInput, ReferenceManyField, EditButton, SimpleShowLayout, Datagrid, TextField, DateField } from 'react-admin';

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
import { push } from 'react-router-redux';
import { connect } from 'react-redux';
import { useHistory } from "react-router-dom";
import compose from 'recompose/compose';

import { Link } from 'react-router-dom';
import { stringify } from 'query-string';

import { cloneElement, useMemo, useCallback  } from 'react';
import { useForm } from 'react-final-form';
import PropTypes from 'prop-types';
import {
    TopToolbar, Filter, CreateButton, ExportButton, Button, sanitizeListRestProps,
} from 'react-admin';
import IconEvent from '@material-ui/icons/Event';
import { Fragment } from 'react';
import { useFormState } from 'react-final-form'
import { Route, Switch } from 'react-router';
import { Drawer } from '@material-ui/core';
import { withStyles } from '@material-ui/core';

var moment = require('moment');

// TODO:
// +1hour +1day


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
    hasList,
    hasEdit,
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
        <TopToolbar className={className} {...sanitizeListRestProps(rest)}>
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
        <SearchInput source="q" alwaysOn />
        <BooleanInput label="custom.rents.filters.opened" source="opened" alwaysOn />
        <BooleanInput label="custom.rents.filters.today" source="endLte" alwaysOn
            parse={v => v ? new Date().toISOString() : null} format={v => v ? true : false} />
    </Filter>
);

// TODO: remove or try make fixed headers
const listStyles = makeStyles({
    headerCell: {
        width: '100px'
    },
});

export const RentsList = ({ permissions, ...props }) => {
    const classes = listStyles();
    const shop = useSelector((state) => state.shop);
    const isMyShop = permissions && permissions.isMyShop(shop);
    return <List {...props} filters={<PostFilter />} 
        actions={<ListActions create={isMyShop} {...props} />}
        filter={{ shopId: shop }}>
        <Datagrid optimized classes={{ headerCell: classes.headerCell }}  rowClick="show">
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

            <TextField source="comment" />
        </Datagrid>
    </List>
};

// Show

const ShowActions = ({ permissions, basePath, data, record, resource }) => {
    const loaded = data != null;
    const isMyShop = loaded && permissions && permissions.isMyShop(data.shopId);
    const open = loaded && data.closed == null
    return (
        <TopToolbar>
            {isMyShop && open && <CloseButton record={data} />}
            {isMyShop && <EditButton basePath={basePath} record={data} />}
        </TopToolbar>
    )
};

const CloseButton = ({ record }) => {
    // const [approve, { loading }] = useUpdate('rents', record.id, { ...record, closed: new Date() }, record);
    let history = useHistory();
    return <Button label="custom.rents.show.close" onClick={() => {history.push(`/rents/${record.id}/show/close`)}}/>;
};

const handleClose = ({history}) => {
    history.goBack();
};

export const RentsShow = ({ permissions, classes, ...props }) => {
    const styles = closeStyles()
    return(
    <Fragment>
        <ShowController  {...props} >
            {controllerProps =>
                <ShowView actions={<ShowActions permissions={permissions} />} {...props} {...controllerProps}>
                    <SimpleShowLayout>
                        <TextField source="id" />

                        <ReferenceField reference="shops" source="shopId">
                            <TextField source="name" />
                        </ReferenceField>

                        <TextField source="customer" />

                        <DateField showTime source="from" />
                        <DateField showTime source="to" />
                        <DateField showTime source="closed" />
                        <NumberField source="payment" />

                        <RichTextField source="comment"/>

                        <ReferenceArrayField reference="equipments" source="equipmentIds" >
                            <SingleFieldList>
                                <ChipField source="name" />
                            </SingleFieldList>
                        </ReferenceArrayField>

                        <EquipmentTable record={controllerProps.record} />
                    </SimpleShowLayout>
                </ShowView>
            }
        </ShowController>
        <Route path="/rents/:id/show/close">
            {({ match }) => (
                <Drawer
                    open={!!match}
                    anchor="right"
                    onClose={() => handleClose(props)}
                >
                    <CloseForm
                        className={styles.close}
                        onCancel={() => handleClose(props)}
                        {...props}
                    />
                </Drawer>
            )}
        </Route>
    </Fragment>
)};

const CloseFormSaveButton = ({ handleSubmitWithRedirect, ...props }) => {
    const form = useForm();

    const handleClick = useCallback(() => {
        const closed = form.getFieldState('_closed');
        form.change('closed', closed.value);
        handleSubmitWithRedirect('show');
    }, [form]);

    return <SaveButton {...props} handleSubmitWithRedirect={handleClick} />;
};

const CloseFormToolbar = ({ onCancel, ...props }) => (
    <Toolbar {...props}>
        <CloseFormSaveButton />
        <Button label='ra.action.close' onClick={onCancel}></Button>
    </Toolbar>
);

const closeStyles = makeStyles({
    close: {
        width: 500
    }
});

const CloseForm = ({ onCancel, ...props }) => (
    <Edit {...props} hasShow={false}>
        <SimpleForm toolbar={<CloseFormToolbar onCancel={onCancel} />}>
            <DateTimeInput label="resources.rents.fields.closed" source="_closed" defaultValue={new Date()}/>
            <TextInput multiline source="comment" />
        </SimpleForm>
    </Edit>
);

// Edit

const MyShopOnly = ({children, ...props}) => { 
    const { loading, permissions } = usePermissions();
    const isMyShop = props.record && permissions && permissions.isMyShop(props.record.shopId);
    // TODO: children can be null
    const result = React.cloneElement(children, { ...props });
    return isMyShop ? result : null
};

export const RentsEdit = ({ permissions, ...props }) => {
    return (
        <Edit {...props} undoable={false} >
            <MyShopOnly>
                <SimpleForm >
                    <TextInput disabled source="id" />
                    <TextInput  source="customer" validate={required()} />
                    <DateTimeInput {...props} source="from" validate={required()} />
                    <DateTimeInput {...props} source="to" validate={required()} />
                    <DateTimeInput {...props} source="closed" />
                    <NumberInput {...props} source="payment" />
                    <TextInput {...props} multiline source="comment" />

                    <ReferenceArrayInput source="equipmentIds" reference="equipments">
                        <AutocompleteArrayInput />
                    </ReferenceArrayInput>
                                        
                    <FormDataConsumer >
                        {formDataProps => (
                            <EquipmentTable record={formDataProps.formData} />
                        )}
                    </FormDataConsumer>

                </SimpleForm>
            </MyShopOnly>
        </Edit>
    )
};

// Create

export const RentsCreate = (props) => (
    <Create {...props}>
        <SimpleForm redirect="list">
            <TextInput source="customer" validate={required()} />

            <DateTimeInput source="from" validate={required()} initialValue={new Date()} />
            <DateTimeInput source="to" validate={required()} initialValue={new Date()} />
            <NumberInput source="payment" />
            <TextInput multiline source="comment" />

            <ReferenceArrayInput source="equipmentIds" reference="equipments">
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



const EquipmentTable = (props) => {
    let record = props.record
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
        return <Loading />;
    }

    let types = typesRequest.data.reduce(function (acc, cur, i) {
        acc[cur.id] = cur;
        return acc;
    }, {});

    let total = 0;
    const eqPrice = eqRequest.data.reduce(function (acc, cur, i) {
        acc[cur.id] = price(types[cur.id], record.from, record.to);
        total += acc[cur.id];
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
                        <TableCell>{translate('custom.rents.table.total')}</TableCell>
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
                            <TableCell>{eqPrice[row.id]}</TableCell>
                        </TableRow>
                    ))}
                    <TableRow>
                        <TableCell />
                        <TableCell />
                        <TableCell />
                        <TableCell />
                        <TableCell />
                        <TableCell><b>{total}</b></TableCell>
                    </TableRow>
                </TableBody>
            </Table>
        </TableContainer>
    );
};

const price = (type, from, to) => {
    var price = 0;
    if (type && from && to) {
        from = moment(from);
        to = moment(to)
        if (to > from) {
            let hours = Math.round(to.diff(from, 'hours', true))
            if (hours < 6) {
                price = hours * type.pricePerHour;
            } else {
                let days = to.diff(from, 'days');
                if (days == 0) {
                    days = 1;
                }
                price = days * type.pricePerDay;
            }
        }
    }
    return price;
};
