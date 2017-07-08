import Vue from 'vue';
import VueRouter from 'vue-router';
import YDUI from 'vue-ydui';

Vue.use(YDUI);
Vue.use(VueRouter);

const routes = [
    { path: '/report/oilstore', component: require('./components/report/oilstore/oilstore.vue') },
    { path: '/order/plan', component: require('./components/order/plan.vue') },
    { path: '/', component: require('./components/home/home.vue') },
    { path: '/counter', component: require('./components/counter/counter.vue') },
    { path: '/fetchdata', component: require('./components/fetchdata/fetchdata.vue') },
    { path: '/ydui', component: require('./components/ydui/ydui.vue') }
];

export var bus = new Vue({
    el: '#app-root',
    router: new VueRouter({ mode: 'history', routes: routes }),
    render: h => h(require('./components/app/app.vue'))
});
