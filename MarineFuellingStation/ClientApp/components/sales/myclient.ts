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
    filterBtns: Array<helper.filterBtn>;
    activedBtnId: number;

    constructor() {
        super();

        this.clients = new Array<server.client>();
        this.filterBtns = [
            { id: 0, name: '全部', actived: true },
            { id: 1, name: '个人', actived: false },
            { id: 2, name: '公司', actived: false }
        ];
        this.activedBtnId = 0;
        this.getClients();
    }

    switchBtn(o: any) {
        if (o.id != this.activedBtnId){
            o.actived = true;
            this.filterBtns[this.activedBtnId].actived = false;
            this.activedBtnId = o.id;
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
        axios.get('/api/Client/GetMyClients').then((res) => {
            let jobj = res.data as server.resultJSON<server.client[]>;
            if (jobj.code == 0) {
                this.clients = jobj.data;
            }
            else
                this.toastError('无法获取客户数据，请重试')
        });
    }
}