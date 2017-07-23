import Vue from 'vue';
import { Component } from 'vue-property-decorator';
@Component({
    components: {
        WeuiSearch: require('../weui-search/search.vue')
    }
})
export default class MyOrderComponent extends Vue {
    radio1: string = "1";
    radio2: string = "1";
    show1: boolean = false;
    carNo: string = "";
    sv: string = "";
    myItems1: object = [
        {
            label: '致电13907741118？',
            method: () => {
                window.location.href = 'tel: 13907741118'
            }
        },{
            label: '详细资料'
        }
    ];
   
    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 员工资料');
    };

    change(label: string, tabkey: string) {
        console.log(label);
        this.$emit('setTitle', this.$store.state.username + ' ' + label);
    }
}