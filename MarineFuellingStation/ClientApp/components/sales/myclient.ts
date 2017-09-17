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

    filterCType: Array<helper.filterBtn>;filterPType: Array<helper.filterBtn>;filterBalances: Array<helper.filterBtn>;filterCycle: Array<helper.filterBtn>;
    
    actBtnId: number; actBtnId1: number; actBtnId2: number; actBtnId3: number;//当前激活状态的条件button
    
    constructor() {
        super();

        this.clients = new Array<server.client>();
        this.filterCType = [
            { id: 0, name: '全部', actived: true },
            { id: 1, name: '个人', actived: false },
            { id: 2, name: '公司', actived: false }
        ];
        this.filterPType = [
            { name: '已计划', actived: false },
            { name: '已完成', actived: false },
            { name: '已审批', actived: false }
        ];
        this.filterBalances = [
            { name: '少于1000', actived: false },
            { name: '少于10000', actived: false }
        ]
        this.filterCycle = [
            { name: '7天不计划', actived: false },
            { name: '15天不计划', actived: false },
            { name: '30天不计划', actived: false },
            { name: '90天不计划', actived: false }
        ]
        this.actBtnId = 0; this.actBtnId1 = -1; this.actBtnId2 = -1; this.actBtnId3 = -1;
        this.getClients(server.clientType.全部);
    }

    switchBtn(o: helper.filterBtn, idx: number, group: string) {
        switch (group) {
            case "客户类型":
                if (idx != this.actBtnId) {
                    o.actived = true;
                    this.filterCType[this.actBtnId].actived = false;
                    this.actBtnId = idx;
                }
                if (o.name == '全部')
                    this.getClients(server.clientType.全部)
                else if (o.name == '个人')
                    this.getClients(server.clientType.个人)
                else if (o.name == '公司')
                    this.getClients(server.clientType.公司)
                break;
            case "计划单":
                o.actived = true;
                if (idx != this.actBtnId1 && this.actBtnId1 != -1) {
                    this.filterPType[this.actBtnId1].actived = false;
                    this.actBtnId1 = idx;
                }
                else
                    this.actBtnId1 = idx;
                break;
            case "账户余额":
                o.actived = true;
                if (idx != this.actBtnId2 && this.actBtnId2 != -1) {
                    this.filterBalances[this.actBtnId2].actived = false;
                    this.actBtnId2 = idx;
                }
                else
                    this.actBtnId2 = idx;
                break;
            case "周期":
                o.actived = true;
                if (idx != this.actBtnId3 && this.actBtnId3 != -1) {
                    this.filterCycle[this.actBtnId3].actived = false;
                    this.actBtnId3 = idx;
                }
                else
                    this.actBtnId3 = idx;
                break;
        }
    }

    setActived() {

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
    getClients(ctype: server.clientType) {
        axios.get('/api/Client/GetMyClients?ctype=' + ctype.toString()).then((res) => {
            let jobj = res.data as server.resultJSON<server.client[]>;
            if (jobj.code == 0) {
                this.clients = jobj.data;
            }
            else
                this.toastError('无法获取客户数据，请重试')
        });
    }
}