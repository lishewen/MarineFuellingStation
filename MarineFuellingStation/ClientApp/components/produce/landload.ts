import ComponentBase from "../../componentbase";
import { Component } from 'vue-property-decorator';
import axios from "axios";

@Component
export default class LandloadComponent extends ComponentBase {
    order: server.order;
    orders: server.order[];
    store: server.store;
    stores: server.store[];

    currStep: number = 0;
    showOrders: boolean = false;
    showStores: boolean = false;

    page: number;

    constructor() {
        super();

        this.order = new Object as server.order;
        this.orders = new Array<server.order>();
        this.store = new Object as server.store;
        this.stores = new Array<server.store>();

        this.getStores();
    }

    showOrdersclick() {
        this.showOrders = true;
        this.getOrders(1);
    }

    orderclick(o: server.order) {
        this.order = o;
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

    changeState(nextState: server.orderState) {
        if (this.currStep == 2) {
            if (this.order.emptyCarWeight == 0 || this.order.emptyCarWeight == null) {
                this.toastError("磅秤数据不能为空或0")
                return;
            }
            if (!this.order.emptyCarWeightPic) {
                this.toastError("请上传空车过磅数据图片");
                return;
            }
        }
        if (this.currStep == 3) {
            if (this.order.instrument1 <= 0) {
                this.toastError("请填写表数1");
                return;
            }
            if (this.order.instrument2 <= 0) {
                this.toastError("请填写表数2");
                return;
            }
            if (this.order.instrument3 <= 0) {
                this.toastError("请填写表数3");
                return;
            }
        }
        if (this.currStep == 4) {
            if (this.order.oilCarWeight == 0 || this.order.oilCarWeight == null) {
                this.toastError("磅秤数据不能为空或0")
                return;
            }
            if (!this.order.oilCarWeightPic) {
                this.toastError("请上传油车过磅数据图片");
                return;
            }
        }
        this.putState(nextState);
    }
   
    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 陆上装车');
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
        axios.post('/api/Order/UploadFile', param, config).then((res) => {
            let jobj = res.data as server.resultJSON<string>;
            if (jobj.code == 0) {
                this.toastSuccess('上传成功！');
                if (this.currStep == 2)
                    this.order.emptyCarWeightPic = jobj.data;
                if (this.currStep == 4)
                    this.order.oilCarWeightPic = jobj.data;
            }
            else
                this.toastError(jobj.msg);
        });
    }

    change(label: string, tabkey: string) {
        console.log(label);
        this.$emit('setTitle', this.$store.state.username + ' ' + label);
    }

    getOrders(toPage?: number) {
        if (this.page == null) this.page = 1;
        if (toPage != null) this.page = toPage;
        axios.get('/api/Order/GetByIsFinished/' + server.salesPlanType.陆上.toString()
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
        axios.put('/api/Order/ChangeState', this.order).then((res) => {
            let jobj = res.data as server.resultJSON<server.order>;
            if (jobj.code == 0) {
                this.order = jobj.data;
                this.currStep++;
            }
            else
                this.toastError(jobj.msg);
        });
    }
}