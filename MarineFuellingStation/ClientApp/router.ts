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

    { path: '/produce/buyboard', component: resolve => require(['./components/produce/buyboard.vue'], resolve) },
    { path: '/produce/planboard', component: resolve => require(['./components/produce/planboard.vue'], resolve) },
    { path: '/produce/planboard/:id/:from', component: resolve => require(['./components/sales/plandetail.vue'], resolve) },
    { path: '/produce/assay', component: resolve => require(['./components/produce/assay.vue'], resolve) },
    { path: '/produce/unload', component: resolve => require(['./components/produce/unload.vue'], resolve) },
    { path: '/produce/unloadaudit', component: resolve => require(['./components/produce/unloadaudit.vue'], resolve) },
    { path: '/produce/load/:ordertype', component: resolve => require(['./components/produce/load.vue'], resolve) },
    { path: '/produce/load/:oid/:ordertype', component: resolve => require(['./components/produce/load.vue'], resolve) },
    { path: '/produce/loadoil', component: resolve => require(['./components/produce/loadoil.vue'], resolve) },
    { path: '/produce/landload', component: resolve => require(['./components/produce/landload.vue'], resolve) },
    { path: '/produce/landload/:oid', component: resolve => require(['./components/produce/landload.vue'], resolve) },
    { path: '/produce/movestore', component: resolve => require(['./components/produce/movestore.vue'], resolve) },
    { path: '/produce/movestoreact', component: resolve => require(['./components/produce/movestoreact.vue'], resolve) },

    { path: '/oilstore/inout', component: resolve => require(['./components/oilstore/inout.vue'], resolve) },
    { path: '/oilstore/store', component: resolve => require(['./components/oilstore/store.vue'], resolve) },
    { path: '/oilstore/product', component: resolve => require(['./components/oilstore/product.vue'], resolve) },
    { path: '/oilstore', component: resolve => require(['./components/oilstore/oilstore.vue'], resolve) },
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
    { path: '/ydui', component: resolve => require(['./components/ydui/ydui.vue'], resolve) },
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