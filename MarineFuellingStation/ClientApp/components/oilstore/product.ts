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
        this.currentproduct.name = '';

        this.getProductTypes();
    }

    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 商品');
    };

    toastError(msg: string) {
        (<any>this).$dialog.toast({
            mes: msg,
            timeout: 1500,
            icon: 'error'
        });
    }

    change(label: string, tabkey: string) {
        this.$emit('setTitle', this.$store.state.username + ' ' + label);
        if (label == '添加') {
            this.currentproduct = (new Object()) as server.product;
            this.selectptname = '请选择分类';
        }
    }

    ptClick(pt: server.productType) {
        this.show1 = true;
        this.currentpt = pt;
    }

    addpt(e: Event) {
        this.show2 = true;
        //取消父级事件相应
        e.cancelBubble = true;
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
        if (this.ptName == '') {
            this.toastError('分类名称不能为空');
            return;
        }

        let model = (new Object()) as server.productType;
        model.name = this.ptName;
        axios.post('/api/ProductType', model).then((res) => {
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

    postProduct() {
        if (this.currentproduct.name == '') {
            this.toastError('商品名称不能为空');
            return;
        }
        if (this.currentproduct.minPrice <= 0) {
            this.toastError('商品单价不能为0');
            return;
        }
        if (this.currentproduct.productTypeId <= 0) {
            this.toastError('商品必须选择分类');
            return;
        }

        axios.post('/api/Product', this.currentproduct).then((res) => {
            let jobj = res.data as server.resultJSON<server.product>;
            if (jobj.code == 0) {
                (<any>this).$dialog.toast({
                    mes: jobj.msg,
                    timeout: 1500,
                    icon: 'success'
                });
            }
        });
    }
}