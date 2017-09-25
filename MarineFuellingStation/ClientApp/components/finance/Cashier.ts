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
    payState: server.payState;
    scrollRef: any;
    pSize: number = 30;//分页每页显示记录

    orderPayTypes: Array<string>;
    orderPayMoneys: Array<number>;

    showInputs: Array<boolean>;
    payments: Array<server.payment>;
    selectedOrder: server.order;

    readypayorders: server.order[];
    haspayorders: server.order[];
    nopayorders: server.order[];
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

        this.readypayorders = new Array<server.order>();
        this.haspayorders = new Array<server.order>();
        this.nopayorders = new Array<server.order>();
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
        });
    };

    loadList() {
        this.getOrders((list: server.order[]) => {
            switch (this.payState) {
                case server.payState.未结算:
                    this.readypayorders = this.page > 1 ? [...this.readypayorders, ...list] : this.readypayorders;
                    this.scrollRef = this.$refs.orderinfinitescroll1;
                    break;
                case server.payState.已结算:
                    this.haspayorders = this.page > 1 ? [...this.haspayorders, ...list] : this.haspayorders;
                    this.scrollRef = this.$refs.orderinfinitescroll2;
                    break;
                case server.payState.挂账:
                    this.nopayorders = this.page > 1 ? [...this.nopayorders, ...list] : this.nopayorders;
                    this.scrollRef = this.$refs.orderinfinitescroll3;
                    break;
            }
            if (list.length < this.pSize) {
                this.scrollRef.$emit("ydui.infinitescroll.loadedDone");
                return;
            }

            //通知加载数据完毕
            (<any>this).$refs.infinitescroll.$emit("ydui.infinitescroll.finishLoad");

            if (list.length > 0)
                this.page++;
            else
                this.page = 1;
            console.log("page = " + this.page)
        });
    }

    change(label: string, tabkey: string) {
        console.log(label);
        this.$emit('setTitle', this.$store.state.username + ' ' + label);

        switch (label) {
            case "待结算":
                this.payState = server.payState.未结算;
                break;
            case "已结算":
                this.payState = server.payState.已结算;
                break;
            case "挂账":
                this.payState = server.payState.挂账;
                break;
        }
        (<any>this).$refs.orderinfinitescroll1.$emit('ydui.infinitescroll.reInit');
        (<any>this).$refs.orderinfinitescroll2.$emit('ydui.infinitescroll.reInit');
        (<any>this).$refs.orderinfinitescroll3.$emit('ydui.infinitescroll.reInit');
        this.readypayorders = null; this.haspayorders = null; this.nopayorders = null;
        this.page = 1;
        this.getOrders();
    }

    getOrders(callback?: Function) {
        if (this.page == null) this.page = 1;
        axios.get('/api/Order/GetByPayState?'
            + 'paystate=' + this.payState
            + '&page=' + this.page.toString()
            + '&pagesize=' + this.pSize)
            .then((res) => {
                let jobj = res.data as server.resultJSON<server.order[]>;
                if (jobj.code == 0) {
                    if (callback)
                        callback(jobj.data);
                    else {
                        switch (this.payState) {
                            case server.payState.未结算:
                                this.readypayorders = jobj.data;
                                break;
                            case server.payState.已结算:
                                this.haspayorders = jobj.data;
                                break;
                            case server.payState.挂账:
                                this.nopayorders = jobj.data;
                                break;
                        }
                        console.log(this.readypayorders);
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