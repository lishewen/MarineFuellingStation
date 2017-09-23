import ComponentBase from "../../componentbase";
import { Component } from 'vue-property-decorator';
import axios from "axios";
import moment from "moment";

@Component
export default class OilStoreComponent extends ComponentBase {
    datestr: string = "";
    progress1: number = 0.4;
    progress2: number = 0.4;
    show1: boolean = false;

    survey: server.survey;
    surveys: server.survey[];
    sts: server.storeType[];
    salesSts: server.store[];
    selectStore: server.store;
    
    constructor() {
        super();

        this.datestr = this.formatDate(new Date());
        this.sts = new Array<server.storeType>();
        this.salesSts = new Array<server.store>();
        this.surveys = new Array<server.survey>();
        this.selectStore = new Object() as server.store;
        this.survey = new Object() as server.survey;
        this.getStoreTypes();
    }
    
    mounted() {
        this.$emit('今日油仓情况');
    };

    /**
     * 添加测量记录
     */
    storeclick(st: server.store) {
        this.show1 = true;
        this.survey.storeId = st.id;
        this.survey.name = st.name;
        this.selectStore = st;
        this.getSurveys(st.id);
    }

    /**
     * 当前数量百分比
     */
    getPercent(val: number, vol: number) {
        return Math.round(val / vol * 10) / 10;
    }

    getStoreTypes() {
        axios.get('/api/StoreType').then((res) => {
            let jobj = res.data as server.resultJSON<server.storeType[]>;
            if (jobj.code == 0) {
                this.sts = jobj.data;
                jobj.data.forEach((o, i) => {
                    o.stores.forEach((s, ii) => {
                        if (s.storeClass == server.storeClass.销售仓)
                            this.salesSts.push(s);
                    });
                })
                console.log(this.sts);
            }
        });
    }

    formatDate(d: Date): string {
        return moment(d).format('MM-DD hh:mm');
    }

    getSurveys(stid: number) {
        axios.get('/api/Survey/GetTop10/' + stid.toString()).then((res) => {
            let jobj = res.data as server.resultJSON<server.survey[]>;
            if (jobj.code == 0) {
                this.surveys = jobj.data;
            }
        });
    }

    validate() {
        if (this.survey.temperature == null || this.survey.temperature == '') {
            this.toastError('油温不能为空');
            return false;
        }
        if (this.survey.density == null || this.survey.density == '') {
            this.toastError('密度不能为空');
            return false;
        }
        if (this.survey.height == null || this.survey.height == '') {
            this.toastError('油高不能为空');
            return false;
        }
        return true;
    }

    postSurveyclick() {
        if (!this.validate()) return;
        axios.post('/api/Survey', this.survey).then((res) => {
            let jobj = res.data as server.resultJSON<server.survey>;
            if (jobj.code == 0) {
                this.toastSuccess(jobj.msg);
            }
        });
    }
}