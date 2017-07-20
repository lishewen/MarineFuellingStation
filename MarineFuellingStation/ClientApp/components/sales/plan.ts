import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import axios from "axios";

@Component
export default class PlanComponent extends Vue {
    radio2: string = '1';
    unit: string = '升';
    carNo: string = '';
    isinvoice: boolean = false;
    username: string;
    salesPlanNo: string;

    constructor() {
        super();

        this.username = this.$store.state.username;
        this.getSalesPlanNo();
    }

    mounted() {
        this.$emit('setTitle', this.username + ' 销售计划');
        //观察者模式
        this.$watch('radio2', (v, ov) => {
            switch (v) {
                case "1":
                    this.unit = '升';
                    break;
                case "2":
                    this.unit = '吨';
                    break;
                case "3":
                    this.unit = '桶';
                    break;
            }
        });
    };
    change(label: string, tabkey: string) {
        console.log(label);
        this.$emit('setTitle', this.username + ' ' + label);
    }

    getSalesPlanNo() {
        axios.get('/api/SalesPlan/SalesPlanNo').then((res) => {
            let jobj = res.data as server.resultJSON<string>;
            if (jobj.code == 0)
                this.salesPlanNo = jobj.data;
        });
    }
}