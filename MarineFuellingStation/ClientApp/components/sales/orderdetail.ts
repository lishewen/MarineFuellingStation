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
            return "代号1";
        else
            return "代号2";
    }

    getTicketType(tt: server.ticketType) {
        switch (tt) {
            //case server.ticketType.普通票:
            //    return "普通票";
            //case server.ticketType.专用票:
            //    return "专用票";
            case server.ticketType.循票:
                return "循";
            case server.ticketType.柴票:
                return "柴";
        }
    }

    strPayType(p: server.payment) {
        switch (p.payTypeId) {
            case server.orderPayType.现金:
                return "现金";
            case server.orderPayType.微信:
                return "微信";
            case server.orderPayType.支付宝:
                return "支付宝";
            case server.orderPayType.桂行刷卡:
                return "桂行刷卡";
            case server.orderPayType.账户扣减:
                return "账户扣减";
            case server.orderPayType.工行刷卡:
                return "工行刷卡";
            case server.orderPayType.公司账户扣减:
                return "公司账户扣减";
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