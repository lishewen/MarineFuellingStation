import ComponentBase from "../../componentbase";
import { Component } from 'vue-property-decorator';
import axios from "axios";
import moment from "moment";

@Component
export default class CashierBcComponent extends ComponentBase {
    showPayTypes: boolean = false;
    showPayments: boolean = false;
    showStep1: boolean = true;
    showAct: boolean = false;
    page: number;
    scrollRef: any;
    pSize: number = 30;//分页每页显示记录
    readypayboats: server.boatClean[];
    hasypayboats: server.boatClean[];
    nopayboats: server.boatClean[];
    boatPayments: server.payment[];
    selectedBc: server.boatClean;
    payState: server.boatCleanPayState;
    searchVal: string;
    totalPayMoney: number = 0;
    payTypes: Array<string>;
    payMoneys: Array<string>;
    payInfact: number = 0;//实收总数

    sv1: string = ""; sv2: string = ""; sv3: string = "";

    actItems: ydui.actionSheetItem[];

    constructor() {
        super();

        this.selectedBc = new Object as server.boatClean;
        this.readypayboats = new Array<server.boatClean>();
        this.hasypayboats = new Array<server.boatClean>();
        this.nopayboats = new Array<server.boatClean>();
        this.actItems = new Array<ydui.actionSheetItem>();
        this.payTypes = new Array<string>('现金|0');
        this.payMoneys = new Array<string>();

        this.selectedBc.money = 0;
    }
    
    mounted() {
        this.$emit('setTitle', this.$store.state.username + '结算');
        this.$watch("payTypes", (v, ov) => {
            if (v.length > 0)
                this.payMoneys = new Array<string>();
        });
        this.$watch("payMoneys", (v, ov) => {
            this.payInfact = 0;
            for (let m in v) {
                if(v[m])
                    this.payInfact += parseFloat(v[m]);
            }
        });
    };

    nextclick() {
        this.showStep1 = false;
    }

    lastclick() {
        this.showStep1 = true;
    }
    
    loadList() {
        this.getBoatCleans((list: server.boatClean[]) => {
            switch (this.payState) {
                case server.boatCleanPayState.未结算:
                    this.readypayboats = this.page > 1 ? [...this.readypayboats, ...list] : this.readypayboats;
                    this.scrollRef = this.$refs.bcinfinitescroll1;
                    break;
                case server.boatCleanPayState.已结算:
                    this.hasypayboats = this.page > 1 ? [...this.hasypayboats, ...list] : this.hasypayboats;
                    this.scrollRef = this.$refs.bcinfinitescroll2;
                    break;
                case server.boatCleanPayState.挂账:
                    this.nopayboats = this.page > 1 ? [...this.nopayboats, ...list] : this.nopayboats;
                    this.scrollRef = this.$refs.bcinfinitescroll3;
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
                this.payState = server.boatCleanPayState.未结算;
                break;
            case "已结算":
                this.payState = server.boatCleanPayState.已结算;
                break;
            case "挂账":
                this.payState = server.boatCleanPayState.挂账;
                break;
        }
        (<any>this).$refs.bcinfinitescroll1.$emit('ydui.infinitescroll.reInit');
        (<any>this).$refs.bcinfinitescroll2.$emit('ydui.infinitescroll.reInit');
        (<any>this).$refs.bcinfinitescroll3.$emit('ydui.infinitescroll.reInit');
        this.readypayboats = null; this.hasypayboats = null; this.nopayboats = null;
        this.searchVal = "";
        this.page = 1;
        this.getBoatCleans();
    }

    showPaymentsclick(b: server.boatClean) {
        this.selectedBc = b;
        this.showPayments = true;
        this.getPayments(b.id);
    }

    getDiff(d: Date) {
        moment.locale('zh-cn');
        return moment(d).startOf('day').fromNow();
    }

    boatclick(b: server.boatClean) {
        this.selectedBc = b;
        
        this.actItems = [
            {
                label: '结账',
                method: () => {
                    this.showPayTypes = true;
                }
            }
        ];
        this.showAct = true;
    }

    validateMoney() {
        for (let i in this.payTypes) {
            if (this.payMoneys[i] == null || this.payMoneys[i] == '0' || this.payMoneys[i] == '') {
                this.toastError("请输入金额，不能为空或0");
                return;
            }   
        }
        this.putPay();
    }
    
    //根据boatCleanId获取该订单的付款记录
    getPayments(bid: number) {
        if (bid == null) {
            this.toastError("订单数据有误，无法取得付款记录")
            return;
        }
        axios.get('/api/payment/GetByBoatCleanId?bid=' + bid).then((res => {
            let jobj = res.data as server.resultJSON<server.payment[]>;
            if (jobj.code == 0) {
                this.boatPayments = jobj.data;
                //计算所有已支付金额
                this.totalPayMoney = 0;
                this.boatPayments.forEach((p, idx) => {
                    this.totalPayMoney += p.money;
                });
            }
        }))
    }

    //获取订单
    getBoatCleans(callback?: Function) {
        if (this.page == null) this.page = 1;
        axios.get('/api/BoatClean/GetByPayState?'
            + 'paystate=' + this.payState
            + '&page=' + this.page.toString()
            + '&pagesize=' + this.pSize
            + '&searchval=' + this.searchVal)
            .then((res) => {
                let jobj = res.data as server.resultJSON<server.boatClean[]>;
                if (jobj.code == 0) {
                    if (callback)
                        callback(jobj.data);
                    else {
                        switch (this.payState) {
                            case server.boatCleanPayState.未结算:
                                this.readypayboats = jobj.data;
                                break;
                            case server.boatCleanPayState.已结算:
                                this.hasypayboats = jobj.data;
                                break;
                            case server.boatCleanPayState.挂账:
                                this.nopayboats = jobj.data;
                                break;
                        }
                        this.page++;
                    }
                }
            });
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
    
    //结账
    putPay() {
        let model = this.selectedBc;
        model.payState = server.boatCleanPayState.已结算;
        model.payments = new Array<server.payment>();
        this.payTypes.forEach((pt, idx) => {
            let payment = new Object() as server.payment;
            payment.payTypeId = pt.split('|')[1];//支付方式
            payment.money = parseFloat(this.payMoneys[idx]);//金额
            payment.name = model.name;//单号
            payment.boatCleanId = model.id;//对应boatClean的id
            model.payments.push(payment);
            console.log(payment);
        });
        axios.put("/api/BoatClean/pay", model).then((res) => {
            let jobj = res.data as server.resultJSON<server.boatClean>;
            if (jobj.code == 0) {
                this.toastSuccess("结算成功");
                this.showPayTypes = false;
                this.page = 1;
                this.getBoatCleans();
            }
        })
    }

    //挂账
    putPayOnCredit() {
        let model = this.selectedBc;
        model.payState = server.boatCleanPayState.挂账;
        axios.put("/api/boatClean/payoncredit", model).then((res) => {
            let jobj = res.data as server.resultJSON<server.order>;
            if (jobj.code == 0) {
                this.toastSuccess("挂账成功")
                this.getBoatCleans();
                this.showPayTypes = false;
            }
        })
    }
    
}