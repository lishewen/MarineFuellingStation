import Vue from 'vue';
import { Component } from 'vue-property-decorator';

@Component
export default class LoadComponent extends Vue {
    carNo: string = "";

    order: server.order;
    orders: server.order[];

    currStep: number = 0;
    showOrders: boolean = false;
    
    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 水上装油');
    };

    change(label: string, tabkey: string) {
        console.log(label);
        this.$emit('setTitle', this.$store.state.username + ' ' + label);
    }
}