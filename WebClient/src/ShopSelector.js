import React, { useState, useEffect } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { useGetList, useGetOne, Loading, Error } from 'react-admin';
import Select from '@material-ui/core/Select';
import MenuItem from '@material-ui/core/MenuItem';
import { changeShop } from './actions'

const ShopSelector = props => {
    const shop = useSelector((state) => state.shop);
    const dispatch = useDispatch();

    const { data, ids, loading, error } = useGetList(
        'shops',
        { page: 1, perPage: 10 },
        { field: 'id', order: 'ASC' }
    );

    // if (loading) return <Loading />;
    if (error) return <Error />;
    if (!data) return null;

    return (
        <span>
            Shop:
            <Select
                value={shop}
                onChange={event => dispatch(changeShop(event.target.value))}
            >
                {ids.map(id => <MenuItem key={id} value={id}>{data[id].name}</MenuItem>)}
            </Select>
        </span>
    );
};

export default ShopSelector;
