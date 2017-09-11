import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import axios from "axios";

@Component({
    components: {
        WeuiSearch: require('../weui-search/search.vue')
    }
})
export default class PurchaseComponent extends Vue {
    model: server.purchase;
    list: server.purchase[];
    oilshow: boolean = false;
    oiloptions: ydui.actionSheetItem[];
    oilName: string = '';

    radio1: string = "2";
    show2: boolean = false;
    carNo: string = "";
    sv: string = "";
    filterclick(): void {
        this.show2 = false;
    };
    constructor() {
        super();

        this.oiloptions = (new Array()) as ydui.actionSheetItem[];

        this.model = (new Object()) as server.purchase;
        this.model.name = '';
        this.model.price = 0;
        this.model.count = 0;
        this.model.origin = '';

        this.getPurchaseNo();
        this.getPurchases();
    }

    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 的采购计划');
        this.$watch('radio1', (v, ov) => {
            switch (v) {
                case "1":
                    this.show2 = false;
                    break;
                case "2":
                    this.show2 = true;
                    break;
            }
        });
        this.$watch('sv', (v: string, ov) => {
            //3个字符开始才执行请求操作，减少请求次数
            if (v.length >= 3)
                this.searchPurchases(v);
        });
    };

    change(label: string, tabkey: string) {
        console.log(label);
        this.$emit('setTitle', this.$store.state.username + ' ' + label);
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
        this.postPurchase(this.model);
    }

    toastError(msg: string) {
        (<any>this).$dialog.toast({
            mes: msg,
            timeout: 1500,
            icon: 'error'
        });
    }

    getPurchaseNo() {
        axios.get('/api/Purchase/PurchaseNo').then((res) => {
            let jobj = res.data as server.resultJSON<string>;
            if (jobj.code == 0)
                this.model.name = jobj.data;
        });
    }

    getPurchases() {
        axios.get('/api/Purchase').then((res) => {
            let jobj = res.data as server.resultJSON<server.purchase[]>;
            if (jobj.code == 0)
                this.list = jobj.data;
        });
    }

    searchPurchases(sv: string) {
        axios.get('/api/Purchase/' + sv).then((res) => {
            let jobj = res.data as server.resultJSON<server.purchase[]>;
            if (jobj.code == 0)
                this.list = jobj.data;
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
                            this.model.price = o.lastPrice;
                        }
                    });
                });
            }
        });
    }

    postPurchase(model: server.purchase) {
        axios.post('/api/Purchase', model).then((res) => {
            let jobj = res.data as server.resultJSON<server.purchase>;
            if (jobj.code == 0) {
                this.getPurchaseNo();
                (<any>this).$dialog.toast({
                    mes: jobj.msg,
                    timeout: 1500,
                    icon: 'success'
                });
            }
        });
    }
}