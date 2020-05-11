import React from 'react';
import { BooleanField, Filter, ReferenceArrayInput, ReferenceInput, SelectInput, SelectArrayInput, ReferenceArrayField, SingleFieldList, ChipField, Datagrid, List, ReferenceField, TextField } from 'react-admin';
import { useSelector } from 'react-redux';
import { makeStyles } from '@material-ui/core/styles';

const useStyles = makeStyles({
    button: {
        width: '200px',
    },
});

const ListFilter = (props) => { 
    const classes = useStyles();
    return (
        <Filter {...props}>
            <ReferenceInput  source="equipmentType" reference="equipmentTypes" alwaysOn>
                <SelectInput optionText="name" />
            </ReferenceInput>
            {/* <ReferenceArrayInput reference="equipmentTypes" source="equipmentTypes" alwaysOn>
                <SelectArrayInput className={classes.button} >
                    <ChipField source="name" />
                </SelectArrayInput>
            </ReferenceArrayInput> */}
        </Filter>
    );
}

const EquipmentList = props => {
    return (
        <List {...props} filters={<ListFilter/>}>
            <Datagrid optimized>
                <TextField source="type" />
                <TextField source="name" />
                <BooleanField source="available" />
                <ReferenceField source="shopId" reference="shops">
                    <TextField source="name" />
                </ReferenceField>
            </Datagrid>
        </List>
    );
}

export default EquipmentList;
