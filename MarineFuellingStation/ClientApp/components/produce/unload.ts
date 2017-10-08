import ComponentBase from "../../componentbase";
import { Component } from 'vue-property-decorator';
import axios from "axios";

@Component
export default class UnloadComponent extends ComponentBase {
    radio2: string = "1";
    carNo: string = "";
    showPurchases: boolean = false;
    showStores: boolean = false;
    currStep: number = 0;
    isPrevent: boolean = true;//控制上传后提交的标识
    isPrevent1: boolean = true;
    store: server.store;
    stores: server.store[];
    toStores: server.toStore[];
    selectedStIds: Array<number>;
    notice: server.notice;
    toStoreCounts: Array<number>;
    instruments: Array<string>;

    purchases: server.purchase[];
    purchase: server.purchase;
    lastPurchase: server.purchase;

    constructor() {
        super();

        this.purchases = new Array<server.purchase>();
        this.purchase = new Object as server.purchase;
        this.lastPurchase = new Object as server.purchase;
        this.store = new Object as server.store;
        this.stores = new Array<server.store>();
        this.toStores = new Array<server.toStore>();
        this.toStoreCounts = new Array<number>();
        this.selectedStIds = new Array<number>();
        this.instruments = new Array<string>();
        this.notice = new Array<server.notice>();
        this.getPurchases();
        this.getLastPurchase();
        this.getStores();
        this.getNotice();
    }

    purchaseclick(pu: server.purchase) {
        this.purchase = pu;
        this.showPurchases = false;
        if (pu.state != server.unloadState.已开单 && pu.state != server.unloadState.已到达) {
            switch (pu.state) {
                case server.unloadState.选择油仓:
                    this.currStep = 3;
                    break;
                case server.unloadState.油车过磅:
                    this.currStep = 1;
                    if (pu.scaleWithCarPic == "") this.isPrevent = true;
                    break;
                case server.unloadState.化验:
                    this.currStep = 2;
                    break;
                case server.unloadState.卸油中:
                    this.currStep = 4;
                    break;
                case server.unloadState.空车过磅:
                    this.currStep = 5;
                    if (pu.scalePic == "") this.isPrevent1 = true;
                    break;
                case server.unloadState.完工:
                    this.currStep = 6;
                    break;
            }
        }
        else
            this.currStep = 1;
    }

    storeclick(st: server.store) {
        this.store = st;
        this.purchase.store = st;
        this.showStores = false;
        this.goNext();
        console.log(this.store);
    }

    storeOKclick() {
        this.toStores = new Array<number>();
        //console.log(this.selectedStIds);

        //把所选的油仓的Id和Name复制到toStores
        this.selectedStIds.forEach((sst, i) => {
            this.stores.forEach((st, j) => {
                if (sst == st.id) 
                    this.toStores.push({ id: st.id, name: st.name });
            });
        });
        //不初始化的话会出现bug
        this.toStoreCounts = new Array<number>(this.toStores.length);

        this.showStores = false;
    }
    
    toStoresOKclick() {
        this.goNext();
    }

    isHas(name: string) {
        return this.instruments.indexOf(name) > -1
    }

    strClass(sc: server.storeClass) {
        if (sc == server.storeClass.存储仓) 
            return "存储仓"
        else
            return "销售仓"
    }

    goNext() {
        let nextState;
        switch (this.currStep) {
            case 1:
                nextState = server.unloadState.化验;
                break;
            case 2:
                nextState = server.unloadState.选择油仓;

                if (this.purchase.scaleWithCar == 0 || !this.purchase.scaleWithCar) { this.toastError("磅秤数据不能为空或0"); return; };
                if (!this.purchase.scaleWithCarPic) { this.toastError("请上传油车过磅数据图片"); return; };
                if (this.purchase.density <= 0) { this.toastError('请输入测量密度'); return; };
                break;
            case 3:
                nextState = server.unloadState.卸油中;
                break;
            case 4:
                nextState = server.unloadState.空车过磅;

                let isValid = true;
                this.purchase.toStores = new Array<server.toStore>();
                this.toStores.forEach((tst, idx) => {
                    switch (this.instruments[idx]) {
                        case "表1":
                            if (this.purchase.instrument1 <= 0) { this.toastError("请填写表数1"); isValid = false; return;};
                            if (this.purchase.instrument1 < this.lastPurchase.instrument1) { this.toastError("卸油后表数1应大于或等于卸油前表数1"); isValid = false; return;};
                            tst.count = this.purchase.instrument1 - this.lastPurchase.instrument1;
                            break;
                        case "表2":
                            if (this.purchase.instrument2 <= 0) { this.toastError("请填写表数2"); isValid = false; };
                            if (this.purchase.instrument2 < this.lastPurchase.instrument2) { this.toastError("卸油后表数2应大于或等于卸油前表数2"); isValid = false; return;};
                            tst.count = this.purchase.instrument2 - this.lastPurchase.instrument2;
                            break;
                        case "表3":
                            if (this.purchase.instrument3 <= 0) { this.toastError("请填写表数3"); isValid = false; };
                            if (this.purchase.instrument3 < this.lastPurchase.instrument3) { this.toastError("卸油后表数3应大于或等于卸油前表数3"); isValid = false; return;};
                            tst.count = this.purchase.instrument3 - this.lastPurchase.instrument3;
                            break;
                    }
                    this.purchase.toStores.push(tst);
                });
                if (!isValid) return;
                break;
            case 5:
                nextState = server.unloadState.完工;

                if (this.purchase.scale == 0 || !this.purchase.scale) { this.toastError("磅秤数据不能为空或0"); return; };
                if (!this.purchase.scalePic) { this.toastError("请上传空车过磅数据图片"); return; };
                break;
        }
        this.putState(nextState);
    }

    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 陆上卸油');
        this.$watch('show1', (v, ov) => {
        });
    };
    uploadfile(e) {
        let file = e.target.files[0];
        let param = new FormData(); //创建form对象
        param.append('file', file, file.name);//通过append向form对象添加数据

        let config = {
            headers: {
                'Content-Type': 'multipart/form-data',
                'x-username': encodeURIComponent(this.$store.state.username),
                'x-userid': encodeURIComponent(this.$store.state.userid)
            }
        };  //添加请求头
        axios.post('/api/Purchase/UploadFile', param, config).then((res) => {
            let jobj = res.data as server.resultJSON<string>;
            if (jobj.code == 0) {
                this.toastSuccess('上传成功！');
                if (this.currStep == 1) {
                    this.purchase.scaleWithCarPic = jobj.data;
                    this.isPrevent = false;
                }
                if (this.currStep == 5) {
                    this.purchase.scalePic = jobj.data;
                    this.isPrevent1 = false;
                }
            }
            else
                this.toastError(jobj.msg);
        });
    }
    change(label: string, tabkey: string) {
        console.log(label);
        this.$emit('setTitle', this.$store.state.username + ' ' + label);
    }

    getPurchases() {
        axios.get('/api/Purchase').then((res) => {
            let jobj = res.data as server.resultJSON<server.purchase[]>;
            if (jobj.code == 0) {
                this.purchases = jobj.data;
            }
        });
    }
    getLastPurchase() {
        axios.get('/api/Purchase/LastPurchase').then((res) => {
            let jobj = res.data as server.resultJSON<server.purchase>;
            if (jobj.code == 0) {
                this.lastPurchase = jobj.data;
            }
        });
    }

    getStores() {
        axios.get('/api/Store/GetByClass?sc=' + server.storeClass.全部.toString()).then((res) => {
            let jobj = res.data as server.resultJSON<server.store[]>;
            if (jobj.code == 0)
                this.stores = jobj.data;
        });
    }

    getNotice() {
        //获取该应用的通知信息
        axios.get('/api/Notice/GetByAppName?app=陆上卸油').then((res) => {
            let jobj = res.data as server.resultJSON<server.notice>;
            if (jobj.code == 0) {
                this.notice = jobj.data;
                if (jobj.data)
                    this.$dialog.alert({ mes: jobj.data.content });
            }
        });
    }

    putState(state: server.unloadState) {
        this.purchase.state = state;
        axios.put('/api/Purchase/ChangeState', this.purchase).then((res) => {
            let jobj = res.data as server.resultJSON<server.purchase>;
            if (jobj.code == 0) {
                this.purchase = jobj.data;
                this.currStep += 1;//下一流程
                if (this.currStep == 6)
                    this.getPurchases();
            }
            else
                this.toastError(jobj.msg);
        });
    }
}