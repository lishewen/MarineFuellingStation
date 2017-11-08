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
    isPrevent2: boolean = true;//控制选择油仓下一步的标识
    isScaleUpload: boolean = false;
    isScaleWithCarUpload: boolean = false;
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

    diff1: number = 0;//表数差值1
    diff2: number = 0;//表数差值2
    diff3: number = 0;//表数差值3

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
        this.notice = new Object as server.notice;
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
                    break;
                case server.unloadState.化验:
                    this.currStep = 2;
                    break;
                case server.unloadState.卸油中:
                    this.currStep = 3;
                    break;
                case server.unloadState.空车过磅:
                    this.currStep = 5;
                    break;
                case server.unloadState.完工:
                    this.currStep = 6;
                    break;
            }
        }
        else
            this.currStep = 1;

        if (this.purchase.scalePic) this.isScaleUpload = true;
        if (this.purchase.scaleWithCarPic) this.isScaleWithCarUpload = true;
        console.log(this.purchase);
    }

    strState(sta: server.unloadState) {
        switch (sta) {
            case server.unloadState.已开单:
                return "已开单";
            case server.unloadState.已到达:
                return "已到达";
            case server.unloadState.选择油仓:
                return "选择油仓";
            case server.unloadState.油车过磅:
                return "油车过磅";
            case server.unloadState.化验:
                return "化验";
            case server.unloadState.卸油中:
                return "卸油中";
            case server.unloadState.空车过磅:
                return "空车过磅";
            case server.unloadState.完工:
                return "完工";
        }
    }

    storeclick(st: server.store) {
        this.store = st;
        this.showStores = false;
        this.goNext();
        console.log(this.store);
    }

    storeOKclick() {
        if (this.selectedStIds.length > 3) { this.toastError("最多只可以选择三个卸油仓"); return; }

        this.toStores = new Array<server.toStore>();
        //console.log(this.selectedStIds);

        //把所选的油仓的Id和Name复制到toStores
        this.selectedStIds.forEach((sst, i) => {
            this.stores.forEach((st, j) => {
                if (sst == st.id) {
                    this.toStores.push({ id: st.id, name: st.name });
                    //初始化下拉列表的值
                    this.instruments[j] = "";
                }   
            });
        });
        //不初始化的话会出现bug
        this.toStoreCounts = new Array<number>(this.toStores.length);

        //释放下一步提交按钮
        this.isPrevent2 = false;
        this.showStores = false;
    }

    toStoresOKclick() {
        //判断选择的油表是否相同
        console.log(this.instruments);
        if (this.instruments.length == 1) { if (this.instruments[0] == "") { this.toastError("请选择使用的油表"); return; } };
        if (this.instruments.length == 2) {
            if (this.instruments[0] == "" || this.instruments[1] == "") { this.toastError("请选择使用的油表"); return; };
            if (this.instruments[0] == this.instruments[1]) { this.toastError("两个油仓不能同时使用一个油表"); return; };
        }
        if (this.instruments.length == 3) {
            if (this.instruments[0] == "" || this.instruments[1] == "" || this.instruments[2] == "") { this.toastError("请选择使用的油表"); return; };
            if (this.instruments[0] == this.instruments[1] || this.instruments[0] == this.instruments[2] || this.instruments[1] == this.instruments[2]) {
                this.toastError("油仓不能同时使用同一个油表");
                return;
            };
        }

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
                this.purchase.toStoresList = new Array<server.toStore>();
                let total = 0;//统计总卸油数量
                this.toStores.forEach((tst, idx) => {
                    switch (this.instruments[idx]) {
                        case "表1":
                            if (this.purchase.instrument1 <= 0) { this.toastError("请填写表数1"); isValid = false; return; };
                            if (this.purchase.instrument1 < this.lastPurchase.instrument1) { this.toastError("卸油后表数1应大于或等于卸油前表数1"); isValid = false; return; };
                            tst.count = this.purchase.instrument1 - this.lastPurchase.instrument1;
                            break;
                        case "表2":
                            if (this.purchase.instrument2 <= 0) { this.toastError("请填写表数2"); isValid = false; };
                            if (this.purchase.instrument2 < this.lastPurchase.instrument2) { this.toastError("卸油后表数2应大于或等于卸油前表数2"); isValid = false; return; };
                            tst.count = this.purchase.instrument2 - this.lastPurchase.instrument2;
                            break;
                        case "表3":
                            if (this.purchase.instrument3 <= 0) { this.toastError("请填写表数3"); isValid = false; };
                            if (this.purchase.instrument3 < this.lastPurchase.instrument3) { this.toastError("卸油后表数3应大于或等于卸油前表数3"); isValid = false; return; };
                            tst.count = this.purchase.instrument3 - this.lastPurchase.instrument3;
                            break;
                    }
                    total += tst.count;
                    this.purchase.toStoresList.push(tst);
                });
                if (!isValid) return;
                this.purchase.oilCount = total;
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
        this.$watch('purchase.instrument1', (v, ov) => {
            this.diff1 = v - this.lastPurchase.instrument1;
        });
        this.$watch('purchase.instrument2', (v, ov) => {
            this.diff2 = v - this.lastPurchase.instrument2;
        });
        this.$watch('purchase.instrument3', (v, ov) => {
            this.diff3 = v - this.lastPurchase.instrument3;
        });
        this.$watch('lastPurchase.instrument1', (v, ov) => {
            this.diff1 = this.purchase.instrument1 - v;
        });
        this.$watch('lastPurchase.instrument2', (v, ov) => {
            this.diff2 = this.purchase.instrument2 - v;
        });
        this.$watch('lastPurchase.instrument3', (v, ov) => {
            this.diff3 = this.purchase.instrument3 - v;
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
        this.$dialog.loading.open("正在上传图片");
        axios.post('/api/Purchase/UploadFile', param, config).then((res) => {
            let jobj = res.data as server.resultJSON<string>;
            this.$dialog.loading.close();
            if (jobj.code == 0) {
                this.toastSuccess('上传成功！');
                if (this.currStep == 1) {
                    this.purchase.scaleWithCarPic = jobj.data;
                    this.isScaleWithCarUpload = true;

                }
                if (this.currStep == 5) {
                    this.purchase.scalePic = jobj.data;
                    this.isScaleUpload = true;
                }
            }
            else {
                this.toastError("无法上传图片，请重试")
            }
        });
    }
    change(label: string, tabkey: string) {
        console.log(label);
        this.$emit('setTitle', this.$store.state.username + ' ' + label);
    }

    getPurchases() {
        axios.get('/api/Purchase/GetReadyUnload').then((res) => {
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

    putRestart() {
        let pid = this.purchase.id;
        axios.put('/api/Purchase/UnloadRestart?pid=' + pid, null).then((res) => {
            let jobj = res.data as server.resultJSON<server.purchase>;
            if (jobj.code == 0) {
                this.toastSuccess('操作成功');
                this.currStep = 1;
            }
        });
    }
}