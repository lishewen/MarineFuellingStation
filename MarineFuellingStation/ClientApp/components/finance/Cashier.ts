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
        this.selectedOrder.client.balances = 0;
        this.orderPayTypes = [server.orderPayType.现金.toString()];//默认支付方式“现金”
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

    loadList() {
        this.getOrders((list: server.order[]) => {
            if (this.page > 1)
                //叠加新内容进orders
                this.orders = [...list, ...this.orders];
            else
                this.orders = list;

            this.toastSuccess(list.length > 0 ? '为您更新了' + list.length + '条内容' : '已是最新内容');

            //通知控件刷新完成
            (<any>this.$refs.logpullrefresh).$emit('ydui.pullrefresh.finishLoad');

            //如果有内容则page+1，否则则把page重置为1
            if (list.length > 0)
                this.page++;
            else
                this.page = 1;
        });
    }

    loadList1() {
        this.getOrders((list: server.order[]) => {
            if (this.page > 1)
                //叠加新内容进orders
                this.orders = [...list, ...this.orders];
            else
                this.orders = list;

            this.toastSuccess(list.length > 0 ? '为您更新了' + list.length + '条内容' : '已是最新内容');

            //通知控件刷新完成
            (<any>this.$refs.orderpullrefresh1).$emit('ydui.pullrefresh.finishLoad');

            //如果有内容则page+1，否则则把page重置为1
            if (list.length > 0)
                this.page++;
            else
                this.page = 1;
        });
    }

    loadList2() {
        this.getOrders((list: server.order[]) => {
            if (this.page > 1)
                //叠加新内容进orders
                this.orders = [...list, ...this.orders];
            else
                this.orders = list;

            this.toastSuccess(list.length > 0 ? '为您更新了' + list.length + '条内容' : '已是最新内容');

            //通知控件刷新完成
            (<any>this.$refs.orderpullrefresh2).$emit('ydui.pullrefresh.finishLoad');

            //如果有内容则page+1，否则则把page重置为1
            if (list.length > 0)
                this.page++;
            else
                this.page = 1;
        });
    }

    change(label: string, tabkey: string) {
        console.log(label);
        this.$emit('setTitle', this.$store.state.username + ' ' + label);
    }

    getOrders(callback?: Function) {
        if (!this.page) this.page = 1;
        axios.get('/api/Order/GetIncludeProduct?page=' + this.page.toString()).then((res) => {
            let jobj = res.data as server.resultJSON<server.order[]>;
            if (jobj.code == 0) {
                if (callback)
                    callback(jobj.data);
                else {
                    this.orders = jobj.data;
                    this.page++;
                }
            }
        });
    }
    //验证输入的账户扣减金额是否大于账户余额
    validateMoney() {
        if (this.payInfact - this.selectedOrder.totalMoney < 0) {
            this.toastError("实收金额应大于等于应收金额")
            return;
        }

        let flag = true;
        let isNull = false;

        this.orderPayTypes.forEach((p, idx) => {
            if (!this.orderPayMoneys[parseInt(p)]) {
                isNull = true;
                return;
            }
            if (parseInt(p) == server.orderPayType.账户扣减) {
                if (this.orderPayMoneys[parseInt(p)] > this.selectedOrder.client.balances) {
                    flag = false;
                }
            }
        });
        if (!flag) {
            this.toastError("输入的扣减金额不能大于账户余额");
            return;
        }
        if (isNull) {
            this.toastError("请输入金额");
            return;
        }

        this.putPay();
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