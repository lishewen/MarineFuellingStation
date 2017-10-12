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
    showPayTypes: boolean = false;
    lastshow: boolean = true;
    showAct: boolean = false;
    showCharge: boolean = false;
    showPayments: boolean = false;
    isCompanyCharge: boolean = false;
    actItems: ydui.actionSheetItem[];
    totalPayMoney: number;//所有已支付的金额总和

    //搜索
    searchVal: string = "";
    sv1: string = "";
    sv2: string = "";
    sv3: string = "";

    payInfact: number = 0;//实收金额
    payState: server.payState;
    scrollRef: any;
    pSize: number = 30;//分页每页显示记录

    orderPayTypes: Array<string>;
    orderPayMoneys: Array<number>;

    showInputs: Array<boolean>;
    payments: Array<server.payment>;
    orderPayments: Array<server.payment>;//用于读取点击订单的付款记录
    selectedOrder: server.order;

    chargeLog: server.chargeLog;//充值记录对象

    readypayorders: server.order[];
    haspayorders: server.order[];
    nopayorders: server.order[];
    page: number;

    chargeAccount: string = "";//当前选择充值的账户，船号或公司

    constructor() {
        super();

        this.payments = new Array<server.payment>();
        this.orderPayments = new Array<server.payment>();
        this.chargeLog = new Object() as server.chargeLog;
        this.chargeLog.payType = server.orderPayType.现金;
        this.selectedOrder = new Object() as server.order;
        this.selectedOrder.client = new Object() as server.client;
        this.selectedOrder.client.balances = 0;
        this.orderPayTypes = [server.orderPayType.现金.toString()];//默认支付方式“现金”
        this.orderPayMoneys = new Array<number>(0, 0, 0, 0, 0, 0, 0, 0);
        this.showInputs = new Array<boolean>(false, false, false, false, false, false, false, false);
        this.totalPayMoney = 0;//

        this.readypayorders = new Array<server.order>();
        this.haspayorders = new Array<server.order>();
        this.nopayorders = new Array<server.order>();

        this.actItems = new Array<ydui.actionSheetItem>();
    }

    strPayType(pt: server.orderPayType) {
        switch (pt) {
            case server.orderPayType.现金:
                return "现金"
            case server.orderPayType.微信:
                return "微信"
            case server.orderPayType.支付宝:
                return "支付宝"
            case server.orderPayType.桂行刷卡:
                return "桂行刷卡"
            case server.orderPayType.工行刷卡:
                return "工行刷卡"
            case server.orderPayType.刷卡三:
                return "刷卡三"
            case server.orderPayType.账户扣减:
                return "账户扣减"
            case server.orderPayType.公司账户扣减:
                return "公司账户扣减"
        }
    }

    strCompanyName(o: server.order) {
        if (o.client != null)
            if (o.client.company != null)
                return o.client.company.name
    }

    strBalances() {
        if (this.isCompanyCharge)
            return this.selectedOrder.client.company == null ? "" : this.selectedOrder.client.company.balances
        else
            return this.selectedOrder.client == null ? "" : this.selectedOrder.client.balances
    }

    orderclick(o: server.order) {
        this.selectedOrder = o;

        this.actItems = [
            {
                label: '结账',
                method: () => {
                    this.showPayTypes = true;
                    this.lastshow = true;
                }
            },
            {
                label: '充值【个人账户】',
                method: () => {
                    this.showCharge = true;
                    this.isCompanyCharge = false;
                    this.chargeAccount = this.selectedOrder.client.carNo;
                }
            }  
        ];
        let coItem = {
            label: '充值【公司账户】',
            method: () => {
                this.showCharge = true;
                this.isCompanyCharge = true;
                this.chargeAccount = this.selectedOrder.client.company.name;
            }
        };

        if (this.selectedOrder.client.company != null)
            this.actItems = [...this.actItems, coItem];

        this.showAct = true;
        
    }
    showPaymentsclick(o: server.order) {
        this.selectedOrder = o;
        this.showPayments = true;
        this.getOrderPayments(o.id);
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
        this.showInputs = new Array<boolean>(false, false, false, false, false, false, false, false);
        this.orderPayTypes.forEach((p, idx) => {
            this.showInputs[p] = true;
        });
    };
    lastclick(): void {
        this.lastshow = true;
        //清空所有input的值
        this.orderPayMoneys = new Array<number>(0, 0, 0, 0, 0, 0, 0, 0);
    };

    getDiff(d: Date) {
        moment.locale('zh-cn');
        return moment(d).startOf('day').fromNow();
    }

    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 结算');
        this.$watch("showPayTypes", (v, ov) => {
            //初始化
            this.orderPayTypes = ["0"];
            this.orderPayMoneys = new Array<number>(0, 0, 0, 0, 0, 0, 0, 0);
        });
        this.$watch("orderPayMoneys", (v, ov) => {
            let balanClient = this.selectedOrder.client == null ? 0 : this.selectedOrder.client.balances;
            let balanCompany = this.selectedOrder.client.company == null ? 0 : this.selectedOrder.client.company.balances;
            //let input = (v[6] == null || v[6] == "") ? 0 : v[6];
            if (v != null) {
                if (balanClient > 0) {
                    if (v[6] > this.selectedOrder.client.balances){
                        this.toastError("超出客户可扣余额")
                        this.orderPayMoneys[6] = this.selectedOrder.client.balances;
                    }
                }
                if (balanCompany > 0) {
                    if (v[7] > this.selectedOrder.client.company.balances) {
                        this.toastError("超出公司账户可扣余额")
                        this.orderPayMoneys[7] = this.selectedOrder.client.company.balances;
                    }
                }
            }
        });
        this.$watch("sv1", (v, ov) => {
            this.searchVal = v;
            this.page = 1;
            this.getOrders();
        });
        this.$watch("sv2", (v, ov) => {
            this.searchVal = v;
            this.page = 1;
            this.getOrders();
        });
        this.$watch("sv3", (v, ov) => {
            this.searchVal = v;
            this.page = 1;
            this.getOrders();
        });
    };
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
        this.searchVal = "";
        this.page = 1;
        this.getOrders();
    }

    //根据orderId获取该订单的付款记录
    getOrderPayments(oid: number) {
        if (oid == null) {
            this.toastError("订单数据有误，无法取得付款记录")
            return;
        }
        axios.get('/api/payment/GetByOrderId?oid=' + oid).then((res => {
            let jobj = res.data as server.resultJSON<server.payment[]>;
            if (jobj.code == 0) {
                this.orderPayments = jobj.data;
                //计算所有已支付金额
                this.totalPayMoney = 0;
                this.orderPayments.forEach((p, idx) => {
                    this.totalPayMoney += p.money;
                });
            }
        }))
    }

    //获取订单
    getOrders(callback?: Function) {
        if (this.page == null) this.page = 1;
        axios.get('/api/Order/GetByPayState?'
            + 'paystate=' + this.payState
            + '&page=' + this.page.toString()
            + '&pagesize=' + this.pSize
            + '&searchval=' + this.searchVal)
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
    

    //结账
    putPay() {
        let model = new Object() as server.order;
        model.id = this.selectedOrder.id;
        model.carNo = this.selectedOrder.carNo;
        model.payState = server.payState.已结算;
        model.payments = new Array<server.payment>();
        model.clientId = this.selectedOrder.clientId;
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
                this.showPayTypes = false;
            }
        })
    }

    //充值到客户或公司账户
    postCharge() {
        if (this.selectedOrder.clientId == null) { this.toastError("请先建立客户档案！"); return; }
        this.chargeLog.chargeType = server.chargeType.充值;
        this.chargeLog.clientId = this.selectedOrder.clientId;
        axios.post("/api/chargelog?isCompanyCharge=" + this.isCompanyCharge.toString(), this.chargeLog).then((res) => {
            let jobj = res.data as server.resultJSON<server.chargeLog>;
            if (jobj.code == 0) {
                this.toastSuccess("充值成功")
            }
            if (jobj.code == 501) {
                this.toastError(jobj.msg)
            }
        });
    }
    
}