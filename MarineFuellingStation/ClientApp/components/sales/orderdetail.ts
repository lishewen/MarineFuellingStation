import ComponentBase from "../../componentbase";
import { Component } from 'vue-property-decorator';
import axios from "axios";

@Component
export default class OrderDetailComponent extends ComponentBase {
    model: server.order;

    constructor() {
        super();

        this.model = (new Object()) as server.order;
    }

    mounted() {
        let id = this.$route.params.id;
        let from = this.$route.params.from;
        switch (from) {
            case "order":
                from = '/sales/order';
                break;
            case "myorder":
                from = '/sales/myorder';
                break;
        }
        this.getOrder(id, () => {
            //设置返回键的连接
            this.$emit('setTitle', this.model.name + ' 销售单明细', from);
        });
    }

    getOrderType(ot: server.salesPlanType) {
        if (ot == server.salesPlanType.机油)
            return "机油"
        else if (ot == server.salesPlanType.水上)
            return "水上"
        else if (ot == server.salesPlanType.陆上)
            return "陆上"
    }

    getIsInvoice(isInv: boolean) {
        if (isInv)
            return "开票";
        else
            return "不开票";
    }

    getTicketType(tt: server.ticketType) {
        switch (tt) {
            //case server.ticketType.普通票:
            //    return "普通票";
            //case server.ticketType.专用票:
            //    return "专用票";
            case server.ticketType.循票:
                return "循票";
            case server.ticketType.柴票:
                return "柴票";
        }
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