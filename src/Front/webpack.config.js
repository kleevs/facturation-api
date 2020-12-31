'use strict';

const webpack = require('webpack');
const path = require('path');
const tsconfig = require('./tsconfig.json');
const CopyWebpackPlugin = require('copy-webpack-plugin');

const alias = {};

for (var key in tsconfig.paths) {
    alias[key] = path.resolve(__dirname, tsconfig.paths[key][0]);
}

module.exports = (env) => ({
    mode: 'development',
    entry: [
        path.resolve(__dirname, 'src/main.tsx'),
        path.resolve(__dirname, 'src/style.less')
    ],
    output: {
        filename: 'bundle.js',
        path: path.resolve(__dirname, 'dist')
    },
    plugins: [
        new CopyWebpackPlugin([
            { from: 'node_modules/font-awesome/fonts', to: './fonts' },
            { from: 'src/index.html', to: '.' }
        ]),
        new webpack.DefinePlugin({
            __API__: '"' + (env.api) + '"'
        })
    ],
    module: {
        rules: [
            { test: /\.tsx?$/, loader: "ts-loader", exclude: /node_modules/ },
            { test: /\.png$/, use: ['url-loader'] },
            {
                test: /\.less$/, use: [{
                    loader: 'file-loader',
                    options: { name: "style.css" },
                }, {
                    loader: 'string-replace-loader',
                    options: {
                        search: "url\\('\\.\\.\\/",
                        replace: "url('",
                        flags: 'g'
                    }
                }, 'less-loader']
            }
        ]
    },
    resolve: {
        modules: [
            path.resolve(__dirname, 'src'),
            path.resolve(__dirname, 'node_modules')
        ],
        alias: alias,
        extensions: ['.tsx', '.ts', '.js']
    },
    devServer: {
        contentBase: path.join(__dirname, 'src'),
        compress: true,
        historyApiFallback: true
    }
});