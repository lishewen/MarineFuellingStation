import ComponentBase from "../../componentbase";
import { Component } from 'vue-property-decorator';
import axios from "axios";

@Component
export default class ChargeComponent extends ComponentBase {
    isCompany: boolean | string = false;
    showClients: boolean = false;//显示搜索返回的列表
    showCompanys: boolean = false;
    showStep2: boolean = false;
    isPrevent: boolean = true;
    accName: string = "";
    chargeLog: server.chargeLog;
    keyword: string;
    money: number | string;
    client: server.client;//当前选择的客户
    clients: server.client[];
    company: server.company;
    companys: server.company[];
    balances: number;
    sumNoPay: number = 0;
    constructor() {
        super();

        this.chargeLog = new Object as server.chargeLog;//发现不初始化的话this.$watch没反应
        this.chargeLog.payType = -1;

        this.clients = new Array<server.client>();
        this.companys = new Array<server.company>();

        this.client = new Object as server.client;
        this.company = new Object as server.company;

    }
    mounted() {
        this.$emit('setTitle', '充值');
        this.$watch("chargeLog.money", (v, ov) => {
            if (v == null || v == "" || v <= 0 || this.chargeLog.payType == -1)
                this.isPrevent = true;
            else
                this.isPrevent = false;
        });
    }
    chargeclick() {
        this.$dialog.confirm({
            title: '确认',
            mes: '是否向账户"' + this.accName + '"充值￥' + this.chargeLog.money + '？',
            opts: () => {
                this.postCharge()
            }
        });
    }
    queryclick() {
        //真服了，要这样写才不会有bug
        if (this.isCompany == true || this.isCompany == "1") {
            this.getCompanys();
        }
        else {
            this.getClients();
        }
    }
    clientclick(c: server.client) {
        this.client = c;
        this.balances = this.client.balances;
        this.accName = c.carNo;
        if (this.showClients) this.showClients = false;
        this.clients = null;
        this.chargeLog.clientId = c.id;
        this.showStep2 = true;
        //读取挂账金额
        this.getNoPayMoney(c.id);
        console.log(c);
    }
    companyclick(co: server.company) {
        this.company = co;
        this.balances = this.company.balances;
        this.accName = co.name;
        if (this.showCompanys) this.showCompanys = false;
        this.companys = null;
        this.chargeLog.companyId = co.id;
        this.showStep2 = true;
        //读取挂账金额
        this.getNoPayMoney(co.id);
    }
    confirmPrint(cl: server.chargeLog) {
        this.$dialog.confirm({
            title: '打印',
            mes: '充值成功，是否需要打印？',
            opts: () => {
                this.postPrintTo(cl, "收银台")
            }
        });
    }
    postCharge() {
        this.chargeLog.chargeType = server.chargeType.充值;
        //console.log(this.isCompany); return;
        if (this.isCompany == true || this.isCompany == "1") {
            this.chargeLog.isCompany = true;
            this.chargeLog.companyId = this.company.id
        }
        else {
            this.chargeLog.isCompany = false;
            this.chargeLog.clientId = this.client.id;
        }
        
        axios.post("/api/chargelog", this.chargeLog).then((res) => {
            let jobj = res.data as server.resultJSON<server.chargeLog>;
            if (jobj.code == 0) {
                //this.toastSuccess("充值成功")
                this.confirmPrint(jobj.data);
            }
            if (jobj.code == 501) {
                this.toastError(jobj.msg)
            }
        });
    }
    //搜索相关客户
    getClients() {
        axios.get('/api/Client/GetByClientKeyword'
            + '?kw=' + this.keyword
        ).then((res) => {
            let jobj = res.data as server.resultJSON<server.client[]>;
            if (jobj.code == 0) {
                this.clients = jobj.data;
                //当返回的列表数量大于1，则显示列表供选择；否则直接赋值client
                if (this.clients.length > 1) {
                    this.showClients = true;
                }
                else if (this.clients.length == 1) {
                    this.client = this.clients[0]
                    this.accName = this.client.carNo;
                    this.showStep2 = true;
                }
                else {
                    this.toastError("查询没有相关数据");
                    this.balances = 0;
                }
            }
            else
                this.toastError('无法获取客户数据，请重试')
        });
    }
    //搜索相关公司
    getCompanys() {
        axios.get('/api/Company/GetByCompanyKeyword'
            + '?kw=' + this.keyword
        ).then((res) => {
            let jobj = res.data as server.resultJSON<server.company[]>;
            if (jobj.code == 0) {
                this.companys = jobj.data;
                //当返回的列表数量大于1，则显示列表供选择；否则直接赋值company
                if (this.companys.length > 1) {
                    this.showCompanys = true;
                }
                else if (this.companys.length == 1) {
                    this.company = this.companys[0];
                    this.accName = this.company.name;
                    this.balances = this.company.balances;
                    this.showStep2 = true;
                    //读取挂账金额
                    this.getNoPayMoney(this.company.id);
                }
                else {
                    this.accName = "找不到" + this.keyword;
                    this.toastError("查询没有相关数据");
                    this.balances = 0;
                }
            }
            else
                this.toastError('无法获取公司数据，请重试')
        });
    }
    getNoPayMoney(id: number) {
        let url = this.isCompany ? "/api/order/GetCompanyNoPayMoney?coid=" + id : "/api/order/GetClientNoPayMoney?cid=" + id;
        axios.get(url).then((res) => {
            let jobj = res.data as server.resultJSON<number>;
            if (jobj.code == 0) {
                this.sumNoPay = jobj.data;
            }
        });
    }
    //打印
    postPrintTo(cl: server.chargeLog, to: string) {
        let actName = cl.isCompany ? "postPrintCompanyPrepay" : "postPrintClientPrepay";
        axios.post('/api/ChargeLog/' + actName + '?' +
            '&to=' + to, cl).then((res) => {
                let jobj = res.data as server.resultJSON<server.chargeLog>;
                if (jobj.code == 0) {
                    this.toastSuccess('打印指令已发出')
                }
            });
    }
}