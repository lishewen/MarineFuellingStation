import ComponentBase from "../../componentbase";
import { Component } from 'vue-property-decorator';
import axios from "axios";
import wx from 'wx-sdk-ts';

@Component
export default class UnloadComponent extends ComponentBase {
    radio2: string = "1";
    carNo: string = "";
    showPurchases: boolean = false;
    showStores: boolean = false;
    showSelectWorker: boolean = true;
    currStep: number = 0;
    isScaleUpload: boolean = false;
    isScaleWithCarUpload: boolean = false;
    workers: work.userlist[];
    store: server.store;
    stores: server.store[];
    selectedStIds: Array<number>;
    notice: server.notice;

    purchases: server.purchase[];
    purchase: server.purchase;
    //lastPurchase: server.purchase;

    constructor() {
        super();

        this.purchases = new Array<server.purchase>();
        this.purchase = new Object as server.purchase;
        //this.lastPurchase = new Object as server.purchase;
        this.store = new Object as server.store;
        this.stores = new Array<server.store>();
        this.workers = new Array<work.userlist>();
        this.purchase.worker = "";
        this.purchase.toStoresList = new Array<server.toStore>();
        this.selectedStIds = new Array<number>();
        this.notice = new Object as server.notice;
        this.getPurchases();
        //this.getLastPurchase();
        this.getStores();
        this.getNotice();
        this.getWorkers();
    }

    workerSelectedClick() {
        this.showSelectWorker = false;
        this.$emit("setTitle", this.purchase.worker + ' 陆上卸油')
    }

    purchaseclick(pu: server.purchase) {
        this.purchase = pu;
        this.showPurchases = false;
        if (pu.state != server.unloadState.已开单 && pu.state != server.unloadState.已到达) {
            switch (pu.state) {
                case server.unloadState.油车过磅:
                    this.currStep = 1;
                    break;
                case server.unloadState.化验:
                    this.currStep = 2;
                    break;
                case server.unloadState.空车过磅:
                    this.currStep = 3;
                    break;
                case server.unloadState.卸油中:
                    this.currStep = 4;
                    break;
                case server.unloadState.完工:
                    this.currStep = 5;
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

    showStoresclick() {
        this.showStores = true;
        this.selectedStIds = new Array();
        if (this.purchase.toStoresList) {
            this.purchase.toStoresList.forEach((tst) => {
                this.selectedStIds.push(tst.id)
            });
        }
    }

    isFinish() {
        if (this.purchase.toStoresList && this.purchase.toStoresList.length > 0)
            return true;
        else
            return false;
    }

    storeOKclick() {
        console.log(this.selectedStIds);
        //if (this.selectedStIds.length > 3) { this.toastError("最多只可以选择三个卸油仓"); return; }

        this.purchase.toStoresList = new Array<server.toStore>();
        //console.log(this.selectedStIds);

        //把所选的油仓的Id和Name复制到toStoresList
        this.selectedStIds.forEach((sst, i) => {
            this.stores.forEach((st, j) => {
                if (sst == st.id) {
                    this.purchase.toStoresList.push({ id: st.id, name: st.name });
                }   
            });
        });
        
        this.showStores = false;
    }

    toStoresOKclick() {
        this.goNext();
    }
    
    strClass(sc: server.storeClass) {
        if (sc == server.storeClass.存储仓)
            return "存储仓"
        else
            return "销售仓"
    }

    goNext() {
        let nextState;
        let isValid = true;
        switch (this.currStep) {
            case 1:
                nextState = server.unloadState.化验;
                break;
            case 2:
                nextState = server.unloadState.空车过磅;

                if (this.purchase.scaleWithCar == 0 || !this.purchase.scaleWithCar) { this.toastError("磅秤数据不能为空或0"); return; };
                if (!this.purchase.scaleWithCarPic) { this.toastError("请上传油车过磅数据图片"); return; };
                if (this.purchase.density <= 0) { this.toastError('请输入测量密度'); return; };
                break;
            case 3:
                nextState = server.unloadState.卸油中;

                if (this.purchase.scale == 0 || !this.purchase.scale) { this.toastError("磅秤数据不能为空或0"); return; };
                if (!this.purchase.scalePic) { this.toastError("请上传空车过磅数据图片"); return; };
                if (this.purchase.scaleWithCar <= this.purchase.scale) { this.toastError("皮重应小于毛重"); return; };
                break;
            case 4:
                nextState = server.unloadState.完工;
                console.log(this.purchase.toStoresList);
                let total = 0;//统计总卸油数量
                this.purchase.toStoresList.forEach((tst, idx) => {
                    tst.count = tst.instrumentAf - tst.instrumentBf;
                    if (tst.count <= 0 || !tst.count) {
                        this.toastError("表数输入有误！");
                        isValid = false;
                    }
                    else 
                        total += tst.count;
                });
                this.purchase.oilCount = total;
                break;
        }
        if (isValid) this.putState(nextState);
    }

    mounted() {
        this.$emit('setTitle', '陆上卸油');
    };
    //uploadfile(e) {
    //    let file = e.target.files[0];
    //    if (!file || file.name == '') { this.toastError("请选择文件！"); return; }
    //    let param = new FormData(); //创建form对象
    //    param.append('file', file, file.name);//通过append向form对象添加数据

    //    let config = {
    //        headers: {
    //            'Content-Type': 'multipart/form-data',
    //            'x-username': encodeURIComponent(this.$store.state.username),
    //            'x-userid': encodeURIComponent(this.$store.state.userid)
    //        }
    //    };  //添加请求头
    //    this.$dialog.loading.open("正在上传图片");
    //    axios.post('/api/Purchase/UploadFile', param, config).then((res) => {
    //        let jobj = res.data as server.resultJSON<string>;
    //        this.$dialog.loading.close();
    //        if (jobj.code == 0) {
    //            this.toastSuccess('上传成功！');
    //            if (this.currStep == 1) {
    //                this.purchase.scaleWithCarPic = jobj.data;
    //                this.isScaleWithCarUpload = true;

    //            }
    //            if (this.currStep == 3) {
    //                this.purchase.scalePic = jobj.data;
    //                this.isScaleUpload = true;
    //            }
    //        }
    //        else {
    //            this.toastError("无法上传图片，请重试")
    //        }
    //    });
    //}
    uploadByWeixin() {
        let that = this;
        this.$wechat = wx;
        this.SDKRegister(this, () => {
            this.$wechat.chooseImage({
                count: 1, // 默认9
                sizeType: ['compressed'], // 可以指定是原图还是压缩图，默认二者都有
                sourceType: ['camera'], // 可以指定来源是相册还是相机，默认二者都有
                success: function (res) {
                    let localIds = res.localIds; // 返回选定照片的本地ID列表，localId可以作为img标签的src属性显示图片
                    that.$wechat.uploadImage({
                        localId: localIds[0], // 需要上传的图片的本地ID，由chooseImage接口获得
                        isShowProgressTips: 1,// 默认为1，显示进度提示
                        success: res => {
                            var serverId = res.serverId; // 返回图片的服务器端ID
                            console.log(serverId);
                            that.getUploadFile(serverId);
                        }
                    });
                }
            });
        });
    }
    change(label: string, tabkey: string) {
        console.log(label);
        
    }

    getPurchases() {
        axios.get('/api/Purchase/GetReadyUnload').then((res) => {
            let jobj = res.data as server.resultJSON<server.purchase[]>;
            if (jobj.code == 0) {
                this.purchases = jobj.data;
            }
        });
    }
    //getLastPurchase() {
    //    axios.get('/api/Purchase/LastPurchase').then((res) => {
    //        let jobj = res.data as server.resultJSON<server.purchase>;
    //        if (jobj.code == 0) {
    //            this.lastPurchase = jobj.data;
    //        }
    //    });
    //}

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
    //获取生产员
    getWorkers() {
        axios.get('/api/User/Worker').then((res) => {
            let jobj = res.data as work.tagMemberResult;
            if (jobj.errcode == 0) {
                this.workers = jobj.userlist;
            }
        });
    }

    getUploadFile(id: string) {
        axios.get('/api/Purchase/GetUploadFile?fileId=' + id).then((res) => {
            let jobj = res.data as server.resultJSON<string>;
            if (jobj.code == 0) {
                if (this.currStep == 1) {
                    this.purchase.scaleWithCarPic = jobj.data;
                    this.isScaleWithCarUpload = true;
                }
                if (this.currStep == 3) {
                    this.purchase.scalePic = jobj.data;
                    this.isScaleUpload = true;
                }
            }
            else
                this.toastError(jobj.msg)
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