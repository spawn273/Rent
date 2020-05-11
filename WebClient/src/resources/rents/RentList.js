import { makeStyles } from '@material-ui/core';
import React, { cloneElement } from 'react';
import { BooleanInput, ChipField, CreateButton, Datagrid, DateField, ExportButton, Filter, List, NumberField, ReferenceArrayField, sanitizeListRestProps, SearchInput, SingleFieldList, TextField, TopToolbar } from 'react-admin';
import { useSelector } from 'react-redux';

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

const ListFilter = (props) => (
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

const RentList = ({ permissions, ...props }) => {
    const classes = listStyles();
    const shop = useSelector((state) => state.shop);
    const isMyShop = permissions && permissions.isMyShop(shop);
    return <List {...props} filters={<ListFilter />} 
        actions={<ListActions create={isMyShop} {...props} />}
        filter={{ shopId: shop }}>
        <Datagrid optimized classes={{ headerCell: classes.headerCell }}  rowClick="show">
            <TextField source="id" />
            <TextField source="customer" />
            <DateField showTime source="from" />
            <DateField showTime source="to" />
            <DateField showTime source="closed" />
            <NumberField source="payment" />

            <ReferenceArrayField reference="equipment" source="equipmentIds">
                <SingleFieldList>
                    <ChipField source="name" />
                </SingleFieldList>
            </ReferenceArrayField>

            <TextField source="comment" />
        </Datagrid>
    </List>
};

export default RentList;
