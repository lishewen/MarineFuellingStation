import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import axios from "axios";
import moment from "moment";

@Component
export default class PlanComponent extends Vue {
    radio2: string = '1';
    username: string;
    model: server.salesPlan;
    oildate: string;
    options: server.product[];

    constructor() {
        super();
        (<any>this).$dialog.loading.open('很快加载好了');
        this.options = (new Object()) as server.product[];

        this.model = (new Object()) as server.salesPlan;
        this.model.name = '';
        this.model.unit = '升';
        this.model.isInvoice = false;
        this.model.carNo = '';
        this.model.price = 0;
        this.model.count = 0;
        this.model.remainder = 0;
        this.model.oilDate = new Date();
        this.model.billingCompany = '';
        this.model.billingPrice = 0;
        this.model.billingCount = 0;
        this.model.productId = 0;

        this.oildate = moment(this.model.oilDate).format('YYYY-MM-DD');

        this.username = this.$store.state.username;
        this.getSalesPlanNo();
        this.getOilProducts();
        (<any>this).$dialog.loading.close();
    }

    mounted() {
        this.$emit('setTitle', this.username + ' 销售计划');
        //观察者模式
        this.$watch('radio2', (v, ov) => {
            switch (v) {
                case "1":
                    this.model.unit = '升';
                    this.model.salesPlanType = server.salesPlanType.水上;
                    break;
                case "2":
                    this.model.unit = '吨';
                    this.model.salesPlanType = server.salesPlanType.陆上;
                    break;
                case "3":
                    this.model.unit = '桶';
                    this.model.salesPlanType = server.salesPlanType.机油;
                    break;
            }
        });
        this.$watch('model.price', (v, ov) => {
            this.model.billingPrice = v;
        });
        this.$watch('model.count', (v, ov) => {
            this.model.billingCount = v;
        });
        this.$watch('oildate', (v, ov) => {
            this.model.oilDate = new Date(this.oildate);
        });
    };
    change(label: string, tabkey: string) {
        this.$emit('setTitle', this.username + ' ' + label);
    }

    getSalesPlanNo() {
        axios.get('/api/SalesPlan/SalesPlanNo').then((res) => {
            let jobj = res.data as server.resultJSON<string>;
            if (jobj.code == 0)
                this.model.name = jobj.data;
        });
    }

    getOilProducts() {
        axios.get('/api/Product/OilProducts').then((res) => {
            let jobj = res.data as server.resultJSON<server.product[]>;
            if (jobj.code == 0)
                this.options = jobj.data;
        });
    }

    postSalesPlan(model: server.salesPlan) {
        axios.post('/api/SalesPlan', model).then((res) => {
            let jobj = res.data as server.resultJSON<string>;
            if (jobj.code == 0)
                (<any>this).$dialog.toast({
                    mes: jobj.msg,
                    timeout: 1500,
                    icon: 'success'
                });
        });
    }
}