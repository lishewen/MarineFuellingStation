import Vue from 'vue';
import VueRouter from 'vue-router';
import store from './store'

Vue.use(VueRouter);

const routes = [
    { path: '/report/oilstore', component: require('./components/oilstore/oilstore.vue') },
    { path: '/sales/plan', component: require('./components/sales/plan.vue') },
    { path: '/funcmenu', component: require('./components/funcmenu/funcmenu.vue') },
    { path: '/', component: require('./components/home/home.vue') },
    { path: '/counter', component: require('./components/counter/counter.vue') },
    { path: '/fetchdata', component: require('./components/fetchdata/fetchdata.vue') },
    { path: '/ydui', component: require('./components/ydui/ydui.vue') },
    {
        //服务端一律跳转到这个URL上
        path: '/wxhub/:id/:redirectUrl', redirect: to => {
            /**
            * 通过dispatch触发保存openid的action
            * 将URL上的OPENID保存到store中
            */
            store.commit('setUserName', decodeURI(to.params.id));
            //在回跳到需要来访的正确页面
            return decodeURI(to.params.redirectUrl);
        }
    }
];

var router = new VueRouter({ mode: 'hash', routes: routes });
//在每次使用路由时对username进行校验，如果不存在则获取username到前端
router.beforeEach((to, from, next) => {
    console.log(store.state)
    if (store.state.username != "") {
        next();
    } else {
        console.log(to);
        window.location.href = "/home/GetOpenId?id=" + encodeURIComponent(to.fullPath);
    }
});
export default router;