import Vue from 'vue';
import axios from "axios";
import { Component } from 'vue-property-decorator';
@Component({
    components: {
        WeuiSearch: require('../weui-search/search.vue')
    }
})
export default class MyOrderComponent extends Vue {
    modelCompany: server.company;
    companys: server.company[];
    products: server.product[];
    sales: Object[] = [
        { id: 1, name: '张三', clientcount:20 },
        { id: 2, name: '李四', clientcount: 21 },
        { id: 3, name: '王五', clientcount: 22 }
    ];

    oiloptions: ydui.actionSheetItem[];
    oilName: string = '请选择';

    radio1: string = "2";
    radio2: string = "1";
    oilshow: boolean = false;
    nonshow: boolean = false;
    show1: boolean = false;
    show2: boolean = false;
    show3: boolean = true;
    showcompany: boolean = false;
    showaddcompany: boolean = false;
    showsales: boolean = false;
    carNo: string = "";
    sv: string = "";
    picked: string = "";
    filterclick(): void {
        this.show2 = false;
    };

    constructor() {
        super();

        this.oiloptions = new Array();

        this.modelCompany = (new Object()) as server.company;
        this.getOilProducts();
        this.picked = '';
    }

    toastError(msg: string) {
        (<any>this).$dialog.toast({
            mes: msg,
            timeout: 1500,
            icon: 'error'
        });
    }
    toastSuccess(msg: string) {
        (<any>this).$dialog.toast({
            mes: msg,
            timeout: 1500,
            icon: 'success'
        });
    }
   
    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 的客户');
        this.$watch('radio2', (v, ov) => {
            switch (v) {
                case "1":
                    this.show1 = false;
                    break;
                case "2":
                    this.show1 = true;
                    break;
            }
        });
        this.$watch('radio1', (v, ov) => {
            switch (v) {
                case "1":
                    this.show3 = false;
                    break;
                case "2":
                    this.show3 = true;
                    break;
            }
        });
        this.$watch('showcompany', (v, ov) => {
            if (v)
                console.log('打开公司列表popup')
                //this.getCompanys();
        });
        this.$watch('showsales', (v, ov) => {
            if (v)
                console.log('打开公司列表popup')
                //this.getCompanys();
        });
    };

    switchaddcompany() {
        this.showcompany = false;
        this.showaddcompany = true;
    }

    change(label: string, tabkey: string) {
        console.log(label);
        this.$emit('setTitle', this.$store.state.username + ' ' + label);
    }

    addcompanyclick() {
        if (this.modelCompany.name == '') {
            this.toastError("名称不能为空")
        }
        this.postCompany(this.modelCompany);
    }

    selectsalesclick(sales: Object) {
        console.log(sales);
        this.showsales = false;
    }

    //后台提交
    //新增公司
    postCompany(model: server.company) {
        axios.post('/api/Company', model).then((res) => {
            let jobj = res.data as server.resultJSON<server.company>;
            if (jobj.code == 0) {
                this.modelCompany.name = '';
                this.toastSuccess('操作成功')
            }
            else
                this.toastError(jobj.msg);
        });
    }
    //获得公司列表
    getCompanys(kw: string) {
        axios.get('/api/Company?kw=' + kw).then((res) => {
            let jobj = res.data as server.resultJSON<server.company[]>;
            if (jobj.code == 0)
                this.companys = jobj.data;
            else
                this.toastError('无法获取公司数据，请重试')
        });
    }
    //获得商品列表
    getOilProducts() {
        axios.get('/api/Product/OilProducts').then((res) => {
            let jobj = res.data as server.resultJSON<server.product[]>;
            if (jobj.code == 0) {
                jobj.data.forEach((o, i) => {
                    this.oiloptions.push({
                        label: o.name,
                        method: () => {
                            this.oilName = o.name;
                        }
                    });
                });
            }
        });
    }
    //获得销售员
    getSales() {
        axios.get('/api/User').then((res) => {
            let jobj = res.data as server.resultJSON<server.salesPlan[]>;
            if (jobj.code == 0)
                //this.salesplans = jobj.data;
                console.log('获取销售员成功')
        });
    }
}