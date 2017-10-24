import Vue from 'vue';
import VueRouter, { RouteConfig } from 'vue-router';
import store from './store'

Vue.use(VueRouter);

const routes: RouteConfig[] = [
    { path: '/sales/plan', component: require('./components/sales/plan.vue') },
    { path: '/sales/plan/:id/:from', component: require('./components/sales/plandetail.vue') },
    { path: '/sales/order', component: require('./components/sales/order.vue') },
    { path: '/sales/order/:id/:from', component: require('./components/sales/orderdetail.vue') },
    { path: '/sales/myorder', component: require('./components/sales/myorder.vue') },
    { path: '/sales/myclient', component: require('./components/sales/myclient.vue') },
    { path: '/sales/myclient/:id', component: require('./components/sales/myclientdetail.vue') },
    { path: '/sales/boatclean', component: require('./components/sales/boatclean.vue') },
    { path: '/sales/auditing/:islandplan', component: require('./components/sales/auditing.vue') },

    { path: '/produce/buyboard', component: require('./components/produce/buyboard.vue') },
    { path: '/produce/planboard', component: require('./components/produce/planboard.vue') },
    { path: '/produce/planboard/:id/:from', component: require('./components/sales/plandetail.vue') },
    { path: '/produce/assay', component: require('./components/produce/assay.vue') },
    { path: '/produce/unload', component: require('./components/produce/unload.vue') },
    { path: '/produce/unloadaudit', component: require('./components/produce/unloadaudit.vue') },
    { path: '/produce/load', component: require('./components/produce/load.vue') },
    { path: '/produce/loadoil', component: require('./components/produce/loadoil.vue') },
    { path: '/produce/landload', component: require('./components/produce/landload.vue') },
    { path: '/produce/movestore', component: require('./components/produce/movestore.vue') },
    { path: '/produce/movestoreact', component: require('./components/produce/movestoreact.vue') },

    { path: '/oilstore/inout', component: require('./components/oilstore/inout.vue') },
    { path: '/oilstore/store', component: require('./components/oilstore/store.vue') },
    { path: '/oilstore/product', component: require('./components/oilstore/product.vue') },
    { path: '/oilstore', component: require('./components/oilstore/oilstore.vue') },

    { path: '/purchase/purchase', component: require('./components/purchase/purchase.vue') },
    { path: '/purchase/purchase/:id/:from', component: require('./components/purchase/purchasedetail.vue') },

    { path: '/client/client', component: require('./components/client/client.vue') },
    { path: '/client/client/:id', component: require('./components/client/clientdetail.vue') },

    { path: '/user/user', component: require('./components/user/user.vue') },
    { path: '/user/user/:id', component: require('./components/user/userdetail.vue') },

    { path: '/finance/cashier', component: require('./components/finance/cashier.vue') },
    { path: '/finance/cashierbc', component: require('./components/finance/cashierbc.vue') },
    { path: '/finance/orderlist', component: require('./components/finance/orderlist.vue') },
    { path: '/finance/chargelog', component: require('./components/finance/chargelog.vue') },

    { path: '/report', component: require('./components/report/report.vue') },
    { path: '/wages', component: require('./components/report/wages.vue') },

    { path: '/notice', component: require('./components/notice/notice.vue') },

    { path: '/funcmenu', component: require('./components/funcmenu/funcmenu.vue') },
    { path: '/', component: require('./components/home/home.vue') },
    {
        //服务端一律跳转到这个URL上
        path: '/wxhub/:id/:userid/:isSuperAdmin/:redirectUrl', redirect: to => {
            /**
            * 通过dispatch触发保存openid的action
            * 将URL上的OPENID保存到store中
            */
            store.commit('setUserName', decodeURI(to.params.id));
            store.commit('setUserId', decodeURI(to.params.userid));
            store.commit('setIsSuperAdmin', decodeURI(to.params.isSuperAdmin));
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
        console.log(store.state.isSuperAdmin);
        next();
    } else {
        console.log(to);
        window.location.href = "/home/GetOpenId?id=" + encodeURIComponent(to.fullPath);
    }
});
export default router;