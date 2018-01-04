import ComponentBase from "../../componentbase";
import { Component } from 'vue-property-decorator';
import axios from "axios";

@Component
export default class ApplyToMyClientComponent extends ComponentBase {
    model: server.client;
    followSalesman: string;
    isLeader: boolean = false;

    constructor() {
        super();

        this.model = (new Object()) as server.client;
        this.model.product = new Object() as server.product;
        this.model.company = new Object() as server.company;
    }

    mounted() {
        let id = this.$route.params.id;
        let applier = this.$route.params.applier;
        this.followSalesman = applier;
        this.isLeader = this.$store.state.isLeader;
        this.getClient(id, () => {
            //设置返回键的连接
            this.$emit('setTitle', applier + ' 申请成为客户', '/sales/myclient');
        });
    }

    getClient(id: string, callback: Function) {
        axios.get('/api/Client/GetDetail/' + id).then((res) => {
            let jobj = res.data as server.resultJSON<server.client>;
            if (jobj.code == 0) {
                this.model = jobj.data;
                callback();
            }
        });
    }
    //批准申请
    authApplyclick() {
        this.model.followSalesman = this.followSalesman;
        axios.put('api/Client', this.model).then((res) => {
            let jobj = res.data as server.resultJSON<server.client>;
            if (jobj.code == 0)
                this.toastSuccess("操作成功")
            else {
                this.model.followSalesman = "";
            }
        });
    }
}