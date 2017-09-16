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
    
    activedBtnId: number;

    constructor() {
        super();

        this.clients = new Array<server.client>();
        this.filterCType = [
            { id: 0, name: '全部', actived: true },
            { id: 1, name: '个人', actived: false },
            { id: 2, name: '公司', actived: false }
        ];
        this.filterPType = [
            { id: 0, name: '已计划', actived: true },
            { id: 1, name: '已完成', actived: false },
            { id: 2, name: '已审批', actived: false }
        ];
        this.filterBalances = [
            { id: 0, name: '少于1000', actived: true },
            { id: 1, name: '少于10000', actived: false }
        ]
        this.filterCycle = [
            { id: 0, name: '7天不计划', actived: true },
            { id: 1, name: '15天不计划', actived: false },
            { id: 2, name: '30天不计划', actived: false },
            { id: 3, name: '90天不计划', actived: false }
        ]
        this.activedBtnId = 0;
        this.getClients(server.clientType.全部);
    }

    switchBtn(o: any) {
        if (o.id != this.activedBtnId){
            o.actived = true;
            this.filterCType[this.activedBtnId].actived = false;
            this.activedBtnId = o.id;
        }
        if (o.name == '全部')
            this.getClients(server.clientType.全部)
        else if (o.name == '个人')
            this.getClients(server.clientType.个人)
        else if (o.name == '公司')
            this.getClients(server.clientType.公司)
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