import ComponentBase from "../../ComponentBase";
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

    mounted() {
        let id = this.$route.params.id;
        let from = this.$route.params.from;
        if (from == 'purchase')
            from = '/purchase/purchase'
        else if (from == 'board')
            from = '/produce/buyboard'
        else if (from == 'auditing')
            from = '/produce/unloadaudit'
        this.getPurchase(id, from, () => {
            //设置返回键的连接
            this.$emit('setTitle', this.model.name + ' 进油单明细', from);
        });
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
                return "卸油完工";
        }
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