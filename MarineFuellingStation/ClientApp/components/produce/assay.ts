import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import axios from "axios";

@Component({
    components: {
        WeuiSearch: require('../weui-search/search.vue')
    }
})
export default class AssayComponent extends Vue {
    model: server.assay;
    store: server.store[];
    purchase: server.purchase[];
    selectedStore: number | string = '';
    selectedPurchase: number | string = '';

    radio2: string = "1";
    carNo: string = "";
    show1: boolean = true;
    sv: string = "";
    filterclick(): void {
    };

    constructor() {
        super();

        this.model = (new Object()) as server.assay;
        this.model.name = '';

        this.getAssayNo();
        this.getStore();
    }

    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 化验');
        this.$watch('radio2', (v, ov) => {
            this.show1 = (v == "1") ? true : false;
        });
    };

    change(label: string, tabkey: string) {
        console.log(label);
        this.$emit('setTitle', this.$store.state.username + ' ' + label);
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

    getPurchase() {
        axios.get('/api/Purchase').then((res) => {
            let jobj = res.data as server.resultJSON<server.purchase[]>;
            if (jobj.code == 0)
                this.purchase = jobj.data;
        });
    }

    postAssay(model: server.assay) {
        axios.post('/api/Assay', model).then((res) => {
            let jobj = res.data as server.resultJSON<server.assay>;
            if (jobj.code == 0) {
                this.getAssayNo();
                (<any>this).$dialog.toast({
                    mes: jobj.msg,
                    timeout: 1500,
                    icon: 'success'
                });
            }
        });
    }
}