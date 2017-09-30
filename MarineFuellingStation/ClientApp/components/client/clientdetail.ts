import ComponentBase from "../../componentbase";
import { Component } from 'vue-property-decorator';
import axios from "axios";

@Component({
    components: {
        WeuiSearch: require('../weui-search/search.vue')
    }
})
export default class MyClientDetailComponent extends ComponentBase {
    model: server.client;
    showcompany: boolean = false;
    companyName: string = "";
    labelBoatOrCar: string = "";
    showsales: boolean = false;
    oilshow: boolean = false;
    show1: boolean = false;
    svCompany1: string = "";
    svSales: string = "";
    companys: server.company[];
    sales: work.userlist[];
    oiloptions: ydui.actionSheetItem[];

    constructor() {
        super();

        this.model = (new Object()) as server.client;
        this.model.product = new Object() as server.product;
        this.model.company = new Object() as server.company;

        this.companys = new Array<server.company>();
        this.oiloptions = new Array();
        this.sales = new Array<work.userlist>();

        this.getOilProducts();
        this.getSales();
        this.getCompanys('');
    }
    
    mounted() {
        let id = this.$route.params.id;
        this.getClient(id, () => {
            //设置返回键的连接
            this.$emit('setTitle', this.model.carNo + ' 客户明细', '/client/client');
        });

        this.$watch('model.clientType', (v, ov) => {
            switch (parseInt(v)) {
                case server.clientType.个人:
                    this.companyName = "";
                    this.model.companyId = null;
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
        this.$watch("svCompany1", (v: string, ov) => {
            if (v.length >= 2)
                this.getCompanys(v);
        });
    }

    //提交保存客户
    saveclientclick() {
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
        $model.name = $model.clientType == server.clientType.公司 ? "个人" : this.companyName;
        console.log($model);
        this.putClient($model);
    }

    selectcompanyclick(company: server.company) {
        this.model.company = new Array<server.company>();
        this.model.company.name = company.name;
        this.model.companyId = company.id;
        this.showcompany = false;
    }

    selectsalesclick(sales: work.userlist) {
        this.model.followSalesman = sales.name;
        this.showsales = false;
    }

    //保存提交客户
    putClient(model: server.client) {
        let pModel = model;
        pModel.company = null;
        pModel.product = null;
        axios.put('/api/Client', pModel).then((res) => {
            let jobj = res.data as server.resultJSON<server.client>;
            if (jobj.code == 0) {
                this.toastSuccess('操作成功')
            }
            else
                this.toastError(jobj.msg);
        });
    }
    //获得当前客户
    getClient(id: string, callback: Function) {
        axios.get('/api/Client/GetDetail/' + id).then((res) => {
            let jobj = res.data as server.resultJSON<server.client>;
            if (jobj.code == 0) {
                this.model = jobj.data;
                callback();
            }
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
                            this.model.product.name = o.name;
                            this.model.defaultProductId = o.id;
                        }
                    });
                });
            }
        });
    }

    //获得销售员
    getSales() {
        axios.get('/api/User/Salesman').then((res) => {
            let jobj = res.data as work.tagMemberResult;
            if (jobj.errcode == 0)
                this.sales = jobj.userlist;
        });
    }
}