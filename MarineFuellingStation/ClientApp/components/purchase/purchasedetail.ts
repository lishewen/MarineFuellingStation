import ComponentBase from "../../componentbase";
import { Component } from 'vue-property-decorator';
import axios from "axios";
import moment from "moment";

@Component
export default class PurchaseDetailComponent extends ComponentBase {
    model: server.purchase;

    constructor() {
        super();

        this.model = (new Object()) as server.purchase;
    }

    formatDate(d: Date) {
        return moment(d).format("YYYY-MM-DD");
    }

    mounted() {
        let id = this.$route.params.id;
        let from = this.$route.params.from;
        if (from == 'purchase')
            from = '/purchase/purchase'
        else if (from == 'board')
            from = '/produce/buyboard'
        this.getPurchase(id, from, () => {
            //设置返回键的连接
            this.$emit('setTitle', this.model.name + ' 进油单明细', from);
        });
    }
    
    getPurchase(id: string, from: string, callback: Function) {
        axios.get('/api/Purchase/GetDetail/' + id).then((res) => {
            let jobj = res.data as server.resultJSON<server.purchase>;
            if (jobj.code == 0) {
                this.model = jobj.data;
                //应客户要求不让生产部看到进油单价
                if (from == '/produce/buyboard')
                    this.model.price = 0;
                callback();
            }
        });
    }
}