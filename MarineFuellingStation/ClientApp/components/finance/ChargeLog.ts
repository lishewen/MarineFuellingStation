import ComponentBase from "../../ComponentBase";
import { Component } from 'vue-property-decorator';
import axios from "axios";
import moment from "moment";

@Component
export default class ChargeLogComponent extends ComponentBase {
    chargeLogs: server.chargeLog[];
    
    sv: string = "";
    page: number;
    scrollRef: any;
    pSize: number = 10;

    constructor() {
        super();

        this.chargeLogs = new Array<server.chargeLog>();
        this.getChargeLogs();
    }
    
    mounted() {
        this.$emit('setTitle', '账户变动记录');
        this.$watch("sv", (v, ov) => {
            this.sv = v;
            this.page = 1;
            this.getChargeLogs();
        });
    };

    loadList() {
        this.getChargeLogs((list: server.chargeLog[]) => {
            this.chargeLogs = this.page > 1 ? [...this.chargeLogs, ...list] : this.chargeLogs;
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

    classChargeType(t: server.chargeType) {
        if (t == server.chargeType.充值) {
            return { 'clog-color_green': true }
        }
        else if (t == server.chargeType.消费) {
            return { 'clog-color_red': true }
        }
    }

    classMoney(t: server.chargeType) {
        if (t == server.chargeType.充值) {
            return { 'clog-font_green': true }
        }
        else if (t == server.chargeType.消费) {
            return { 'clog-font_red': true }
        }
    }

    //充值到客户账户
    postCharge() {
        axios.post("/api/chargelog?cid=").then((res) => {
            let jobj = res.data as server.resultJSON<server.chargeLog>;
            if (jobj.code == 0) {
                this.toastSuccess("充值成功")
            }
            if (jobj.code == 501) {
                this.toastError(jobj.msg)
            }
        });
    }

    getChargeLogs(callback?: Function) {
        if (this.page == null) this.page = 1;
        if (this.pSize == null) this.pSize = 10;
        if (this.sv == null) this.sv = "";
        axios.get('/api/ChargeLog/GetByPager?page='
            + this.page
            + '&pagesize=' + this.pSize
            + '&sv=' + this.sv).then((res) => {
            let jobj = res.data as server.resultJSON<server.chargeLog[]>;
            if (jobj.code == 0) {
                if (callback) {
                    callback(jobj.data);
                }
                else {
                    this.chargeLogs = jobj.data;
                    console.log(this.chargeLogs);
                    this.page++;
                }
            }
        });
    }
    
}