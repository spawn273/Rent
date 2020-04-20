import React, { useState, useEffect } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { useGetList, useGetOne, Loading, Error,usePermissions } from 'react-admin';
import Select from '@material-ui/core/Select';
import MenuItem from '@material-ui/core/MenuItem';
import { changeShop } from './actions'

let _fetched = false;

const EmployeeInfo = () => {
    const { loading, permissions } = usePermissions();
    const dispatch = useDispatch();
    if (!loading) {
        dispatch(changeShop(permissions.shop))
    }
    
    return null;
};

export default EmployeeInfo;
