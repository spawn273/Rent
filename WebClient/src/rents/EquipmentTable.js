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

const EquipmentTable = (props) => {
    let record = props.record
    let equipmentIds = record.equipmentIds;
    if (!equipmentIds) {
        equipmentIds = [];
    }

    const translate = useTranslate();

    let eqRequest = useGetMany('equipment', equipmentIds);

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
                        <TableCell>{translate('resources.equipment.fields.name')}</TableCell>
                        <TableCell>{translate('resources.equipment.fields.pricePerHour')}</TableCell>
                        <TableCell>{translate('resources.equipment.fields.pricePerDay')}</TableCell>
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

export default EquipmentTable;
