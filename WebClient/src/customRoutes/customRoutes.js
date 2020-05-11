import React from 'react';
import { Route } from 'react-router-dom';
import Foo from './Foo';
import App1 from '../App1';

export default [
    <Route exact path="/foo" component={Foo} />,
    <Route exact path="/asd" component={App1} noLayout/>,
];
