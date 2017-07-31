import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import axios from "axios";
import moment from "moment";

@Component({
    components: {
        WeuiSearch: require('../weui-search/search.vue')
    }
})
export default class OrderComponent extends Vue {
    salesplans: server.salesPlan[];
    salesplanshow: boolean = false;

    radio2: string = '1';
    unit: string = '升';
    carNo: string = '';
    isinvoice: boolean = false;

    show1: boolean = false;
    show2: boolean = false;
    selectedplanNo: string = "请选择";
    selectedtransord: string = "";
    hasplan: boolean = false;
    istrans: boolean = false;
    sv: string = "";

    constructor() {
        super();

        this.salesplans = new Array();
    }

    salesplanselect() {
        this.getSalesPlans();
        this.salesplanshow = true;
    }

    formatShortDate(d: Date): string {
        return moment(d).format('MM-DD');
    }

    planitemclick(): void {
        this.selectedplanNo = "JH201707070001";
        this.salesplanshow = false;
        this.hasplan = true;
    };

    transitemclick(): void {
        this.selectedtransord = "YS07070001";
        this.show1 = false;
    };

    emptyclick(): void {
        this.selectedplanNo = "散客";
        this.salesplanshow = false;
        this.hasplan = false;
    };

    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 销售单');

        //观察者模式
        this.$watch('radio2', (v, ov) => {
            switch (v) {
                case "1":
                    this.unit = '升';
                    this.show2 = false;
                    break;
                case "2":
                    this.unit = '吨';
                    this.show2 = true;
                    break;
                case "3":
                    this.unit = '桶';
                    this.show2 = false;
                    break;
            }
        });
    };

    change(label: string, tabkey: string) {
        console.log(label);
        this.$emit('setTitle', this.$store.state.username + ' ' + label);
    }

    getSalesPlans() {
        axios.get('/api/SalesPlan').then((res) => {
            let jobj = res.data as server.resultJSON<server.salesPlan[]>;
            if (jobj.code == 0)
                this.salesplans = jobj.data;
        });
    }
}