import React from 'react';
import { Filter, SearchInput, ReferenceArrayField, SingleFieldList, ChipField, Datagrid, List, ReferenceField, TextField } from 'react-admin';
import { useSelector } from 'react-redux';

const ListFilter = (props) => (
    <Filter {...props}>
        <SearchInput source="q" alwaysOn />
    </Filter>
);

const EquipmentList = props => {
    const shop = useSelector((state) => state.shop);
    return (
        <List {...props} filter={{ shopId: shop }} filters={<ListFilter />}>
        <Datagrid rowClick="edit">
                <TextField source="id" />
                <TextField source="name" />
                <ReferenceField source="equipmentTypeId" reference="equipmentTypes">
                    <TextField source="name" />
                </ReferenceField>
                <ReferenceArrayField reference="rents" source="rentIds">
                    <SingleFieldList>
                        <ChipField source="id" />
                    </SingleFieldList>
                </ReferenceArrayField>
            </Datagrid>
        </List>
    );
}

export default EquipmentList;
