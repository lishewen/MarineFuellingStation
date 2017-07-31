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
    model: server.order;
    selectedplanNo: string = "请选择";
    oiloptions: ydui.actionSheetItem[];
    oilName: string = '';
    oilshow: boolean = false;

    radio2: string = '1';
    unit: string = '升';
    carNo: string = '';

    show1: boolean = false;
    show2: boolean = false;

    selectedtransord: string = "";
    hasplan: boolean = false;
    istrans: boolean = false;
    sv: string = "";

    constructor() {
        super();

        this.salesplans = new Array();
        this.model = (new Object()) as server.order;
        this.model.isInvoice = false;

        this.getOrderNo();
        this.getOilProducts();
    }

    salesplanselect() {
        this.getSalesPlans();
        this.salesplanshow = true;
    }

    formatShortDate(d: Date): string {
        return moment(d).format('MM-DD');
    }

    planitemclick(s: server.salesPlan): void {
        this.selectedplanNo = s.name;
        this.model.salesPlanId = s.id;
        this.model.carNo = s.carNo;
        this.model.price = s.price;
        this.model.count = s.count;

        this.hasplan = true;

        this.salesplanshow = false;
    };

    transitemclick(): void {
        this.selectedtransord = "YS07070001";
        this.show1 = false;
    };

    emptyclick(): void {
        this.selectedplanNo = "散客";
        this.model.salesPlanId = null;

        this.hasplan = false;

        this.salesplanshow = false;
    };

    buttonclick() {
        //信息验证
        if (this.model.carNo == '') {
            this.toastError('车牌不能为空');
            return;
        }
        if (this.model.count <= 0) {
            this.toastError('数量必须大于1');
            return;
        }
        if (this.model.productId == 0) {
            this.toastError('必须选择油品');
            return;
        }
        this.postOrder(this.model);
    }

    toastError(msg: string) {
        (<any>this).$dialog.toast({
            mes: msg,
            timeout: 1500,
            icon: 'error'
        });
    }

    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 销售单');

        //观察者模式
        this.$watch('radio2', (v, ov) => {
            switch (v) {
                case "1":
                    this.unit = '升';
                    this.show2 = false;
                    this.model.orderType = server.salesPlanType.水上;
                    break;
                case "2":
                    this.unit = '吨';
                    this.show2 = true;
                    this.model.orderType = server.salesPlanType.陆上;
                    break;
                case "3":
                    this.unit = '桶';
                    this.show2 = false;
                    this.model.orderType = server.salesPlanType.机油;
                    break;
            }
        });

        this.$watch('model.price', (v, ov) => {
            this.model.billingPrice = v;
            this.model.totalMoney = v * this.model.count;
        });
        this.$watch('model.count', (v, ov) => {
            this.model.billingCount = v;
            this.model.totalMoney = this.model.price * v;
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

    getOrderNo() {
        axios.get('/api/Order/OrderNo').then((res) => {
            let jobj = res.data as server.resultJSON<string>;
            if (jobj.code == 0)
                this.model.name = jobj.data;
        });
    }

    getOilProducts() {
        axios.get('/api/Product/OilProducts').then((res) => {
            let jobj = res.data as server.resultJSON<server.product[]>;
            if (jobj.code == 0) {
                jobj.data.forEach((o, i) => {
                    this.oiloptions.push({
                        label: o.name,
                        method: () => {
                            this.oilName = o.name;
                            this.model.productId = o.id;
                            if (o.lastPrice > 0)
                                this.model.price = o.lastPrice;
                            else
                                this.model.price = o.minPrice;
                        }
                    });
                });
            }
        });
    }

    postOrder(model: server.order) {
        axios.post('/api/Order', model).then((res) => {
            let jobj = res.data as server.resultJSON<server.order>;
            if (jobj.code == 0)
                (<any>this).$dialog.toast({
                    mes: jobj.msg,
                    timeout: 1500,
                    icon: 'success'
                });
        });
    }
}