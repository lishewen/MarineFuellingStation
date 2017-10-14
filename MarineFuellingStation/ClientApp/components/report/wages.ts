import ComponentBase from "../../componentbase";
import { Component } from 'vue-property-decorator';
import moment from "moment";
import axios from "axios";

@Component({
    components: {
        WeuiSearch: require('../weui-search/search.vue')
    }
})
export default class WageComponent extends ComponentBase {
    list: server.wage[];
    model: server.wage;
    /** 部门字典 */
    departmentdict: { [index: number]: string; } = {};
    departments: work.department[];
    sumwage: number;
    showwage: boolean = false;
    selecteddate: string = moment(new Date()).format('YYYY-MM-DD');
    sdate: string;
    traffic: number = 0;
    qingjia: number = 0;
    lend: number = 0;
    jixiao: number = 0;//绩效工资
    security: number = 0;
    sv: string = "";
    departshow: boolean = false;
    picked: string[];

    constructor() {
        super();

        this.picked = new Array<string>();
        this.list = new Array<server.wage>();
        this.departments = new Array<work.department>();
        this.model = new Object as server.wage;
        this.sumwage = 0;

        this.getDepartments();
        this.sdate = moment(this.selecteddate).format("YYYYMM");
        this.getWage(this.sdate);
    }

    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 工资');
        this.$watch('traffic', (v, ov) => {
            this.model.交通 = parseInt(v);
            this.changeMoney();
        });
        this.$watch('qingjia', (v, ov) => {
            this.model.请假 = parseInt(v);
            this.changeMoney();
        });
        this.$watch('lend', (v, ov) => {
            this.model.借支 = parseInt(v);
            this.changeMoney();
        });
        this.$watch('security', (v, ov) => {
            this.model.安全保障金 = parseInt(v);
            this.changeMoney();
        });
        this.$watch('selecteddate', (v: string, ov: string) => {
            this.sdate = moment(v).format("YYYYMM");
        });
        this.$watch('jixiao', (v: string, ov: string) => {
            this.model.绩效工资 = parseInt(v);
            this.changeMoney();
        });
    };

    change(label: string, tabkey: string) {
        console.log(label);
        this.$emit('setTitle', this.$store.state.username + ' ' + label);
    }

    clickWage(wage: server.wage) {
        this.model = wage;
        this.traffic = this.model.交通;
        this.security = this.model.安全保障金;
        this.lend = this.model.借支;
        this.qingjia = this.model.请假;
        this.jixiao = this.model.绩效工资;
        this.showwage = true;
    }

    clickDepart() {
        let ids = this.picked.join('|');
        this.getWageByDepart(this.sdate, ids);
        this.departshow = false;
    }

    saveWage() {
        //TODO: 验证操作
        if (this.model.实发 == 0 || this.model.实发 != this.model.现金 + this.model.转卡金额) { this.toastError("实发 = 现金 + 转卡金额"); return; }
        this.postWage(this.model);
    }

    sumWage() {
        this.sumwage = 0;
        this.list.forEach(w => { this.sumwage += w.实发 });
    }

    changeMoney() {
        this.model.实发 = this.model.基本 + this.model.提成 + this.model.交通 + this.model.绩效工资
            - this.model.社保 - this.model.安全保障金 - this.model.请假 - this.model.餐费 - this.model.借支;
    }

    getWage(ym: string) {
        axios.get('/api/Wage/' + ym).then((res) => {
            let jobj = res.data as server.resultJSON<server.wage[]>;
            if (jobj.code == 0) {
                this.list = jobj.data;
                this.sumWage();
            }
        });
    }

    getWageByDepart(ym: string, departids: string) {
        axios.get('/api/Wage/GetByDepart/' + ym + '/' + departids).then((res) => {
            let jobj = res.data as server.resultJSON<server.wage[]>;
            if (jobj.code == 0) {
                this.list = jobj.data;
                this.sumWage();
            }
        });
    }

    postWage(model: server.wage) {
        axios.post('/api/Wage', model).then((res) => {
            let jobj = res.data as server.resultJSON<server.wage>;
            if (jobj.code == 0) {
                this.model = jobj.data;
                this.getWage(this.sdate);
                this.toastSuccess(jobj.msg);
            }
        });
    }

    getDepartments() {
        axios.get('/api/Department').then((res) => {
            let jobj = res.data as work.departmentListResult;
            if (jobj.errcode == 0) {
                this.departments = jobj.department;
                jobj.department.forEach((o, i) => {
                    this.departmentdict[o.id] = o.name;
                });
            }
        });
    }
}