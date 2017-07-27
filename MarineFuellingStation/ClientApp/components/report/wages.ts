import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import moment from "moment";
@Component({
    components: {
        WeuiSearch: require('../weui-search/search.vue')
    }
})
export default class MyOrderComponent extends Vue {
    carNo: string = "";
    sv: string = "";
    show1: boolean = false;
    show2: boolean = false;
    selecteddate: string = moment(new Date()).format('YYYY-MM-DD');
    picked: object = ['生产部'];
    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 工资');
    };

    change(label: string, tabkey: string) {
        console.log(label);
        this.$emit('setTitle', this.$store.state.username + ' ' + label);
    }
}