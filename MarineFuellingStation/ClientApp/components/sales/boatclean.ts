import ComponentBase from "../../componentbase";
import { Component } from 'vue-property-decorator';
import axios from "axios";
import moment from "moment";

@Component
export default class BoatCleanComponent extends ComponentBase {
    model: server.boatClean;
    list: server.boatClean[];

    radio2: string = '1';
    unit: string = '升';
    carNo: string = '';
    sv: string = "";
    isPrevent: boolean = true;

    constructor() {
        super();

        this.list = new Array<server.boatClean>();
        this.model = (new Object()) as server.boatClean;
        this.model.name = '';
        this.model.carNo = '';
        this.model.money = 0;
        this.model.responseId = "梧海事清油（      ）第      号";
        this.model.address = "广西梧州市云龙桥下游500米对开河边";
        this.model.company = "广西梧州市汇保源防污有限公司";
        this.model.phone = "07742031178";
        this.model.isInvoice = false;

        this.getBoatCleanNo();
        this.getBoatCleans();
    }

    getStateName(s: server.boatCleanState): string {
        switch (s) {
            case server.boatCleanState.已开单:
                return '已开单';
            case server.boatCleanState.施工中:
                return '施工中';
            case server.boatCleanState.已完成:
                return '已完成';
        }
    }

    classState(s: server.boatCleanState): any {
        switch (s) {
            case server.boatCleanState.已开单:
                return { color_red: true }
            case server.boatCleanState.施工中:
                return { color_green: true }
            case server.boatCleanState.已完成:
                return { color_blue: true }
        }
    }

    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 船舶清污');
        //观察者模式
        this.$watch('radio2', (v, ov) => {
            switch (v) {
                case "1":
                    this.unit = '升';
                    break;
                case "2":
                    this.unit = '吨';
                    break;
                case "3":
                    this.unit = '桶';
                    break;
            }
        });
        this.$watch('sv', (v: string, ov) => {
            //3个字符开始才执行请求操作，减少请求次数
            if (v.length >= 3)
                this.searchBoatCleans(v);
        });
    };

    buttonclick() {
        //信息验证

        this.postBoatClean(this.model);
    }

    change(label: string, tabkey: string) {
        console.log(label);
        this.$emit('setTitle', this.$store.state.username + ' ' + label);
    }

    getBoatCleanNo() {
        axios.get('/api/BoatClean/BoatCleanNo').then((res) => {
            let jobj = res.data as server.resultJSON<string>;
            if (jobj.code == 0) {
                this.model.name = jobj.data;
                this.isPrevent = false;
            }
                
        });
    }

    getBoatCleans() {
        axios.get('/api/BoatClean').then((res) => {
            let jobj = res.data as server.resultJSON<server.boatClean[]>;
            if (jobj.code == 0)
                this.list = jobj.data;
        });
    }

    searchBoatCleans(sv: string) {
        axios.get('/api/BoatClean/' + sv).then((res) => {
            let jobj = res.data as server.resultJSON<server.boatClean[]>;
            if (jobj.code == 0)
                this.list = jobj.data;
        });
    }

    postBoatClean(model: server.boatClean) {
        axios.post('/api/BoatClean', model).then((res) => {
            let jobj = res.data as server.resultJSON<server.boatClean>;
            if (jobj.code == 0) {
                this.getBoatCleanNo();
                this.toastSuccess(jobj.msg);
            }
        });
    }
}