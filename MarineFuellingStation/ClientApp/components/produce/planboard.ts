import ComponentBase from "../../componentbase";
import { Component } from 'vue-property-decorator';
import axios from "axios";

@Component
export default class PlanBoardComponent extends ComponentBase {
    carNo: string = "";
    salesPlans: server.salesPlan[];
    
    mounted() {
        this.$emit('setTitle', '计划看板');
    };

    constructor() {
        super();

        this.salesPlans = new Array<server.salesPlan>();

        this.getSalesPlans();
    }

    getState(st: server.salesPlanState) {
        switch (st) {
            case server.salesPlanState.未审批:
                return "未审批";
            case server.salesPlanState.已审批:
                return "已审批"
        }
    }

    godetail(s: server.salesPlan) {
        this.$router.push('/produce/planboard/' + s.id + '/board')
    }

    change(label: string, tabkey: string) {
        console.log(label);
        this.$emit('setTitle', this.$store.state.username + ' ' + label);
    }

    getSalesPlans() {
        axios.get('/api/SalesPlan/GetHasFinish').then((res) => {
            let jobj = res.data as server.resultJSON<server.salesPlan[]>;
            if (jobj.code == 0)
                this.salesPlans = jobj.data;
        });
    }
}