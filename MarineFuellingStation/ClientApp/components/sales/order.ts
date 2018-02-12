import ComponentBase from "../../ComponentBase";
import { Component } from 'vue-property-decorator';
import axios from "axios";
import moment from "moment";

@Component
export default class OrderComponent extends ComponentBase {
    salesplans: server.salesPlan[];
    salesplan: server.salesPlan;
    selectProduct: server.product;
    selectOrder: server.order;
    stores: server.store[];
    workers: work.userlist[];
    salesplanshow: boolean = false;
    isPrevent: boolean = true;
    showMenus: boolean = false;
    showStores: boolean = false; 
    showWorkers: boolean = false; 
    model: server.order;
    selectedplanNo: string = "请选择";
    oiloptions: ydui.actionSheetItem[];
    menus: ydui.actionSheetItem[];
    oilName: string = '请选择';
    selectStoreText: string = "请选择销售仓";
    selectWorkerText: string = "请选择施工人员";
    oilshow: boolean = false;
    orders: server.order[];
    clients: server.client[];
    client: server.client = null;
    sales: work.userlist[];
    showSalesmans: boolean = false;
    type: number = null;

    showStep1: boolean = true;
    showStep2: boolean = false;
    showStep3: boolean = false;
    step3Prevent: boolean = true;

    mobile: string;
    contact: string;
    carNo: string = '';
    strCarOrBoat: string = '船号';

    pMinPrice: number = 0;
    pMinInvoicePrice: number = 0;
    
    show2: boolean = false;
    
    hasplan: boolean = false;
    istrans: boolean = false;
    sv: string = "";
    ordersv: string = "";
    page: number;
    sp_page: number;//salesplans分页使用的page
    scrollRef: any;
    scrollRef_sp: any;//salesplans引用使用的下拉刷新对象
    pSize: number = 20;

    showAddDelReason: boolean = false;
    delReason: string = "";//删单原因

    filterCType: Array<helper.filterBtn>;
    filterOrderType: Array<helper.filterBtn>;
    actBtnId: number; //当前激活状态的button
    actBtnId1: number;

    constructor() {
        super();

        this.filterCType = [
            { id: 0, name: '水上', value: true, actived: true },
            { id: 1, name: '陆上', value: false, actived: false }
        ];
        this.filterOrderType = [
            { id: 0, name: '水上', value: true, actived: true },
            { id: 1, name: '陆上', value: false, actived: false }
        ];
        this.actBtnId = 0;
        this.actBtnId1 = 0;

        this.salesplans = new Array();
        this.salesplan = new Object() as server.salesPlan;
        this.model = (new Object()) as server.order;
        this.model.isInvoice = false;
        this.model.isDeliver = false;
        this.model.carNo = null;
        this.model.price = '';
        this.model.count = 0;
        this.model.billingPrice = 0;
        this.model.billingCount = 0;
        this.model.totalMoney = 0;
        this.model.ticketType = -1;
        this.model.unit = '升';
        this.model.billingCompany = '';
        this.model.deliverMoney = 0;
        this.model.orderType = 0;

        this.client = new Object() as server.client;
        this.clients = new Array<server.client>();

        this.orders = new Array();
        this.oiloptions = new Array();

        this.selectOrder = new Object() as server.order;

        this.getOrderNo();
        this.getOilProducts();
        this.stores = new Array<server.store>();
        this.workers = new Array<work.userlist>();
    }

    salesplanselect() {
        this.sp_page = 1;
        this.getSalesPlans();
        this.salesplanshow = true;
    }

    strMinPriceTip() {
        if (this.pMinPrice > 0 && this.pMinInvoicePrice > 0)
            return "最低：￥" + this.pMinPrice + "，开票：￥" + this.pMinInvoicePrice;
        else
            return ""
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
    switchBtn(o: helper.filterBtn, idx: number) {
        o.actived = true;
        if (idx != this.actBtnId) {
            this.filterCType[this.actBtnId].actived = false;
            this.actBtnId = idx;
            
            this.sp_page = 1;
            this.getSalesPlans();
        }
    }
    switchOrderTypeBtn(o: helper.filterBtn, idx: number) {
        o.actived = true;
        if (idx != this.actBtnId1) {
            this.filterOrderType[this.actBtnId1].actived = false;
            this.actBtnId1 = idx;

            this.page = 1;
            this.getOrders();
        }
    }

    planitemclick(s: server.salesPlan): void {
        if (s.state == server.salesPlanState.未审批) { this.toastError("该计划未经审核，请通知上级审核通过！"); return; }
        this.salesplan = s;
        this.selectedplanNo = s.name;
        this.model.salesPlanId = s.id;
        this.model.carNo = s.carNo;
        this.model.price = s.price;
        this.model.minPrice = s.minPrice;
        this.model.count = s.count;
        this.model.unit = s.unit;
        this.model.isInvoice = s.isInvoice;
        this.model.ticketType = s.ticketType;
        this.model.billingCompany = s.billingCompany;
        this.model.billingPrice = s.billingPrice;
        this.model.billingCount = s.billingCount;
        this.model.salesman = s.createdBy;
        this.model.isDeliver = s.isDeliver;
        this.model.isPrintPrice = s.isPrintPrice;
        this.model.remark = (s.remark == null) ? "" : s.remark;
        
        //this.model.clientId = s.cl
        this.oilName = s.oilName;
        this.model.productId = s.productId;
        this.model.orderType = s.salesPlanType;
        
        this.hasplan = true;

        this.salesplanshow = false;

        this.strCarOrBoat = s.salesPlanType == server.salesPlanType.水上加油 || s.salesPlanType == server.salesPlanType.水上机油 ? "船号" : "车号/客户名称";
    };

    emptyclick(): void {
        this.selectedplanNo = "无计划或散客";
        this.model.carNo = null;
        this.model.salesPlanId = null;

        this.hasplan = false;

        this.salesplanshow = false;
    };

    goStep2() {
        this.showStep1 = false;
        this.showStep2 = true;
        this.getClient();
    }

    //完善客户资料，第三步
    goStep3() {
        console.log(this.$refs.contact["valid"]);
        if (!this.$refs.contact["valid"]) { this.toastError("请正确填写联系人"); return; };
        if (!this.$refs.mobile["valid"]) { this.toastError("请正确填写手机"); return; };
        this.client.mobile = this.mobile;
        this.client.contact = this.contact;
        console.log(this.client);
        this.putUpdateClientInfo();
    }

    showSalesmansclick() {
        if (this.model.orderType == server.salesPlanType.水上加油.toString()
            || this.model.orderType == server.salesPlanType.水上机油.toString())
            this.getWaterSales();
        else if (this.model.orderType == server.salesPlanType.陆上装车.toString()
            || this.model.orderType == server.salesPlanType.汇鸿车辆加油.toString()
            || this.model.orderType == server.salesPlanType.外来车辆加油.toString())
            this.getLandSales();
    };

    selectsalesclick(s: work.userlist) {
        this.model.salesman = s.name;
        this.showSalesmans = false;
    };

    //显示选择销售仓
    selectStoreclick() {
        this.showStores = true;
        this.getStores();
    }

    storeclick(s: server.store) {
        this.model.storeId = s.id;
        this.selectStoreText = s.name;
        this.showStores = false;
    }

    //显示选择生产工
    selectWorkerclick() {
        this.showWorkers = true;
        this.getWorkers();
    }

    workerclick(w: work.userlist) {
        this.model.worker = w.name;
        this.selectWorkerText = w.name;
        this.showWorkers = false;
    }

    buttonclick() {
        //信息验证
        if (this.model.carNo == '') {this.toastError('船号或车牌号不能为空');return;}
        if (!this.model.productId) {this.toastError('必须选择商品');return;}
        if (!this.model.count || this.model.count <= 0) {this.toastError('数量必须大于1');return;}
        if (this.model.salesman == "") {this.toastError('必须指定销售员');return;}
        if (this.model.price == '' || this.model.price <= 0) { this.toastError("销售单价输入有误"); return; }
        if (!this.model.isInvoice && this.model.price < this.pMinPrice) { this.toastError("当前最低销售单价是￥" + this.pMinPrice + "/升"); return; }
        if (this.model.isInvoice && this.model.price < this.pMinInvoicePrice) { this.toastError("当前开票最低销售单价是￥" + this.pMinInvoicePrice + "/升"); return; }

        if (this.model.isInvoice) {
            if (this.model.billingCompany == '' || this.model.billingCompany == null) { this.toastError('请输入开票单位') }
            if (this.model.billingPrice <= 0 || this.model.billingPrice == null) { this.toastError('请输入开票单价') }
            if (this.model.billingCount <= 0 || this.model.billingCount == null) { this.toastError('请输入开票数量') }
        }

        if (this.model.orderType == server.salesPlanType.水上加油 && (this.model.storeId == null)) { this.toastError("请选择销售仓"); return; }
        if (this.model.orderType == server.salesPlanType.水上加油 && (this.model.worker == null)) { this.toastError("请选择施工人员"); return; }
        console.log(this.model);
        this.postOrder(this.model);
    }

    godetail(id: number) {
        this.$router.push('/sales/order/' + id + '/order');
    }
    
    showMenuclick(o: server.order) {
        this.menus = new Array();
        this.menus = [
            {
                label: '单据明细',
                callback: () => {
                    this.godetail(o.id)
                }
            },
            {
                label: '打印【调拨单】到【收银台】',
                callback: () => {
                    this.getPrintOrder(o.id, "收银台")
                }
            },
            {
                label: '打印【调拨单】到【地磅室】',
                callback: () => {
                    this.getPrintOrder(o.id, "地磅室")
                }
            }
        ];
        //如果为陆上销售，则添加相应打印菜单
        if (o.orderType == server.salesPlanType.陆上装车) {
            if (o.isDeliver) {
                this.menus.push({
                    label: '打印【送货单】到【地磅室】',
                    callback: () => {
                        this.getPrintDeliver(o.id, '地磅室')
                    }
                });
            }
            //如果施工状态为已完成，则添加相应打印菜单
            if (o.state == server.orderState.已完成) {
                this.menus.push({
                    label: '打印【陆上装车单】到【地磅室】',
                    callback: () => {
                        this.getPrintLandload(o.id, '地磅室')
                    }
                });
                this.menus.push({
                    label: '打印【过磅单】到【地磅室】',
                    callback: () => {
                        this.getPrintPonderation(o.id, '地磅室')
                    }
                });
            }
        }

        //删单
        this.menus.push({
            label: '作废单据',
            callback: () => {
                this.selectOrder = o;
                this.showAddDelReason = true;
            }
        })
        this.showMenus = true;
    }

    //删除单据
    delOrderclick() {
        if (this.delReason == null || this.delReason == "") { this.toastError("请填写作废原因"); return; }
        if (this.selectOrder.state == server.orderState.已完成) {
            this.$dialog.confirm({
                title: '提示',
                mes: '当前单据施工状态为已完成，作废单据将恢复油量' + this.selectOrder.oilCountLitre + "升到油仓，是否确认？",
                opts: () => {
                    this.deleteOrder();
                }
            })
        }
        else
            this.deleteOrder()
    }

    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 销售单');

        //观察者模式
        this.$watch('model.orderType', (v, ov) => {
            switch (v) {
                case "0":
                    this.model.unit = '升';
                    this.strCarOrBoat = "船号";
                    break;
                case "1":
                    this.strCarOrBoat = "车牌号";
                    this.model.unit = '吨';
                    break;
                case "2":
                    this.strCarOrBoat = "车牌号";
                    this.model.unit = '桶';
                    break;
                case "4":
                    this.strCarOrBoat = "车牌号";
                    this.model.unit = '升';
                    break;
                case "5":
                    this.strCarOrBoat = "车牌号";
                    this.model.unit = '升';
                    break;
            }
        });

        this.$watch('type', (v, ov) => {
            if (v == 1) {
                this.model.orderType = server.salesPlanType.陆上装车;
                this.model.unit = "吨";
                this.strCarOrBoat = "车牌号";
            }
            else {
                this.model.orderType = server.salesPlanType.水上加油;
                this.model.unit = "升";
                this.strCarOrBoat = "船号";
            }
        });

        this.$watch('model.price', (v, ov) => {
            this.model.billingPrice = v;
            this.model.totalMoney = <number>this.model.price * this.model.count;
        });

        this.$watch('model.count', (v, ov) => {
            this.model.billingCount = v;
            this.model.totalMoney = <number>this.model.price * v;
            console.log(this.model.totalMoney)
        });
        //搜索计划单
        this.$watch('sv', (v, ov) => {
            if (v.length > 1 || v == "") {
                this.sp_page = 1;
                this.getSalesPlans();
            }
        });
        //搜索销售单
        this.$watch('ordersv', (v, ov) => {
            if (v.length > 1 || v == "") {
                this.page = 1;
                this.getOrders();
            }
        });
    };

    change(label: string, tabkey: string) {
        console.log(label);
        (<any>this).$refs.infinitescroll.$emit('ydui.infinitescroll.reInit');
        this.salesplans = null;
        this.page = 1;
        if (label == '单据记录') {
            this.getOrders();
        }
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
            this.scrollRef.$emit("ydui.infinitescroll.finishLoad");

            if (list.length > 0)
                this.page++;
            else
                this.page = 1;
            console.log("page = " + this.page)
        });
    }

    loadList_sp() {
        this.getSalesPlans((list: server.salesPlan[]) => {
            this.salesplans = this.sp_page > 1 ? [...this.salesplans, ...list] : this.salesplans;
            this.scrollRef_sp = (<any>this).$refs.spInfinitescroll;
            if (list.length < this.pSize) {
                this.scrollRef_sp.$emit("ydui.infinitescroll.loadedDone");
                return;
            }

            //通知加载数据完毕
            this.scrollRef_sp.$emit("ydui.infinitescroll.finishLoad");

            if (list.length > 0)
                this.sp_page++;
            else
                this.sp_page = 1;
            console.log("sp_page = " + this.sp_page)
        });
    }

    getSalesPlans(callback?: Function) {
        if (this.sv == null) this.sv = "";
        if (this.sp_page == null) this.sp_page = 1;
        axios.get('/api/SalesPlan/Unfinish?kw=' + this.sv +
            "&iswater=" + this.filterCType[this.actBtnId].value +
            "&page=" + this.sp_page +
            "&pagesize=" + this.pSize).then((res) => {
            let jobj = res.data as server.resultJSON<server.salesPlan[]>;
            if (jobj.code == 0) {
                if (callback) {
                    callback(jobj.data);
                }
                else {
                    this.salesplans = jobj.data;
                    this.sp_page++;
                }
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

    getOilProducts() {
        axios.get('/api/Product/OilProducts').then((res) => {
            let jobj = res.data as server.resultJSON<server.product[]>;
            if (jobj.code == 0) {
                jobj.data.forEach((o, i) => {
                    this.oiloptions.push({
                        label: o.name,
                        callback: () => {
                            this.oilName = o.name;
                            this.model.productId = o.id;
                            this.model.price = o.minPrice;
                            this.pMinInvoicePrice = o.minInvoicePrice;
                            this.pMinPrice = o.minPrice;
                            if (!this.selectProduct) {
                                this.selectProduct = new Object as server.product;
                            }
                            this.selectProduct = o;
                        }
                    });
                });
            }
        });
    }

    getOrders(callback?: Function) {
        if (this.ordersv == null) this.ordersv = "";
        if (this.page == null) this.page = 1;
        axios.get('/api/Order/GetByPager?page='
            + this.page
            + '&pagesize=' + this.pSize
            + '&iswater=' + this.filterOrderType[this.actBtnId1].value
            + '&sv=' + this.ordersv
            ).then((res) => {
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

    //获得水上销售员
    getWaterSales() {
        axios.get('/api/User/WaterSalesman').then((res) => {
            let jobj = res.data as work.tagMemberResult;
            if (jobj.errcode == 0) {
                this.showSalesmans = true;
                this.sales = jobj.userlist;
            }
        });
    }

    //获得陆上销售员
    getLandSales() {
        axios.get('/api/User/LandSalesman').then((res) => {
            let jobj = res.data as work.tagMemberResult;
            if (jobj.errcode == 0) {
                this.sales = jobj.userlist;
                this.showSalesmans = true;
            }
        });
    }

    //获取生产员
    getWorkers() {
        axios.get('/api/User/Worker').then((res) => {
            let jobj = res.data as work.tagMemberResult;
            if (jobj.errcode == 0) {
                this.workers = jobj.userlist;
            }
        });
    }

    //取得客户，如果没有找到该客户，则新增一个客户
    getClient() {
        let carNo = this.model.carNo;
        if (carNo == "" || carNo == null) {
            this.toastError("请输入船号或车号");
            return;
        }
        axios.get('/api/client/CreateOrGetClientByCarNo?carno=' + carNo).then((res) => {
            let jobj = res.data as server.resultJSON<server.client>;
            if (jobj.code == 0) {
                this.showStep1 = false;
                this.showStep2 = true;
                this.step3Prevent = false;//释放下一步
                this.client = jobj.data;
                if (jobj.data != null) {
                    if (this.model.billingCompany == '') {
                        this.model.billingCompany = this.client.company != null ? this.client.company.name : "";
                        this.model.ticketType = this.client.company != null ? this.client.company.ticketType : -1;
                    }

                    this.mobile = this.client.mobile ? this.client.mobile : "";
                    this.contact = this.client.contact ? this.client.contact : "";
                }
            }
        });
    }

    //取得所有销售仓
    getStores() {
        axios.get('/api/Store/GetByClass?sc=' + server.storeClass.销售仓.toString()).then((res) => {
            let jobj = res.data as server.resultJSON<server.store[]>;
            if (jobj.code == 0)
                this.stores = jobj.data;
            else
                this.toastError(jobj.msg)
        });
    }

    postOrder(model: server.order) {
        if (this.selectProduct)
            model.minPrice = model.isInvoice ? this.selectProduct.minInvoicePrice : this.selectProduct.minPrice;
        console.log(model.minPrice);
        axios.post('/api/Order', model).then((res) => {
            let jobj = res.data as server.resultJSON<server.order>;
            if (jobj.code == 0) {
                this.addNextConfirm();
                this.isPrevent = true;
            }
            else
                this.toastError(jobj.msg);
        });
    }

    putUpdateClientInfo() {
        axios.put('/api/Client', this.client).then((res) => {
            let jobj = res.data as server.resultJSON<server.client>;
            if (jobj.code == 0) {
                this.showStep2 = false;
                this.showStep3 = true;
                console.log("showStep3 = " + this.showStep3);
                console.log("model.orderType = " + this.model.orderType);

            }
        })
    }

    deleteOrder() {
        axios.delete('/api/Order?id=' + this.selectOrder.id + "&delreason=" + this.delReason).then((res) => {
            let jobj = res.data as server.resultJSON<server.client>;
            if (jobj.code == 0) {
                this.toastSuccess("作废成功！");
                this.showAddDelReason = false;
                this.delReason = "";
            }
            else
                this.toastError(jobj.msg);
        })
    }
}