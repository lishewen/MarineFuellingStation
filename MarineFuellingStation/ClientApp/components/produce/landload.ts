import ComponentBase from "../../componentbase";
import { Component } from 'vue-property-decorator';
import axios from "axios";

@Component
export default class LandloadComponent extends ComponentBase {
    order: server.order;
    orders: server.order[];

    currStep: number = 0;
    showOrders: boolean = false;

    page: number;

    constructor() {
        super();

        this.order = new Object as server.order;
        this.orders = new Array<server.order>();
        
        this.getOrders();
    }

    orderclick(o: server.order) {
        console.log(o);
        this.order = o;
        this.showOrders = false;
        if (this.order.state == server.orderState.已开单)
            this.currStep = 1;
        else
            this.currStep = this.order.state;
    }

    changeState(nextState: server.orderState) {
        if (this.currStep == server.orderState.空车过磅) {
            if (this.order.emptyCarWeight == 0 || this.order.emptyCarWeight == null) {
                this.toastError("磅秤数据不能为空或0")
                return;
            }
            if (!this.order.emptyCarWeightPic) {
                this.toastError("请上传空车过磅数据图片");
                return;
            }
        }
        if (this.currStep == server.orderState.油车过磅) {
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
        this.$emit('setTitle', this.$store.state.username + ' 陆上装油');
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
                if (this.currStep == server.orderState.空车过磅)
                    this.order.emptyCarWeightPic = jobj.data;
                if (this.currStep == server.orderState.油车过磅)
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

    getOrders() {
        if (this.page == null) this.page = 1;
        axios.get('/api/Order/GetIncludeProduct/' + server.salesPlanType.陆上.toString() + '?page=' + this.page.toString())
            .then((res) => {
            let jobj = res.data as server.resultJSON<server.order[]>;
            if (jobj.code == 0){
                this.orders = jobj.data;
                this.page++;
            }
        });
    }

    putState(state: server.orderState) {
        this.order.state = state;
        axios.put('/api/Order/ChangeState', this.order).then((res) => {
            let jobj = res.data as server.resultJSON<server.order>;
            if (jobj.code == 0) {
                this.order = jobj.data;
                this.currStep = this.order.state;
                if (this.currStep == server.orderState.完工)
                    this.getOrders();
            }
            else
                this.toastError(jobj.msg);
        });
    }
}