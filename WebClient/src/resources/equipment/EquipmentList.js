import React from 'react';
import { DateField, ReferenceArrayField, SingleFieldList, ChipField, Datagrid, List, ReferenceField, TextField } from 'react-admin';
import { useSelector } from 'react-redux';

const EquipmentList = props => {
    const shop = useSelector((state) => state.shop);
    return (
        <List {...props} filter={{ shopId: shop }}>
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
