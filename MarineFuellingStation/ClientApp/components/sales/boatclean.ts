import ComponentBase from "../../componentbase";
import { Component } from 'vue-property-decorator';
import axios from "axios";
import moment from "moment";

@Component
export default class BoatCleanComponent extends ComponentBase {
    model: server.boatClean;
    bc: server.boatClean;
    boatCleans: server.boatClean[];
    workers: work.userlist[];
    selectedworker: string;
    showDetail: boolean = false;
    showWorkers: boolean = false;

    radio2: string = '1';
    unit: string = '升';
    carNo: string = '';
    sv: string = "";
    isPrevent: boolean = true;
    page: number;
    scrollRef: any;
    pSize: number = 20;

    constructor() {
        super();

        this.boatCleans = new Array<server.boatClean>();
        this.model = (new Object()) as server.boatClean;
        this.model.name = '';
        this.model.carNo = '';
        this.model.money = 0;
        this.model.responseId = "梧海事清油（      ）第      号";
        this.model.address = "广西梧州市云龙桥下游500米对开河边";
        this.model.company = "广西梧州市汇保源防污有限公司";
        this.model.phone = "07742031178";
        this.model.isInvoice = false;
        this.model.worker = '';

        this.selectedworker = "请选择";

        this.bc = new Object as server.boatClean;

        this.getBoatCleanNo();
        this.getWorkers();
    }

    loadList() {
        this.getBoatCleans((list: server.boatClean[]) => {
            this.boatCleans = this.page > 1 ? [...this.boatCleans, ...list] : this.boatCleans;
            this.scrollRef = (<any>this).$refs.infinitescroll;
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
            if (v.length >= 2 || v == "")
                this.searchBoatCleans(v);
        });
    };

    itemclick(b: server.boatClean) {
        this.showDetail = true;
        this.bc = b;
    }

    selectworkerclick(w: work.userlist) {
        this.model.worker = w.name;
        this.selectedworker = w.name;
        this.showWorkers = false;
    }

    buttonclick() {
        //信息验证
        if (this.model.carNo == '') { this.toastError('船号不能为空'); return; };
        if (this.model.company == '') { this.toastError('公司名称不能为空'); return; };
        if (this.model.money <= 0) { this.toastError('金额应大于0'); return; };
        if (this.model.worker == '') { this.toastError('请选择施工人员'); return; };
        this.postBoatClean(this.model);
    }

    change(label: string, tabkey: string) {
        console.log(label);
        
        if (label == '单据记录') {
            this.page = 1;
            this.getBoatCleans();
        }  
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

    //获得生产员
    getWorkers() {
        axios.get('/api/User/Worker').then((res) => {
            let jobj = res.data as work.tagMemberResult;
            if (jobj.errcode == 0)
                this.workers = jobj.userlist;
        });
    }

    getBoatCleans(callback?: Function) {
        if (this.page == null) this.page = 1;
        axios.get('/api/BoatClean/GetByPager?page='
            + this.page
            + '&pagesize=' + this.pSize).then((res) => {
            let jobj = res.data as server.resultJSON<server.boatClean[]>;
            if (jobj.code == 0) {
                if (callback) {
                    callback(jobj.data);
                }
                else {
                    this.boatCleans = jobj.data;
                    this.page++;
                }
            }
        });
    }

    searchBoatCleans(sv: string) {
        axios.get('/api/BoatClean/' + sv).then((res) => {
            let jobj = res.data as server.resultJSON<server.boatClean[]>;
            if (jobj.code == 0)
                this.boatCleans = jobj.data;
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