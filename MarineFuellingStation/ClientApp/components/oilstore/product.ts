import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import axios from "axios";

@Component
export default class ProductComponent extends Vue {
    carNo: string = "";
    show1: boolean = false;
    show2: boolean = false;
    ptshow: boolean = false;
    pts: server.productType[];
    currentpt: server.productType;
    ptoptions: ydui.actionSheetItem[];
    currentproduct: server.product;
    selectptname: string = '请选择分类';

    constructor() {
        super();

        this.pts = new Array();
        this.ptoptions = new Array();
        this.currentpt = (new Object()) as server.productType;
        this.currentproduct = (new Object()) as server.product;

        this.getProductTypes();
    }

    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 商品');
    };

    change(label: string, tabkey: string) {
        this.$emit('setTitle', this.$store.state.username + ' ' + label);
        if (label == '添加')
            this.currentproduct = (new Object()) as server.product;
    }

    ptClick(pt: server.productType) {
        this.show1 = true;
        this.currentpt = pt;
    }

    getProductTypes() {
        axios.get('/api/ProductType').then((res) => {
            let jobj = res.data as server.resultJSON<server.productType[]>;
            if (jobj.code == 0) {
                this.pts = jobj.data;
                jobj.data.forEach((o, i) => {
                    this.ptoptions.push({
                        label: o.name,
                        method: () => {
                            this.currentproduct.productTypeId = o.id;
                            this.selectptname = o.name;
                        }
                    });
                });
            }
        });
    }
}