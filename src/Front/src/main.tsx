import 'jquery';
import 'popper.js';
import 'bootstrap';
import './tool/sb-admin-2';
import * as ReactDOM from 'react-dom';
import * as React from 'react';
import Router from './view/router';

ReactDOM.render(
  <Router />,
  document.getElementById('root')
)