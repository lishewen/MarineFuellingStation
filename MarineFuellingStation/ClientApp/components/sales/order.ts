import ComponentBase from "../../componentbase";
import { Component } from 'vue-property-decorator';
import axios from "axios";
import moment from "moment";

@Component
export default class OrderComponent extends ComponentBase {
    salesplans: server.salesPlan[];
    salesplan: server.salesPlan;
    salesplanshow: boolean = false;
    isPrevent: boolean = true;
    model: server.order;
    selectedplanNo: string = "请选择";
    oiloptions: ydui.actionSheetItem[];
    oilName: string = '请选择';
    oilshow: boolean = false;
    orders: server.order[];
    clients: server.client[];
    sales: work.userlist[];
    showSalesmans: boolean = false;

    radio2: string = '1';
    carNo: string = '';
    strCarOrBoat: string = '船号';

    pMinPrice: number = 0;
    pMinInvoicePrice: number = 0;

    show1: boolean = false;
    show2: boolean = false;

    selectedtransord: string = "";
    hasplan: boolean = false;
    istrans: boolean = false;
    sv: string = "";
    page: number;
    scrollRef: any;
    pSize: number = 10;

    constructor() {
        super();

        this.salesplans = new Array();
        this.salesplan = new Object() as server.salesPlan;
        this.model = (new Object()) as server.order;
        this.model.isInvoice = false;
        this.model.carNo = '';
        this.model.price = '';
        this.model.billingPrice = 0;
        this.model.count = 0;
        this.model.billingCount = 0;
        this.model.totalMoney = 0;
        this.model.ticketType = -1;
        this.model.unit = '升';
        this.model.billingCompany = '';

        this.clients = new Array<server.client>();

        this.orders = new Array();
        this.oiloptions = new Array();

        this.getOrderNo();
        this.getOilProducts();
        this.getSales();
    }

    salesplanselect() {
        this.getSalesPlans();
        this.salesplanshow = true;
    }

    strMinPriceTip() {
        if (this.pMinPrice > 0 && this.pMinInvoicePrice > 0)
            return "最低：￥" + this.pMinPrice + "，开票：￥" + this.pMinInvoicePrice;
        else
            return ""
    }

    formatShortDate(d: Date): string {
        return moment(d).format('MM-DD');
    }

    classState(s: server.orderState): any {
        switch (s) {
            case server.orderState.已完成:
                return { color_red: true }
            case server.orderState.装油中:
                return { color_green: true }
            case server.orderState.已开单:
                return { color_darkorange: true }
        }
    }

    strPlanState(s: server.salesPlan) {
        if (s.state == server.salesPlanState.未审批) { return "未审批" };
        if (s.state == server.salesPlanState.已审批) { return "已审批" };
        if (s.state == server.salesPlanState.已完成) { return "已完成" };
    }

    getStateName(s: server.orderState): string {
        switch (s) {
            case server.orderState.已完成:
                return '已完成';
            case server.orderState.装油中:
                return '装油中';
            case server.orderState.已开单:
                return '已开单';
        }
    }

    planitemclick(s: server.salesPlan): void {
        if (s.state == server.salesPlanState.未审批) { this.toastError("该计划未经审核，请通知上级审核通过！"); return; }
        this.salesplan = s;
        this.selectedplanNo = s.name;
        this.model.salesPlanId = s.id;
        this.model.carNo = s.carNo;
        this.model.price = s.price;
        this.model.count = s.count;
        this.model.isInvoice = s.isInvoice;
        this.model.ticketType = s.ticketType;
        this.model.billingCompany = s.billingCompany;
        this.model.billingPrice = s.billingPrice;
        this.model.billingCount = s.billingCount;
        this.model.salesman = s.createdBy;
        
        //this.model.clientId = s.cl
        this.oilName = s.oilName;
        this.model.productId = s.productId;
        this.radio2 = (s.salesPlanType + 1).toString();

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

    selectsalesclick(s: work.userlist) {
        this.model.salesman = s.name;
        this.showSalesmans = false;
    }

    buttonclick() {
        //信息验证
        if (this.model.carNo == '') {
            this.toastError('船号或车牌号不能为空');
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
        if (this.model.salesman == "") {
            this.toastError('必须指定销售员');
            return;
        }
        if (this.model.price == '' || this.model.price <= 0) { this.toastError("销售单价输入有误"); return; }
        if (!this.model.isInvoice && this.model.price < this.pMinPrice) { this.toastError("当前最低销售单价是￥" + this.pMinPrice + "/升"); return; }
        if (this.model.isInvoice && this.model.price < this.pMinInvoicePrice) { this.toastError("当前开票最低销售单价是￥" + this.pMinInvoicePrice + "/升"); return; }
        this.postOrder(this.model);
    }

    godetail(id: number) {
        this.$router.push('/sales/order/' + id + '/order');
    }

    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 销售单');

        //观察者模式
        this.$watch('radio2', (v, ov) => {
            switch (v) {
                case "1":
                    this.model.unit = '升';
                    this.strCarOrBoat = "船号";
                    this.show2 = false;
                    this.model.orderType = server.salesPlanType.水上;
                    break;
                case "2":
                    this.strCarOrBoat = "车牌号";
                    this.model.unit = '吨';
                    this.show2 = true;
                    this.model.orderType = server.salesPlanType.陆上;
                    break;
                case "3":
                    this.strCarOrBoat = "车牌号";
                    this.model.unit = '桶';
                    this.show2 = false;
                    this.model.orderType = server.salesPlanType.机油;
                    break;
            }
        });

        this.$watch('model.price', (v, ov) => {
            console.log(v);
            this.model.billingPrice = v;
            this.model.totalMoney = <number>this.model.price * this.model.count;
        });
        this.$watch('model.count', (v, ov) => {
            this.model.billingCount = v;
            this.model.totalMoney = <number>this.model.price * v;
        });
    };

    change(label: string, tabkey: string) {
        console.log(label);
        this.$emit('setTitle', this.$store.state.username + ' ' + label);

        (<any>this).$refs.infinitescroll.$emit('ydui.infinitescroll.reInit');
        this.salesplans = null;
        this.page = 1;
        if (label == '单据记录')
            this.getOrders();
    }

    loadList() {
        this.getOrders((list: server.order[]) => {
            this.orders = this.page > 1 ? [...this.orders, ...list] : this.orders;
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

    getSalesPlans() {
        axios.get('/api/SalesPlan/Unfinish').then((res) => {
            let jobj = res.data as server.resultJSON<server.salesPlan[]>;
            if (jobj.code == 0) {
                this.salesplans = jobj.data;
            }   
        });
    }

    getOrderNo() {
        axios.get('/api/Order/OrderNo').then((res) => {
            let jobj = res.data as server.resultJSON<string>;
            if (jobj.code == 0) {
                this.model.name = jobj.data;
                this.isPrevent = false;
            }
        });
    }

    getClients() {
        let carNo = this.model.carNo;
        if (carNo == "" || carNo == null) {
            this.toastError("请输入船号或车号");
            return;
        }
        axios.get('/api/Client/' + carNo).then((res) => {
            let jobj = res.data as server.resultJSON<server.client[]>;
            if (jobj.code == 0) {
                this.clients = jobj.data;
                if (this.clients.length > 0) {
                    this.model.billingCompany = this.clients[0].company.name;
                    this.model.ticketType = this.clients[0].company.ticketType;
                }
                else
                    this.toastError('没有找到' + carNo + '相关数据，请手动输入');
            }
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
                            this.model.price = o.minPrice;
                            this.pMinInvoicePrice = o.minInvoicePrice;
                            this.pMinPrice = o.minPrice;
                        }
                    });
                });
            }
        });
    }

    getOrders(callback?: Function) {
        if (this.page == null) this.page = 1;
        axios.get('/api/Order/GetByPager?page='
            + this.page
            + '&pagesize=' + this.pSize).then((res) => {
                let jobj = res.data as server.resultJSON<server.order[]>;
                if (jobj.code == 0) {
                    if (callback) {
                        callback(jobj.data);
                    }
                    else {
                        this.orders = jobj.data;
                        this.page++;
                    }
                }
            });
    }

    //获得销售员
    getSales() {
        axios.get('/api/User/Salesman').then((res) => {
            let jobj = res.data as work.tagMemberResult;
            if (jobj.errcode == 0)
                this.sales = jobj.userlist;
        });
    }

    postOrder(model: server.order) {
        axios.post('/api/Order', model).then((res) => {
            let jobj = res.data as server.resultJSON<server.order>;
            if (jobj.code == 0) {
                this.getOrderNo();
                this.toastSuccess(jobj.msg);
            }
        });
    }
}