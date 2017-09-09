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
    clients: server.client[];
    products: server.product[];

    sales: Object[] = [
        { id: 1, name: '张三', clientcount:20 },
        { id: 2, name: '李四', clientcount: 21 },
        { id: 3, name: '王五', clientcount: 22 }
    ];
    companyName: string;

    oiloptions: ydui.actionSheetItem[];
    oilName: string = '请选择';

    radio1: string = "2";
    radio2: string = "1";
    oilshow: boolean = false;
    nonshow: boolean = false;
    show1: boolean = false;
    show2: boolean = false;
    iswater: boolean = true;
    showcompany: boolean = false;
    showaddcompany: boolean = false;
    showsales: boolean = false;
    labelBoatOrCar: string = "";
    svCompany: string = "";
    svCompany1: string = "";
    svClient: string = "";
    svSales: string = "";
    
    constructor() {
        super();

        this.oiloptions = new Array();

        this.model = (new Object()) as server.client;
        this.modelCompany = (new Object()) as server.company;
        this.clients = new Array<server.client>();
        this.companys = new Array<server.company>();
        this.model.placeType = server.placeType.水上;
        this.model.clientType = server.clientType.个人;
        this.model.followSalesman = '请选择';
        this.model.maxOnAccount = 0;
        
        this.labelBoatOrCar = "船号";
        this.getOilProducts();
        this.companyName = '请选择';
        this.getCompanys('');
        this.getClients('');
        
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
            switch (parseInt(v)) {
                case server.clientType.个人:
                    this.companyName = "";
                    this.show1 = false;
                    break;
                case server.clientType.公司:
                    this.show1 = true;
                    break;
            }
        });
        this.$watch('model.placeType', (v, ov) => {
            switch (parseInt(v)) {
                case server.placeType.陆上:
                    this.labelBoatOrCar = "车牌号"
                    break;
                case server.placeType.水上:
                    this.labelBoatOrCar = "船号"
                    break;
            }
        });
        this.$watch('svCompany', (v: string, ov) => {
            //3个字符开始才执行请求操作，减少请求次数
            if (v.length >= 3)
                this.getCompanys(v);
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

    selectcompanyclick(company: server.company) {
        this.companyName = company.name;
        this.model.companyId = company.id;
        this.showcompany = false;
    }

    selectsalesclick(sales: Object) {
        this.model.followSalesman = sales.name;
        this.showsales = false;
    }
    //提交新增公司
    addcompanyclick() {
        if (this.modelCompany.name == '') {
            this.toastError("名称不能为空")
        }
        this.postCompany(this.modelCompany);
    }
    //提交新增客户
    addclientclick() {
        let $model = this.model;
        if ($model.carNo == "" || $model.carNo == null) {
            this.toastError("请输入" + this.labelBoatOrCar);
            return;
        }
        if ($model.followSalesman == "请选择" || $model.followSalesman == null) {
            this.toastError("请选择跟进销售人员");
            return;
        }
        if ($model.defaultProductId == 0 || $model.defaultProductId == null) {
            this.toastError("请选择默认商品");
            return;
        }
        if ($model.contact == "" || $model.contact == null) {
            this.toastError("请输入联系人");
            return;
        }
        if ($model.mobile == "" || $model.mobile == null) {
            this.toastError("请输入联系电话");
            return;
        }
        if ($model.maxOnAccount == "")
            $model.maxOnAccount = 0;
        console.log($model);
        this.postClient($model);
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
    //新增客户
    postClient(model: server.client) {
        axios.post('/api/Client', model).then((res) => {
            let jobj = res.data as server.resultJSON<server.client>;
            if (jobj.code == 0) {
                this.toastSuccess('操作成功')
            }
            else
                this.toastError(jobj.msg);
        });
    }
    //获得公司列表
    getCompanys(kw: string) {
        axios.get('/api/Company/' + kw).then((res) => {
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
                            this.model.defaultProductId = o.id;
                        }
                    });
                });
            }
        });
    }
    //获得客户列表
    getClients(kw: string) {
        axios.get('/api/Client/' + kw).then((res) => {
            let jobj = res.data as server.resultJSON<server.client[]>;
            if (jobj.code == 0)
                this.clients = jobj.data;
            else
                this.toastError('无法获取客户数据，请重试')
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