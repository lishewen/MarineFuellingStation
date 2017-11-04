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
    list: server.assay[];

    radio2: string = "1";
    carNo: string = "";
    isPrevent: boolean = true;
    show1: boolean = true;
    showDetail: boolean = false;
    sv: string = "";
    showPurchases: boolean = false;

    filterclick(): void {
    };

    constructor() {
        super();

        this.model = (new Object()) as server.assay;
        this.assay = (new Object()) as server.assay;
        this.store = new Array<server.store>();
        this.purchases = new Array<server.purchase>();
        this.list = new Array<server.assay>();
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
            if (v.length >= 3)
                this.searchAssays(v);
        });
    };

    change(label: string, tabkey: string) {
        console.log(label);
        this.$emit('setTitle', this.$store.state.username + ' ' + label);
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
        this.list = ls;
    }

    getAssays() {
        axios.get('/api/Assay/GetWithStANDPur').then((res) => {
            let jobj = res.data as server.resultJSON<server.assay[]>;
            if (jobj.code == 0) {
                this.list = jobj.data;
            }
        });
    }

    searchAssays(sv: string) {
        axios.get('/api/Assay/' + sv).then((res) => {
            let jobj = res.data as server.resultJSON<server.assay[]>;
            if (jobj.code == 0)
                this.list = jobj.data;
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