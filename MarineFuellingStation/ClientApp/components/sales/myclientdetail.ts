import ComponentBase from "../../componentbase";
import { Component } from 'vue-property-decorator';
import axios from "axios";

@Component
export default class MyClientDetailComponent extends ComponentBase {
    model: server.client;

    constructor() {
        super();

        this.model = (new Object()) as server.client;
        this.model.product = new Object() as server.product;
        this.model.company = new Object() as server.company;
    }

    mounted() {
        let id = this.$route.params.id;
        this.getClient(id, () => {
            //设置返回键的连接
            this.$emit('setTitle', this.model.carNo + '明细', '/sales/myclient');
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
}