import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import axios from "axios";

@Component
export default class ProductComponent extends Vue {
    carNo: string = "";
    show1: boolean = false;
    show2: boolean = false;
    ptshow: boolean = false;
    /** 分类列表 */
    pts: server.productType[];
    currentpt: server.productType;
    ptoptions: ydui.actionSheetItem[];
    currentproduct: server.product;
    selectptname: string = '请选择分类';
    ptName: string = '';

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

    postProductType() {
        let name = this.ptName;
        axios.post('/api/ProductType', name).then((res) => {
            let jobj = res.data as server.resultJSON<server.productType>;
            if (jobj.code == 0) {
                (<any>this).$dialog.toast({
                    mes: jobj.msg,
                    timeout: 1500,
                    icon: 'success'
                });
                //将新增的分类加入到列表中
                this.pts.push(jobj.data);
                //关闭popup
                this.show2 = false;
            }
        });
    }
}