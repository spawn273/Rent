import { Drawer, makeStyles } from '@material-ui/core';
import React, { Fragment, useCallback } from 'react';
import { Button, ChipField, DateField, DateTimeInput, Edit, EditButton, NumberField, ReferenceArrayField, ReferenceField, RichTextField, SaveButton, ShowController, ShowView, SimpleForm, SimpleShowLayout, SingleFieldList, TextField, TextInput, Toolbar, TopToolbar } from 'react-admin';
import { useForm } from 'react-final-form';
import { Route } from 'react-router';
import { useHistory } from "react-router-dom";
import EquipmentTable from './EquipmentTable';

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

const RentShow = ({ permissions, classes, ...props }) => {
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

                        <ReferenceArrayField reference="equipment" source="equipmentIds" >
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

export default RentShow;
