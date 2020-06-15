import * as React from "react";
import { useState, useEffect } from "react";
import { Card, CardHeader, CardContent } from '@material-ui/core';
import { Loading, fetchStart, fetchEnd } from 'react-admin';
import { useDispatch } from 'react-redux';
import { Title } from 'react-admin';
import Pie from './pie';
import Line from './line';

const styles = {
    flex: { display: 'flex' },
    flexColumn: { display: 'flex', flexDirection: 'column' },
    leftCol: { flex: 1, marginRight: '1em' },
    rightCol: { flex: 1, marginLeft: '1em' },
    singleCol: { marginTop: '2em', marginBottom: '2em' },
    pie: { height: '300px' },
    line: { height: '300px'},
};
           
export default () => {
    const dispatch = useDispatch();
    const [loading, setLoading] = useState(true);
    const [dashboards, setDashboards] = useState({});

    async function fetchData() {
        setLoading(true);
        const res = await fetch(`${process.env.REACT_APP_API_URL}/admin/dashboards/all`);
        res.json()
            .then(res => setDashboards(res))
            .finally(() => {
                setLoading(false);
                dispatch(fetchEnd());
            });
    }

    useEffect(() => {
        fetchData();
    }, []);

    if (loading) {
        return <Loading />;
    }

    return (
        <div style={styles.flex}>
            <div style={styles.leftCol}>
                <div style={styles.flex}>

                </div>
                <div style={styles.singleCol}>
                    <Card>
                        <CardHeader title={'Аренды по магазину'} />
                        <CardContent style={styles.line}>
                            <Line data={dashboards.rentsPerShop}></Line>
                        </CardContent>
                    </Card>
                </div>
                <div style={styles.singleCol}>
                </div>
                <div style={styles.singleCol}>
                </div>
            </div>
            <div style={styles.rightCol}>
                <div style={styles.singleCol}>
                    <Card>
                        <CardHeader title={'Аренды по типу оборудования'} />
                        <CardContent style={styles.pie}>
                            <Pie data={dashboards.rentsPerEquipmentType}></Pie>
                        </CardContent>
                    </Card>
                </div>
                <div style={styles.singleCol}>
                    <Card>
                        <CardHeader title={'Аренды по сотруднику'} /> 
                        <CardContent style={styles.pie}>
                            <Pie data={dashboards.rentsPerEmployee}></Pie>
                        </CardContent>
                    </Card>
                </div>
            </div>
        </div>
    )
};
