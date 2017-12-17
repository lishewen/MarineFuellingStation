import ComponentBase from "../../componentbase";
import { Component } from 'vue-property-decorator';
import axios from "axios";

@Component
export default class PlanBoardComponent extends ComponentBase {
    carNo: string = "";
    salesPlans: server.salesPlan[];
    page: number;
    scrollRef: any;
    pSize: number = 10;
    islandplan: boolean = false;
    landPlans: server.salesPlan[];
    waterPlans: server.salesPlan[];
    
    mounted() {
        this.$emit('setTitle', '计划看板');
    };

    constructor() {
        super();

        this.salesPlans = new Array<server.salesPlan>();
        this.landPlans = new Array<server.salesPlan>();
        this.waterPlans = new Array<server.salesPlan>();

        this.getSalesPlans();
    }

    loadList() {
        this.getSalesPlans((list: server.salesPlan[]) => {
            if (this.islandplan) {
                this.landPlans = this.page > 1 ? [...this.landPlans, ...list] : this.landPlans;
                this.scrollRef = (<any>this).$refs.landinfinitescroll;
            }
            else {
                this.waterPlans = this.page > 1 ? [...this.waterPlans, ...list] : this.waterPlans;
                this.scrollRef = (<any>this).$refs.waterinfinitescroll;
            }
            
            if (list.length < this.pSize) {
                this.scrollRef.$emit("ydui.infinitescroll.loadedDone");
                return;
            }

            //通知加载数据完毕
            this.scrollRef.$emit("ydui.infinitescroll.finishLoad");

            if (list.length > 0)
                this.page++;
            else
                this.page = 1;
            console.log("page = " + this.page)
        });
    }

    change(label: string, tabkey: string) {
        this.$emit('setTitle', label + '计划');

        (<any>this).$refs.waterinfinitescroll.$emit('ydui.infinitescroll.reInit');
        (<any>this).$refs.landinfinitescroll.$emit('ydui.infinitescroll.reInit');
        this.page = 1;
        if (label == '水上') {
            this.landPlans = null;
            this.islandplan = false;
        }
        if (label == '陆上') {
            this.waterPlans = null;
            this.islandplan = true;
        }
        this.page = 1;
        this.getSalesPlans();
    }

    stateClass(st: server.salesPlanState): any{
        if (st == server.salesPlanState.未审批)
            return { color_red: true }
        if (st == server.salesPlanState.已审批)
            return { color_green: true }
    }

    godetail(s: server.salesPlan) {
        this.$router.push('/produce/planboard/' + s.id + '/board')
    }

    getSalesPlans(callback?: Function) {
        if (this.page == null) this.page = 1;
        if (this.pSize == null) this.pSize = 10;
        axios.get('/api/SalesPlan/GetAuditings?page='
            + this.page
            + '&pagesize=' + this.pSize
            + '&islandplan=' + this.islandplan).then((res) => {
            let jobj = res.data as server.resultJSON<server.salesPlan[]>;
            if (jobj.code == 0) {
                if (callback) {
                    callback(jobj.data);
                }
                else {
                    if (this.islandplan)
                        this.landPlans = jobj.data;
                    else
                        this.waterPlans = jobj.data;
                    console.log(this.salesPlans);
                    this.page++;
                }
            }

        });
    }
}