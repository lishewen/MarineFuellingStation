import ComponentBase from "../../componentbase";
import { Component } from 'vue-property-decorator';
import axios from "axios";
import moment from "moment";

@Component
export default class PlanComponent extends ComponentBase {
    radio2: string = '1';
    username: string;
    isPrevent: boolean = true;
    showPd: boolean = false;
    model: server.salesPlan;
    oildate: string;
    salesplans: server.salesPlan[];
    products: server.product[];
    oilshow: boolean = false;
    showNext: boolean = false;
    oiloptions: ydui.actionSheetItem[];
    client: server.client;
    sv: string = "";
    page: number;
    scrollRef: any;
    pSize: number = 10;
    pMinInvoicePrice: number = 0;
    pMinPrice: number = 0;

    isWaterDept: boolean = true;//标识水上部和陆上部

    //isLandSalesman: boolean = false;//标识当前用户是否“陆上部”
    //isWaterSalesman: boolean = false;//标识是否“水上部”
    isLeader: boolean = false;//上级领导标识

    constructor() {
        super();

        this.$dialog.loading.open('很快加载好了');

        this.salesplans = (new Array()) as server.salesPlan[];
        this.oiloptions = (new Array()) as ydui.actionSheetItem[];

        this.model = (new Object()) as server.salesPlan;
        this.model.name = '';
        this.model.unit = '升';
        this.model.isInvoice = false;
        this.model.carNo = '';
        this.model.price = '';
        //this.model.count = 0;
        this.model.remainder = 0;
        this.model.oilDate = new Date();
        this.model.billingCompany = '';
        this.model.billingPrice = 0;
        this.model.billingCount = 0;
        this.model.productId = 0;
        this.model.oilName = '请选择油品';
        this.model.isPrintPrice = true;
        this.model.deliverMoney = 0;

        this.oildate = this.formatDate(this.model.oilDate);

        this.client = new Object() as server.client;
        this.client.company = new Object() as server.company;
        this.client.company.name = "";
        this.client.company.ticketType = -1;

        this.pMinPrice = 0;
        this.pMinInvoicePrice = 0;

        this.username = this.$store.state.username;

        this.$dialog.loading.close();
    }

    mounted() {
        //透过路由获取参数iswaterdept
        let iswaterdept = this.$route.params.iswaterdept;
        this.isWaterDept = iswaterdept == "forland" ? false : true;

        this.model.salesPlanType = this.isWaterDept ? server.salesPlanType.水上 : server.salesPlanType.陆上;

        this.$emit('setTitle', this.username + ' 销售计划');
        this.isLeader = this.$store.state.isLeader;

        this.$watch('model.price', (v, ov) => {this.model.billingPrice = v;});
        this.$watch('model.count', (v, ov) => {this.model.billingCount = v;});
        this.$watch('oildate', (v, ov) => { this.model.oilDate = new Date(this.oildate); });

        this.$watch('sv', (v: string, ov) => {
            //2个字符开始才执行请求操作，减少请求次数
            if (v.length >= 2 || v == "")
                this.searchSalesPlans(v);
        });
        
        this.getSalesPlanNo();
        this.getOilProducts();
    };

    change(label: string, tabkey: string) {
        this.$emit('setTitle', this.username + ' ' + label);

        (<any>this).$refs.infinitescroll.$emit('ydui.infinitescroll.reInit');
        this.salesplans = null;
        this.page = 1;
        if (label == '单据记录')
            this.getSalesPlans();
    }

    loadList() {
        this.getSalesPlans((list: server.salesPlan[]) => {
            this.salesplans = this.page > 1 ? [...this.salesplans, ...list] : this.salesplans;
            this.scrollRef = (<any>this).$refs.infinitescroll;
            if (list.length < this.pSize) {
                this.scrollRef.$emit("ydui.infinitescroll.loadedDone");
                return;
            }

            //通知加载数据完毕
            this.scrollRef.$emit("ydui.infinitescroll.finishLoad");

            if (list.length > 0)
                this.page++;
            else
                this.page = 1;
            console.log("page = " + this.page)
        });
    }

    showProducts() {
        this.showPd = true;
    }

    prodsaveclick() {
        console.log(this.products);
        this.putModifyProdPrice(this.products);
    }

    strMinPriceTip() {
        if (this.pMinPrice > 0 && this.pMinInvoicePrice > 0)
            return "最低：￥" + this.pMinPrice + "，开票：￥" + this.pMinInvoicePrice;
        else
            return ""
    }

    isShowCompanyAccount() {
        //避免调试出错，只能这样写
        if (this.client != null)
            if (this.client.clientType == server.clientType.公司)
                return true;
            else
                return false;
    }

    strCompanyName() {
        //避免调试出错，只能这样写
        if (this.client != null) {
            if (this.client.company != null)
                return this.client.company.name
        }
        else
            return ""

    }

    strCompanyBalances() {
        //避免调试出错，只能这样写
        if (this.client != null) {
            if (this.client.company != null)
                return this.client.company.balances
        }
        else
            return 0

    }
    //输入船号后，下一步
    goNext() {
        this.showNext = true;
        this.getClient();
    }

    buttonclick() {
        //信息验证
        if (this.model.carNo == '') {this.toastError('车牌不能为空');return;}
        if (this.model.productId == 0) {this.toastError('必须选择油品');return;}
        if (!this.model.count || this.model.count <= 0) {
            this.toastError('数量必须大于1');
            return;
        }
        if (this.model.price == '' || this.model.price <= 0) { this.toastError("计划单价输入有误"); return; }
        if (!this.model.isInvoice && this.model.price < this.pMinPrice) { this.toastError("当前最低销售单价是￥" + this.pMinPrice + "/升"); return; }
        if (this.model.isInvoice && this.model.price < this.pMinInvoicePrice) { this.toastError("当前开票最低销售单价是￥" + this.pMinInvoicePrice + "/升"); return; }
        console.log(this.model);
        this.postSalesPlan(this.model);
    }

    godetail(id) {
        this.$router.push('/sales/plan/' + id + '/plan')
    }

    getStateName(s: server.salesPlanState): string {
        switch (s) {
            case server.salesPlanState.未审批:
                return '未审批';
            case server.salesPlanState.已审批:
                return '已审批';
            case server.salesPlanState.已完成:
                return '已完成';
        }
    }

    classState(s: server.salesPlanState): any {
        switch (s) {
            case server.salesPlanState.未审批:
                return { color_red: true }
            case server.salesPlanState.已审批:
                return { color_green: true }
            case server.salesPlanState.已完成:
                return { color_blue: true }
        }
    }
    
    getSalesPlanNo() {
        axios.get('/api/SalesPlan/SalesPlanNo').then((res) => {
            let jobj = res.data as server.resultJSON<string>;
            //console.log(jobj);
            if (jobj.code == 0) {
                this.model.name = jobj.data;
                this.isPrevent = false;//允许提交
            }
        }).catch(error => {
            //console.log(error);
            let jobj = error.response.data as server.resultJSON<string>;
            alert(jobj.msg);
        });
    }

    getSalesPlans(callback?: Function) {
        if (this.page == null) this.page = 1;
        let type: server.salesPlanType;
        type = this.isWaterDept ? server.salesPlanType.水上 : server.salesPlanType.陆上;
        axios.get('/api/SalesPlan/GetByPager?page='
            + this.page
            + '&pagesize=' + this.pSize
            + '&type=' + type
            + '&isLeader=' + this.isLeader)
            .then((res) => {
                let jobj = res.data as server.resultJSON<server.salesPlan[]>;
                if (jobj.code == 0) {
                    if (callback) {
                        callback(jobj.data);
                    }
                    else {
                        this.salesplans = jobj.data;
                        this.page++;
                    }
                }
            });
    }

    getClient() {
        let carNo = this.model.carNo;
        if (carNo == "" || carNo == null) {
            this.toastError("请输入船号或车号");
            return;
        }
        axios.get('/api/client/GetClientByCarNo?carno=' + carNo).then((res) => {
            let jobj = res.data as server.resultJSON<server.client>;
            if (jobj.code == 0) {
                this.client = jobj.data;
                if (jobj.data != null) {
                    this.model.billingCompany = this.client.company != null ? this.client.company.name : "";
                    this.model.ticketType = this.client.company != null ? this.client.company.ticketType : -1;
                }
            }
        });
    }

    searchSalesPlans(sv: string) {
        axios.get('/api/SalesPlan/' + sv).then((res) => {
            let jobj = res.data as server.resultJSON<server.salesPlan[]>;
            if (jobj.code == 0)
                this.salesplans = jobj.data;
        });
    }

    getOilProducts() {
        let isforland;
        isforland = this.isWaterDept ? false : true;
        axios.get('/api/Product/OilProducts?isforland=' + isforland).then((res) => {
            let jobj = res.data as server.resultJSON<server.product[]>;
            if (jobj.code == 0) {
                this.products = jobj.data;
                this.oiloptions = new Array();
                jobj.data.forEach((o, i) => {
                    this.oiloptions.push({
                        label: o.name,
                        method: () => {
                            this.model.oilName = o.name;
                            this.model.productId = o.id;
                            this.model.price = o.lastPrice;
                            this.pMinInvoicePrice = o.minInvoicePrice;
                            this.pMinPrice = o.minPrice;
                        }
                    });
                });
                //如果为上级领导，则显示修改商品单价的入口
                if (this.isLeader) {
                    this.oiloptions.push({
                        label: '修改商品限价',
                        method: () => {
                            this.showProducts()
                        }
                    });
                }
            }
        });
    }

    postSalesPlan(model: server.salesPlan) {
        this.isPrevent = true;
        axios.post('/api/SalesPlan', model).then((res) => {
            let jobj = res.data as server.resultJSON<server.salesPlan>;
            if (jobj.code == 0) {
                this.isPrevent = false;//释放提交按钮状态，避免重复提交
                this.getSalesPlanNo();
                this.toastSuccess(jobj.msg);
                this.showNext = false;
            }
        });
    }

    putModifyProdPrice(model: server.product[]) {
        axios.put('/api/Product/ModifyProdPrice', model).then((res) => {
            let jobj = res.data as server.resultJSON<server.product>;
            if (jobj.code == 0) {
                this.toastSuccess("修改成功")
                this.showPd = false;
                this.getOilProducts();
            }
            else
                this.toastError(jobj.msg)
        });
    }
}