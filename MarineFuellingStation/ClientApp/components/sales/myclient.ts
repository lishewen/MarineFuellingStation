import ComponentBase from "../../componentbase";
import axios from "axios";
import moment from "moment";
import { Component } from 'vue-property-decorator';

@Component
export default class MyClientComponent extends ComponentBase {
    clients: server.client[];
    client: server.client;
    radio2: string = "1";
    show1: boolean = false;
    show2: boolean = false;
    showAct: boolean = false;
    showRemark: boolean = false;
    remark: string = "";
    carNo: string = "";
    actItems: ydui.actionSheetItem[];

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

        this.actItems = new Array();

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
                o.actived = !o.actived;
                this.ptype = <server.salesPlanState>o.value;
                if (idx != this.actBtnId1 && this.actBtnId1 != -1) {
                    this.filterPType[this.actBtnId1].actived = false;
                    this.actBtnId1 = idx;
                }
                else
                    this.actBtnId1 = idx;
                break;
            case "账户余额":
                o.actived = !o.actived;
                this.balances = <number>o.value;
                if (idx != this.actBtnId2 && this.actBtnId2 != -1) {
                    this.filterBalances[this.actBtnId2].actived = false;
                    this.actBtnId2 = idx;
                }
                else
                    this.actBtnId2 = idx;
                break;
            case "周期":
                o.actived = !o.actived;
                this.cycle = <number>o.value;
                if (idx != this.actBtnId3 && this.actBtnId3 != -1) {
                    this.filterCycle[this.actBtnId3].actived = false;
                    this.actBtnId3 = idx;
                }
                else
                    this.actBtnId3 = idx;
                break;
        }
        if (o.actived) this.getClients();
    }

    filterclick(): void {
        this.show2 = false;
        this.getClients();
    };

    //显示actionsheet
    clientclick(c: server.client) {
        console.log(c);
        let elseActItems = new Array();
        this.client = c;
        if (c.remark != null) this.remark = c.remark;
        this.showAct = true;
        //actionsheet
        this.actItems = new Array();
        if (c.isMark)
            this.actItems.push(
                {
                    label: '取消标记',
                    method: () => {
                        this.putMark(c, false);
                    }
                },
            );
        else
            this.actItems.push(
                {
                    label: '标记',
                    method: () => {
                        this.putMark(c, true);
                    }
                },
            );
        elseActItems = [
            {
                label: '详细信息',
                method: () => {
                    this.godetail(c.id);
                }
            },
            {
                label: '备注',
                method: () => {
                    this.showRemark = true;
                }
            },
            {
                label: '清空所有标记',
                method: () => {
                    this.putClearAllMark();
                }
            }
        ];
        this.actItems = [...this.actItems, ...elseActItems];
    }

    classMark(isMark: boolean) {
        return isMark ? {color_blue: true} : {color_blue: false};
    }

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
    godetail(id: number) {
        this.$router.push('/sales/myclient/' + id);
    }

    //获得我的客户列表
    getClients() {
        if (this.ctype == null) this.ctype = server.clientType.全部;
        if (this.ptype == null) this.ptype = -1;//-1标识没有选择任何项
        if (this.balances == null) this.balances = -1;
        if (this.cycle == null) this.cycle = -1;
        
        axios.get('/api/Client/GetClients'
            + '?ctype=' + this.ctype.toString()
            + '&ptype=' + this.ptype.toString()
            + '&balances=' + this.balances.toString()
            + '&cycle=' + this.cycle.toString()
            + '&kw='
            + '&isMy=true'
        ).then((res) => {
            let jobj = res.data as server.resultJSON<server.client[]>;
            if (jobj.code == 0) {
                this.clients = jobj.data;
            }
            else
                this.toastError('无法获取客户数据，请重试')
        });
    }

    putMark(c: server.client, isMark: boolean) {
        c.isMark = isMark;
        axios.put('/api/Client/MarkTag', c).then((res) => {
            let jobj = res.data as server.resultJSON<server.client>;
            if (jobj.code == 0) {
                this.client = jobj.data;
                this.toastSuccess('标记成功')
            }
        }); 
    }

    //备注提交
    putReMark(c: server.client) {
        c.remark = this.remark;
        axios.put('/api/Client/Remark', c).then((res) => {
            let jobj = res.data as server.resultJSON<server.client>;
            if (jobj.code == 0) {
                this.client = jobj.data;
                this.toastSuccess('操作成功')
            }
        });
    }

    putClearAllMark() {
        axios.put('/api/Client/ClearMyClientMark', null).then((res) => {
            let jobj = res.data as server.resultJSON<server.client>;
            if (jobj.code == 0) {
                this.toastSuccess('清空标记成功')
            }
        }); 
    }
}