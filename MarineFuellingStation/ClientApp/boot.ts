import Vue from 'vue';
import YDUI from 'vue-ydui';
import store from './store';
import router from './router';
import axios from "axios";
import { Loading } from 'vue-ydui/dist/lib.rem/dialog';
import FastClick from "fastclick";

document.addEventListener('DOMContentLoaded', function () {
    FastClick.attach(document.body);
}, false);

Vue.use(YDUI);

axios.interceptors.request.use(function (config) {    // 这里的config包含每次请求的内容
    if (store.state.username != "") {
        config.headers['x-username'] = encodeURIComponent(store.state.username);
        config.headers['x-userid'] = encodeURIComponent(store.state.userid);
    }

    //loading效果
    Loading.open('正在提交');

    return config;
});

axios.interceptors.response.use(function (response) {    // 这里的response包含每次响应的内容

    //loading效果
    Loading.close();

    return response;
});

export default new Vue({
    store,
    router,
    render: h => h(require('./components/app/app.vue'))
}).$mount('#app-root');;
