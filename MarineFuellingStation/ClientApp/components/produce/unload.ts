import ComponentBase from "../../componentbase";
import { Component } from 'vue-property-decorator';
import axios from "axios";

@Component
export default class UnloadComponent extends ComponentBase {
    radio2: string = "1";
    carNo: string = "";
    showPurchases: boolean = false;
    currStep: number = -1;

    purchases: server.purchase[];
    purchase: server.purchase;

    constructor() {
        super();

        this.purchases = new Array<server.purchase>();
        this.purchase = new Object as server.purchase;
        this.getPurchases();
    }

    purchaseclick(pu: server.purchase) {
        console.log(pu);
        this.purchase = pu;
        this.showPurchases = false;
        this.currStep = this.purchase.state;
    }

    changeState(nextState: server.unloadState) {
        if (this.currStep == server.unloadState.油车过磅) {
            if (this.purchase.scaleWithCar == 0 || !this.purchase.scaleWithCar) {
                this.toastError("磅秤数据不能为空或0")
                return;
            }
            if (!this.purchase.scaleWithCarPic) {
                this.toastError("请上传油车过磅数据图片");
                return;
            }
        }
        if (this.currStep == server.unloadState.空车过磅) {
            if (this.purchase.scale == 0 || !this.purchase.scale) {
                this.toastError("磅秤数据不能为空或0")
                return;
            }
            if (!this.purchase.scalePic) {
                this.toastError("请上传空车过磅数据图片");
                return;
            }
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
                if (this.currStep == server.unloadState.空车过磅)
                    this.purchase.scalePic = jobj.data;
                if (this.currStep == server.unloadState.油车过磅)
                    this.purchase.scaleWithCarPic = jobj.data;
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
            if (jobj.code == 0)
                this.purchases = jobj.data;
        });
    }

    putState(state: server.unloadState) {
        this.purchase.state = state;
        axios.put('/api/Purchase/ChangeState', this.purchase).then((res) => {
            let jobj = res.data as server.resultJSON<server.purchase>;
            if (jobj.code == 0) {
                this.purchase = jobj.data;
                this.currStep = this.purchase.state;
                if (this.currStep == server.unloadState.完工)
                    this.getPurchases();
            }
            else
                this.toastError(jobj.msg);
        });
    }
}