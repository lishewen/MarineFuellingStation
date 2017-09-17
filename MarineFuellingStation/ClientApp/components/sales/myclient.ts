import ComponentBase from "../../componentbase";
import axios from "axios";
import moment from "moment";
import { Component } from 'vue-property-decorator';

@Component
export default class MyClientComponent extends ComponentBase {
    clients: server.client[];
    radio2: string = "1";
    show1: boolean = false;
    show2: boolean = false;
    carNo: string = "";

    filterCType: Array<helper.filterBtn>; filterPType: Array<helper.filterBtn>; filterBalances: Array<helper.filterBtn>; filterCycle: Array<helper.filterBtn>;

    actBtnId: number; actBtnId1: number; actBtnId2: number; actBtnId3: number;//当前激活状态的条件button
    ctype: server.clientType; ptype: server.salesPlanState; balances: number; cycle: number;

    constructor() {
        super();

        this.clients = new Array<server.client>();
        this.filterCType = [
            { id: 0, name: '全部', value: server.clientType.全部, actived: true },
            { id: 1, name: '个人', value: server.clientType.个人, actived: false },
            { id: 2, name: '公司', value: server.clientType.公司, actived: false }
        ];
        this.filterPType = [
            { name: '已计划', value: server.salesPlanState.未审批, actived: false },
            { name: '已完成', value: server.salesPlanState.已完成, actived: false },
            { name: '已审批', value: server.salesPlanState.已审批, actived: false }
        ];
        this.filterBalances = [
            { name: '少于1000', value: 1000, actived: false },
            { name: '少于10000', value: 10000, actived: false }
        ]
        this.filterCycle = [
            { name: '7天不计划', value: 7, actived: false },
            { name: '15天不计划', value: 15, actived: false },
            { name: '30天不计划', value: 30, actived: false },
            { name: '90天不计划', value: 90, actived: false }
        ]
        this.actBtnId = 0; this.actBtnId1 = -1; this.actBtnId2 = -1; this.actBtnId3 = -1;
        this.getClients();
    }

    switchBtn(o: helper.filterBtn, idx: number, group: string) {
        switch (group) {
            case "客户类型":
                if (idx != this.actBtnId) {
                    o.actived = true;
                    this.ctype = <server.clientType>o.value;
                    this.filterCType[this.actBtnId].actived = false;
                    this.actBtnId = idx;
                }
                break;
            case "计划单":
                o.actived = true;
                this.ptype = <server.salesPlanState>o.value;
                if (idx != this.actBtnId1 && this.actBtnId1 != -1) {
                    this.filterPType[this.actBtnId1].actived = false;
                    this.actBtnId1 = idx;
                }
                else
                    this.actBtnId1 = idx;
                break;
            case "账户余额":
                o.actived = true;
                this.balances = <number>o.value;
                if (idx != this.actBtnId2 && this.actBtnId2 != -1) {
                    this.filterBalances[this.actBtnId2].actived = false;
                    this.actBtnId2 = idx;
                }
                else
                    this.actBtnId2 = idx;
                break;
            case "周期":
                o.actived = true;
                this.cycle = <number>o.value;
                if (idx != this.actBtnId3 && this.actBtnId3 != -1) {
                    this.filterCycle[this.actBtnId3].actived = false;
                    this.actBtnId3 = idx;
                }
                else
                    this.actBtnId3 = idx;
                break;
        }
    }

    filterclick(): void {
        this.show2 = false;
    };

    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 的客户');
        this.$watch('radio2', (v, ov) => {
            switch (v) {
                case "1":
                    this.show1 = false;
                    break;
                case "2":
                    this.show1 = true;
                    break;
            }
        });
    };

    change(label: string, tabkey: string) {
        console.log(label);
        this.$emit('setTitle', this.$store.state.username + ' ' + label);
    }

    //获得我的客户列表
    getClients() {
        if (!this.ctype) this.ctype = server.clientType.全部;
        if (!this.ptype) this.ptype = -1;//-1标识没有选择任何项
        if (!this.balances) this.balances = -1;
        if (!this.cycle) this.cycle = -1;
        axios.get('/api/Client/GetMyClients'
            + '?ctype=' + this.ctype.toString()
            + '&ptype=' + this.ptype.toString()
            + '&balances=' + this.balances.toString()
            + '&cycle=' + this.cycle.toString()
        ).then((res) => {
            let jobj = res.data as server.resultJSON<server.client[]>;
            if (jobj.code == 0) {
                this.clients = jobj.data;
            }
            else
                this.toastError('无法获取客户数据，请重试')
        });
    }
}