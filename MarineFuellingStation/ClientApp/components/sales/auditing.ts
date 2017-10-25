import ComponentBase from "../../componentbase";
import axios from "axios";
import moment from "moment";
import { Component } from 'vue-property-decorator';

@Component
export default class AuditingComponent extends ComponentBase {
    plans: server.salesPlan[];
    plan: server.salesPlan;
    actItems: ydui.actionSheetItem[];

    showAct: boolean = false;

    filterCType: Array<helper.filterBtn>;
    state: server.salesPlanState;

    actBtnId: number; //当前激活状态的条件button

    page: number;
    scrollRef: any;
    pSize: number = 30;

    strIsLp: string;//水上审核和陆上审核的标识，路由传值

    constructor() {
        super();

        this.plans = new Array<server.salesPlan>();
        this.filterCType = [
            { id: 0, name: '未审核', value: server.salesPlanState.未审批, actived: true },
            { id: 1, name: '已审核', value: server.salesPlanState.已审批, actived: false }
        ];

        this.actBtnId = 0;
        this.actItems = new Array();

        this.state = server.salesPlanState.未审批;
    }

    switchBtn(o: helper.filterBtn, idx: number) {
        o.actived = true;
        if (idx != this.actBtnId) {
            this.filterCType[this.actBtnId].actived = false;
            this.actBtnId = idx;

            this.state = o.value as server.salesPlanState;
            this.page = 1;
            this.getSalesPlans();
        }
    }

    //显示actionsheet
    planclick(s: server.salesPlan) {
        console.log(s);
        this.showAct = true;
        this.actItems = [
            {
                label: '详细信息',
                method: () => {
                    this.godetail(s.id);
                }
            },
            {
                label: '审核',
                method: () => {
                    this.putAuditingOK(s);
                }
            }
        ];
    }

    mounted() {
        this.strIsLp = this.$route.params.islandplan;
        console.log("strIsLp = " + this.strIsLp);
        this.getSalesPlans();
        this.$emit('setTitle', '计划审核');
    };

    change(label: string, tabkey: string) {
        console.log(label);
        this.$emit('setTitle', this.$store.state.username + ' ' + label);
    }
    godetail(id: number) {
        this.$router.push('/sales/plan/' + id + '/auditing')
    }

    loadList() {
        this.getSalesPlans((list: server.salesPlan[]) => {
            this.plans = this.page > 1 ? [...this.plans, ...list] : this.plans;
            this.scrollRef = (<any>this).$refs.infinitescroll;
            if (list.length < this.pSize) {
                this.scrollRef.$emit("ydui.infinitescroll.loadedDone");
                return;
            }

            //通知加载数据完毕
            (<any>this).$refs.infinitescroll.$emit("ydui.infinitescroll.finishLoad");

            if (list.length > 0)
                this.page++;
            else
                this.page = 1;
            console.log("page = " + this.page)
        });
    }

    //获得计划列表
    getSalesPlans(callback?: Function) {
        if (this.page == null) this.page = 1;
        if (this.pSize == null) this.pSize = 30;
        if (this.state == null) this.state = server.salesPlanState.未审批;
        axios.get('/api/SalesPlan/GetByState?'
            + 'page=' + this.page
            + '&pageSize=' + this.pSize
            + '&sps=' + this.state
            + '&islandplan=' + this.strIsLp).then((res) => {
                let jobj = res.data as server.resultJSON<server.salesPlan[]>;
                if (jobj.code == 0) {
                    if (callback) {
                        callback(jobj.data);
                    }
                    else {
                        this.plans = jobj.data;
                        console.log(this.plans);
                        this.page++;
                    }
                }
            });
    }

    putAuditingOK(s: server.salesPlan) {
        axios.put('/api/SalesPlan/AuditingOK', s).then((res) => {
            let jobj = res.data as server.resultJSON<server.salesPlan>;
            if (jobj.code == 0) {
                this.plan = jobj.data;
                this.toastSuccess('审核成功')
                this.page = 1;
                this.getSalesPlans();
            }
        });
    }
}