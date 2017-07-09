import Vue from 'vue';
import YDUI from 'vue-ydui';
import store from './store';
import router from './router';

Vue.use(YDUI);

export default new Vue({
    el: '#app-root',
    router: router,
    render: h => h(require('./components/app/app.vue'))
});
