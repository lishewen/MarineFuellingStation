import ComponentBase from "../../componentbase";
import { Component } from 'vue-property-decorator';
import axios from "axios";
import moment from "moment";

@Component({
    components: {
        WeuiSearch: require('../weui-search/search.vue')
    }
})
export default class PlanComponent extends ComponentBase {
    radio2: string = '1';
    username: string;
    isPrevent: boolean = true;
    model: server.salesPlan;
    oildate: string;
    salesplans: server.salesPlan[];
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
        this.model.count = 0;
        this.model.remainder = 0;
        this.model.oilDate = new Date();
        this.model.billingCompany = '';
        this.model.billingPrice = 0;
        this.model.billingCount = 0;
        this.model.productId = 0;
        this.model.oilName = '请选择油品';

        this.oildate = this.formatDate(this.model.oilDate);

        this.client = new Object() as server.client;
        this.client.company = new Object() as server.company;
        this.client.company.name = "";
        this.client.company.ticketType = -1;

        this.pMinPrice = 0;
        this.pMinInvoicePrice = 0;
        
        this.username = this.$store.state.username;
        this.getSalesPlanNo();
        this.getOilProducts();
        
        this.$dialog.loading.close();
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
        this.$watch('sv', (v: string, ov) => {
            //3个字符开始才执行请求操作，减少请求次数
            if (v.length >= 3)
                this.searchSalesPlans(v);
        });
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
            (<any>this).$refs.infinitescroll.$emit("ydui.infinitescroll.finishLoad");

            if (list.length > 0)
                this.page++;
            else
                this.page = 1;
            console.log("page = " + this.page)
        });
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
        if (this.client != null){
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

    goNext() {
        this.showNext = true;
        this.getClient();
    }

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
        if (this.model.price == '' || this.model.price <= 0) { this.toastError("计划单价输入有误"); return; }
        if (!this.model.isInvoice && this.model.price < this.pMinPrice) { this.toastError("当前最低销售单价是￥" + this.pMinPrice + "/升"); return; }
        if (this.model.isInvoice && this.model.price < this.pMinInvoicePrice) { this.toastError("当前开票最低销售单价是￥" + this.pMinInvoicePrice + "/升"); return; }
        this.postSalesPlan(this.model);
    }

    formatDate(d: Date): string {
        return moment(d).format('YYYY-MM-DD');
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
            if (jobj.code == 0) {
                this.model.name = jobj.data;
                this.isPrevent = false;//允许提交
            }
        });
    }

    getSalesPlans(callback?: Function) {
        if (this.page == null) this.page = 1;
        axios.get('/api/SalesPlan/GetByPager?page='
            + this.page
            + '&pagesize=' + this.pSize).then((res) => {
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
        axios.get('/api/Product/OilProducts').then((res) => {
            let jobj = res.data as server.resultJSON<server.product[]>;
            if (jobj.code == 0) {
                jobj.data.forEach((o, i) => {
                    this.oiloptions.push({
                        label: o.name,
                        method: () => {
                            this.model.oilName = o.name;
                            this.model.productId = o.id;
                            this.model.price = o.price;
                            this.pMinInvoicePrice = o.minInvoicePrice;
                            this.pMinPrice = o.minPrice;
                        }
                    });
                });
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
}