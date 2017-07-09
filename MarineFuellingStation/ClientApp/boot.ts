import Vue from 'vue';
import VueRouter from 'vue-router';
import YDUI from 'vue-ydui';
import store from './store'

Vue.use(YDUI);
Vue.use(VueRouter);

const routes = [
    { path: '/report/oilstore', component: require('./components/report/oilstore/oilstore.vue') },
    { path: '/order/plan', component: require('./components/order/plan.vue') },
    { path: '/', component: require('./components/home/home.vue') },
    { path: '/counter', component: require('./components/counter/counter.vue') },
    { path: '/fetchdata', component: require('./components/fetchdata/fetchdata.vue') },
    { path: '/ydui', component: require('./components/ydui/ydui.vue') },
    {
        //服务端一律跳转到这个URL上
        path: '/home/:id/:redirectUrl/', redirect: to => {
            /**
            * 通过dispatch触发保存openid的action
            * 将URL上的OPENID保存到store中
            */
            store.commit({
                type: 'setOpenId',
                amount: to.params.id
            })
            //在回跳到需要来访的正确页面
            return `/${to.params.redirectUrl}/`
        }
    }
];

export default new Vue({
    el: '#app-root',
    router: new VueRouter({ mode: 'history', routes: routes }),
    render: h => h(require('./components/app/app.vue'))
});
