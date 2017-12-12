import ComponentBase from "../../componentbase";
import { Component } from 'vue-property-decorator';
import axios from "axios";

@Component
export default class OilStoreComponent extends ComponentBase {
    datestr: string = "";
    show1: boolean = false;
    show2: boolean = false;
    showAssays: boolean = false;
    showAct: boolean = false;
    litreToTon: number;

    survey: server.survey;
    surveys: server.survey[];
    sts: server.storeType[];
    salesSts: server.store[];
    selectStore: server.store;
    assays: server.assay[];

    actItems: ydui.actionSheetItem[];

    constructor() {
        super();

        this.datestr = this.formatDate(new Date());
        this.sts = new Array<server.storeType>();
        this.salesSts = new Array<server.store>();
        this.surveys = new Array<server.survey>();
        this.assays = new Array<server.assay>();
        this.selectStore = new Object() as server.store;
        this.survey = new Object() as server.survey;
        this.survey.density = null;
        this.survey.count = null;
        this.litreToTon = 0;
        this.getStoreTypes();
    }

    mounted() {
        this.$emit('setTitle', '所有仓情况');
        console.log(this.survey);
        this.$watch('survey.density', (v, ov) => {
            if (v && v != '') {
                if (this.survey != null)
                    this.litreToTon = Math.round(<number>this.survey.count * v / 1000 * 100) / 100;
            }
        });
        this.$watch('survey.count', (v, ov) => {
            if (v && v != '') {
                if (this.survey != null)
                    this.litreToTon = Math.round(v * <number>this.survey.density / 1000 * 100) / 100;
            }
        });
    };

    /**
     * 添加测量记录
     */
    showAddSurvey(st: server.store) {
        this.show1 = true;
        this.selectStore = st;
        this.survey.count = null;
        this.survey.density = null;
        this.survey.storeId = st.id;
        this.survey.name = st.name;
    }

    //显示actionsheet
    storeclick(st: server.store) {
        let that = this;
        this.showAct = true;
        this.actItems = [
            {
                label: '测量',
                callback: () => {
                    that.showAddSurvey(st);
                }
            },
            {
                label: '最近十五次测量记录',
                callback: () => {
                    that.getSurveys(st.id);
                    that.show2 = true;
                }
            },
            {
                label: '化验记录',
                callback: () => {
                    that.getAssays(st.id);
                    that.showAssays = true;
                }
            }
        ];
    }

    /**
     * 当前数量百分比
     */
    getPercent(val: number, vol: number): number {
        //分母为0的情况
        if (vol === 0)
            return 0;

        let percent = Math.round(val / vol * 10) / 10;
        //出现负数按0计算
        if (percent < 0)
            percent = 0;
        //超过100的按100算
        if (percent > 100)
            percent = 100;
        return percent
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

    getSurveys(stid: number) {
        axios.get('/api/Survey/GetTop15/' + stid.toString()).then((res) => {
            let jobj = res.data as server.resultJSON<server.survey[]>;
            if (jobj.code == 0) {
                this.surveys = jobj.data;
            }
        });
    }

    getAssays(stid: number) {
        axios.get('/api/Assay/GetByStoreId/' + stid.toString()).then((res) => {
            let jobj = res.data as server.resultJSON<server.assay[]>;
            if (jobj.code == 0) {
                this.assays = jobj.data;
            }
        });
    }

    strInOutDiff(sumIn: number, sumOut: number) {
        let diff = Math.round(sumIn - sumOut);
        if (diff > 0)
            return "+" + diff.toString()
        else
            return diff
    }

    validate() {
        if (this.survey.temperature == null) {
            this.toastError('油温不能为空');
            return false;
        }
        if (this.survey.density == null) {
            this.toastError('密度不能为空');
            return false;
        }
        if (this.survey.height == null) {
            this.toastError('油高不能为空');
            return false;
        }
        if (this.survey.count == null) {
            this.toastError('对应升数不能为空');
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
                this.show1 = false;
            }
        });
    }
}