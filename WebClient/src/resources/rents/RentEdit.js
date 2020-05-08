import React from 'react';
import { AutocompleteArrayInput, DateTimeInput, Edit, FormDataConsumer, NumberInput, ReferenceArrayInput, required, SimpleForm, TextInput, usePermissions } from 'react-admin';
import EquipmentTable from './EquipmentTable';

const MyShopOnly = ({children, ...props}) => { 
    const { loading, permissions } = usePermissions();
    const isMyShop = props.record && permissions && permissions.isMyShop(props.record.shopId);
    // TODO: children can be null
    const result = React.cloneElement(children, { ...props });
    return isMyShop ? result : null
};

const RentEdit = ({ permissions, ...props }) => {
    return (
        <Edit {...props} undoable={false} >
            <MyShopOnly>
                <SimpleForm >
                    <TextInput disabled source="id" />
                    <TextInput  source="customer" validate={required()} />
                    <DateTimeInput {...props} source="from" validate={required()} />
                    <DateTimeInput {...props} source="to" validate={required()} />
                    <DateTimeInput {...props} source="closed" />
                    <NumberInput {...props} source="payment" />
                    <TextInput {...props} multiline source="comment" />

                    <ReferenceArrayInput source="equipmentIds" reference="equipment">
                        <AutocompleteArrayInput />
                    </ReferenceArrayInput>
                                        
                    <FormDataConsumer >
                        {formDataProps => (
                            <EquipmentTable record={formDataProps.formData} />
                        )}
                    </FormDataConsumer>

                </SimpleForm>
            </MyShopOnly>
        </Edit>
    )
};

export default RentEdit;
