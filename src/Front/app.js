'use strict';

var isProduction = !!process.argv.filter((val) => val.startsWith("--production"))[0];
var apiUri = process.argv.filter((val) => val.startsWith("--env=")).map(val => val.substr(6))[0] || 'http://localhost';
var port = isProduction && 80 || 9000;
const express = require('express');
const app = express();

app.use((req, res, next) => (console.log('Request Type:', req.method, req.url), next()));

if (!isProduction) {
    const webpack = require('webpack');
    const middlewareFactory = require('webpack-dev-middleware');
    const webpackConfig = require('./webpack.config');
    const compiler = webpack(webpackConfig({ api: apiUri }));
    var middleware = middlewareFactory(compiler);
    app.use(middleware)
        .use((req, res, next) => {
            req.url = req.originalUrl = '/';
            middleware(req, res, next);
        });
} else {
    app.use(express.static('dist'));
    app.use("*", express.static(__dirname + '/dist/index.html'));
}

app.listen(port, () => console.log('Example app listening on port ' + port + '!'));
