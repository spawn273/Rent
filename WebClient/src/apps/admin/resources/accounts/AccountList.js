import React from 'react';
import { BooleanField, SearchInput, BooleanInput, Filter, ReferenceArrayInput, ReferenceInput, SelectInput, SelectArrayInput, ReferenceArrayField, SingleFieldList, ChipField, Datagrid, List, ReferenceField, TextField } from 'react-admin';
import { useSelector } from 'react-redux';
import { makeStyles } from '@material-ui/core/styles';

const useStyles = makeStyles({
    button: {
        width: '200px',
    },
});

// const ListFilter = (props) => { 
//     const classes = useStyles();
//     return (
//         <Filter {...props}>
//             <SearchInput source="q" alwaysOn />
//             <ReferenceInput  source="equipmentType" reference="equipmentTypes" alwaysOn>
//                 <SelectInput optionText="name" />
//             </ReferenceInput>
//             <ReferenceInput  source="shop" reference="shops" alwaysOn>
//                 <SelectInput optionText="name" />
//             </ReferenceInput>
//             <BooleanInput source="available" alwaysOn />
//             {/* <ReferenceArrayInput reference="equipmentTypes" source="equipmentTypes" alwaysOn>
//                 <SelectArrayInput className={classes.button} >
//                     <ChipField source="name" />
//                 </SelectArrayInput>
//             </ReferenceArrayInput> */}
//         </Filter>
//     );
// }

const FullNameField = ({ record = {} }) => <span>{record.firstName} {record.middleName} {record.lastName}</span>;
FullNameField.defaultProps = { label: 'Name' };

const AccountList = props => {
    return (
        <List {...props} bulkActionButtons={false} /*filters={<ListFilter/>}*/ filterDefaultValues={{ available: true }}>
            <Datagrid optimized rowClick="edit">
                <ReferenceField source="roleId" reference="roles" link="" >
                    <TextField source="name" />
                </ReferenceField>
                <FullNameField source="name" />
                <TextField source="userName" />
                <TextField source="" />
                <ReferenceField source="shopId" reference="shops" link="show" >
                    <TextField source="name" />
                </ReferenceField>
            </Datagrid>
        </List>
    );
}

export default AccountList;
