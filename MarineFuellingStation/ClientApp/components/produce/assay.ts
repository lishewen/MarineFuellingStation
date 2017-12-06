import ComponentBase from "../../componentbase";
import { Component } from 'vue-property-decorator';
import axios from "axios";

@Component
export default class AssayComponent extends ComponentBase {
    model: server.assay;
    assay: server.assay;
    store: server.store[];
    purchases: server.purchase[];
    selectedStore: number | string = '';
    selectedPurchase: number | string = '';
    selectedPName: string = "";
    assays: server.assay[];

    radio2: string = "1";
    carNo: string = "";
    isPrevent: boolean = true;
    show1: boolean = true;
    showDetail: boolean = false;
    sv: string = "";
    showPurchases: boolean = false;

    page: number;
    scrollRef: any;
    pSize: number = 30;

    filterclick(): void {
    };

    constructor() {
        super();

        this.model = (new Object()) as server.assay;
        this.assay = (new Object()) as server.assay;
        this.store = new Array<server.store>();
        this.purchases = new Array<server.purchase>();
        this.assays = new Array<server.assay>();
        this.model.name = '';
        this.model.oilTempTime = this.formatDate(new Date(), 'YYYY-MM-DD HH:mm');

        this.getAssayNo();
        this.getStore();
        this.getAssays();
    }

    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 化验');
        this.$watch('radio2', (v, ov) => {
            if (v == "2") {
                this.show1 = false;
                this.getPurchase(10);
                this.model.assayType = server.assayType.进油化验;
            }
            else {
                this.show1 = true;
                this.model.assayType = server.assayType.油舱化验;
            }
        });
        this.$watch('sv', (v: string, ov) => {
            //3个字符开始才执行请求操作，减少请求次数
            if (v.length >= 2 || v == "")
                this.searchAssays(v);
        });
    };

    change(label: string, tabkey: string) {
        console.log(label);
        if (label == '记录') {
            this.page = 1;
            this.getAssays();
        }
    }
    loadList() {
        this.getAssays((list: server.assay[]) => {
            this.assays = this.page > 1 ? [...this.assays, ...list] : this.assays;
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

    /**
     * 显示化验单详细
     */
    assayclick(as: server.assay) {
        this.assay = as;
        this.showDetail = true;
    }

    buttonclick() {
        //信息验证
        if (this.model.assayType == server.assayType.油舱化验)
            if (this.selectedStore == null || this.selectedStore == '') {
                this.toastError("请选择油仓")
            }
        if (this.model.assayType == server.assayType.进油化验)
            if (this.selectedPurchase == null || this.selectedPurchase == '') {
                this.toastError
            }
        this.postAssay(this.model);
    }

    purchaseclick(p: server.purchase) {
        this.selectedPurchase = p.id;
        this.selectedPName = p.name;
        this.showPurchases = false;
    }

    getAssayNo() {
        axios.get('/api/Assay/AssayNo').then((res) => {
            let jobj = res.data as server.resultJSON<string>;
            if (jobj.code == 0) {
                this.model.name = jobj.data;
                this.isPrevent = false;
            }
        });
    }

    getStore() {
        axios.get('/api/Store').then((res) => {
            let jobj = res.data as server.resultJSON<server.store[]>;
            if (jobj.code == 0)
                this.store = jobj.data;
        });
    }

    getPurchase(n: number) {
        axios.get('/api/Purchase/GetTopN/' + n.toString()).then((res) => {
            let jobj = res.data as server.resultJSON<server.purchase[]>;
            if (jobj.code == 0) {
                this.purchases = jobj.data;
            }
        });
    }

    initlist(ls: server.assay[]) {
        this.assays = ls;
    }

    getAssays(callback?: Function) {
        if (this.page == null) this.page = 1;
        if (this.pSize == null) this.pSize = 30;
        axios.get('/api/Assay/GetByPager?'
            + 'page=' + this.page
            + '&pageSize=' + this.pSize
            ).then((res) => {
                let jobj = res.data as server.resultJSON<server.assay[]>;
                if (jobj.code == 0) {
                    if (callback) {
                        callback(jobj.data);
                    }
                    else {
                        this.assays = jobj.data;
                        this.page++;
                    }
                }
            });
    }

    searchAssays(sv: string) {
        axios.get('/api/Assay/' + sv).then((res) => {
            let jobj = res.data as server.resultJSON<server.assay[]>;
            if (jobj.code == 0)
                this.assays = jobj.data;
        });
    }

    postAssay(model: server.assay) {
        model.storeId = this.selectedStore as number;
        model.purchaseId = this.selectedPurchase as number;
        if (model.初硫 == null || model.初硫 == '') { this.toastError("请输入初硫"); return; };
        if (model.视密 == null || model.视密 == '') { this.toastError("请输入视密"); return; };
        if (model.标密 == null || model.标密 == '') { this.toastError("请输入标密"); return; };
        axios.post('/api/Assay', model).then((res) => {
            let jobj = res.data as server.resultJSON<server.assay>;
            if (jobj.code == 0) {
                this.toastSuccess(jobj.msg);
                this.isPrevent = true;
                (<any>this).$dialog.confirm({
                    title: '提示',
                    mes: '继续化验？',
                    opts: () => {
                        this.isPrevent = false;
                        this.getAssayNo();
                    }
                })
            }
        });
    }
}