import Vue from 'vue';
import axios from "axios";
import { Component } from 'vue-property-decorator';
@Component({
    components: {
        WeuiSearch: require('../weui-search/search.vue')
    }
})
export default class MyOrderComponent extends Vue {
    model: server.client;
    modelCompany: server.company;
    companys: server.company[];
    products: server.product[];
    carsOrBoats: Array<Object>;

    sales: Object[] = [
        { id: 1, name: '张三', clientcount:20 },
        { id: 2, name: '李四', clientcount: 21 },
        { id: 3, name: '王五', clientcount: 22 }
    ];
    companyName: string;
    salesName: string;

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
    boatNo: Array<string>;
    sv: string = "";
    
    constructor() {
        super();

        this.oiloptions = new Array();

        this.model = (new Object()) as server.client;
        this.modelCompany = (new Object()) as server.company;
        this.model.placeType = server.placeType.水上;
        this.model.clientType = server.clientType.个人;

        this.getOilProducts();
        this.salesName = '请选择';
        this.companyName = '请选择';
        //测试公司数据
        this.companys = new Array<server.company>();
        this.companys.push({ id: 1, name: 'XXXX有限公司' });
        this.companys.push({ id: 2, name: 'AAAA有限公司' });

        this.carsOrBoats = new Array<Object>();
    }

    filterclick(): void {
        this.show2 = false;
    };

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
        this.$watch('model.clientType', (v, ov) => {
            switch (v) {
                case "0":
                    this.show1 = false;
                    break;
                case "1":
                    this.show1 = true;
                    break;
            }
        });
        this.$watch('model.placeType', (v, ov) => {
            switch (v) {
                case "0":
                    this.show3 = false;
                    break;
                case "1":
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

    addNo() {

    }

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

    selectcompanyclick(company: server.company) {
        this.companyName = company.name;
        this.showcompany = false;
    }

    selectsalesclick(sales: Object) {
        this.salesName = sales.name;
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