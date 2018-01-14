import ComponentBase from "../../componentbase";
import { Component } from 'vue-property-decorator';
import axios from "axios";
import wx from 'wx-sdk-ts';

@Component
export default class LandloadComponent extends ComponentBase {
    order: server.order;
    orders: server.order[];
    store: server.store;
    stores: server.store[];
    lastorder: server.order;
    workers: work.userlist[];

    worker: string = "";
    currStep: number = 0;
    showOrders: boolean = false;
    showStores: boolean = false;

    showSelectWorker: boolean = true;

    page: number;

    constructor() {
        super();

        this.order = new Object as server.order;
        this.lastorder = new Object as server.order;
        this.orders = new Array<server.order>();
        this.store = new Object as server.store;
        this.stores = new Array<server.store>();
        this.workers = new Array<work.userlist>();

        this.order.worker = "";

        this.getStores();
        this.getLastOrder();
        this.getWorkers();
    }

    workerSelectedClick() {
        this.showSelectWorker = false;
        this.$emit("setTitle", this.worker + ' 陆上装车')
    }

    showOrdersclick() {
        this.showOrders = true;
        this.getOrders(1);
    }

    orderclick(o: server.order) {
        this.order = o;
        //重新选择生产员
        if (this.order.worker != this.worker)
            this.showSelectWorker = true;
        this.showOrders = false;
        this.matchCurrStep();

    }
    matchCurrStep() {
        if (this.order.state == server.orderState.已开单) this.currStep = 1;
        if (this.order.state == server.orderState.空车过磅) this.currStep = 2;
        if (this.order.state == server.orderState.装油中) this.currStep = 3;
        if (this.order.state == server.orderState.油车过磅) this.currStep = 4;
    }

    storeclick(st: server.store) {
        this.store = st;
        this.order.storeId = st.id;
        this.order.store = st;
        this.showStores = false;
        this.changeState(server.orderState.空车过磅);
        console.log(st.id);
    }

    //打印到地磅室
    printToDBclick() {
        this.getPrintLandload(this.order.id, "地磅室");
    }

    changeState(nextState: server.orderState) {
        console.log(this.currStep);
        switch (this.currStep) {
            case 2:
                if (this.order.emptyCarWeight == 0 || this.order.emptyCarWeight == null) { this.toastError("磅秤数据不能为空或0"); return;}
                if (!this.order.emptyCarWeightPic) { this.toastError("请上传空车过磅数据图片"); return;}
                break;
            case 3:
                if (this.lastorder.instrument1 <= 0) { this.toastError("请填写加油前表数"); return;}
                if (this.order.instrument1 <= 0) { this.toastError("请填写加油后表数1"); return;}
                if (this.order.oilCountLitre <= 0) { this.toastError("加油后表数应大于加油前表数"); return;}
                break;
            case 4:
                if (this.order.oilCarWeight == 0 || this.order.oilCarWeight == null) { this.toastError("磅秤数不能为空或0"); return;}
                if (!this.order.oilCarWeightPic) { this.toastError("请上传油车过磅数据图片"); return;}
                break;
            case 5:
                if (this.order.oilCarWeight <= this.order.emptyCarWeight) { this.toastError("皮重应小于毛重"); return;}
                break;
        }
        this.putState(nextState);
    }
   
    mounted() {
        this.$emit('setTitle', this.order.worker + ' 陆上装车');
        this.$watch("lastorder.instrument1", (v, ov) => {
            this.order.oilCountLitre = this.order.instrument1 - v;
        });
        this.$watch("order.instrument1", (v, ov) => {
            this.order.oilCountLitre = v - this.lastorder.instrument1;
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
        axios.post('/api/Order/UploadFile', param, config).then((res) => {
            let jobj = res.data as server.resultJSON<string>;
            this.$dialog.loading.close();
            if (jobj.code == 0) {
                this.toastSuccess('上传成功！');
                if (this.currStep == 2)
                    this.order.emptyCarWeightPic = jobj.data;
                if (this.currStep == 4)
                    this.order.oilCarWeightPic = jobj.data;
            }
            else
                this.toastError("无法上传图片，请重试")
        });
    }
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

    getOrders(toPage?: number) {
        if (this.page == null) this.page = 1;
        if (toPage != null) this.page = toPage;
        axios.get('/api/Order/GetByIsFinished/' + server.salesPlanType.陆上装车.toString()
            + '?page=' + this.page.toString()
            + '&isFinished=false')
            .then((res) => {
                let jobj = res.data as server.resultJSON<server.order[]>;
                if (jobj.code == 0 && jobj.data.length > 0) {
                    this.orders = jobj.data;
                    this.page++;
                }
                else {
                    this.toastError("没有相关数据");
                    this.showOrders = false;
                }
            });
    }

    getStores() {
        axios.get('/api/Store/GetByClass?sc=' + server.storeClass.销售仓.toString()).then((res) => {
            let jobj = res.data as server.resultJSON<server.store[]>;
            if (jobj.code == 0)
                this.stores = jobj.data;
        });
    }
    
    putState(state: server.orderState) {
        this.order.state = state;
        this.order.worker = this.worker;
        console.log(this.order);
        //axios.put('/api/Order/ChangeState', this.order).then((res) => {
        //    let jobj = res.data as server.resultJSON<server.order>;
        //    if (jobj.code == 0) {
        //        this.order = jobj.data;
        //        this.currStep++;
        //    }
        //    else
        //        this.toastError(jobj.msg);
        //});
    }

    getLastOrder() {
        axios.get('/api/Order/GetLastOrder/' + server.salesPlanType.陆上装车).then((res) => {
            let jobj = res.data as server.resultJSON<server.order>;
            if (jobj.code == 0) {
                this.lastorder = jobj.data;
                console.log(this.lastorder)
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
                if (this.currStep == 2)
                    this.order.emptyCarWeightPic = jobj.data;
                if (this.currStep == 4)
                    this.order.oilCarWeightPic = jobj.data;
            }
            else
                this.toastError(jobj.msg)
        });
    }

    putRestart() {
        axios.put('/api/Order/Restart', this.order).then((res) => {
            let jobj = res.data as server.resultJSON<server.order>;
            if (jobj.code == 0) {
                this.toastSuccess('操作成功');
                this.currStep = 1;
            }
        });
    }
}