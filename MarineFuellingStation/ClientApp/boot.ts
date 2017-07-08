import Vue from 'vue';
import VueRouter from 'vue-router';
import YDUI from 'vue-ydui';
import VueResource from 'vue-resource';

Vue.use(YDUI);
Vue.use(VueRouter);
Vue.use(VueResource);

const routes = [
    { path: '/report/oilstore', component: require('./components/report/oilstore/oilstore.vue.html') },
    { path: '/order/plan', component: require('./components/order/plan.vue.html') },
    { path: '/', component: require('./components/home/home.vue.html') },
    { path: '/counter', component: require('./components/counter/counter.vue.html') },
    { path: '/fetchdata', component: require('./components/fetchdata/fetchdata.vue.html') },
    { path: '/ydui', component: require('./components/ydui/ydui.vue.html') }
];

new Vue({
    el: '#app-root',
    router: new VueRouter({ mode: 'history', routes: routes }),
    render: h => h(require('./components/app/app.vue.html'))
});
