import React from 'react';
import { Route } from 'react-router-dom';
import Foo from './Foo';
import App from './App';

export default [
    <Route exact path="/foo" component={Foo} />,
];
