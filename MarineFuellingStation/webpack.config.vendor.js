const path = require('path');
const webpack = require('webpack');
const ExtractTextPlugin = require('extract-text-webpack-plugin');
const UglifyJsPlugin = require('uglifyjs-webpack-plugin');

module.exports = (env) => {
    const isDevBuild = !(env && env.prod);
    const extractCSS = new ExtractTextPlugin('vendor.css');

    return [{
        mode: isDevBuild ? 'development' : 'production',
        optimization: {
            minimizer: [
                // we specify a custom UglifyJsPlugin here to get source maps in production
                new UglifyJsPlugin()
            ]
        },
        stats: { modules: false },
        resolve: { extensions: ['.js'] },
        entry: {
            vendor: [
                'event-source-polyfill',
                'isomorphic-fetch',
                'vue',
                'vue-router',
                'axios',
                'vue-ydui',
                'vue-ydui/dist/ydui.rem.css',
                'ydui-district/dist/gov_province_city_area_id',
                'vuex',
                'vue-echarts',
                'moment',
                'fastclick',
                'wx-sdk-ts'
            ],
        },
        externals: {
            'vue': 'Vue',
            'vue-router': 'VueRouter',
            'vuex': 'Vuex',
            'axios': 'axios',
            'moment': 'moment',
            'fastclick': 'FastClick'
        },
        module: {
            rules: [
                { test: /\.css(\?|$)/, use: extractCSS.extract({ use: isDevBuild ? 'css-loader' : 'css-loader?minimize' }) },
                { test: /\.(png|woff|woff2|eot|ttf|svg)(\?|$)/, use: 'url-loader?limit=100000' }
            ]
        },
        output: {
            path: path.join(__dirname, 'wwwroot', 'dist'),
            publicPath: '/dist/',
            filename: '[name].js',
            library: '[name]_[hash]'
        },
        plugins: [
            extractCSS,
            new webpack.ProvidePlugin({ $: 'jquery', jQuery: 'jquery' }), // Maps these identifiers to the jQuery package (because Bootstrap expects it to be a global variable)
            new webpack.DefinePlugin({
                'process.env.NODE_ENV': isDevBuild ? '"development"' : '"production"'
            }),
            new webpack.DllPlugin({
                path: path.join(__dirname, 'wwwroot', 'dist', '[name]-manifest.json'),
                name: '[name]_[hash]'
            })
        ]//.concat(isDevBuild ? [] : [
        //new webpack.optimize.UglifyJsPlugin()
        //])
    }];
};
