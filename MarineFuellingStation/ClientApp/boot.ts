import Vue from 'vue';
import YDUI from 'vue-ydui';
import store from './store';
import router from './router';
import axios from "axios";
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

    return config;
});

export default new Vue({
    store,
    router,
    render: h => h(require('./components/app/app.vue').default)
}).$mount('#app-root');;
