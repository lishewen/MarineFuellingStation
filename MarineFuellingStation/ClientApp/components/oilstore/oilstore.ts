import ComponentBase from "../../componentbase";
import { Component } from 'vue-property-decorator';
import axios from "axios";
import moment from "moment";

@Component
export default class OilStoreComponent extends ComponentBase {
    datestr: string = "";
    show1: boolean = false;
    show2: boolean = false;
    showAssays: boolean = false;
    showAct: boolean = false;

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
        this.survey.density = 0;
        this.survey.count = 0;
        this.getStoreTypes();
    }

    mounted() {
        this.$emit('setTitle', '所有仓情况');
    };

    /**
     * 添加测量记录
     */
    showAddSurvey(st: server.store) {
        this.show1 = true;
        this.survey = new Object as server.survey;
        this.survey.count = 0;
        this.survey.density = 0;
        this.survey.storeId = st.id;
        this.survey.name = st.name;
        this.selectStore = st;
    }

    //显示actionsheet
    storeclick(st: server.store) {
        this.showAct = true;
        this.actItems = [
            {
                label: '测量',
                method: () => {
                    this.showAddSurvey(st)
                }
            },
            {
                label: '最近十五次测量记录',
                method: () => {
                    this.getSurveys(st.id);
                    this.show2 = true;
                }
            },
            {
                label: '化验记录',
                method: () => {
                    this.getAssays(st.id);
                    this.showAssays = true;
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

    strToton() {
        return this.survey.count * <number>this.survey.density / 1000
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
        let diff = sumIn - sumOut;
        if (diff > 0)
            return "+" + diff.toString()
        else
            return diff
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
        if (this.survey.height == null || this.survey.height == '') {
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
            }
        });
    }
}