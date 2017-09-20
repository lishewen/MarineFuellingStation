import ComponentBase from "../../componentbase";
import { Component } from 'vue-property-decorator';
import axios from "axios";
import moment from "moment";

@Component({
    components: {
        WeuiSearch: require('../weui-search/search.vue')
    }
})
export default class CashierComponent extends ComponentBase {
    show2: boolean = false;
    lastshow: boolean = true;
    sv: string = "";
    payInfact: number = 0;//实收金额

    orderPayTypes: Array<string>;
    orderPayMoneys: Array<number>;

    showInputs: Array<boolean>;
    payments: Array<server.payment>;
    selectedOrder: server.order;

    orders: server.order[];
    page: number;

    constructor() {
        super();

        this.payments = new Array<server.payment>();
        this.selectedOrder = new Object() as server.order;
        this.selectedOrder.client = new Object() as server.client;
        this.orderPayTypes = ["0"];
        this.orderPayMoneys = new Array<number>();
        this.showInputs = new Array<boolean>(false, false, false, false, false, false, false);
        this.orders = new Array<server.order>();

        this.getOrders();
    }

    orderclick(o: server.order) {
        this.selectedOrder = o;
        this.show2 = true;
        this.lastshow = true;
    }

    getTotalPayMoney() {
        let infact = 0;
        if (this.orderPayTypes) {
            this.orderPayTypes.forEach((pt, idx) => {
                if (this.orderPayMoneys[parseInt(pt)])
                    infact += parseFloat(this.orderPayMoneys[parseInt(pt)].toString());
            })
        }
        this.payInfact = infact;
        return infact
    }

    nextclick(): void {
        this.lastshow = false;
        this.showInputs = new Array<boolean>(false, false, false, false, false, false, false);
        this.orderPayTypes.forEach((p, idx) => {
            this.showInputs[p] = true;
        });
    };
    lastclick(): void {
        this.lastshow = true;
        //清空所有input的值
        this.orderPayMoneys = new Array<number>();
    };

    getDiff(d: Date) {
        return moment(d).startOf('day').fromNow();
    }

    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 结算');
        this.$watch("show2", (v, ov) => {
            //初始化
            this.orderPayTypes = ["0"];
            this.orderPayMoneys = new Array<number>();
        })
    };

    change(label: string, tabkey: string) {
        console.log(label);
        this.$emit('setTitle', this.$store.state.username + ' ' + label);
    }

    getOrders() {
        if (!this.page) this.page = 1;
        axios.get('/api/Order/GetIncludeProduct?page=' + this.page.toString()).then((res) => {
            let jobj = res.data as server.resultJSON<server.order[]>;
            if (jobj.code == 0) {
                this.orders = jobj.data;
                this.page += 1;
            }   
        });
    }
    //验证输入的账户扣减金额是否大于账户余额
    validateMoney() {
        console.log(this.orderPayMoneys);
        console.log(this.orderPayTypes);
        let flag = true;
        this.orderPayTypes.forEach((p, idx) => {
            //console.log(parseInt(p));
            if (parseInt(p) == server.orderPayType.账户扣减) {
                if (this.orderPayMoneys[parseInt(p)] > this.selectedOrder.client.balances) {
                    //console.log(this.orderPayMoneys[parseInt(p)])
                    this.toastError("输入的扣减金额不能大于账户余额")
                    flag = false;
                }   
            }
        });

        //if (flag) this.putPay();
    }

    //结账
    putPay() {
        let model = new Object() as server.order;
        model.id = this.selectedOrder.id;
        model.carNo = this.selectedOrder.carNo;
        model.payState = server.payState.已结算;
        model.payments = new Array<server.payment>();
        this.orderPayTypes.forEach((p, idx) => {
            let payment = new Object as server.payment;
            payment.name = this.selectedOrder.name;
            payment.payTypeId = parseInt(p);
            payment.money = this.orderPayMoneys[parseInt(p)];
            model.payments.push(payment);
        });
        axios.put("/api/order/pay", model).then((res) => {
            let jobj = res.data as server.resultJSON<server.order>;
            if (jobj.code == 0) {
                this.toastSuccess("结算成功")
            }
        })
    }

    //挂账
    putPayOnCredit() {
        let model = new Object() as server.order;
        model.id = this.selectedOrder.id;
        model.payState = server.payState.挂账;
        axios.put("/api/order/payoncredit", model).then((res) => {
            let jobj = res.data as server.resultJSON<server.order>;
            if (jobj.code == 0) {
                this.toastSuccess("挂账成功")
                this.getOrders();
                this.show2 = false;
            }
        })
    }
}