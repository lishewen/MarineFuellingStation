import ComponentBase from "../../componentbase";
import { Component } from 'vue-property-decorator';
import axios from "axios";

@Component({
    components: {
        WeuiSearch: require('../weui-search/search.vue')
    }
})
export default class AssayComponent extends ComponentBase {
    model: server.assay;
    assay: server.assay;
    store: server.store[];
    purchase: server.purchase[];
    selectedStore: number | string = '';
    selectedPurchase: number | string = '';
    list: server.assay[];

    radio2: string = "1";
    carNo: string = "";
    show1: boolean = true;
    showDetail: boolean = false;
    sv: string = "";
    filterclick(): void {
    };

    constructor() {
        super();

        this.model = (new Object()) as server.assay;
        this.assay = (new Object()) as server.assay;
        this.store = new Array<server.store>();
        this.purchase = new Array<server.purchase>();
        this.list = new Array<server.assay>();
        this.model.name = '';

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
                this.model.assayType = server.assayType.采购化验;
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
        this.postAssay(this.model);
    }

    getAssayNo() {
        axios.get('/api/Assay/AssayNo').then((res) => {
            let jobj = res.data as server.resultJSON<string>;
            if (jobj.code == 0)
                this.model.name = jobj.data;
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
            if (jobj.code == 0){
                this.purchase = jobj.data;
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
        model.storeId = this.selectedStore;
        model.purchaseId = this.selectedPurchase;
        axios.post('/api/Assay', model).then((res) => {
            let jobj = res.data as server.resultJSON<server.assay>;
            if (jobj.code == 0) {
                this.getAssayNo();
                this.toastSuccess(jobj.msg)
            }
        });
    }
}