import React, { useState, useEffect } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { useGetList, useGetOne, Loading, Error } from 'react-admin';
import Select from '@material-ui/core/Select';
import MenuItem from '@material-ui/core/MenuItem';
import { changeShop } from './actions'

let _fetched = false;

const EmployeeInfo = props => {
    const dispatch = useDispatch();

    const { data, loading, error } = useGetOne(
        'employees',
        1
    ,);
    if (!data) return null;

    if (!_fetched) {
        dispatch(changeShop(data.shopId)) // TODO: action 'UserInfoReceived'
        _fetched = true;
    }

    return null;
};

export default EmployeeInfo;
