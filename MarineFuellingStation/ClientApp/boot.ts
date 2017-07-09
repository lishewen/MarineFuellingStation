import Vue from 'vue';
import YDUI from 'vue-ydui';
import { sync } from 'vuex-router-sync'
import store from './store';
import router from './router';

Vue.use(YDUI);

sync(store, router) //路由状态同步组件.

export default new Vue({
    el: '#app-root',
    store,
    router,
    render: h => h(require('./components/app/app.vue'))
});
