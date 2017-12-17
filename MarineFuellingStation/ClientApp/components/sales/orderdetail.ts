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
            case "orderlist":
                from = '/finance/orderlist';
                break;
        }
        this.getOrder(id, () => {
            //设置返回键的连接
            this.$emit('setTitle', this.model.name + ' 销售单明细', from);
        });
    }

    getOrderType(ot: server.salesPlanType) {
        if (ot == server.salesPlanType.水上机油)
            return "水上机油"
        else if (ot == server.salesPlanType.水上加油)
            return "水上加油"
        else if (ot == server.salesPlanType.陆上装车)
            return "陆上装车"
    }

    getIsInvoice(isInv: boolean) {
        if (isInv)
            return "代号1";
        else
            return "代号2";
    }

    strOrderState1(o: server.order) {
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

    totalMoneyClass() {
        if (this.model.salesPlan != null)
            return this.model.totalMoney - this.model.salesPlan.totalMoney > 0 ? { "col-green": true } : { "col-red": true };
        else
            return;
    }

    strDiffOil(o: server.order) {
        if (o.unit == "升")
            return o.diffOil + "升 = " + Math.round(o.diffOil * o.density / 1000 * 100)/100 + "吨";
        else if (o.unit == "吨")
            return o.diffOil + "吨 = " + Math.round(o.diffOil / o.density * 1000 * 100)/100 + "升";
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