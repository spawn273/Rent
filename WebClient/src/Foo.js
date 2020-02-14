import React from 'react';
import Card from '@material-ui/core/Card';
import CardContent from '@material-ui/core/CardContent';
import { Title } from 'react-admin';
import Button from '@material-ui/core/Button';
import { useSelector, useDispatch } from 'react-redux';
import {changeShop} from './actions'

const Foo = () => {
    
    const shop = useSelector((state) => state.shop);
    const dispatch = useDispatch();
    return (
        <Card>
            <Title title="My Page" />
            <CardContent>
                <span>hui! {shop}</span>
                
                <Button
                    variant="contained"
                    // className={classes.button}
                    // color={theme === 'light' ? 'primary' : 'default'}
                    onClick={() => dispatch(changeShop(shop + 1))}
                >
                    button
                </Button>
            </CardContent>
        </Card>
    );
}

export default Foo;
