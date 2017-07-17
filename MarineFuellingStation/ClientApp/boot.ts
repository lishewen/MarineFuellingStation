import Vue from 'vue';
import YDUI from 'vue-ydui';
import { sync } from 'vuex-router-sync'
import store from './store';
import router from './router';
import axios from "axios";

Vue.use(YDUI);

sync(store, router) //路由状态同步组件.

axios.interceptors.request.use(function (config) {    // 这里的config包含每次请求的内容
    if (store.state.username != "") {
        config.headers['x-username'] = encodeURIComponent(store.state.username);
    }
    return config;
}, function (err) {
    return console.log(err);
});

export default new Vue({
    el: '#app-root',
    store,
    router,
    render: h => h(require('./components/app/app.vue'))
});
