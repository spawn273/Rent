import React from 'react';
import Card from '@material-ui/core/Card';
import CardContent from '@material-ui/core/CardContent';
import { Title } from 'react-admin';
import Button from '@material-ui/core/Button';
import { useSelector, useDispatch } from 'react-redux';

const Foo = () => {
    
    const shop = useSelector((state) => state.shop);
    const dispatch = useDispatch();
    return (
        <Card>
            <Title title="My Page" />
            <CardContent>
                <span> {shop}</span>
                
                <Button
                    variant="contained"
                    // className={classes.button}
                    // color={theme === 'light' ? 'primary' : 'default'}
                >
                    button
                </Button>
            </CardContent>
        </Card>
    );
}

export default Foo;
