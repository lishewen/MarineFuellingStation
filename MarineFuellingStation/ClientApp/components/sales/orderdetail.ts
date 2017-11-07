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

    strOrderState(o: server.order) {
        let str = "当前施工状态：";
        switch (o.state) {
            case server.orderState.已开单:
                return str + "待施工";
            case server.orderState.选择油仓:
                return str + "待施工";
            case server.orderState.空车过磅:
                return str + "待施工";
            case server.orderState.装油中:
                return str + "待施工";
            case server.orderState.油车过磅:
                return str + "待施工";
            case server.orderState.已完成:
                return str + "已完成";
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

    totalMoneyClass() {
        if (this.model.salesPlan != null)
            return this.model.totalMoney - this.model.salesPlan.totalMoney > 0 ? { "col-green": true } : { "col-red": true };
        else
            return;
    }

    strDiffOil(o: server.order) {
        if (o.unit == "升")
            return o.diffOil + "升 换算吨：" + o.diffOil * o.density / 1000 + "吨";
        else if (o.unit == "吨")
            return o.diffOil + "吨 换算升：" + o.diffOil / o.density * 1000 + "升";
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