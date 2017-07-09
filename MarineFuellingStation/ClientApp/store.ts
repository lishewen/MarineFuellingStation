import Vue from 'vue';
import Vuex from 'vuex'

Vue.use(Vuex);

const state = {
    openid: ''
};

const mutations = {
    setOpenId(state, amount) {
        state.openid = amount;
        console.log(state.openid);
    }
};

export default new Vuex.Store({
    state,
    mutations
});