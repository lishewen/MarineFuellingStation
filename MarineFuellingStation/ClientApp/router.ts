import Vue from 'vue';
import VueRouter, { RouteConfig } from 'vue-router';
import store from './store'

Vue.use(VueRouter);

const routes: RouteConfig[] = [
    { path: '/sales/plan', component: require('./components/sales/plan.vue').default },
    { path: '/sales/plan/:id/:from', component: require('./components/sales/plandetail.vue').default },
    { path: '/sales/order', component: require('./components/sales/order.vue').default },
    { path: '/sales/order/:id/:from', component: require('./components/sales/orderdetail.vue').default },
    { path: '/sales/myorder', component: require('./components/sales/myorder.vue').default },
    { path: '/sales/myclient', component: require('./components/sales/myclient.vue').default },
    { path: '/sales/myclient/:id', component: require('./components/sales/myclientdetail.vue').default },
    { path: '/sales/boatclean', component: require('./components/sales/boatclean.vue').default },
    { path: '/sales/auditing/:islandplan', component: require('./components/sales/auditing.vue').default },

    { path: '/produce/buyboard', component: require('./components/produce/buyboard.vue').default },
    { path: '/produce/planboard', component: require('./components/produce/planboard.vue').default },
    { path: '/produce/planboard/:id/:from', component: require('./components/sales/plandetail.vue').default },
    { path: '/produce/assay', component: require('./components/produce/assay.vue').default },
    { path: '/produce/unload', component: require('./components/produce/unload.vue').default },
    { path: '/produce/unloadaudit', component: require('./components/produce/unloadaudit.vue').default },
    { path: '/produce/load', component: require('./components/produce/load.vue').default },
    { path: '/produce/loadoil', component: require('./components/produce/loadoil.vue').default },
    { path: '/produce/landload', component: require('./components/produce/landload.vue').default },
    { path: '/produce/movestore', component: require('./components/produce/movestore.vue').default },
    { path: '/produce/movestoreact', component: require('./components/produce/movestoreact.vue').default },

    { path: '/oilstore/inout', component: require('./components/oilstore/inout.vue').default },
    { path: '/oilstore/store', component: require('./components/oilstore/store.vue').default },
    { path: '/oilstore/product', component: require('./components/oilstore/product.vue').default },
    { path: '/oilstore', component: require('./components/oilstore/oilstore.vue').default },

    { path: '/purchase/purchase', component: require('./components/purchase/purchase.vue').default },
    { path: '/purchase/purchase/:id/:from', component: require('./components/purchase/purchasedetail.vue').default },

    { path: '/client/client', component: require('./components/client/client.vue').default },
    { path: '/client/client/:id', component: require('./components/client/clientdetail.vue').default },

    { path: '/user/user', component: require('./components/user/user.vue').default },
    { path: '/user/user/:id', component: require('./components/user/userdetail.vue').default },

    { path: '/finance/cashier', component: require('./components/finance/cashier.vue').default },
    { path: '/finance/cashierbc', component: require('./components/finance/cashierbc.vue').default },
    { path: '/finance/orderlist', component: require('./components/finance/orderlist.vue').default },
    { path: '/finance/charge', component: require('./components/finance/charge.vue').default },
    { path: '/finance/chargelog', component: require('./components/finance/chargelog.vue').default },
    { path: '/finance/account', component: require('./components/finance/account.vue').default },

    { path: '/report', component: require('./components/report/report.vue').default },
    { path: '/wages', component: require('./components/report/wages.vue').default },

    { path: '/notice', component: require('./components/notice/notice.vue').default },

    { path: '/funcmenu', component: require('./components/funcmenu/funcmenu.vue').default },
    { path: '/', component: require('./components/home/home.vue').default },
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