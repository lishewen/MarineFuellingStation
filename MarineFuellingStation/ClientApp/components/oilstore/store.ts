import Vue from 'vue';
import { Component } from 'vue-property-decorator';

@Component
export default class MyOrderComponent extends Vue {
    carNo: string = "";
    show1: boolean = false;
    show2: boolean = false;
    radio1: string = "1";
   
    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 油仓');
    };

    change(label: string, tabkey: string) {
        console.log(label);
        this.$emit('setTitle', this.$store.state.username + ' ' + label);
    }
}