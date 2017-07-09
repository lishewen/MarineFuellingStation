import Vue from 'vue';
import Vuex from 'vuex'

Vue.use(Vuex);

const state = {
    openid: '',
    userid: '',
    username: ''
};

const mutations = {
    setOpenId: (state, amount) => {
        state.openid = amount;
        console.log(state.openid);
    },
    setUserId: (state, amount) => {
        state.userid = amount;
        console.log(state.userid);
    },
    setUserName: (state, amount) => {
        state.username = amount;
        console.log(state.userid);
    }
};

export default new Vuex.Store({
    state,
    mutations
});