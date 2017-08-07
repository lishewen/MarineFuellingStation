import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import axios from "axios";

@Component
export default class OrderDetailComponent extends Vue {
    model: server.order;

    constructor() {
        super();

        this.model = (new Object()) as server.order;
    }

    mounted() {
        let id = this.$route.params.id;
        this.getOrder(id, () => {
            //设置返回键的连接
            this.$emit('setTitle', this.model.name + ' 销售单明细', '/sales/order');
        });
    }

    getOrder(id: string, callback: Function) {
        axios.get('/api/Order/' + id).then((res) => {
            let jobj = res.data as server.resultJSON<server.order>;
            if (jobj.code == 0) {
                this.model = jobj.data;
                callback();
            }
        });
    }
}