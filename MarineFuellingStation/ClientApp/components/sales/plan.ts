import Vue from 'vue';
import { Component } from 'vue-property-decorator';

@Component
export default class PlanComponent extends Vue {
    radio2: string = '1';
    unit: string = '吨';
    carNo: string = 'xxx';
    switch1: boolean = true;
    
    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 销售计划开单');
    };
    
    change(label: string, tabkey: string) {
        console.log(label);
        this.$emit('setTitle', this.$store.state.username + ' ' + label);
    }
}