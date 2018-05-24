import Vue from 'vue';
import VueRouter, { RouteConfig } from 'vue-router';
import store from './store';

Vue.use(VueRouter);

const routes: RouteConfig[] = [
    { path: '/sales/plan/:iswaterdept', component: require('./components/sales/plan.vue').default },
    { path: '/sales/plan', component: require('./components/sales/plan.vue').default },
    { path: '/sales/plan/:id/:from', component: require('./components/sales/plandetail.vue').default },
    { path: '/sales/order', component: require('./components/sales/order.vue').default },
    { path: '/sales/order/:id/:from', component: require('./components/sales/orderdetail.vue').default },
    { path: '/sales/myorder', component: require('./components/sales/myorder.vue').default },
    { path: '/sales/myclient', component: require('./components/sales/myclient.vue').default },
    { path: '/sales/myclient/:id', component: require('./components/sales/myclientdetail.vue').default },
    { path: '/sales/myclient/:id/:applier', component: require('./components/sales/applytomyclient.vue').default },
    { path: '/sales/clienttocompany/:cid/:coid/:coname', component: require('./components/sales/clienttocompany.vue').default },
    { path: '/sales/boatclean', component: require('./components/sales/boatclean.vue').default },
    { path: '/sales/auditing/:islandplan', component: require('./components/sales/auditing.vue').default },

    { path: '/produce/buyboard', component: require('./components/produce/buyboard.vue').default },
    { path: '/produce/planboard', component: require('./components/produce/planboard.vue').default },
    { path: '/produce/planboard/:id/:from', component: require('./components/sales/plandetail.vue').default },
    { path: '/produce/assay', component: require('./components/produce/assay.vue').default },
    { path: '/produce/unload', component: require('./components/produce/unload.vue').default },
    { path: '/produce/unloadaudit', component: require('./components/produce/unloadaudit.vue').default },
    { path: '/produce/load/:ordertype', component: require('./components/produce/load.vue').default },
    { path: '/produce/load/:oid/:ordertype', component: require('./components/produce/load.vue').default },
    { path: '/produce/loadoil', component: require('./components/produce/loadoil.vue').default },
    { path: '/produce/landload', component: require('./components/produce/landload.vue').default },
    { path: '/produce/landload/:oid', component: require('./components/produce/landload.vue').default },
    { path: '/produce/movestore', component: require('./components/produce/movestore.vue').default },
    { path: '/produce/movestoreact', component: require('./components/produce/movestoreact.vue').default },

    { path: '/oilstore/inout', component: require('./components/oilstore/inout.vue').default },
    { path: '/oilstore/store', component: require('./components/oilstore/store.vue').default },
    { path: '/oilstore/product', component: require('./components/oilstore/product.vue').default },
    { path: '/oilstore', component: require('./components/oilstore/oilstore.vue').default },
    { path: '/oilstore/setting', component: resolve => require(['./components/oilstore/setting.vue'], resolve) },

    { path: '/purchase/purchase', component: resolve => require(['./components/purchase/purchase.vue'], resolve) },
    { path: '/purchase/purchase/:id/:from', component: resolve => require(['./components/purchase/purchasedetail.vue'], resolve) },

    { path: '/client/client', component: resolve => require(['./components/client/client.vue'], resolve) },
    { path: '/client/client/:id', component: resolve => require(['./components/client/clientdetail.vue'], resolve) },

    { path: '/user/user', component: resolve => require(['./components/user/user.vue'], resolve) },
    { path: '/user/user/:id', component: resolve => require(['./components/user/userdetail.vue'], resolve) },

    { path: '/finance/cashier', component: resolve => require(['./components/finance/cashier.vue'], resolve) },
    { path: '/finance/cashierbc', component: resolve => require(['./components/finance/cashierbc.vue'], resolve) },
    { path: '/finance/orderlist', component: resolve => require(['./components/finance/orderlist.vue'], resolve) },
    { path: '/finance/charge', component: resolve => require(['./components/finance/charge.vue'], resolve) },
    { path: '/finance/chargelog', component: resolve => require(['./components/finance/chargelog.vue'], resolve) },
    { path: '/finance/account', component: resolve => require(['./components/finance/account.vue'], resolve) },

    { path: '/report', component: resolve => require(['./components/report/report.vue'], resolve) },
    { path: '/wages', component: resolve => require(['./components/report/wages.vue'], resolve) },

    { path: '/notice', component: resolve => require(['./components/notice/notice.vue'], resolve) },

    { path: '/oa/checkin', component: resolve => require(['./components/oa/checkin.vue'], resolve) },

    { path: '/excel/export', component: resolve => require(['./components/excel/export.vue'], resolve) },

    { path: '/funcmenu', component: resolve => require(['./components/funcmenu/funcmenu.vue'], resolve) },
    { path: '/', component: resolve => require(['./components/home/home.vue'], resolve) },
    {
        //服务端一律跳转到这个URL上
        path: '/wxhub/:id/:userid/:isSuperAdmin/:isLeader/:redirectUrl', redirect: to => {
            /**
            * 通过dispatch触发保存openid的action
            * 将URL上的OPENID保存到store中
            */
            store.commit('setUserName', decodeURI(to.params.id));
            store.commit('setUserId', decodeURI(to.params.userid));
            store.commit('setIsSuperAdmin', decodeURI(to.params.isSuperAdmin));
            store.commit('setIsLeader', decodeURI(to.params.isLeader));
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
        console.log("isSuperAdmin = " + store.state.isSuperAdmin);
        console.log("isLeader = " + store.state.isLeader);
        next();
    } else {
        console.log(to);
        window.location.href = "/home/GetOpenId?id=" + encodeURIComponent(to.fullPath);
    }
});
export default router;