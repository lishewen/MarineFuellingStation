import Vue from 'vue';
import { Component } from 'vue-property-decorator';

@Component
export default class PlanComponent extends Vue {
    radio2: string = '2';
    unit: string = '吨';
    carNo: string = '';
    isinvoice: boolean = false;

    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 销售计划开单');
        //观察者模式
        this.$watch('radio2', (v, ov) => {
            if (v == 1) {
                this.unit = '升';
            } else {
                this.unit = '吨';
            }
        });
    };

    change(label: string, tabkey: string) {
        console.log(label);
        this.$emit('setTitle', this.$store.state.username + ' ' + label);
    }
}